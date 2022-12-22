using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventDispatcher : MonoBehaviour
{
    private Queue<Tick> _ticks;
    private GameplayController _controller;
    private NoteSpawner _noteSpawner;
    private SongAssetDownloader _assets;
    private GameObject _metronome;

    // Connect to dependencies
    void Awake()
    {
        _ticks = new Queue<Tick>();
        _controller = GameObject.Find("GameplayController").GetComponent<GameplayController>();
        _assets = GameObject.Find("GameplayController").GetComponent<SongAssetDownloader>();
        _noteSpawner = GameObject.Find("NoteField").GetComponent<NoteSpawner>();
    }

    // Load note note data into gameplay scene
    void Start()
    {
        string songData = _assets.GetNoteJson();
        RawNoteDataSet dataSet = RawNoteDataSet.createEventsFromJSON(songData);
        int currentTick = 0;
        int firstElementInTick = 0;
        bool foundFirstNotes = false;
        int noteCount = 0;

        // The process below finds each event and aggregates all events that occur at the same time into one enqueued Tick object
        for (int i = 0; i <= dataSet.events.Length; i++)
        {
            // We track notecount for determining scoring logic
            if (i < dataSet.events.Length && dataSet.events[i].type == 0) {
                noteCount++;
            }
            // If we're out of bounds, drop tick to -1 which will save the last events and then exit loop
            int newTick = i == dataSet.events.Length ? -1 : dataSet.events[i].tick;
            if (newTick != currentTick)
            {
                // Create gameplay events from data found on the same tick
                GameplayEvent[] newEvent = GameplayEvent.createFromRangeOfRawData(ref dataSet.events, firstElementInTick, i - 1);
                if (!foundFirstNotes) {
                    for (int x=0; x<newEvent.Length; x++) {
                        GameplayEvent currEvent = newEvent[x];
                        if (currEvent.type == GameplayEventType.Note) {
                            foundFirstNotes = true;
                            _noteSpawner.markFirstNote(currEvent.eventMeta);
                        }
                    }
                }
                _ticks.Enqueue(new Tick(currentTick, newEvent));
                firstElementInTick = i;
                currentTick = newTick;
            }
        }
        // Report final note count to scorekeeper to set up calculation logic
        _controller.GetComponent<ScoreKeeper>().ReportNoteCount(noteCount);
        _metronome = GameObject.Find("Metronome");
    }

    // Fire all events before/during a given tick
    public void FireEventsForTick(float tick) {
        // Half second lead time for animations
        while(_ticks.Count > 0 && _ticks.Peek().tick <= tick+150) {
            Tick tickObject = _ticks.Dequeue();
            FireEvents(tickObject.events, tickObject.tick);
        }
    }

    // Fire a group of events
    private void FireEvents(GameplayEvent[] events, float tick) {
        for (int i=0; i<events.Length; i++) {
            FireEvent(events[i], tick);
        }
    }

    // Fire a single event
    private void FireEvent(GameplayEvent eventToFire, float tick) {
        switch(eventToFire.type) {
            case GameplayEventType.Note:
                _noteSpawner.TriggerNote(eventToFire.eventMeta, tick);
                break;
            case GameplayEventType.Beat:
                _metronome.BroadcastMessage("OnBeat");
                break;
            case GameplayEventType.Measure:
                _metronome.BroadcastMessage("OnMeasure");
                break;
        }
    }
    
    // Load, store in memory, and unload the song notefile.
    private static string LoadSongJSON(string path)
    {
        TextAsset songFile = Resources.Load<TextAsset>(path);
        string songFileText = songFile.text;
        Resources.UnloadAsset(songFile);
        return songFileText;
    }
}

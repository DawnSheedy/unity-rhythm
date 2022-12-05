using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventDispatcher : MonoBehaviour
{
    private Queue<Tick> _ticks;
    private GameplayController _controller;
    private NoteSpawner _noteSpawner;

    void Awake()
    {
        _ticks = new Queue<Tick>();
        _controller = GameObject.Find("GameplayController").GetComponent<GameplayController>();
        _noteSpawner = GameObject.Find("NoteField").GetComponent<NoteSpawner>();
    }

    // Start is called before the first frame update
    void Start()
    {
        string songData = LoadSongJSON(_controller.getNoteFilePath());
        RawNoteDataSet dataSet = RawNoteDataSet.createEventsFromJSON(songData);
        int currentTick = 0;
        int firstElementInTick = 0;
        bool foundFirstNotes = false;
        int noteCount = 0;
        for (int i = 0; i <= dataSet.events.Length; i++)
        {
            if (i < dataSet.events.Length && dataSet.events[i].type == 0) {
                noteCount++;
            }
            // If we're out of bounds, drop tick to -1 which will save the last events and then exit loop
            int newTick = i == dataSet.events.Length ? -1 : dataSet.events[i].tick;
            if (newTick != currentTick)
            {
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
        _controller.GetComponent<ScoreKeeper>().ReportNoteCount(noteCount);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void FireEventsForTick(float tick) {
        // Half second lead time for animations
        while(_ticks.Count > 0 && _ticks.Peek().tick <= tick+150) {
            Tick tickObject = _ticks.Dequeue();
            FireEvents(tickObject.events, tickObject.tick);
        }
    }

    private void FireEvents(GameplayEvent[] events, float tick) {
        for (int i=0; i<events.Length; i++) {
            FireEvent(events[i], tick);
        }
    }

    private void FireEvent(GameplayEvent eventToFire, float tick) {
        switch(eventToFire.type) {
            case GameplayEventType.Note:
                _noteSpawner.TriggerNote(eventToFire.eventMeta, tick);
                break;
        }
    }
    
    private static string LoadSongJSON(string path)
    {
        Debug.Log(path);
        TextAsset songFile = Resources.Load<TextAsset>(path);

        return songFile.text;
    }
}

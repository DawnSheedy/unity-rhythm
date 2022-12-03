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
        string songData = LoadSongJSON(_controller.getNoteFilePath());
        RawNoteDataSet dataSet = RawNoteDataSet.createEventsFromJSON(songData);

        int currentTick = 0;
        int firstElementInTick = 0;
        for (int i = 0; i <= dataSet.events.Length; i++)
        {
            // If we're out of bounds, drop tick to -1 which will save the last events and then exit loop
            int newTick = i == dataSet.events.Length ? -1 : dataSet.events[i].tick;
            if (newTick != currentTick)
            {
                _ticks.Enqueue(new Tick(currentTick, GameplayEvent.createFromRangeOfRawData(ref dataSet.events, firstElementInTick, i - 1)));
                firstElementInTick = i;
                currentTick = newTick;
            }
        }
        // All events now loaded into ticks.
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void FireEventsForTick(float tick) {
        // Half second lead time for animations
        while(_ticks.Count > 0 && _ticks.Peek().tick <= tick+150) {
            FireEvents(_ticks.Dequeue().getEvents());
        }
    }

    private void FireEvents(GameplayEvent[] events) {
        for (int i=0; i<events.Length; i++) {
            FireEvent(events[i]);
        }
    }

    private void FireEvent(GameplayEvent eventToFire) {
        switch(eventToFire.type) {
            case GameplayEventType.Note:
                _noteSpawner.TriggerNote(eventToFire.eventMeta);
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

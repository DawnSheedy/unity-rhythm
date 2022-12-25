using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetronomeSynchronizer : MonoBehaviour
{
    private Queue<double> _nextBeats;
    private Queue<double> _nextMeasures;
    private Queue<double> _nextNotes;
    // Start is called before the first frame update
    void Start()
    {
        _nextBeats = new Queue<double>();
        _nextMeasures = new Queue<double>();
        _nextNotes = new Queue<double>();
    }

    // Update is called once per frame
    void Update()
    {
        double currentTime = AudioSettings.dspTime;

        if (_nextBeats.Count > 0 && currentTime >= _nextBeats.Peek())
        {
            Pulse("Beat");
            _nextBeats.Dequeue();
        }
        if (_nextNotes.Count > 0 && currentTime >= _nextNotes.Peek())
        {
            Pulse("Note");
            _nextNotes.Dequeue();
        }
        if (_nextMeasures.Count > 0 && currentTime >= _nextMeasures.Peek())
        {
            Pulse("Measure");
            _nextMeasures.Dequeue();
        }
    }

    void OnBeat()
    {
        _nextBeats.Enqueue(AudioSettings.dspTime + 0.5);
    }

    void OnMeasure()
    {
        _nextMeasures.Enqueue(AudioSettings.dspTime + 0.5);
    }

    void OnNote()
    {
        _nextNotes.Enqueue(AudioSettings.dspTime + 0.5);
    }

    void Pulse(string key) {
        gameObject.BroadcastMessage("Timed"+key);
    }

}

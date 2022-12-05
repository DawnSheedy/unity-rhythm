using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTimeKeeper : MonoBehaviour
{
    public static float tickFrequencyInHz = 300;
    public float tick;
    public float songPosition;
    public float dspSongTime;
    public AudioSource _audio;
    public GameplayController _controller;
    public AudioClip _audioResource;
    private EventDispatcher _eventDispatcher;

    void Awake() {
        _controller = GameObject.Find("GameplayController").GetComponent<GameplayController>();
        _audio = gameObject.AddComponent<AudioSource>();
        _audioResource = Resources.Load<AudioClip>(_controller.getSongMusicPath());
        _audio.clip = _audioResource;
    }
    // Start is called before the first frame update
    void Start()
    {
        _eventDispatcher = gameObject.GetComponent<EventDispatcher>();
        dspSongTime = (float)AudioSettings.dspTime;
        _audio.PlayDelayed(0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float getTick() {
        return tick;
    }

    void FixedUpdate() {
        songPosition = (float)AudioSettings.dspTime - dspSongTime;
        tick = tickFrequencyInHz * songPosition;
        _eventDispatcher.FireEventsForTick(tick);
    }
}

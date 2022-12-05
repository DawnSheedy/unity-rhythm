using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTimeKeeper : MonoBehaviour
{
    public static float tickFrequencyInHz = 300;
    public float tick = 0;
    public float songPosition;
    public float dspSongTime;
    public float timeTilStart = 100;
    public bool songStarted;
    private float songStartOffset = 5f;
    private float _songStartTime;
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
        _songStartTime = (float)AudioSettings.dspTime + songStartOffset;
        _eventDispatcher = gameObject.GetComponent<EventDispatcher>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!songStarted && AudioSettings.dspTime >= _songStartTime) {
            dspSongTime = (float)AudioSettings.dspTime;
            _audio.PlayDelayed(0f);
            songStarted = true;
        } else {
            timeTilStart = _songStartTime - (float)AudioSettings.dspTime;
        }
    }

    public float getTick() {
        return tick;
    }

    void FixedUpdate() {
        if (!songStarted) return;
        songPosition = (float)AudioSettings.dspTime - dspSongTime;
        tick = tickFrequencyInHz * songPosition;
        _eventDispatcher.FireEventsForTick(tick);
    }
}

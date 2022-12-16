using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    In charge of playing the songfile for the selected song, as well as keeping track of the current tick.
    Fires events every tick using FixedUpdate.
*/
public class AudioTimeKeeper : MonoBehaviour
{
    [Tooltip("Tick frequency in Hz (jubeat default 300)")]
    public static float tickFrequencyInHz = 300;

    [Tooltip("How long to wait before starting the song playback on Load")]
    public static float songStartOffset = 5f;

    private float tick = 0;
    private float songPosition;
    private float dspSongTime;
    private float timeTilStart = 100;
    private bool songStarted;
    private float _songStartTime;
    private AudioSource _audio;
    private GameplayController _controller;
    private AudioClip _audioResource;
    private EventDispatcher _eventDispatcher;

    // Connect to dependent components and load audio.
    void Awake() {
        _controller = GameObject.Find("GameplayController").GetComponent<GameplayController>();
        _audio = gameObject.AddComponent<AudioSource>();
        _audioResource = GameObject.Find("GameplayController").GetComponent<SongAssetDownloader>().GetSong();
        _eventDispatcher = gameObject.GetComponent<EventDispatcher>();
        _audio.clip = _audioResource;
    }

    // Unload Audio Asset on song end
    void Destroy() {
        Resources.UnloadAsset(_audioResource);
    }

    // Set song start time based on configured offset
    void Start()
    {
        _songStartTime = (float)AudioSettings.dspTime + songStartOffset;
    }

    // Wait til song is supposed to play and then play it and start counting ticks
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

    // Accessor for tick
    public float getTick() {
        return tick;
    }

    public float getTimeTilStart() {
        return timeTilStart;
    }

    // Calculate tick and fire events
    void FixedUpdate() {
        if (!songStarted) return;
        songPosition = (float)AudioSettings.dspTime - dspSongTime;
        tick = tickFrequencyInHz * songPosition;
        _eventDispatcher.FireEventsForTick(tick);
    }
}

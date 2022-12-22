using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    [Tooltip("Adjust latency")]
    public static float audioDelay;

    private float tick = 0;
    private float songPosition;
    private float dspSongTime;
    private float timeTilStart = 100;
    private bool songStarted;
    private float _songStartTime;
    private AudioSource _audio;
    private AudioClip _audioResource;
    private EventDispatcher _eventDispatcher;
    private float _songLength;

    // Connect to dependent components and load audio.
    void Awake() {
        _audio = gameObject.AddComponent<AudioSource>();
        _audio.volume = SettingRetriever.getSetting("NoteSounds") || SettingRetriever.getSetting("Metronome") ? 0.25f : 1;
        SongAssetDownloader assetDownloader = GameObject.Find("GameplayController").GetComponent<SongAssetDownloader>();
        _audioResource = assetDownloader.GetSong();
        _songLength = assetDownloader.GetSongMeta().length + 300*2;

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
            UnityEngine.iOS.Device.hideHomeButton = true;
            _audio.PlayDelayed(audioDelay);
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
        if (tick >= _songLength)
        {
            UnityEngine.iOS.Device.hideHomeButton = false;
            SceneManager.LoadScene("SongSelectScene");
        }
        _eventDispatcher.FireEventsForTick(tick);
    }
}

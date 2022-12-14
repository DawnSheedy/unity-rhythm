using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MetronomeModes
{
    Beat,
    Measure,
    Note
}

public class MetronomeRenderer : MonoBehaviour
{
    public float speed = 20f;
    [Tooltip("Sound to play on beat")]
    public AudioClip Sound;
    [Tooltip("Enable metronome")]
    public bool AudioEnabled;
    public MetronomeModes metronomeMode = MetronomeModes.Beat;
    private bool _playMetronome;
    private bool _playNotes;

    SpriteRenderer _spriteRenderer;
    private AudioSource _audio;
    private Queue<double> _nextTaps;
    // Start is called before the first frame update
    void Start()
    {
        _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        _audio = gameObject.AddComponent<AudioSource>();
        _audio.loop = false;
        _nextTaps = new Queue<double>();
        _playMetronome = SettingRetriever.getSetting("Metronome");
        _playNotes = SettingRetriever.getSetting("NoteSounds");
    }

    // Update is called once per frame
    void Update()
    {
        _spriteRenderer.color = new Color(_spriteRenderer.color.r, _spriteRenderer.color.g, _spriteRenderer.color.b, Mathf.MoveTowards(_spriteRenderer.color.a, 0, Time.deltaTime * speed));
        double currentTime = AudioSettings.dspTime;

        if (_nextTaps.Count > 0 && currentTime >= _nextTaps.Peek())
        {
            Pulse();
            _nextTaps.Dequeue();
        }
    }

    void OnBeat()
    {
        if (metronomeMode != MetronomeModes.Beat || !_playMetronome) { return; }
        _nextTaps.Enqueue(AudioSettings.dspTime + 0.5);
    }

    void OnMeasure()
    {
        if (metronomeMode != MetronomeModes.Measure || !_playMetronome) { return; }
        _nextTaps.Enqueue(AudioSettings.dspTime + 0.5);
    }

    void OnNote()
    {
        if (metronomeMode != MetronomeModes.Note || !_playNotes) { return; }
        _nextTaps.Enqueue(AudioSettings.dspTime + 0.5);
    }

    void Pulse() {
        _spriteRenderer.color = new Color(_spriteRenderer.color.r, _spriteRenderer.color.g, _spriteRenderer.color.b, 1);
        _audio.PlayOneShot(Sound);
    }

}

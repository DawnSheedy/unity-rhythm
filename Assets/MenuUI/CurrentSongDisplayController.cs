using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class CurrentSongDisplayController : MonoBehaviour
{
    AudioSource _audioSource;
    Label _title;
    Label _artist;
    VisualElement _icon;

    Button _basicStart;
    Button _advancedStart;
    Button _extremeStart;

    // Start is called before the first frame update
    void Start()
    {
        _audioSource = gameObject.AddComponent<AudioSource>();
        VisualElement root = gameObject.GetComponent<UIDocument>().rootVisualElement;
        _title = root.Q<Label>("SongDisplayTitle");
        _artist = root.Q<Label>("SongDisplayArtist");
        _icon = root.Q<VisualElement>("SongDisplayIcon");

        _basicStart = root.Q<Button>("Basic");
        _basicStart.RegisterCallback<ClickEvent>(HandleStartBasic);
        _advancedStart = root.Q<Button>("Advanced");
        _advancedStart.RegisterCallback<ClickEvent>(HandleStartAdvanced);
        _extremeStart = root.Q<Button>("Extreme");
        _extremeStart.RegisterCallback<ClickEvent>(HandleStartExtreme);
    }

    void HandleStartBasic(ClickEvent evt)
    {
        SetDifficultyAndStartSong(Difficulty.Basic);
    }

    void HandleStartAdvanced(ClickEvent evt)
    {
        SetDifficultyAndStartSong(Difficulty.Advanced);
    }

    void HandleStartExtreme(ClickEvent evt)
    {
        SetDifficultyAndStartSong(Difficulty.Extreme);
    }

    void SetDifficultyAndStartSong(Difficulty difficulty)
    {
        if (!PlayerPrefs.HasKey("SelectedSongTitle"))
        {
            return;
        }
        PlayerPrefs.SetInt("SelectedSongDifficulty", (int)difficulty);
        SceneManager.LoadScene("GameplayScene");
    }

    void NewSongSelected()
    {
        ClearCurrentSong();
        GetNewSongData();
    }

    public void GetNewSongData()
    {
        string serverHost = PlayerPrefs.GetString("SongServerHost");
        string songTitle = PlayerPrefs.GetString("SelectedSongTitle");
        StartCoroutine(getSongMeta(serverHost, songTitle));
        StartCoroutine(getSongIndexClip(serverHost, songTitle));
        StartCoroutine(getSongLogo(serverHost, songTitle));
    }

    public void ClearCurrentSong()
    {
        _audioSource.Stop();
        _title.text = "Loading...";
        _artist.text = "Loading...";
        _icon.style.backgroundImage = null;
    }

    IEnumerator getSongMeta(string host, string songTitle)
    {
        string uri = "http://" + host + "/songdata/" + songTitle + "/meta.json";
        UnityWebRequest uwr = UnityWebRequest.Get(uri);
        yield return uwr.SendWebRequest();

        if (!string.IsNullOrEmpty(uwr.error))
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            ProcessNewSongMeta(uwr.downloadHandler.text);
        }
    }

    void ProcessNewSongMeta(string json)
    {
        SongMeta newMeta = SongMeta.createFromJSON(json);
        _title.text = newMeta.title;
        _artist.text = newMeta.artist;
    }

    IEnumerator getSongIndexClip(string host, string songTitle)
    {
        string uri = "http://" + host + "/songdata/" + songTitle + "/index.wav";
        UnityWebRequest uwr = UnityWebRequestMultimedia.GetAudioClip(uri, AudioType.WAV);
        yield return uwr.SendWebRequest();

        if (!string.IsNullOrEmpty(uwr.error))
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            ProcessNewSongIndex(DownloadHandlerAudioClip.GetContent(uwr));
        }
    }

    private void ProcessNewSongIndex(AudioClip audioClip)
    {
        _audioSource.clip = audioClip;
        _audioSource.loop = true;
        _audioSource.Play();
    }

    IEnumerator getSongLogo(string host, string songTitle)
    {
        string uri = "http://" + host + "/songdata/" + songTitle + "/bnr.png";
        UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(uri);
        yield return uwr.SendWebRequest();

        if (!string.IsNullOrEmpty(uwr.error))
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            ProcessNewLogo(DownloadHandlerTexture.GetContent(uwr));
        }
    }

    private void ProcessNewLogo(Texture iconTexture)
    {
        _icon.style.backgroundImage = new StyleBackground(TextureToTexture2D(iconTexture));
    }

    private Texture2D TextureToTexture2D(Texture rTex)
    {
        Texture2D dest = new Texture2D(rTex.width, rTex.height, TextureFormat.RGBA32, false);

        Graphics.CopyTexture(rTex, dest);

        return dest;
    }
}

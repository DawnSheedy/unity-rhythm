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
    Button _favoriteButton;
    private bool _favorited;

    void Start()
    {
        _audioSource = gameObject.AddComponent<AudioSource>();
        _audioSource.volume = SettingRetriever.getSetting("Metronome") || SettingRetriever.getSetting("NoteSounds") ? 0.25f : 1f;
        VisualElement root = gameObject.GetComponent<UIDocument>().rootVisualElement;
        _title = root.Q<Label>("SongDisplayTitle");
        _artist = root.Q<Label>("SongDisplayArtist");
        _icon = root.Q<VisualElement>("SongDisplayIcon");

        root.Q<Button>("Basic").RegisterCallback<ClickEvent>(HandleStartBasic);
        root.Q<Button>("Advanced").RegisterCallback<ClickEvent>(HandleStartAdvanced);
        root.Q<Button>("Extreme").RegisterCallback<ClickEvent>(HandleStartExtreme);
        _favoriteButton = root.Q<Button>("FavoriteButton");
        _favoriteButton.RegisterCallback<ClickEvent>(ToggleFavorite);
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

    void ToggleFavorite(ClickEvent evt)
    {
        string serverHost = PlayerPrefs.GetString("SongServerHost");
        string songTitle = PlayerPrefs.GetString("SelectedSongUuid");
        StartCoroutine(favoriteSong(serverHost, songTitle));
    }

    void SetDifficultyAndStartSong(Difficulty difficulty)
    {
        if (!PlayerPrefs.HasKey("SelectedSongUuid"))
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
        string songTitle = PlayerPrefs.GetString("SelectedSongUuid");
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
        string uri = "http://" + host + "/api/songs/specific/" + songTitle;
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

    IEnumerator favoriteSong(string host, string songTitle)
    {
        string uri = "http://" + host + "/api/songs/favorites";
        string json = "{ \"songId\": \"" + songTitle + "\", \"favorite\": " + (_favorited ? "false" : "true") + "}";

        var req = new UnityWebRequest(uri, "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        req.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        req.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        req.SetRequestHeader("Content-Type", "application/json");

        //Send the request then wait here until it returns
        yield return req.SendWebRequest();

        if (!string.IsNullOrEmpty(req.error))
        {
            Debug.Log("Error While Sending: " + req.error);
        }
        else
        {
            ProcessNewSongMeta(req.downloadHandler.text);
        }
    }

    void ProcessNewSongMeta(string json)
    {
        SongMeta newMeta = SongMeta.createFromJSON(json);
        _title.text = newMeta.title;
        _artist.text = newMeta.artist;
        _favorited = newMeta.favorited;
        _favoriteButton.text = _favorited ? "Remove from Favorites" : "Favorite";
    }

    IEnumerator getSongIndexClip(string host, string songTitle)
    {
        string uri = "http://" + host + "/songdata/" + songTitle + "/index.ogg";
        UnityWebRequest uwr = UnityWebRequestMultimedia.GetAudioClip(uri, AudioType.OGGVORBIS);
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

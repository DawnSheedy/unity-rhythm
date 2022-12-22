using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SongAssetDownloader : MonoBehaviour
{
    Texture2D _icon;
    SongMeta _meta;
    string _noteJson;
    AudioClip _song;
    bool _allAssetsLoaded;

    int ready = 0;

    // Start is called before the first frame update
    void Start()
    {
        GetSongData();
    }

    void Update()
    {
        if (!_allAssetsLoaded)
        {
            _allAssetsLoaded = ready >= 4;

            if (_allAssetsLoaded)
            {
                gameObject.BroadcastMessage("AssetsLoaded");
                Debug.Log("All Assets Loaded!!");
            }
        }
    }

    public Texture2D GetIcon()
    {
        return _icon;
    }

    public SongMeta GetSongMeta()
    {
        return _meta;
    }

    public string GetNoteJson()
    {
        return _noteJson;
    }

    public AudioClip GetSong()
    {
        return _song;
    }

    public void GetSongData()
    {
        string serverHost = PlayerPrefs.GetString("SongServerHost");
        string songTitle = PlayerPrefs.GetString("SelectedSongUuid");
        Difficulty difficulty = (Difficulty)PlayerPrefs.GetInt("SelectedSongDifficulty");
        StartCoroutine(getSongMeta(serverHost, songTitle));
        StartCoroutine(getSongNoteFile(serverHost, songTitle, difficulty));
        StartCoroutine(getSongIndexClip(serverHost, songTitle));
        StartCoroutine(getSongLogo(serverHost, songTitle));
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

    void ProcessNewSongMeta(string json)
    {
        _meta = SongMeta.createFromJSON(json);
        ready++;
    }

    private static string[] difficultyFileNames = { "bsc", "adv", "ext" };
    IEnumerator getSongNoteFile(string host, string songTitle, Difficulty difficulty)
    {
        string uri = "http://" + host + "/songdata/" + songTitle + "/"+difficultyFileNames[(int)difficulty]+".json";
        UnityWebRequest uwr = UnityWebRequest.Get(uri);
        yield return uwr.SendWebRequest();

        if (!string.IsNullOrEmpty(uwr.error))
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            ProcessNewSongNoteFile(uwr.downloadHandler.text);
        }
    }

    void ProcessNewSongNoteFile(string json)
    {
        _noteJson = json;
        ready++;
    }

    IEnumerator getSongIndexClip(string host, string songTitle)
    {
        string uri = "http://" + host + "/songdata/" + songTitle + "/song.ogg";
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
        _song = audioClip;
        ready++;
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
        _icon = TextureToTexture2D(iconTexture);
        ready++;
    }

    private Texture2D TextureToTexture2D(Texture rTex)
    {
        Texture2D dest = new Texture2D(rTex.width, rTex.height, TextureFormat.RGBA32, false);

        Graphics.CopyTexture(rTex, dest);

        return dest;
    }
}

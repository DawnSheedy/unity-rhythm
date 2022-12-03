using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayController : MonoBehaviour
{
    public string songLoadOverride;
    public Difficulty songDifficultyOverride;

    private SongMeta _songMeta;

    private GameObject _conductor;

    void Awake() {
        string metaJson = LoadSongMetaJSON();
        _songMeta = SongMeta.createEventsFromJSON(metaJson);
    }

    // Start is called before the first frame update
    void Start()
    {
        _conductor = new GameObject("Conductor");
        _conductor.AddComponent<EventDispatcher>();
        _conductor.AddComponent<AudioTimeKeeper>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Destroy() {
        Destroy(_conductor);
    }

    public string getTitle() {
        return _songMeta.title;
    }

    public string getArtist() {
        return _songMeta.artist;
    }

    public Difficulty getSelectedDifficulty() {
        return songDifficultyOverride;
    }

    private static string[] difficultyFileNames = { "bsc", "adv", "ext" };
    public string getNoteFilePath() {
        return getSongBasePath() + difficultyFileNames[(int)getSelectedDifficulty()];
    }

    public string getSongMusicPath() {
        return getSongBasePath() + "song";
    }

    public string getSongMetaPath() {
        return getSongBasePath() + "meta";
    }

    private string getSongBasePath() {
        return "SongFiles/" + songLoadOverride + "/";
    }

    private string LoadSongMetaJSON()
    {
        TextAsset songMetaTextAsset = Resources.Load<TextAsset>(getSongMetaPath());

        return songMetaTextAsset.text;
    }
}

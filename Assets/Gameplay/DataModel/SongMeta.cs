using UnityEngine;

[System.Serializable]
public class SongMeta {
    public string title;
    public string artist;
    public int tempo;
    public int length;
    public SongDifficultyMeta[] versions;

    public static SongMeta createEventsFromJSON(string jsonInput) {
        return JsonUtility.FromJson<SongMeta>(jsonInput);
    }
}
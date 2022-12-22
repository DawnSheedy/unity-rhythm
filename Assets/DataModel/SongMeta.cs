using UnityEngine;

[System.Serializable]
public class SongMeta {
    public string title;
    public string artist;
    public string uuid;
    public int tempo;
    public int length;
    public bool favorited;
    public SongDifficultyMeta[] versions;

    public static SongMeta createFromJSON(string jsonInput) {
        return JsonUtility.FromJson<SongMeta>(jsonInput);
    }
}
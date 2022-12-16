using UnityEngine;

[System.Serializable]
public class SongMetaApiResponse
{
    public SongMeta[] songs;

    public static SongMetaApiResponse createFromJSON(string jsonInput)
    {
        return JsonUtility.FromJson<SongMetaApiResponse>(jsonInput);
    }
}
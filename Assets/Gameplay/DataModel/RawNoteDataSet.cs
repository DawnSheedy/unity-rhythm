using UnityEngine;

[System.Serializable]
public class RawNoteDataSet {
    public RawNoteData[] events;
    public static RawNoteDataSet createEventsFromJSON(string jsonInput) {
        return JsonUtility.FromJson<RawNoteDataSet>(jsonInput);
    }
}
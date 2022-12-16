using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// In charge of loading/kicking off gameplay based on song data
public class GameplayController : MonoBehaviour
{
    public string songLoadOverride;
    public Difficulty songDifficultyOverride;

    private GameObject _conductor;

    void AssetsLoaded() {
        _conductor = new GameObject("Conductor");
        _conductor.AddComponent<EventDispatcher>();
        _conductor.AddComponent<AudioTimeKeeper>();
        GameObject.Find("NoteField").BroadcastMessage("ConductorAlive");
        GameObject.Find("InGameUIController").BroadcastMessage("ConductorAlive");
    }

    // On Destroy, destroy created gameObjects as well.
    void Destroy() {
        Destroy(_conductor);
    }

    // Get the resources path for the song directory
    private string getSongBasePath() {
        return "SongFiles/" + songLoadOverride + "/";
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteSpawner : MonoBehaviour
{
    [Tooltip("Prefab for notes to spawn")]
    public GameObject notePrefab;

    private Bounds _noteFieldBounds;
    private Bounds _noteDimensions;
    private float _boardLineWidth;
    
    private int noteCount = 0;
    // Start is called before the first frame update
    void Start()
    {
        _noteFieldBounds = Camera.main.GetComponent<GameplayBoundsResolver>().PlayAreaBounds;
        _boardLineWidth = gameObject.GetComponent<NotefieldRenderer>().lineWidth;
        // Get the w/h of an individual note
        float noteSize = ((_noteFieldBounds.size.x-(_boardLineWidth*3)) / 4);
        _noteDimensions = new Bounds(new Vector3(0,0,0), new Vector3(noteSize, noteSize, 0));

        for (int x=0; x<4; x++) {
            for (int y=0; y<4; y++) {
                SpawnNote(x, y);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnNote(int x, int y) {
        GameObject newNote = Instantiate(notePrefab, GetSpawnPoint(x, y), Quaternion.identity);
        newNote.name = "Note "+noteCount++;
        newNote.transform.localScale = _noteDimensions.size;
    }

    private Vector3 GetSpawnPoint(int x, int y) {
        float tileX = (_noteDimensions.size.x + _boardLineWidth) * x;
        float tileY = (_noteDimensions.size.y + _boardLineWidth) * y;
        return (_noteFieldBounds.min + (new Vector3(tileX, tileY, 0) + (_noteDimensions.extents)));
    }
}

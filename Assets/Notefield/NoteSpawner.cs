using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteSpawner : MonoBehaviour
{
    [Tooltip("Prefab for notes to spawn")]
    public GameObject notePrefab;

    private GameplayBoundsResolver _boundsResolver;
    private NotefieldRenderer _noteFieldRenderer;
    private Bounds _noteFieldBounds;
    private Bounds _noteDimensions;
    private float _boardLineWidth;
    
    private int noteCount = 0;

    private GameObject[] _notes = new GameObject[16];

    private NoteController[] _noteControllers = new NoteController[16];

    void Awake() {
        _boundsResolver = Camera.main.GetComponent<GameplayBoundsResolver>();
        _noteFieldRenderer = gameObject.GetComponent<NotefieldRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _noteFieldBounds = _boundsResolver.PlayAreaBounds;
        _boardLineWidth = _noteFieldRenderer.lineWidth;
        // Get the w/h of an individual note
        float noteSize = ((_noteFieldBounds.size.x-(_boardLineWidth*3)) / 4);
        _noteDimensions = new Bounds(new Vector3(0,0,0), new Vector3(noteSize, noteSize, 0));

        for (int x=0; x<_notes.Length; x++) {
            _notes[x] = SpawnNote(x);
            _noteControllers[x] = _notes[x].GetComponent<NoteController>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TriggerNote(int index, float targetTiming) {
        _noteControllers[index].LeadIn(targetTiming);
    }

    public void markFirstNote(int index) {
        _noteControllers[index].MarkFirst();   
    }

    private GameObject SpawnNote(int index) {
        int x = index % 4;
        int y = index / 4;
        return SpawnNote(x, y);
    }

    GameObject SpawnNote(int x, int y) {
        GameObject newNote = Instantiate(notePrefab, GetSpawnPoint(x, y), Quaternion.identity);
        newNote.name = "Note "+noteCount++;
        newNote.transform.localScale = _noteDimensions.size;
        return newNote;
    }

    private Vector3 GetSpawnPoint(int x, int y) {
        float tileX = (_noteDimensions.size.x + _boardLineWidth) * x;
        float tileY = (_noteDimensions.size.y + _boardLineWidth) * y;
        return (_noteFieldBounds.min + (new Vector3(tileX, tileY, 0) + (_noteDimensions.extents)));
    }
}

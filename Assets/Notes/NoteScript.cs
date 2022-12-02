using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteScript : MonoBehaviour
{
    [Tooltip("Is the note a hold note?")]
    public bool IsHold = false;

    [Tooltip("Hold Duration (in MS)")]
    public int HoldDuration = 0;

    private float _holdStartTime = -1;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseDown()
    {
        if (!IsHold) {
            Destroy(gameObject);
        }
    }
}

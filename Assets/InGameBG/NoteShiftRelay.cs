using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteShiftRelay : MonoBehaviour
{
    void TimedNote() {
        gameObject.BroadcastMessage("Shift");
    }
}

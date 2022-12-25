using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatShiftRelay : MonoBehaviour
{
    void TimedBeat() {
        gameObject.BroadcastMessage("Shift");
    }
}

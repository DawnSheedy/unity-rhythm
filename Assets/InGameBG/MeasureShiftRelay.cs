using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeasureShiftRelay : MonoBehaviour
{
    void TimedMeasure() {
        gameObject.BroadcastMessage("Shift");
    }
}

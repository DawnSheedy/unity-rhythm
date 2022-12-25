using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboShiftRelay : MonoBehaviour
{
    void ComboChanged() {
        gameObject.BroadcastMessage("Shift");
    }
}

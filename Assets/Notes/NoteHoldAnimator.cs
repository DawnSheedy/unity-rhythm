using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteHoldAnimator : MonoBehaviour
{
    private string TouchStartAnim = "TouchStart";
    private string TouchEndAnim = "TouchEnd";
    private string GetReadyAnim = "GetReady";
    private bool first;
    private Animator _animator;
    // Start is called before the first frame update
    void Awake()
    {
        _animator = gameObject.GetComponent<Animator>();       
    }

    void OnTouchStart() {
        _animator.Play(TouchStartAnim);
    }

    void OnTouchEnd() {
        _animator.Play(TouchEndAnim);
    }

    public void MarkFirst() {
        _animator.Play(GetReadyAnim);
        first = true;
    }

    void JudgementStarting() {
        if (first) {
            first = false;
            OnTouchEnd();
        }
    }
}

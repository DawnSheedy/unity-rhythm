using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteHoldAnimator : MonoBehaviour
{
    private string TouchStartAnim = "TouchStart";
    private string TouchEndAnim = "TouchEnd";
    private Animator _animator;
    // Start is called before the first frame update
    void Start()
    {
        _animator = gameObject.GetComponent<Animator>();       
    }

    void OnTouchStart() {
        _animator.Play(TouchStartAnim);
    }

    void OnTouchEnd() {
        _animator.Play(TouchEndAnim);
    }
}

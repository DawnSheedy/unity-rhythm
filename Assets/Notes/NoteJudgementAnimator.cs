using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteJudgementAnimator : MonoBehaviour
{
    public GameObject parentNote;
    private string PerfectAnim = "PerfectHit";
    private string GreatAnim = "GreatHit";
    private string GoodAnim = "GoodHit";
    private Animator _animator;
    // Start is called before the first frame update
    void Start()
    {
        _animator = gameObject.GetComponent<Animator>();       
    }
 
    public void DisplayJudgement(int level) {
        switch (level) {
            case 0:
                _animator.Play(PerfectAnim);
                break;
            case 1:
                _animator.Play(GreatAnim);
                break;
            case 2:
                _animator.Play(GoodAnim);
                break;
        }
        
    }

    void DismissShutter() {
        parentNote.SendMessage("DismissShutter");
    }
}

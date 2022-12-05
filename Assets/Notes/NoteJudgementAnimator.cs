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
 
    public void DisplayJudgement(Judgement judgement) {
        switch (judgement) {
            case Judgement.Perfect:
                _animator.Play(PerfectAnim);
                break;
            case Judgement.Great:
                _animator.Play(GreatAnim);
                break;
            case Judgement.Good:
                _animator.Play(GoodAnim);
                break;
        }
        
    }

    void DismissShutter() {
        parentNote.SendMessage("DismissShutter");
    }
}

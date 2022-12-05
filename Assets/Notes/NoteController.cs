using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteController : MonoBehaviour
{
    public GameObject animationLayerPrefab;
    
    private string JudgementApproachAnim = "JudgementApproach";
    private string IdleAnim = "Idle";

    private Animator _animator;
    private SpriteRenderer _spriteRenderer;

    private AudioTimeKeeper _timeKeeper;
    private float _nextJudgementTime;
    private bool _noteReadyForJudgement = false;
    private GameObject _noteHoldLayer;
    private GameObject _noteScoreLayer;
    private NoteHoldAnimator _noteHoldAnimator;
    private NoteJudgementAnimator _noteScoreAnimator;
    private float PerfectThreshold = (40f/1000f)*300f;
    private float GreatThreshold = (80f/1000f)*300f;
    private float GoodThreshold = (160f/1000f)*300f;

    // Start is called before the first frame update
    void Start()
    {
        _noteHoldLayer = Instantiate(animationLayerPrefab, gameObject.transform.localPosition, Quaternion.identity);
        _noteScoreLayer = Instantiate(animationLayerPrefab, gameObject.transform.localPosition, Quaternion.identity);
        _noteHoldLayer.transform.position += new Vector3(0, 0, 0.01f);
        _noteHoldLayer.transform.localScale = gameObject.transform.localScale;
        _noteHoldLayer.name = gameObject.name + " (Note Hold Indicator)";
        _noteHoldAnimator = _noteHoldLayer.AddComponent<NoteHoldAnimator>();
        _noteScoreLayer.transform.position += new Vector3(0, 0, -0.01f);
        _noteScoreLayer.transform.localScale = gameObject.transform.localScale;
        _noteScoreLayer.name = gameObject.name + " (Score Indicator)";
        _noteScoreAnimator = _noteScoreLayer.AddComponent<NoteJudgementAnimator>();
        _noteScoreAnimator.parentNote = gameObject;

        _timeKeeper = GameObject.Find("Conductor").GetComponent<AudioTimeKeeper>();
        _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        _animator = gameObject.GetComponent<Animator>();
    }

    void OnTouchStart() {
        _noteHoldLayer.SendMessage("OnTouchStart");
        JudgeHit();
    }

    void Update() {
        // If note is done rendering, goal was in the past, and the user never clicked it, judge as a miss.
        if (_noteReadyForJudgement && _animator.GetCurrentAnimatorStateInfo(0).IsName(IdleAnim) && _timeKeeper.getTick() > _nextJudgementTime) {
            ProcessJudgement(3);
        }
    }

    void JudgeHit() {
        // Check if there's even anything to judge.
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName(IdleAnim) || !_noteReadyForJudgement) {
            return;
        }

        float currentTick = _timeKeeper.getTick();
        float tickDifference = Mathf.Abs(currentTick - _nextJudgementTime);

        if (tickDifference < PerfectThreshold) {
            ProcessJudgement(0);
        } else if (tickDifference < GreatThreshold) {
            ProcessJudgement(1);
        } else if (tickDifference < GoodThreshold) {
            ProcessJudgement(2);
        } else {
            ProcessJudgement(3);
        }
    }

    void ProcessJudgement(int level) {
        _noteReadyForJudgement = false;
        _noteScoreAnimator.DisplayJudgement(level);
    }

    void OnTouchEnd() {
        _noteHoldLayer.SendMessage("OnTouchEnd");
    }

    void DismissShutter() {
        _animator.Play(IdleAnim);
    }

    public void LeadIn(float targetTiming)
    {
        _nextJudgementTime = targetTiming;
        _noteReadyForJudgement = true;
        _animator.Play(JudgementApproachAnim);
    }
}

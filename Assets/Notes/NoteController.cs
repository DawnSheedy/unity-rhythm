using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteController : MonoBehaviour
{
    public GameObject animationLayerPrefab;
    
    private string JudgementApproachAnim = "JudgementApproach";
    private string IdleAnim = "Idle";

    private Animator _animator;

    private float _nextJudgementTime;
    private bool _noteReadyForJudgement = false;
    private GameObject _noteHoldLayer;
    private GameObject _noteScoreLayer;
    private NoteHoldAnimator _noteHoldAnimator;
    private NoteJudgementAnimator _noteScoreAnimator;
    private ScoreKeeper _scoreKeeper;
    private float PerfectThreshold = (40f/1000f);
    private float GreatThreshold = (80f/1000f);
    private float GoodThreshold = (160f/1000f);

    private bool _demoMode;

    void Awake() {
        _noteHoldLayer = Instantiate(animationLayerPrefab, gameObject.transform.localPosition, Quaternion.identity);
        _noteScoreLayer = Instantiate(animationLayerPrefab, gameObject.transform.localPosition, Quaternion.identity);
        _noteScoreAnimator = _noteScoreLayer.AddComponent<NoteJudgementAnimator>();
        _noteHoldAnimator = _noteHoldLayer.AddComponent<NoteHoldAnimator>();
        _scoreKeeper = GameObject.Find("GameplayController").GetComponent<ScoreKeeper>();
    }
    // Start is called before the first frame update
    void Start()
    {
        _noteHoldLayer.transform.position += new Vector3(0, 0, 0.01f);
        _noteHoldLayer.transform.localScale = gameObject.transform.localScale;
        _noteHoldLayer.name = gameObject.name + " (Note Hold Indicator)";
        _noteScoreLayer.transform.position += new Vector3(0, 0, -0.01f);
        _noteScoreLayer.transform.localScale = gameObject.transform.localScale;
        _noteScoreLayer.name = gameObject.name + " (Score Indicator)";
        _noteScoreAnimator.parentNote = gameObject;
        _animator = gameObject.GetComponent<Animator>();
        _demoMode = SettingRetriever.getSetting("DemoMode");
    }

    void OnTouchStart() {
        _noteHoldLayer.SendMessage("OnTouchStart");
        JudgeHit();
    }

    void Update() {
        // If note is done rendering, goal was in the past, and the user never clicked it, judge as a miss.
        if (_noteReadyForJudgement && _animator.GetCurrentAnimatorStateInfo(0).IsName(IdleAnim) && (float)AudioSettings.dspTime > _nextJudgementTime) {
            ProcessJudgement(Judgement.Miss);
        }
    }

    void JudgeHit() {
        // Check if there's even anything to judge.
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName(IdleAnim) || !_noteReadyForJudgement) {
            return;
        }

        float currentTime = (float)AudioSettings.dspTime;
        float timeDifference = Mathf.Abs(currentTime - _nextJudgementTime);

        if (timeDifference < PerfectThreshold) {
            ProcessJudgement(Judgement.Perfect);
        } else if (timeDifference < GreatThreshold) {
            ProcessJudgement(Judgement.Great);
        } else if (timeDifference < GoodThreshold) {
            ProcessJudgement(Judgement.Good);
        } else {
            ProcessJudgement(Judgement.Miss);
        }
    }

    void ProcessJudgement(Judgement judgement) {
        _noteReadyForJudgement = false;
        _noteScoreAnimator.DisplayJudgement(judgement);
        _scoreKeeper.ProcessJudgement(judgement);
    }

    void OnTouchEnd() {
        _noteHoldLayer.SendMessage("OnTouchEnd");
    }

    void DismissShutter() {
        _animator.Play(IdleAnim);
    }

    void TouchPointAnimReached()
    {
        if (_demoMode)
        {
            ProcessJudgement(Judgement.Perfect);
        }
    }

    public void MarkFirst() {
        _noteHoldAnimator.MarkFirst();
    }

    public void LeadIn(float targetTiming)
    {
        _nextJudgementTime = (float)AudioSettings.dspTime + 0.5f;
        _noteReadyForJudgement = true;
        _noteHoldLayer.SendMessage("JudgementStarting");
        _animator.Play(JudgementApproachAnim);
    }
}

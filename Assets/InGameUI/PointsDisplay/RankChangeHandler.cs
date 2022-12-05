using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankChangeHandler : MonoBehaviour
{
    private static string ScoreChangeAnim = "ScoreChange";
    private Animator _animator;
    private Rank _rankOnDeck;
    private RankDisplayController _rankDisplayController;
    private ScoreKeeper _scoreKeeper;
    // Start is called before the first frame update
    void Start()
    {
        _animator = gameObject.GetComponent<Animator>();
        _rankDisplayController = GameObject.Find("ScoreTextRating").GetComponent<RankDisplayController>();
        _scoreKeeper = GameObject.Find("GameplayController").GetComponent<ScoreKeeper>();
        SetRank(0);
    }

    void Update() {
        if (_rankOnDeck != _scoreKeeper.currentRank) {
            SetRank(_scoreKeeper.currentRank);
        }
    }

    void SetRank(Rank newRank) {
        _rankOnDeck = newRank;
        _animator.Play(ScoreChangeAnim);
    }

    // Trigger to indicate that the score text is fully hidden, and can safely be changed.
    void ScoreObstructed() {
        _rankDisplayController.SetRank(_rankOnDeck);
    }
}

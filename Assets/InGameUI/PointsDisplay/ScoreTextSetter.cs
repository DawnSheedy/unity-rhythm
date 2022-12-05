using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreTextSetter : MonoBehaviour
{
    private TextMeshPro _textMesh;
    private ScoreKeeper _scoreKeeper;
    private float score;
    // Start is called before the first frame update
    void Start()
    {
        _textMesh = gameObject.GetComponent<TextMeshPro>();
        _scoreKeeper = GameObject.Find("GameplayController").GetComponent<ScoreKeeper>();
    }

    // Update is called once per frame
    void Update()
    {
        if (score != _scoreKeeper.currentScore) {
            score = _scoreKeeper.currentScore;
            SetTextScore();
        }
    }

    void SetTextScore() {
        _textMesh.text = ((int)(score)).ToString("D6");
    }
}

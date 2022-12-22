using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ComboTextSetter : MonoBehaviour
{
    private TextMeshPro _textMesh;
    private ScoreKeeper _scoreKeeper;
    private float combo;
    // Start is called before the first frame update
    void Start()
    {
        _textMesh = gameObject.GetComponent<TextMeshPro>();
        _scoreKeeper = GameObject.Find("GameplayController").GetComponent<ScoreKeeper>();
    }

    // Update is called once per frame
    void Update()
    {
        if (combo != _scoreKeeper.currentCombo)
        {
            combo = _scoreKeeper.currentCombo;
            SetTextScore();
        }
    }

    void SetTextScore()
    {
        _textMesh.text = ((int)(combo)).ToString("D3");
    }
}
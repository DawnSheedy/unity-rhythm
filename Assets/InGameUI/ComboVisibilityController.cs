using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboVisibilityController : MonoBehaviour
{
    public int threshold = 10;
    private FlickerTransition _transition;
    private ScoreKeeper _scoreKeeper;
    private bool visible = false;
    // Start is called before the first frame update
    void Start()
    {
        _scoreKeeper = GameObject.Find("GameplayController").GetComponent<ScoreKeeper>();
        _transition = gameObject.GetComponent<FlickerTransition>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_scoreKeeper.currentCombo >= threshold && !visible) {
            visible = true;
            _transition.ToggleVisibility();
        }
        if (_scoreKeeper.currentCombo < threshold && visible) {
            visible = false;
            _transition.ToggleVisibility();
        }
    }
}

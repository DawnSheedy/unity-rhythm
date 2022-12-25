using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboParticles : MonoBehaviour
{
    public int particlesToEmit = 50;
    public int emissionInterval = 100;
    ParticleSystem _pSystem;
    ScoreKeeper _scoreKeeper;
    private int lastCombo = 0;
    private int lastMilestone = 0;
    private bool particlesAppeared = false;
    // Start is called before the first frame update
    void Start()
    {
        _scoreKeeper = GameObject.Find("GameplayController").GetComponent<ScoreKeeper>();
        _pSystem = gameObject.GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        int currentCombo = _scoreKeeper.currentCombo;
        if (lastCombo == currentCombo)
        {
            return;
        }

        if (currentCombo < lastCombo) {
            lastMilestone = 0;
            particlesAppeared = false;
        }

        lastCombo = currentCombo;

        if (currentCombo >= 10 && !particlesAppeared)
        {
            _pSystem.Emit(50);
            particlesAppeared = true;
        }
        if (currentCombo / emissionInterval > lastMilestone) {
            _pSystem.Emit(particlesToEmit);
            lastMilestone = currentCombo / emissionInterval;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadScreenTransparencyController : MonoBehaviour
{
    private AudioTimeKeeper _timeKeeper;
    private SpriteRenderer _spriteRenderer;
    private bool visible = true;
    private bool inactive = false;
    // Start is called before the first frame update
    void Start()
    {
        _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_timeKeeper) {
            _timeKeeper = GameObject.Find("Conductor").GetComponent<AudioTimeKeeper>();
        }

        if (inactive) return;

        if (visible) {
            if (_timeKeeper.timeTilStart < 2f) {
                gameObject.BroadcastMessage("DestroyGracefully");
                visible = false;
            } 
            return;
        }

        _spriteRenderer.color = new Color(0,0,0, Mathf.MoveTowards(_spriteRenderer.color.a, 0, Time.deltaTime));
    }
}

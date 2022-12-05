using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickerTransition : MonoBehaviour
{
    public float speed = 20f;
    public int count = 5;
    public bool visible;
    public bool isAnimating;
    public float randomDelayRange;
    private float delay;
    private float startTime;
    private int flickerCount;
    private bool doDestroy;
    private float maxOpacity;
    private SpriteRenderer _spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        maxOpacity = _spriteRenderer.color.a;
        if (!visible) {
            _spriteRenderer.color = new Color(_spriteRenderer.color.r, _spriteRenderer.color.g, _spriteRenderer.color.b, 0);
        }
        delay = Random.Range(0, randomDelayRange);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAnimating || Time.time < startTime) return;

        _spriteRenderer.color = new Color(_spriteRenderer.color.r, _spriteRenderer.color.g, _spriteRenderer.color.b, Mathf.MoveTowards(_spriteRenderer.color.a, visible ? 0 : maxOpacity, Time.deltaTime * speed));

        if (_spriteRenderer.color.a == 0) {
            flickerCount++;
            visible = false;
        } else if (_spriteRenderer.color.a == maxOpacity) {
            flickerCount++;
            visible = true;
        }

        if (count <= flickerCount) {
            if (doDestroy) {
                Destroy(gameObject);
            }
            isAnimating = false;
            flickerCount = 0;   
        }
    }

    public void ToggleVisibility() {
        isAnimating = true;
        startTime = Time.time + delay;
    }

    public void ToggleVisibility(bool destroy) {
        doDestroy = destroy;
        ToggleVisibility();
    }

    void DestroyGracefully() {
        ToggleVisibility(true);
    }
}

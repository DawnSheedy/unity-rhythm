using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
    private TextMeshPro _textMesh;
    // Start is called before the first frame update
    void Start()
    {
        _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        _textMesh = gameObject.GetComponent<TextMeshPro>();
        Color c = getColor();
        maxOpacity = c.a;
        if (!visible) {
            setColor(new Color(c.r, c.g, c.b, 0));
        }
        delay = Random.Range(0, randomDelayRange);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAnimating || Time.time < startTime) return;

        Color c = getColor();
        setColor(new Color(c.r, c.g, c.b, Mathf.MoveTowards(c.a, visible ? 0 : maxOpacity, Time.deltaTime * speed)));

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

    void setColor(Color c) {
        if (_spriteRenderer) {
            _spriteRenderer.color = c;
        }
        if (_textMesh) {
            _textMesh.color = c;
        }
    }

    Color getColor() {
        if (_textMesh) {
            return _textMesh.color;
        }
        return _spriteRenderer.color;
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

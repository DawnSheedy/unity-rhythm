using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteScript : MonoBehaviour
{
    [Tooltip("Is the note a hold note?")]
    public bool IsHold = false;

    [Tooltip("Hold Duration (in MS)")]
    public int HoldDuration = 0;

    [Tooltip("Name of CueAnim to use")]
    public string AnimationName;

    private float _holdStartTime = -1;

    private Animation _animation;
    private Animator _animator;

    private SpriteRenderer _spriteRenderer;


    // Start is called before the first frame update
    void Start()
    {
        _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        _animation = Resources.Load<Animation>("CueAnims/"+AnimationName);
        _animator = gameObject.GetComponent<Animator>();
        _animator.Play(AnimationName);
    }

    void Destroy() {
        Resources.UnloadAsset(_animation);
    }

    // Update is called once per frame
    void Update()
    {
        if(_animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !_animator.IsInTransition(0)) {
            _spriteRenderer.material.color = new Color(1f, 1f, 1f, 0f);
        } else {
            _spriteRenderer.material.color = new Color(1f, 1f, 1f, 1f);
        }
    }

    public void LeadIn() {
        _animator.Play(AnimationName, -1, 0f);
    }

    void OnMouseDown()
    {
    }
}

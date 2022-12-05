using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleHider : MonoBehaviour
{
    public string IdleAnim = "Idle";
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        _animator = gameObject.GetComponent<Animator>();
        _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName(IdleAnim))
        {
            _spriteRenderer.material.color = new Color(1f, 1f, 1f, 0f);
        }
        else
        {
            _spriteRenderer.material.color = new Color(1f, 1f, 1f, 1f);
        }
    }
}

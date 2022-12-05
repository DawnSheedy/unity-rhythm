using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BannerImageLoader : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private Sprite _bannerSprite;
    private GameplayController _gamePlayController;
    void Awake() {
        _gamePlayController = GameObject.Find("GameplayController").GetComponent<GameplayController>();
        _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _bannerSprite = Resources.Load<Sprite>(_gamePlayController.getSongBannerArtPath());
        _spriteRenderer.sprite = _bannerSprite;
    }

    void Destroy() {
        Resources.UnloadAsset(_bannerSprite);
    }
}

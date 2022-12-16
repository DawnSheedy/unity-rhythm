using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BannerImageLoader : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private Sprite _bannerSprite;

    void Awake() {
        _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Texture2D songLogo = GameObject.Find("GameplayController").GetComponent<SongAssetDownloader>().GetIcon();
        _bannerSprite = Sprite.Create(songLogo, new Rect(0,0, songLogo.width, songLogo.height), new Vector2(0.5f,0.5f));
        _spriteRenderer.sprite = _bannerSprite;
    }

    void Destroy() {
        Resources.UnloadAsset(_bannerSprite);
    }
}

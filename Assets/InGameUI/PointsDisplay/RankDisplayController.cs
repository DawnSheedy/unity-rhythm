using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankDisplayController : MonoBehaviour
{
    private static string[] RankFileMap = { "PreScore", "SSS", "SS", "S", "AAA", "AA", "A", "B", "C", "D", "F" };
    private static Sprite[] RankSprites = new Sprite[11];
    private static SpriteRenderer _spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        for (int i=0; i<RankFileMap.Length; i++) {
            RankSprites[i] = Resources.Load<Sprite>("Judgements/"+RankFileMap[i]);
        }
    }

    void Destroy() {
        for (int i=0; i<RankSprites.Length; i++) {
            Resources.UnloadAsset(RankSprites[i]);
        }
    }

    public void SetRank(Rank rank) {
        _spriteRenderer.sprite = RankSprites[(int)rank];
    }
}

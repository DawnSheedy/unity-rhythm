using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SongTitleSetter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        TextMeshPro textMesh = gameObject.GetComponent<TextMeshPro>();
        textMesh.text = GameObject.Find("GameplayController").GetComponent<SongAssetDownloader>().GetSongMeta().title;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

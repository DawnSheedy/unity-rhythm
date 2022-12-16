using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SongArtistSetter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        TextMeshPro textMesh = gameObject.GetComponent<TextMeshPro>();
        textMesh.text = GameObject.Find("GameplayController").GetComponent<SongAssetDownloader>().GetSongMeta().artist;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameUISpawner : MonoBehaviour
{
    [Tooltip("Prefab for banner art rendering sprite.")]
    public GameObject BannerArtRendererPrefab;
    [Tooltip("Prefab for score rendering")]
    public GameObject ScoreRendererPrefab;
    private GameObject _bannerArt;
    private GameObject _scoreArea;
    private Shader _lineShader;
    private GameObject _bottomLine;

    void ConductorAlive() {
        Bounds interfaceBounds = Camera.main.GetComponent<GameplayBoundsResolver>().InterfaceBounds;
        Vector3 upperInterfaceSpawnPoint = interfaceBounds.min + new Vector3(1, interfaceBounds.size.y - 1f, 0);
        Vector3 lowerInterfaceSpawnPoint = interfaceBounds.max - new Vector3(0, interfaceBounds.size.y, 0);
        _lineShader = Resources.Load<Shader>("Shaders/LineShader");
        _bannerArt = GameObject.Instantiate(BannerArtRendererPrefab, upperInterfaceSpawnPoint, Quaternion.identity);
        _scoreArea = GameObject.Instantiate(ScoreRendererPrefab, lowerInterfaceSpawnPoint, Quaternion.identity);
        CreateBottomBorder(interfaceBounds);
    }

    void CreateBottomBorder(Bounds interfaceBounds) {
        GameObject line = new GameObject("InterfaceBottomBorderLine");

        line.AddComponent<LineRenderer>();
        LineRenderer lr = line.GetComponent<LineRenderer>();
        lr.material = new Material(_lineShader);
        lr.startWidth = 0.1f;
        lr.endWidth = 0.1f;
        lr.SetPosition(0, interfaceBounds.min + new Vector3(0,0.05f,0));
        lr.SetPosition(1, interfaceBounds.min + new Vector3(interfaceBounds.size.x,0.05f,0));

        _bottomLine = line;
    }

    void Destroy() {
        Resources.UnloadAsset(_lineShader);
        Destroy(_bottomLine);
        Destroy(_scoreArea);
        Destroy(_bannerArt);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

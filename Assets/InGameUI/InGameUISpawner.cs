using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameUISpawner : MonoBehaviour
{
    [Tooltip("Prefab for banner art rendering sprite.")]
    public GameObject BannerArtRendererPrefab;
    private GameObject _bannerArt;
    private Shader _lineShader;
    private GameObject _bottomLine;
    // Start is called before the first frame update
    void Start()
    {
        Bounds interfaceBounds = Camera.main.GetComponent<GameplayBoundsResolver>().InterfaceBounds;
        Vector3 upperInterfaceSpawnPoint = interfaceBounds.min + new Vector3(1, interfaceBounds.size.y-1f, 0);
        _lineShader = Resources.Load<Shader>("Shaders/LineShader");
        _bannerArt = GameObject.Instantiate(BannerArtRendererPrefab, upperInterfaceSpawnPoint, Quaternion.identity);
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
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

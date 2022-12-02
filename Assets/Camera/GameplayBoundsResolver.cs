using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayBoundsResolver : MonoBehaviour
{
    public Bounds DisplayBounds { get; set; }
    public Bounds PlayAreaBounds { get; set; }
    public Bounds InterfaceBounds { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        Camera camera = Camera.main;

        float height;
        float width;

        if (camera.orthographic) {
            height = 2f * camera.orthographicSize;
            width = height * camera.aspect;
        } else {
            Vector3 v3ViewPort = new Vector3(0,0,10);
            Vector3 v3BottomLeft = Camera.main.ViewportToWorldPoint(v3ViewPort);
            v3ViewPort.Set(1,1,10);
            Vector3 v3TopRight = Camera.main.ViewportToWorldPoint(v3ViewPort);
            height = v3TopRight.y - v3BottomLeft.y;
            width = v3TopRight.x - v3BottomLeft.x;
        }
        
        PlayAreaBounds = CalculatePlayAreaBounds(height, width);
        InterfaceBounds = CalculateInterfaceBounds(height, width);
        DisplayBounds = new Bounds(new Vector3(0, 0, 0), new Vector3(width, height, 0));
    }

    private Bounds CalculatePlayAreaBounds(float screenHeight, float screenWidth) {
        float bottomOfScreenY = -1 * (screenHeight / 2);
        Vector3 boundsCenterPoint = new Vector3(0, bottomOfScreenY + (screenWidth/2), 0);
        Vector3 boundsSize = new Vector3(screenWidth, screenWidth, 0);
        return new Bounds(boundsCenterPoint, boundsSize);
    }

    private Bounds CalculateInterfaceBounds(float screenHeight, float screenWidth) {
        float interfaceHeight = screenHeight - screenWidth;
        float topOfScreenY = (screenHeight / 2);
        Vector3 boundsCenterPoint = new Vector3(0, topOfScreenY - (interfaceHeight/2), 0);
        Vector3 boundsSize = new Vector3(screenWidth, interfaceHeight, 0);
        return new Bounds(boundsCenterPoint, boundsSize);
    }
}

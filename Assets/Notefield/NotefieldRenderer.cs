using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotefieldRenderer : MonoBehaviour
{
    [Tooltip("Width of border lines")]
    public float lineWidth = 0.1f;
    private GameplayBoundsResolver _boundsResolver;
    private Bounds _noteFieldBounds;
    private GameObject[,] _fieldLines = new GameObject[2, 3];
    private Shader _lineShader;

    void Awake() {
        _boundsResolver = Camera.main.GetComponent<GameplayBoundsResolver>();
        _lineShader = Resources.Load("Shaders/LineShader") as Shader;
    }

    // Start is called before the first frame update
    void Start()
    {
        _noteFieldBounds = _boundsResolver.PlayAreaBounds;
        DrawGameFieldLines();
    }

    // Teardown lines on destroy and unload shader
    void OnDestroy()
    {
        for (int x = 0; x < _fieldLines.GetLength(0); x++)
        {
            for (int y = 0; y < _fieldLines.GetLength(1); y++)
            {
                Destroy(_fieldLines[x, y]);
            }
        }

        Resources.UnloadAsset(_lineShader);
    }

    // Update is called once per frame
    void Update()
    {

    }


    void DrawGameFieldLines()
    {
        Vector3 halfPlayFieldWidthVector = new Vector3(_noteFieldBounds.extents.x, 0, 0);
        Vector3 halfPlayFieldHeightVector = new Vector3(0, _noteFieldBounds.extents.x, 0);

        for (int x = 0; x < _fieldLines.GetLength(0); x++)
        {
            Vector3 lengthVector = x == 0 ? halfPlayFieldWidthVector : halfPlayFieldHeightVector;
            Vector3 offsetVector = x != 0 ? halfPlayFieldWidthVector : halfPlayFieldHeightVector;
            Vector3 lineOffset = x != 0 ? new Vector3(lineWidth / 4, 0, 0) : new Vector3(0, lineWidth / 4, 0);
            for (int y = 0; y < _fieldLines.GetLength(1); y++)
            {
                _fieldLines[x, y] = CreateLineGameObjectForLineCoords(x, y, lengthVector, offsetVector, lineOffset);
            }
        }
    }

    private float[] _mutationOffsets = { -0.5f, 0f, 0.5f };
    private float[] _lineOffsets = { -1f, 0f, 1f };
    // Spawn a line based on defined indexes
    private GameObject CreateLineGameObjectForLineCoords(int x, int y, Vector3 lengthVector, Vector3 offsetVector, Vector3 lineOffset)
    {
        GameObject line = new GameObject("NoteFieldLine(" + x + "," + y + ")");

        line.AddComponent<LineRenderer>();
        LineRenderer lr = line.GetComponent<LineRenderer>();
        lr.material = new Material(_lineShader);
        lr.startWidth = lineWidth;
        lr.endWidth = lineWidth;
        Vector3 parallelOffset = offsetVector * _mutationOffsets[y];
        Vector3 transformedLineOffset = lineOffset * _lineOffsets[y];
        lr.SetPosition(0, (_noteFieldBounds.center - lengthVector) + parallelOffset + transformedLineOffset);
        lr.SetPosition(1, (_noteFieldBounds.center + lengthVector) + parallelOffset + transformedLineOffset);

        return line;
    }
}

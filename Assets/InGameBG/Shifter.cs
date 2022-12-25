using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shifter : MonoBehaviour
{
    public float shiftY = 0;
    public float shiftX = 0;
    public float shiftZ = 0;
    public float shiftSpeed = 100f;
    public float restSpeed = 20f;
    private bool _doShift = false;
    private bool _rest = true;

    private Vector3 restPos;
    private Vector3 shiftPos;
    // Start is called before the first frame update
    void Start()
    {
        restPos = gameObject.transform.position;
        shiftPos = new Vector3(restPos.x + shiftX, restPos.y + shiftY, restPos.z + shiftZ);
    }

    // Update is called once per frame
    void Update()
    {
        if (_doShift) {
            if (gameObject.transform.position.Equals(shiftPos)) {
                _doShift = false;
                _rest = false;
            }
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, shiftPos, Time.deltaTime*shiftSpeed);
        }

        if (!_doShift && !_rest) {
            if (gameObject.transform.position.Equals(restPos)) {
                _rest = true;
            }
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, restPos, Time.deltaTime*restSpeed);
        }
    }

    void Shift() {
        _doShift = true;
    }
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MultiTouchClickHandler : MonoBehaviour
{
    private bool _touchStarted = false;
    void Update()
    {
        bool anyTouchInBounds = false;
        // Track a single touch as a direction control.
        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch touch = Input.GetTouch(i);

            if (touch.phase == TouchPhase.Ended)
            {
                continue;
            }

            Ray ray = Camera.main.ScreenPointToRay(touch.position);
            RaycastHit2D[] hits = Physics2D.GetRayIntersectionAll(ray, Mathf.Infinity);
            //RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(touch.position), touch.position);
            for (int hitId = 0; hitId < hits.Length; hitId++)
            {
                RaycastHit2D hit = hits[hitId];
                if (hit.collider && hit.transform.gameObject == gameObject)
                {
                    anyTouchInBounds = true;
                }
            }

        }

        if (anyTouchInBounds && !_touchStarted)
        {
            _touchStarted = true;
            gameObject.SendMessage("OnTouchStart");
            return;
        }
        if (!anyTouchInBounds && _touchStarted)
        {
            _touchStarted = false;
            gameObject.SendMessage("OnTouchEnd");
            return;
        }
    }
}
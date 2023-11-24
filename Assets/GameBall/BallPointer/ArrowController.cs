using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArrowController : MonoBehaviour
{
    //public Transform targetObject;
    public Camera mainCamera;
    public Image arrowImage; // Reference to the Image component

    void Update()
    {
        if (GameBall.s == null || mainCamera == null)
        {
            // Ensure that the target object and main camera are assigned.
            return;
        }

        Vector3 screenPos = mainCamera.WorldToScreenPoint(GameBall.s.transform.position);

        if (screenPos.z > 0 && screenPos.x > 0 && screenPos.x < Screen.width && screenPos.y > 0 && screenPos.y < Screen.height)
        {
            // Object is on-screen, hide the arrow.
            arrowImage.enabled = false;
        }
        else
        {

            if (screenPos.z < 0)
            {
                screenPos *= -1;
            }

            // Object is off-screen, show the arrow and point towards it.
            arrowImage.enabled = true;
            Vector3 arrowDirection = screenPos - new Vector3(Screen.width / 2, Screen.height / 2, 0);
            float angle = Mathf.Atan2(arrowDirection.y, arrowDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
}

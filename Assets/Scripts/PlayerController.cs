using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public LayerMask teleportLayer;

    private TeleportController _currentPoint = null;
    private TeleportController _oldPoint = null;

    // Update is called once per frame
    void Update()
    {
        RaycastHit Hit;

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out Hit, Mathf.Infinity, teleportLayer))
        {
            Hit.collider.GetComponentInParent<TeleportController>().LoadingTeleport(this);
            Debug.DrawLine(Camera.main.transform.position, Hit.point, Color.green);
        }
        else
        {
            Debug.DrawLine(Camera.main.transform.position, Camera.main.transform.forward * 100, Color.red);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void SetCurrentPoint(TeleportController Point)
    {
        if(_currentPoint != null)
        {
            _oldPoint = _currentPoint;
            _oldPoint.gameObject.SetActive(true);
            _oldPoint.RestoreValues();
        }
            _currentPoint = Point;
    }
}

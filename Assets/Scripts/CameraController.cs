using System;
using System.Threading;
using System.Xml.Schema;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;

    private float yMin = -2f, yMax = 18f; // Max top & bottom values.

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    void LateUpdate()
    {
        var newPositionY = Mathf.Clamp(player.transform.position.y, yMin, yMax);
        transform.position = new Vector3(transform.position.x, newPositionY, transform.position.z);
    }
}

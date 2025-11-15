using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 5f;
    private bool isMoving = false;
    private float cooldown = 0f;
    private Vector3 originalPosition;
    private Vector3 secondPosition;
    private Vector3 targetPosition;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        originalPosition = transform.position;
        secondPosition = new Vector3(8.89f, 0f, -10);
        targetPosition = originalPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if(cooldown > 0)
        {
            cooldown -= Time.deltaTime;
            return;
        }

        if (isMoving)
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                transform.position = targetPosition;
                isMoving = false;
            }
        }
    }

    public void MoveCamera(Vector3 direction)
    {
        if (cooldown > 0)
        {
            return;
        }

        targetPosition = direction.x > 0 ? secondPosition : originalPosition;
        isMoving = true;
        cooldown = 0.5f;
    }
}

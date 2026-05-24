using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private float moveSpeed = 5f;

    private Vector3 targetPosition;
    private bool isMoving = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Vector3 mousePositionScreen = Mouse.current.position.value;
            targetPosition = Camera.main.ScreenToWorldPoint(mousePositionScreen);
            isMoving = true;
        }

        if (Mouse.current.leftButton.isPressed)
        {
            Vector3 mousePositionScreen = Mouse.current.position.value; // Position in screen space.
            targetPosition = Camera.main.ScreenToWorldPoint(mousePositionScreen); // Convert to world space.
            targetPosition.z = transform.position.z;

            if (isMoving)
            {
                Vector3 direction = (targetPosition - transform.position).normalized;

                transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

                if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
                {
                    isMoving = false;
                }
            }
        }
    }
}

using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private float moveSpeed = 5f;

    private Vector2 currentMoveInput = Vector2.zero;
    private Vector3 targetPosition;
    private bool isMoving = false;

    void Start()
    {

    }

    void Update()
    {
        MovePlayer();
    }

    public void OnMove(InputValue value)
    {
        // Save moving direction to trigger movement in update.
        currentMoveInput = value.Get<Vector2>();
    }

    public void OnClick(InputValue value)
    {
        Vector2 mousePosition = Mouse.current.position.value;
        targetPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        targetPosition.z = transform.position.z;

        isMoving = true;
    }

    private void MovePlayer()
    {
        var movementDistance = moveSpeed * Time.deltaTime;

        if (currentMoveInput != Vector2.zero)
        {
            // Continuous movement while using WASD.
            transform.position += (Vector3)currentMoveInput.normalized * movementDistance;
        }
        else if (isMoving)
        {
            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                isMoving = false;
            }
            else if (isMoving)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, movementDistance);
            }
        }
    }
}

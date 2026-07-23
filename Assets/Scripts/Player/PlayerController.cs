using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float quicksandDragFactor = 0.5f;

    private Rigidbody2D rb;
    private Vector2 currentMoveInput = Vector2.zero;
    private Vector3 targetPosition;
    private bool isMoving = false;
    private Transform quicksandCenter;

    public bool CanMove { get; set; } = true;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (CanMove)
        {
            MovePlayer();
        }
    }

    public void OnMove(InputValue value)
    {
        // Save moving direction to trigger movement in update.
        currentMoveInput = value.Get<Vector2>();
    }

    private void MovePlayer()
    {
        var effectiveSpeed = moveSpeed;

        if (quicksandCenter != null && currentMoveInput != Vector2.zero)
        {
            Vector2 inputDirection = currentMoveInput.normalized;
            Vector2 directionToQuicksandCenter = (quicksandCenter.position - transform.position).normalized;

            float dotProduct = Vector2.Dot(inputDirection, directionToQuicksandCenter);

            if (dotProduct < 0)
            {
                // Using dotProduct to influence the speed factor instantly. 
                // dot = -1 --> hardest drag (0.2), dot = 0.5 --> slight drag at 0.6 (60%) speed.
                effectiveSpeed *= Mathf.Lerp(1f, quicksandDragFactor, -dotProduct);
            }
        }

        var movementDistance = effectiveSpeed * Time.deltaTime;

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("QuickSand"))
        {
            // Assign quicksand center to trigger movement adjustment in quicksand.
            quicksandCenter = collision.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("QuickSand"))
        {
            // Resetting linear velocity after leaving quicksand 
            // (it stays on the last set value and continues to drag the player).
            rb.linearVelocity = Vector2.zero;
        }
    }
}

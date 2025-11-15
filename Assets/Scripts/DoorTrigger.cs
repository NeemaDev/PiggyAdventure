using UnityEngine;

public class DoorTrigger : MonoBehaviour
{

    private Vector2 direction; //(0,1) up, (0,-1) down, (1,0) right, (-1,0) left
    private CameraController cameraController;

    void Start()
    {
        cameraController = Camera.main.GetComponent<CameraController>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            var playerPosition = other.transform.position;
            var doorPosition = transform.position;

            var diff = playerPosition - doorPosition;

            if (diff.x > 0)
            {
                // Player moves right to left, set camera moving direction to right-to-left.
                direction = new Vector2(-1, 0);
            }
            else
            {
                // Player moves left to right, set camera moving direction to left-to-right.
                direction = new Vector2(1, 0);
            }

            cameraController.MoveCamera(direction);
        }
    }
}

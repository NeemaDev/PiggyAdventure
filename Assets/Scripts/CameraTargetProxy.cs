using UnityEngine;

public class CameraTargetProxy : MonoBehaviour
{
    private Transform playerTransform;
    private float minYThreshold = -3.5f;
    private float maxYThreshold = 19f;
    private float minXThreshold = -3.75f;
    private float maxXThreshold = 3.75f;

    private void Start()
    {
        if (playerTransform == null && transform.parent != null)
        {
            playerTransform = transform.parent;
        }

        UpdateProxyPosition();
    }

    private void Update()
    {
        UpdateProxyPosition();
    }

    private void UpdateProxyPosition()
    {
        if (playerTransform == null)
        {
            return;
        }

        float targetX = Mathf.Clamp(playerTransform.position.x, minXThreshold, maxXThreshold);
        float targetY = Mathf.Clamp(playerTransform.position.y, minYThreshold, maxYThreshold);
        transform.position = new Vector3(targetX, targetY, playerTransform.position.z);
    }
}

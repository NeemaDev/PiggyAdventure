using System.Collections;
using UnityEngine;

public class DesertEnemy : MonoBehaviour
{
    [SerializeField] private float pullFactor = 5f;
    private Coroutine grabCoroutine;
    private Vector3 grabbyPosition;
    private bool isGrabbyActive = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        bool isKillable = collision.gameObject.TryGetComponent(out IKillable target);
        if (isKillable)
        {
            target.Die();
        }
    }

    private void OnDrawGizmos()
    {
        if (isGrabbyActive)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, grabbyPosition);
            Gizmos.DrawWireSphere(grabbyPosition, 0.2f);
        }
    }

    public void StartGrabPlayer(Transform playerTransform)
    {
        if (grabCoroutine == null)
        {
            grabCoroutine = StartCoroutine(GrabPlayer(playerTransform));
        }
    }

    public void CancelGrabPlayer()
    {
        if (grabCoroutine != null)
        {
            StopCoroutine(grabCoroutine);
            grabCoroutine = null;
            isGrabbyActive = false;
        }
    }

    private IEnumerator GrabPlayer(Transform playerTransform)
    {
        isGrabbyActive = true;
        float telegraphTimer = 0f;
        float telegraphDuration = 1.0f;

        while (telegraphTimer < telegraphDuration)
        {
            telegraphTimer += Time.deltaTime;
            float percent = telegraphTimer / telegraphDuration;

            grabbyPosition = Vector3.Lerp(transform.position, playerTransform.position, percent);

            yield return null;

        }

        // Lock player movement.
        PlayerController controller = playerTransform.GetComponent<PlayerController>();
        if (controller != null)
        {
            controller.CanMove = false;
        }

        // Then drag.
        while (Vector3.Distance(playerTransform.position, transform.position) > 0.1f)
        {
            playerTransform.position = Vector3.MoveTowards(playerTransform.position, transform.position, pullFactor * Time.deltaTime);

            grabbyPosition = playerTransform.position;

            yield return null;
        }

        isGrabbyActive = false;
        grabCoroutine = null;
    }
}

using System.Collections;
using UnityEngine;

public class DesertEnemy : MonoBehaviour
{
    [Header("Telegraph")]
    [SerializeField] private float telegraphDuration = 1f;
    [SerializeField] private bool trackPlayerDuringTelegraph = false;

    [Tooltip("How close the player must be to still get caught.")]
    [SerializeField] private float catchRadius = 0.5f;

    [Header("Pull")]
    [SerializeField] private float pullSpeed = 1.5f; // units per second.
    [SerializeField] private float pullPauseDuration = 1f;

    [Header("Escape")]
    [SerializeField] private float progressPerPress = 0.35f;
    [SerializeField] private float progressDecayPerSecond = 0.2f;
    [SerializeField] private float regrabCooldown = 1.5f;

    [Header("Push")]
    [SerializeField] private float pushDistance = 1.5f;
    [SerializeField] private float pushDuration = 0.2f;


    private Coroutine grabCoroutine;
    private PlayerController playerController;
    private Rigidbody2D grabbedBody;
    private Vector3 grabbyPosition;
    private bool isGrabbyActive = false;
    private float escapeProgress;
    private float pullResumeTime;
    private float nextGrabAllowedTime;

    // UI targeted getters.
    public float EscapeProgress => escapeProgress;
    public bool IsHoldingPlayer => playerController != null;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        bool isKillable = collision.gameObject.TryGetComponent(out IKillable target);
        if (isKillable)
        {
            CancelGrabPlayer();
            target.Die();
        }
    }

    private void OnDisable() => CancelGrabPlayer();

    private void OnDrawGizmos()
    {
        if (isGrabbyActive)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, grabbyPosition);
            Gizmos.DrawWireSphere(grabbyPosition, 0.2f);
        }
    }

    public void StartGrabPlayer(GameObject player)
    {
        if (grabCoroutine == null && Time.time >= nextGrabAllowedTime)
        {
            grabCoroutine = StartCoroutine(GrabPlayer(player));
        }
    }

    public void CancelGrabPlayer()
    {
        if (grabCoroutine != null)
        {
            StopCoroutine(grabCoroutine);
        }

        ReleasePlayer(false);
    }

    private IEnumerator GrabPlayer(GameObject player)
    {
        PlayerStats playerStats = player.GetComponentInParent<PlayerStats>();

        // Early guard.
        if (!playerStats || playerStats.isGodMode)
        {
            grabCoroutine = null;
            yield break;
        }

        // Use GetComponentInParent in case a child collider triggered the zone.
        playerController = player.GetComponentInParent<PlayerController>();
        grabbedBody = player.GetComponentInParent<Rigidbody2D>();

        if (playerController == null || grabbedBody == null)
        {
            Debug.LogWarning("DesertEnemy: Player is missing PlayerController or Rigidbody2D!");
            grabCoroutine = null;
            yield break;
        }

        Transform playerTransform = playerController.transform;
        isGrabbyActive = true;

        Vector3 targetPosition = playerTransform.position;
        float timer = 0f;

        while (timer < telegraphDuration)
        {
            timer += Time.deltaTime;
            float percent = timer / telegraphDuration;

            if (trackPlayerDuringTelegraph)
            {
                targetPosition = playerTransform.position;
            }

            grabbyPosition = Vector3.Lerp(transform.position, targetPosition, percent);
            yield return null;
        }

        // Check if the player evaded the grab during the telegraph
        if (!trackPlayerDuringTelegraph && Vector2.Distance(playerTransform.position, targetPosition) > catchRadius)
        {
            ReleasePlayer(true);
            yield break;
        }

        // Player grabbed. 
        playerController.CanMove = false;
        playerController.StrugglePressed += OnStrugglePressed;
        escapeProgress = 0f;
        pullResumeTime = 0f; // Ensure pull starts immediately until they press struggle

        WaitForFixedUpdate waitForPhysics = new WaitForFixedUpdate();
        Vector2 normalizePullDirection = ((Vector2)transform.position - grabbedBody.position).normalized;

        while (escapeProgress < 1f)
        {
            escapeProgress = Mathf.Max(0f, escapeProgress - progressDecayPerSecond * Time.fixedDeltaTime);

            bool isPaused = Time.time < pullResumeTime;
            if (isPaused)
            {
                // Hold drag immediately on struggle press.
                grabbedBody.linearVelocity = Vector2.zero;
            }
            else
            {
                // Continue Drag.
                Vector2 toEnemy = (Vector2)transform.position - grabbedBody.position;

                if (toEnemy.sqrMagnitude > 0.0001f)
                {
                    normalizePullDirection = toEnemy.normalized;
                }

                grabbedBody.linearVelocity = normalizePullDirection * pullSpeed;
            }

            grabbyPosition = grabbedBody.position;
            yield return waitForPhysics;
        }

        // Escaped: push out in opposite direction.
        isGrabbyActive = false;
        playerController.StrugglePressed -= OnStrugglePressed;

        Vector2 pushDirection = -normalizePullDirection;
        float pushSpeed = pushDistance / pushDuration;

        for (float i = 0f; i < pushDuration; i += Time.fixedDeltaTime)
        {
            grabbedBody.linearVelocity = pushDirection * pushSpeed;
            yield return waitForPhysics;
        }

        ReleasePlayer(true);
    }

    /// <summary>
    /// Clean up after escape or release.
    /// </summary>
    /// <param name="escaped">Flag indicating if the player escaped a grap.</param>
    private void ReleasePlayer(bool escaped)
    {
        if (grabbedBody != null)
        {
            grabbedBody.linearVelocity = Vector2.zero;
            grabbedBody = null;
        }

        if (playerController != null)
        {
            // Release player. Unsub from event.
            playerController.StrugglePressed -= OnStrugglePressed;
            playerController.CanMove = true;
            playerController = null;
        }

        isGrabbyActive = false;
        grabCoroutine = null;
        escapeProgress = 0f;

        if (escaped)
        {
            nextGrabAllowedTime = Time.time + regrabCooldown;
        }
    }

    /// <summary>
    /// Progress for escape button mash mechanic.
    /// </summary>
    private void OnStrugglePressed()
    {
        escapeProgress = Mathf.Min(1f, escapeProgress + progressPerPress);
        pullResumeTime = Time.time + pullPauseDuration;
    }
}

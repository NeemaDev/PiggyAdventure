using System;
using System.Collections;
using Unity.VisualScripting.Antlr3.Runtime.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(Animator))]
public class Player : MonoBehaviour
{
    public static Transform PlayerTransform;
    public event Action<float, float> HealthChanged;
    public LayerMask attackableLayer;

    private PlayerInput inputs;
    private Vector2 moveInput;
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Vector2 lastMoveDirection = Vector2.up;
    private float movementSpeed = 4f;
    private float maxHealth = 100f;
    private float currentHealth = 100f;
    [SerializeField]
    private float knockbackForce = 20f;
    [SerializeField]
    private float knockbackDuration = 0.3f;
    private bool canMove = true;
    [SerializeField]
    private float attackCooldown = 0.5f;
    private float lastAttackTime;
    private RaycastHit2D[] hits;
    [SerializeField]
    private float attackRange = 0f;
    [SerializeField]
    private float attackRadius = 1f;
    private float attackDamage = 35f;

    public (float current, float max) GetHealth()
    {
        return (currentHealth, maxHealth);
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log($"Enemy took {damage} damage. Remaining Health: {currentHealth}");

        if (currentHealth <= 0)
        {
            // Die();
        }
        else
        {
            StartCoroutine(MovementLock());
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log($"Collision with {collision.gameObject.tag} occured!");
        if (collision.gameObject.CompareTag("Enemy"))
        {
            currentHealth -= (float)Math.Round(0.2 * maxHealth);
            HealthChanged?.Invoke(currentHealth, maxHealth);

            Vector2 knockbackDirection = (transform.position - collision.transform.position).normalized;
            rb.linearVelocity = Vector2.zero;
            rb.linearVelocity = knockbackDirection * knockbackForce;

            StartCoroutine(MovementLock());
        }
    }

    private void Awake()
    {
        inputs = new PlayerInput();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        PlayerTransform = transform;
        HealthChanged?.Invoke(currentHealth, maxHealth);
    }

    private void OnEnable()
    {
        inputs.PlayerActions.Enable();
    }

    private void OnDisable()
    {
        inputs.PlayerActions.Disable();
    }

    private void Update()
    {
        if (!canMove)
        {
            return;
        }

        moveInput = inputs.PlayerActions.Movement.ReadValue<Vector2>();
        Vector2 movement = moveInput * movementSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + movement);

        if (moveInput != Vector2.zero)
        {
            lastMoveDirection = moveInput.normalized;
        }

        animator.SetFloat("Speed", moveInput.sqrMagnitude);
        animator.SetFloat("MoveX", Mathf.Abs(lastMoveDirection.x));
        animator.SetFloat("MoveY", lastMoveDirection.y);

        if (lastMoveDirection.x != 0)
        {
            spriteRenderer.flipX = lastMoveDirection.x < 0;
        }

        if (inputs.PlayerActions.Attack.WasPerformedThisFrame())
        {
            TryAttack();
        }
    }

    private IEnumerator MovementLock()
    {
        canMove = false;
        yield return new WaitForSeconds(knockbackDuration);
        canMove = true;
        rb.linearVelocity = Vector2.zero;
    }

    private void TryAttack()
    {
        if (Time.time - lastAttackTime < attackCooldown)
        {
            return;
        }

        lastAttackTime = Time.time;
        Vector2 attackDirection = moveInput != Vector2.zero ? moveInput.normalized : lastMoveDirection;
        hits = Physics2D.CircleCastAll(PlayerTransform.position, attackRadius, attackDirection, attackRange, attackableLayer);

        Debug.Log($"Raycast performed in direction {attackDirection}. Hit count: {hits.Length}");

        for (int i = 0; i < hits.Length; i++)
        {
            Debug.Log($"Hit enemy: {hits[i].collider.name}");
            var enemy = hits[i].collider.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(attackDamage);
            }
        }

        // animator.SetTrigger("Attack");
    }
}

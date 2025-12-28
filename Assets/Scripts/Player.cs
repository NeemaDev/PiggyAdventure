using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(Animator))]
public class Player : MonoBehaviour
{
    public static Transform PlayerTransform;
    public event Action<float, float> HealthChanged;
    public LayerMask attackableLayer;
    public Health health;

    private PlayerInput inputs;
    private Vector2 moveInput;
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Vector2 lastMoveDirection = Vector2.up;
    private float movementSpeed = 4f;
    // private float maxHealth = 100f;
    // private float currentHealth = 100f;
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
    private int attackDamage = 20;

    public (float current, float max) GetHealth()
    {
        return (health.CurrentHealth, health.MaxHealth);
    }

    public void TakeDamage()
    {
        Debug.Log($"Player took damage. Remaining Health: {health.CurrentHealth}");
        StartCoroutine(MovementLock());
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log($"Collision with {collision.gameObject.tag} occured!");
        if (collision.gameObject.CompareTag("Enemy"))
        {
            int dmg = (int)Math.Round(0.2 * health.MaxHealth);
            health.ChangeHealth(-dmg);
            TakeDamage();

            HealthChanged?.Invoke(health.CurrentHealth, health.MaxHealth);

            Vector2 knockbackDirection = (transform.position - collision.transform.position).normalized;
            rb.linearVelocity = Vector2.zero;
            rb.linearVelocity = knockbackDirection * knockbackForce;
        }
    }

    private void Awake()
    {
        inputs = new PlayerInput();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        PlayerTransform = transform;

        HealthChanged?.Invoke(health.CurrentHealth, health.MaxHealth);

        health.OnDamaged += TakeDamage;
        health.OnDeath += Die;

        inputs.PlayerActions.Enable();
    }

    private void OnEnable()
    {
        if (inputs != null)
        {
            inputs.PlayerActions.Enable();
        }
    }

    private void OnDisable()
    {
        inputs.PlayerActions.Disable();
        health.OnDamaged -= TakeDamage;
        health.OnDeath -= Die;
    }

    private void Update()
    {
        moveInput = inputs.PlayerActions.Movement.ReadValue<Vector2>();

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

    private void FixedUpdate()
    {
        if (!canMove)
        {
            return;
        }

        Vector2 movement = moveInput * movementSpeed * Time.deltaTime;
        rb.MovePosition(rb.position + movement);
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
                enemy.gameObject.GetComponent<Health>().ChangeHealth(-attackDamage);
                enemy.TakeDamage();
            }
        }

        // animator.SetTrigger("Attack");
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}

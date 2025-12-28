using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public LayerMask playerLayer;
    public Health health;
    private float detectionRange = 2f;
    [SerializeField]
    private float minDistanceToPlayer = 0.5f;
    private Vector2 spawnPosition = new Vector2(5, 3);
    private Rigidbody2D rb;
    private NavMeshAgent agent;
    private Animator animator;
    private bool canMove = true;
    private bool isMoving = true;
    private bool canAttack = true;
    [SerializeField]
    private float attackRange = 0.6f;
    [SerializeField]
    private float attackCooldown = 1f;
    private float lastAttackTime;
    [SerializeField]
    private int attackDamage = 20;

    public void TakeDamage()
    {
        StartCoroutine(DamageStun());
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        health.OnDamaged += TakeDamage;
        health.OnDeath += Die;

        // currentHealth = maxHealth;
    }

    private void OnDisable()
    {
        health.OnDamaged -= TakeDamage;
    }

    // Update is called once per frame
    private void Update()
    {
        if (!canMove && !canAttack)
        {
            isMoving = false;
            return;
        }

        float distanceToSpawn = Vector2.Distance(transform.position, spawnPosition);
        float distanceToPlayer = Vector2.Distance(transform.position, Player.PlayerTransform.position);

        // If close to spawn, stop moving and idle.
        if(distanceToSpawn <= 0.5f && !playerInRange())
        {
            agent.ResetPath();
            isMoving = false;
        }
        // Return to spawn, when far and player not in range.
        else if (distanceToSpawn > 0.5f && !playerInRange())
        {
            isMoving = true;
            agent.SetDestination(spawnPosition);
        }
        // If player in range, chase player.
        else if(playerInRange() && canMove)
        {
            isMoving = true;
            agent.SetDestination(Player.PlayerTransform.position);
        }

        // Attach if in range.
        if (distanceToPlayer <= attackRange && Time.time - lastAttackTime > attackCooldown && canAttack)
        {
            lastAttackTime = Time.time;
            AttackPlayer();
        }

        UpdateAnimator();
    }

    private bool playerInRange()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, Player.PlayerTransform.position);
        if (distanceToPlayer > minDistanceToPlayer && distanceToPlayer < detectionRange)
        {
            return true;
        }

        return false;
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    private IEnumerator DamageStun()
    {
        canMove = false;
        canAttack = false;
        Debug.Log("Enemy Stunned");
        yield return new WaitForSeconds(1.5f);
        canAttack = true;
        canMove = true;
        rb.linearVelocity = Vector2.zero;
    }

    private void AttackPlayer()
    {
        Debug.Log("Enemy attacks player!");
        if (Time.time - lastAttackTime < attackCooldown)
        {
            return;
        }

        lastAttackTime = Time.time;

        Vector2 direction = (Player.PlayerTransform.position - transform.position).normalized;
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, 0.3f, direction, attackRange, playerLayer);

        if (hit.collider != null)
        {
            Debug.Log($"Hit player: {hit.collider.name}");
            var player = hit.collider.GetComponent<Player>();
            if (player != null)
            {
                player.gameObject.GetComponent<Health>().ChangeHealth(-attackDamage);
                player.TakeDamage();
            }

        }
    }

    private void UpdateAnimator()
    {
        Vector2 moveVelocity = new Vector2(agent.velocity.x, agent.velocity.y);

        if (moveVelocity.magnitude < 0.01f)
        {
            moveVelocity = Vector2.zero;
        }

        animator.SetFloat("moveX", moveVelocity.x);
        animator.SetFloat("moveY", moveVelocity.y);
        animator.SetBool("isMoving", isMoving);
    }

}

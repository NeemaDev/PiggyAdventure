using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    private List<Vector2> _directions = new List<Vector2>() { Vector2.up, Vector2.down, Vector2.left, Vector2.right };
    private Vector2 _spawn;
    private Vector2 _turnAroundPoint;
    private EnemyStats _stats;
    private IEnemyState currentState;

    public LayerMask WallLayer;
    public Vector2 DestinationPoint => _turnAroundPoint;
    public Vector2 Spawn => _spawn;
    public float Speed => _stats.Speed;

    private void Awake()
    {
        _stats = GetComponentInParent<EnemyStats>();
    }

    private void Start()
    {
        _spawn = gameObject.transform.position;
        CalculateTarget(_spawn);

        ChangeState(new PatrolState());
    }

    private void Update()
    {
        currentState?.Execute(this);
    }

    private void CalculateTarget(Vector2 spawn)
    {
        Vector2 bestTarget = Vector2.zero;
        float bestTargetDistance = 0f;

        foreach (Vector2 direction in _directions)
        {
            RaycastHit2D hit = Physics2D.Raycast(spawn, direction, Mathf.Infinity, WallLayer);

            if (hit.distance > bestTargetDistance)
            {
                Debug.Log($"Best target: {hit.point}");
                bestTarget = hit.point + (hit.normal * 0.5f);
                bestTargetDistance = hit.distance;
                Debug.Log($"Set distance {hit.distance} and taget: {bestTarget}");
            }
        }

        _turnAroundPoint = bestTarget;
    }

    private void ChangeState(IEnemyState state)
    {
        currentState?.Exit(this);
        currentState = state;
        currentState?.Enter(this);
    }
}

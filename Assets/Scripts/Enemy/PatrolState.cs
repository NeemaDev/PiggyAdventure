using UnityEngine;

public class PatrolState : IEnemyState
{
    private Vector2 _currentTarget;

    private bool _isWaiting = false;
    private float _waitTimer = 0f;
    private readonly float _waitDuration = 0.5f;

    public void Enter(EnemyPatrol enemy)
    {
        _currentTarget = enemy.DestinationPoint;
        _isWaiting = false;
    }

    public void Execute(EnemyPatrol enemy)
    {
        if (_isWaiting)
        {
            _waitTimer -= Time.deltaTime;
            if(_waitTimer <= 0f)
            {
                _currentTarget = (_currentTarget == enemy.DestinationPoint) ? enemy.Spawn : enemy.DestinationPoint;
                _isWaiting = false;
            }

            return;
        }

        // Move towards the target point.
        enemy.transform.position = Vector2.MoveTowards(enemy.transform.position, _currentTarget, enemy.Speed * Time.deltaTime);

        // Check if arrived.
        if(Vector2.Distance(enemy.transform.position, _currentTarget) < 0.05f)
        {
            _isWaiting = true;
            _waitTimer = _waitDuration;
        }
    }

    public void Exit(EnemyPatrol enemy) {}
}

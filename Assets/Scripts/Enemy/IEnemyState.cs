using UnityEngine;

public interface IEnemyState
{
    void Enter(EnemyPatrol enemy);
    void Execute(EnemyPatrol enemy);
    void Exit(EnemyPatrol enemy);
}

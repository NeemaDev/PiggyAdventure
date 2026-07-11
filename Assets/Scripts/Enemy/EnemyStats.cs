using System;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    [SerializeField] private float baseSpeed = 2f;
    [SerializeField] private int _enemyLevel = 1;
    [SerializeField] private float _braveryDrain = 10f;
    
    public int EnemyLevel => _enemyLevel;
    public float Speed => EnemyLevel * baseSpeed;
    public float BraveryDrain => _braveryDrain;
}

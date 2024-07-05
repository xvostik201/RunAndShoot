
using UnityEngine;
[CreateAssetMenu(fileName = "NewEnemyBotData", menuName = "ScriptableObject/EnemyBotData")]
public class EnemyBotData : ScriptableObject
{
    public int Health => _health;
    public float TimeToDestroy => _timeToDestroy;

    [SerializeField] private int _health = 10;
    [SerializeField] private float _timeToDestroy = 5f;
}
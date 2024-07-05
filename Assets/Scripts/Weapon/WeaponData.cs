using UnityEngine;

[CreateAssetMenu(fileName = "NewWeaponData", menuName = "ScriptableObject/WeaponData")]
public class WeaponData : ScriptableObject
{
    public float ShootRate => _shootRate;
    public float ShootForce => _shootForce;
    public float DeathForce => _deathForce;
    public int Damage => _damage;
    public int PoolSize => poolSize;
    public GameObject Bullet => _bullet;

    public AudioClip ShootSound => _shootSound;

    [SerializeField] private float _shootRate = 1f;
    [SerializeField] private float _shootForce = 15f;
    [SerializeField] private float _deathForce = 15f;
    [SerializeField] private int _damage = 1;
    [SerializeField] private int poolSize = 20;

    [SerializeField] private GameObject _bullet;
    [SerializeField] private AudioClip _shootSound;
}
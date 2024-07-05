using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody _rb;
    private int _damage;
    private float _deathForce;
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }
    public void Shoot(float bulletForce, int damage, float deathForce)
    {
        _rb.AddForce(bulletForce * transform.forward, ForceMode.Impulse);
        _damage = damage;
        _deathForce = deathForce;
    }

    private void OnTriggerEnter(Collider other)
    {
        EnemyBot enemyBot = other.GetComponentInParent<EnemyBot>();
        if(enemyBot != null && enemyBot.enabled)
        {
            enemyBot.TakeDamage(_damage, transform.position, _deathForce);
        }
        gameObject.SetActive(false);
    }
}
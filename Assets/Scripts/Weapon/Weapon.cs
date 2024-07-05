using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private WeaponData _weaponData;
    [SerializeField] private Transform _shootPoint;
    

    private Queue<GameObject> bulletPool;
    private float _shootRateTimer;

    private void Start()
    {
        _shootRateTimer = _weaponData.ShootRate;
        InitializeBulletPool();
    }

    void Update()
    {
        ShootRateTimer();
    }

    private void ShootRateTimer()
    {
        _shootRateTimer += Time.deltaTime;
    }

    private void InitializeBulletPool()
    {
        bulletPool = new Queue<GameObject>();

        for (int i = 0; i < _weaponData.PoolSize; i++)
        {
            GameObject bulletObj = Instantiate(_weaponData.Bullet);
            bulletObj.SetActive(false);
            bulletPool.Enqueue(bulletObj);
        }
    }

    private GameObject GetBulletFromPool()
    {
        GameObject bulletObj = bulletPool.Dequeue();
        bulletObj.SetActive(true);
        bulletPool.Enqueue(bulletObj);
        return bulletObj;
    }

    public void Shoot()
    {
        if (_shootRateTimer >= _weaponData.ShootRate)
        {
            _shootRateTimer = 0f;

            Vector3 tapPosition = Input.mousePosition;
            Ray ray = Camera.main.ScreenPointToRay(tapPosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Vector3 hitPoint = hit.point;
                Vector3 direction = (hitPoint - _shootPoint.position).normalized;

                _shootPoint.LookAt(hitPoint);

                GameObject bulletObj = GetBulletFromPool();
                bulletObj.GetComponent<Rigidbody>().velocity = Vector3.zero;
                bulletObj.transform.position = _shootPoint.position;
                bulletObj.transform.rotation = Quaternion.LookRotation(direction);

                Bullet bullet = bulletObj.GetComponent<Bullet>();
                bullet.Shoot(_weaponData.ShootForce, _weaponData.Damage, _weaponData.DeathForce);
                AudioManager.Instance.PlaySFX(_weaponData.ShootSound);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon
{
    private float _fireRate;
    public bool IsAutomatic
    {
        get
        {
            return _isAutomatic;
        }
    }
    private bool _isAutomatic;
    public float BulletSpeed
    {
        get
        {
            return _bulletSpeed;
        }
    }
    private float _bulletSpeed;
    private float _canShoot;

    public Weapon(WeaponData data)
    {
        _fireRate = data.fireRate;
        _isAutomatic = data.isAutomatic;
        _bulletSpeed = data.bulletSpeed;
    }

    public void Shoot(Vector3 hitRayStart, Vector3 muzzlePosition, Vector3 direction)
    {
        if (Time.time < _canShoot)
            return;
        
        _canShoot = Time.time + 1 / _fireRate;

        if (Physics.Raycast(hitRayStart,direction , out RaycastHit hit, Mathf.Infinity))
        {
            BulletManager.instance.SpawnBullet(muzzlePosition, hit.point - muzzlePosition, _bulletSpeed);
        }
        else
        {
            BulletManager.instance.SpawnBullet(muzzlePosition, direction.normalized * 100 - muzzlePosition, _bulletSpeed);
        }
    }
    public void Shoot(Vector3 muzzlePosition, Vector3[] pathPoints)
    {
        if (Time.time < _canShoot)
            return;

        _canShoot = Time.time + 1 / _fireRate;
        
        BulletManager.instance.SpawnBullet(muzzlePosition, pathPoints, _bulletSpeed);
    }
}

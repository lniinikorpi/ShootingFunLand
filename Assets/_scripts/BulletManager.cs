using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    class Bullet
    {
        private Vector3 _direction;
        public Vector3 Direction
        {
            get
            {
                return _direction;
            }
            set
            {
                _direction = value.normalized;
            }
        }
        private Vector3 _currentPosition;
        public Vector3 currentPosition
        {
            get
            {
                return _currentPosition;
            }
            set
            {
                _currentPosition = value;
            }
        }
        private float _speed;
        public float Speed
        {
            get
            {
                return _speed;
            }
            set
            {
                _speed = value;
            }
        }
        private Vector3[] _pathPoints;
        public Vector3[] PathPoints
        {
            get
            {
                return _pathPoints;
            }
        }
        private int _pathIndex;
        public int PathIndex
        {
            get
            {
                return _pathIndex;
            }
            set
            {
                _pathIndex = value;
            }
        }
        
        public Bullet(Vector3 currentPosition, Vector3 direction, float speed)
        {
            _currentPosition = currentPosition;
            _direction = direction.normalized;
            _speed = speed;
        }
        public Bullet(Vector3 currentPosition, Vector3[] pathPoints, float speed)
        {
            _currentPosition = currentPosition;
            _pathPoints = pathPoints;
            _speed = speed;
        }
    }

    public static BulletManager instance = null;
    private List<Bullet> _bullets = new List<Bullet>();
    private ParticleSystem _bulletVisuals;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(gameObject);
        }
        _bulletVisuals = GetComponentInChildren<ParticleSystem>();
    }

    private void Update()
    {
        if(_bullets.Count > 0)
        {
            UpdateBulletPositions();
            UpdateBulletVisuals();
        }
    }

    public void SpawnBullet(Vector3 start, Vector3 direction, float speed)
    {
        Bullet bullet = new Bullet(start, direction, speed);
        _bullets.Add(bullet);
    }
    public void SpawnBullet(Vector3 start, Vector3[] pathPoints, float speed)
    {
        Bullet bullet = new Bullet(start, pathPoints, speed);
        _bullets.Add(bullet);
    }

    void UpdateBulletPositions()
    {
        List<Bullet> tempBullets = new List<Bullet>(_bullets);
        foreach (Bullet b in tempBullets)
        {
            if(!CheckBulletHit(b)) {
                if (b.PathPoints != null)
                {
                    if (Vector3.Distance(b.currentPosition, b.PathPoints[b.PathIndex + 1]) < .1f)
                    {
                        if(b.PathIndex < b.PathPoints.Length - 2)
                        {
                            b.PathIndex++;
                        }
                        else
                        {
                            _bullets.Remove(b);
                        }
                    }

                    b.Direction = b.PathPoints[b.PathIndex + 1] - b.currentPosition;
                }
                b.currentPosition += b.Speed * Time.deltaTime * b.Direction;
            }
            else
            {
                _bullets.Remove(b);
            }
        }
    }

    void UpdateBulletVisuals()
    {
        ParticleSystem.Particle[] particles = new ParticleSystem.Particle[_bullets.Count];
        for(int i = 0; i < _bullets.Count; i ++)
        {
            ParticleSystem.Particle particle = new ParticleSystem.Particle
            {
                position = _bullets[i].currentPosition,
                startColor = Color.red,
                startLifetime = 100,
                startSize3D = Vector3.one,
                remainingLifetime = 100
            };
            particles[i] = particle;
        }

        _bulletVisuals.SetParticles(particles);
    }

    bool CheckBulletHit(Bullet bullet)
    {
        if (Physics.Raycast(bullet.currentPosition, bullet.Direction, out RaycastHit hit, bullet.Speed * Time.deltaTime))
        {
            return true;
        }
        return false;
    }

   /* private void OnDrawGizmos()
    {
        foreach (Bullet b in _bullets)
        {
            Gizmos.DrawLine(b.currentPosition, b.currentPosition + b.Direction * b.Speed * 10);
        }
    }*/
}

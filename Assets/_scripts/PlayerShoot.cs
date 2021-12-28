using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShoot : MonoBehaviour
{
    private Player _player;
    private bool _isFiring = false;
    private bool _targetSet = false;
    private Weapon _currentWeapon;
    private Camera _camera;
    [SerializeField]
    private Transform _muzzle;
    private Vector3 _target;
    [SerializeField]
    private LineRenderer _targetLine;
    [SerializeField]
    private BezierCurve _bezierCurve;
    Vector3 middlePoint;

    private void Awake()
    {
        WeaponData data = Resources.Load<WeaponData>("Pistol");
        _currentWeapon = new Weapon(data);
        _camera = Camera.main;
    }

    void Start()
    {
        _player = GetComponent<Player>();
        _player.GetPlayerInput().Player.Shoot.performed += DoFire;
        _player.GetPlayerInput().Player.Shoot.Enable();
        _player.GetPlayerInput().Player.UnShoot.performed += DoUnFire;
        _player.GetPlayerInput().Player.UnShoot.Enable();
        _player.GetPlayerInput().Player.SetTarget.performed += SetTarget;
        _player.GetPlayerInput().Player.SetTarget.Enable();
        _bezierCurve.lineRenderer = _targetLine;
    }

    // Update is called once per frame
    void Update()
    {
        if(_isFiring)
        {
            HandleFire();
        }
        if(_targetSet)
        {
            _bezierCurve.point1 = _muzzle.position;
            Vector3 endPoint = _muzzle.forward.normalized * Vector3.Distance(_muzzle.position, _target);
            middlePoint = new Vector3((_muzzle.position.x + endPoint.x) / 2, (_muzzle.position.y + endPoint.y) / 2, (_muzzle.position.z + endPoint.z) / 2);
            _bezierCurve.point2 = middlePoint;
        }
    }

    void DoFire(InputAction.CallbackContext obj)
    {
        _isFiring = true;
    }

    void DoUnFire(InputAction.CallbackContext obj)
    {
        _isFiring = false;
    }

    void HandleFire()
    {
        if (!_targetSet)
        {
            _currentWeapon.Shoot(_camera.transform.position, _muzzle.position, _camera.transform.forward); 
        }
        else
        {
            Vector3[] positions = new Vector3[_targetLine.positionCount];
            _targetLine.GetPositions(positions);
            _currentWeapon.Shoot(_camera.transform.position, positions);
        }
        if (!_currentWeapon.IsAutomatic)
        {
            _isFiring = false;
        }
    }

    void SetTarget(InputAction.CallbackContext obj)
    {
        _targetSet = !_targetSet;
        _targetLine.enabled = _targetSet;
        if(_targetSet)
        {
            if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out RaycastHit hit, Mathf.Infinity))
            {
                _target = hit.point;
                _bezierCurve.point3 = _target;
            }
        }
        else
        {
            _bezierCurve.point1 = Vector3.zero;
            _bezierCurve.point2 = Vector3.zero;
            _bezierCurve.point3 = Vector3.zero;
        }
    }
}

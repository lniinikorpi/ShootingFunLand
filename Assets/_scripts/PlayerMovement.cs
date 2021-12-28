using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float _movementSpeed = 10;
    [SerializeField]
    private float _lookSpeed = .1f;
    private InputAction _movementAction;
    private InputAction _lookAction;
    private Player _player;
    private Vector3 _movement;
    private Rigidbody _rb;
    private Camera _camera;
    void Start()
    {
        _player = GetComponent<Player>();
        _movementAction = _player.GetPlayerInput().Player.Move;
        _movementAction.Enable();
        _lookAction = _player.GetPlayerInput().Player.Look;
        _lookAction.Enable();
        _camera = Camera.main;

        _rb = _player.GetRigidbody();
    }

    private void Update()
    {
        HandleLook();
        HandleMovement();
    }

    void HandleMovement()
    {
        Vector2 movement = _movementAction.ReadValue<Vector2>();
        _movement = new Vector3(movement.x,0 , movement.y);
        _rb.velocity = transform.TransformDirection(_movement) * _movementSpeed;
    }

    void HandleLook()
    {
        Vector2 mouseDelta = _lookAction.ReadValue<Vector2>();
        Vector3 mouseMovement = new Vector3(-mouseDelta.y, 0);
        _camera.transform.localEulerAngles += mouseMovement * _lookSpeed;
        transform.localEulerAngles += new Vector3(0, mouseDelta.x) * _lookSpeed;
    }
}

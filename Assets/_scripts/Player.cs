using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private PlayerInput m_playerInput;
    private bool m_mouseIslocked = true;
    private Rigidbody m_rb;
    private void Awake()
    {
        m_playerInput = new PlayerInput();
        m_rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        m_playerInput.Player.FreeMouse.performed += FreeMouse;
        m_playerInput.Player.FreeMouse.Enable();
    }

    public PlayerInput GetPlayerInput()
    {
        return m_playerInput;
    }

    public Rigidbody GetRigidbody()
    {
        return m_rb;
    }

    private void FreeMouse(InputAction.CallbackContext obj)
    {
        m_mouseIslocked = !m_mouseIslocked;
        if (m_mouseIslocked)
            Cursor.lockState = CursorLockMode.Locked;
        else
            Cursor.lockState = CursorLockMode.None;
    }
}

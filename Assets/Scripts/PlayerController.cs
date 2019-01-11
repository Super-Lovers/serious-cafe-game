using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	private Camera _playerCamera;
	public float CameraSensitivity = 2f;

	private CharacterController _charController;
	public float MovementSpeed = 0.05f;
	
	// If we define this in the update function, it wont
	// store the previous state from the frame before and it
	// will keep returning the camera x position to 0.
	private float _mouseX = 0f;
	private float _mouseY = 0f;

    private GameObject _buttonsController;
    public static bool _isEscapeButtonPressed = false;

	void Start ()
	{
		_playerCamera = GetComponentInChildren<Camera>();
		_charController = GetComponent<CharacterController>();
        _buttonsController = GameObject.FindGameObjectWithTag("ButtonsController");
	}
	
	void Update ()
	{
		if (_isEscapeButtonPressed == false)
        {
            // The played must only move to the sides
            _charController.Move(new Vector3(Input.GetAxis("Horizontal") * MovementSpeed, 0, 0));

            // The mouse cursor has to be centered on the screen so that he can interact
            // with objects by looking at them first.
            Cursor.lockState = CursorLockMode.Locked;

            // Here we change AND store the value of the x position, so that
            // we can change it dynamically and use the stored values so when we
            // move the camera, it moves relative to its last stored position.
            _mouseX += Input.GetAxis("Mouse X");
            // Plus or Minus means the direction of the mouse that its going to increment
            // as its value relative to the movement position on the screen.
            _mouseY -= Input.GetAxis("Mouse Y");
            float clampedHorizontalRotation = Mathf.Clamp(_mouseX * CameraSensitivity, -70, 70);
            float clampedVerticalRotation = Mathf.Clamp(_mouseY * CameraSensitivity, -50, 50);

            _playerCamera.transform.localEulerAngles = new Vector3(clampedVerticalRotation, clampedHorizontalRotation, 0);
        } else
        {
            Cursor.lockState = CursorLockMode.None;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _buttonsController.GetComponent<ButtonController>().ToggleEscapeMenu();
        }
    }

	private void OnGUI()
	{
		int labelSize = 50;
		int positionX = _playerCamera.pixelWidth / 2;
		int positionY = _playerCamera.pixelHeight / 2;
		
		GUI.Label(new Rect(positionX, positionY, labelSize, labelSize), "o");
	}
}

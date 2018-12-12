using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiButtonBehaviour : MonoBehaviour {
	private Camera _playerCamera;
	public LayerMask UiButtonsLayer;
	private RaycastHit _hit;
	private bool _isHovering = false;
	
	void Start () {
		_playerCamera = GetComponentInChildren<Camera>();
	}
	
	void Update () {
		Ray ray = new Ray(_playerCamera.transform.position, _playerCamera.transform.forward);

		if (Physics.Raycast(ray, out _hit, 100, UiButtonsLayer))
		{
			Image image = _hit.transform.GetComponent<Image>();
			if (Input.GetMouseButton(0))
			{
				image.color = Color.green;
			}
			else
			{
				image.color = Color.gray;
				_hit.transform.tag = "Hovered";
				_isHovering = true;
			}
		}
		else
		{
			_isHovering = false;
		}

		if (_isHovering == false)
		{
			GameObject[] hoveredObjects = GameObject.FindGameObjectsWithTag("Hovered");

			foreach (GameObject hoveredObject in hoveredObjects)
			{
				hoveredObject.transform.GetComponent<Image>().color = Color.white;
			}
		}
	}
}

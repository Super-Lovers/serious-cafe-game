using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayShooter : MonoBehaviour
{
	private Camera _playerCamera;
	public LayerMask InteractablesLayer;

	public Material HoveredButton;
	public Material ClickedButton;
	public static bool EnteredButton = false;
	private bool _isButtonClicked = false;
	
	void Start ()
	{
		_playerCamera = GetComponentInChildren<Camera>();
	}
	
	void Update () {
		// Shotting a ray from the center of the camera, where the player is looking at
		Ray ray = new Ray(_playerCamera.transform.position, _playerCamera.transform.forward);
		RaycastHit hit;

		if (Physics.Raycast(ray, out hit, InteractablesLayer))
		{
			MeshRenderer interactableRenderer = hit.transform.GetComponent<MeshRenderer>();
			
			if (hit.transform.name == "Next Channel" && _isButtonClicked == false)
			{
				interactableRenderer.material = HoveredButton;
				EnteredButton = true;
			}
			else
			{
				EnteredButton = false;
			}
			
			if (hit.transform.name == "Previous Channel" && _isButtonClicked == false)
			{
				interactableRenderer.material = HoveredButton;
				EnteredButton = true;
			}
			else
			{
				EnteredButton = false;
			}

			if (Input.GetMouseButton(0) &&
			    (hit.transform.name == "Previous Channel" ||
			    hit.transform.name == "Next Channel"))
			{
				interactableRenderer.material = ClickedButton;
			}
		}
	}
}

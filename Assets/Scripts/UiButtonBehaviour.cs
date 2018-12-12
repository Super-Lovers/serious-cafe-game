using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiButtonBehaviour : MonoBehaviour {
	private Camera _playerCamera;
	public LayerMask UiButtonsLayer;
	private RaycastHit _hit;
	private bool _isHovering = false;
	
	// Check if the dependencies for a single coffee order
	// are met and if not then you can keep selecting items.
	private bool _isCafeSelected = false;
	private bool _isSugarSelected = false;

	private Image _cafeSelected;
	private Image _sugarSelected;
	private Image _creamSelected;
	
	void Start () {
		_playerCamera = GetComponentInChildren<Camera>();
	}
	
	void Update () {
		Ray ray = new Ray(_playerCamera.transform.position, _playerCamera.transform.forward);

		if (Physics.Raycast(ray, out _hit, 100, UiButtonsLayer))
		{
			Image image = _hit.transform.GetComponent<Image>();
			// If the button we are hovering over is not selected, then it
			// is legally allowed to hover.
			
			// *************
			// Cafe selectors
			// *************
			if (Input.GetMouseButton(0) && _isCafeSelected == false &&
			    _hit.transform.name[_hit.transform.name.Length - 1] != 'S' &&
			    (_hit.transform.name == "Cafe 1" ||
			    _hit.transform.name == "Cafe 2" ||
			    _hit.transform.name == "Cafe 3" ||
			    _hit.transform.name == "Cafe 4"))
			{
				image.color = Color.green;
				_hit.transform.name += " S"; // Makes sure that button is (S) Selected.
				// This helps highlight the button selected
				_cafeSelected = _hit.transform.gameObject.GetComponent<Image>();
				
				_isCafeSelected = true;
			}
			else
			{
				if (_isCafeSelected == false)
				{
					image.color = Color.gray;
					_hit.transform.tag = "Hovered";
					_isHovering = true;
				}
			}
			
			// *************
			// Sugar Selectors
			// *************
			if (_isCafeSelected && Input.GetMouseButton(0) && _sugarSelected == null &&
			    _hit.transform.name[_hit.transform.name.Length - 1] != 'S' &&
			    (_hit.transform.name == "Sugar 1x" ||
			     _hit.transform.name == "Sugar 2x" ||
			     _hit.transform.name == "Sugar 3x"))
			{
				image.color = Color.green;
				_hit.transform.name += " S"; // Makes sure that button is (S) Selected.
				_sugarSelected = _hit.transform.gameObject.GetComponent<Image>();
				
				_isSugarSelected = true;
			}
			else
			{
				if (_isSugarSelected == false && _sugarSelected == null)
				{
					image.color = Color.gray;
					_hit.transform.tag = "Hovered";
					_isHovering = true;
				}
			}
			
			// *************
			// Cream Selector
			// *************
			if (_isCafeSelected && Input.GetMouseButton(0) &&
			    _creamSelected == null &&
			    _hit.transform.name[_hit.transform.name.Length - 1] != 'S' &&
			    _hit.transform.name == "Cream")
			{
				image.color = Color.green;
				_hit.transform.name += " S"; // Makes sure that button is (S) Selected.
				
				_creamSelected = _hit.transform.gameObject.GetComponent<Image>();
				_isSugarSelected = true;
			}
			else
			{
				if (_isCafeSelected)
				{
					image.color = Color.gray;
					_hit.transform.tag = "Hovered";
					
					_isHovering = true;
				}
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

		if (_cafeSelected != null)
		{
			_cafeSelected.color = Color.green;
		}
		if (_sugarSelected != null)
		{
			_sugarSelected.color = Color.green;
		}
		if (_creamSelected != null)
		{
			_creamSelected.color = Color.green;
		}
	}
}

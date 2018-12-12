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
	private bool _isRequestComplete = true;
	private float _timeToRequestCompletion = 5f;

	private Image _cafeSelected;
	private Image _sugarSelected;
	private Image _creamSelected;
	
	void Start () {
		_playerCamera = GetComponentInChildren<Camera>();
	}
	
	void Update () {
		Ray ray = new Ray(_playerCamera.transform.position, _playerCamera.transform.forward);

		if (Physics.Raycast(ray, out _hit, 100, UiButtonsLayer) && _isRequestComplete)
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
			
			// *************
			// Completing the coffee request
			// *************
			// If the players clicks to finish a request and every dependant
			// selection is complete, then we can finish the order and reset everything back.
			if (Input.GetMouseButton(0) && _hit.transform.name == "Make The Coffee Button" &&
			    _cafeSelected && _sugarSelected && _creamSelected)
			{
				// This resets the selector indicator in the name
				// of the existing selected buttons and restoring their coloring.
				GameObject _cafeButtonSelected = GameObject.Find(_cafeSelected.transform.name);
				_cafeButtonSelected.name = _cafeButtonSelected.name.Substring(0, _cafeButtonSelected.name.Length - 2);
				_cafeButtonSelected.GetComponent<Image>().color = Color.white;
				
				GameObject _sugarButtonSelected = GameObject.Find(_sugarSelected.transform.name);
				_sugarButtonSelected.name = _sugarButtonSelected.name.Substring(0, _sugarButtonSelected.name.Length - 2);
				_sugarButtonSelected.GetComponent<Image>().color = Color.white;
				
				GameObject _creamButtonSelected = GameObject.Find(_creamSelected.transform.name);
				_creamButtonSelected.name = _creamButtonSelected.name.Substring(0, _creamButtonSelected.name.Length - 2);
				_creamButtonSelected.GetComponent<Image>().color = Color.white;
				
				_isCafeSelected = false;
				_isSugarSelected = false;

				_cafeSelected = null;
				_sugarSelected = null;
				_creamSelected = null;
				
				// Begin the process of the request being processed by the machine
				_isRequestComplete = false;
				Invoke("CompletedRequest", _timeToRequestCompletion);
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

	private void CompletedRequest()
	{
		_isRequestComplete = true;
	}
}

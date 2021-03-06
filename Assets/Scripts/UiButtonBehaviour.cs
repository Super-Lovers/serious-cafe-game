﻿using System.Collections;
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
	public static bool IsRequestComplete = true;
	private float _timeToRequestCompletion = 5f;

	private Image _cafeSelected;
	private Image _sugarSelected;
	private Image _creamSelected;

    public static string NewestCoffeeName;
	
	void Start () {
		_playerCamera = GetComponentInChildren<Camera>();
	}
	
	void Update () {
		Ray ray = new Ray(_playerCamera.transform.position, _playerCamera.transform.forward);

        // ******************************************
        // This raycast is used for the hand-made coffees
        if (GameObject.FindGameObjectWithTag("Customer") != null && Physics.Raycast(ray, out _hit, 100, UiButtonsLayer) &&
            Input.GetMouseButtonDown(0))
        {
            if (_hit.collider.gameObject.name == "Serve")
            {
                GameObject currentCustomer = GameObject.FindGameObjectWithTag("Customer");
                CustomerController currentCustomerScript = currentCustomer.GetComponent<CustomerController>(); 

                //*******************************
                // Order template
                // ******************************
                // If we reach a specific dialogue message on the customer where he orders a coffee AND his dialogue text is completely loaded AND the correct order combination is met can the dialogue continue forward.
                if (currentCustomerScript.CurrentDialogueIndex == 2 &&
                    currentCustomerScript.IsDialogueLoaded &&
                    CoffeeCupsController.CupSize == "Large" &&
                    CoffeeCupsController.Base == "Coffee" &&
                    (CoffeeCupsController.PrimaryBase == "Lemon" ||
                    CoffeeCupsController.SecondaryBase == "Lemon"))
                {
                    ProcessOrderAndReset(currentCustomer);
                }

            } else if (_hit.collider.gameObject.name == "Reset")
            {
                ResetPlate();
                Destroy(_hit.collider.gameObject.transform.parent.gameObject.transform.parent.gameObject);
            }
        }

        // ******************************************
        // This raycast is used for the coffee machine
            if (Physics.Raycast(ray, out _hit, 100, UiButtonsLayer) &&
		    IsRequestComplete && CoffeeMachine.IsBasketFull == false)
		{
			Image image = _hit.transform.GetComponent<Image>();
			// If the button we are hovering over is not selected, then it
			// is legally allowed to hover.
			
			// *************
			// Cafe selectors
			// *************
			if (Input.GetMouseButton(0) && _isCafeSelected == false &&
			    _hit.transform.name[_hit.transform.name.Length - 1] != 'S' &&
			    (_hit.transform.name == "Latte" ||
			    _hit.transform.name == "Espresso" ||
			    _hit.transform.name == "Cafe 3" ||
			    _hit.transform.name == "Cafe 4"))
			{
				image.color = Color.green;
                NewestCoffeeName = _hit.transform.name;
                //Debug.Log(NewestCoffeeName);

				_hit.transform.name += " S"; // Makes sure that button is (S) Selected.
				// This helps highlight the button selected
				_cafeSelected = _hit.transform.gameObject.GetComponent<Image>();
				
				_isCafeSelected = true;
			}
			else
			{
				if (_isCafeSelected == false)
				{
					image.color = Color.blue;
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
                    image.color = Color.blue;
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
                    image.color = Color.blue;
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
				IsRequestComplete = false;
				//Invoke("CompletedRequest", _timeToRequestCompletion);
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
		IsRequestComplete = true;
	}

    private void ProcessOrderAndReset(GameObject customerToProcessOrderOf)
    {
        // If the order is to be served, then we remove the 
        // interface and transfer the coffee to the customer's rest.
        Destroy(CoffeeCupsController.SpotForNewCoffee);

        StartCoroutine(customerToProcessOrderOf.GetComponent<CustomerController>().UpdateDialogueText(
            "Thank you, friend. You've made it perfectly!"));

        GameObject newCustomerCoffee = Instantiate(CoffeeCupsController.SpotForNewCoffee.transform.GetChild(1).gameObject, GameObject.FindGameObjectWithTag("Customer").transform.GetChild(2).transform);

        // Resetting the variables that define a complete order of coffee
        // and allow the player to create a new coffee.
        GameObject.FindGameObjectWithTag("Customer").GetComponent<CustomerController>().IsOrderComplete = true;
        ResetPlate();
    }

    private void ResetPlate()
    {
        CoffeeCupsController.CupSize = string.Empty;
        CoffeeCupsController.Base = string.Empty;
        CoffeeCupsController.PrimaryBase = string.Empty;
        CoffeeCupsController.SecondaryBase = string.Empty;
        CoffeeCupsController.FinalCup = string.Empty;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomerController : MonoBehaviour
{
	public GameObject ReceiveOrderSpot;
	public GameObject EnteringRestaurantSpot;
	private Transform _playerTransform;
	private bool _canMove = true;

	private Text _speechBubble;
	private static string[] _coffeeBeverages = {"Latte", "Espresso"};

	private Rigidbody _rb;
	
	void Start ()
	{
		_rb = GetComponent<Rigidbody>();
		_speechBubble = GetComponentInChildren<Text>();
		_playerTransform = GameObject.FindWithTag("Player").transform;
		
		Vector3 currentPos = transform.position;
		currentPos = EnteringRestaurantSpot.transform.position;
		transform.position = currentPos;

		_speechBubble.text = _coffeeBeverages[Random.Range(0, 2)];
		
		// We want the speech bubble to be visible only once the customer
		// sits down and then decides on his order
		_speechBubble.transform.parent.gameObject.SetActive(false);
		//Invoke("MoveToSeat", 6f);
	}
	
	void Update () {
		if (_canMove)
		{
			// We save the new vector3 position to the transform, thats why
			// we need to update the position parameter with the new one, otherwise
			// it wont store it and update the existing one.
			transform.position = Vector3.MoveTowards(transform.position,
				CustomerGenerator.AvailableSeat.transform.position, 0.03f);
		}

		if (transform.position == CustomerGenerator.AvailableSeat.transform.position)
		{
			_canMove = false;
			_speechBubble.transform.parent.gameObject.SetActive(true);
		}
			
		if (_playerTransform)
		{
			_speechBubble.transform.Rotate(_playerTransform.rotation.eulerAngles);
			
			// For some reason the x axis of the speechbubble flips once it
			// turns to the player, so im flipipng it back...
			//Vector3 speechBubbleScale = _speechBubble.transform.localScale;
			//speechBubbleScale.x *= -1;
			//_speechBubble.transform.localScale = speechBubbleScale;
		}
	}

	// You can use this if you want to make the customers teleport
	// instead of walk to their seats.
	private void MoveToSeat()
	{
		Vector3 currentPos = transform.position;
		currentPos = CustomerGenerator.AvailableSeat.transform.position;
		transform.position = currentPos;

		// Stop moving in that direction if you are already in your seat.
		Debug.Log("Moved to seat");
	}
}

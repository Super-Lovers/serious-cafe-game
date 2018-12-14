using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerController : MonoBehaviour
{
	public GameObject ReceiveOrderSpot;
	public GameObject EnteringRestaurantSpot;
	
	void Start ()
	{
		Vector3 currentPos = transform.position;
		currentPos = EnteringRestaurantSpot.transform.position;
		transform.position = currentPos;
		
		Invoke("MoveToSeat", 2f);
	}
	
	void Update () {
		
	}

	private void MoveToSeat()
	{
		Vector3 currentPos = transform.position;
		currentPos = CustomerGenerator.AvailableSeat.transform.position;
		transform.position = currentPos;
		
		Debug.Log("Moved to seat");
	}
}

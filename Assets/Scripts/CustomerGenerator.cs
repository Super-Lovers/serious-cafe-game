using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerGenerator : MonoBehaviour {
	// The timer is responsible for generating customer
	// objects in random intervals throughout the game.
	public static int Timer = 0;
	public GameObject CustomerPrefab;
	[SerializeField] private GameObject[] _spotsForSitting;
	public static GameObject AvailableSeat;
	private bool _canCustomerSpawn = false;
	
	public AudioSource AudioSource;
	
	void Start ()
	{
		Timer = Random.Range(3, 4);
		InvokeRepeating("DecreaseTime", 0, 1);
	}
	
	void Update () {
		if (_canCustomerSpawn)
		{
			foreach (GameObject chair in _spotsForSitting)
			{
				if (chair.transform.childCount <= 2)
				{
					AvailableSeat = chair;
				}
			}
			
			Instantiate(CustomerPrefab, CustomerPrefab.transform.position, Quaternion.identity, AvailableSeat.transform);
			// Once a new customer enters the restaurant, we ring the bell
			AudioSource.PlayOneShot(AudioSource.clip);
			
			// Resetting the interval to spawn a new customer after
			// one is spawned.
			Timer += Random.Range(3, 4);
			_canCustomerSpawn = false;
		}
	}

	private void DecreaseTime()
	{
		Debug.Log(Timer);
		if (Timer > 0)
		{
			Timer -= 1;
		}
		else
		{
			_canCustomerSpawn = true;
		}
	}
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : MonoBehaviour
{
	public GameObject[] LampLights;
	public GameObject SunRays;
	
	void Update ()
	{
		// We get the user's system date format and convert it
		// to a 24 hour format so that the code can work with it better.
		string systemDateFormat = DateTime.Now.ToString("HH");
		int currentHour24 = Int32.Parse(systemDateFormat);
		
		// Updating the light rays and lamps depending on the time of day
		if (currentHour24 > 8 && currentHour24 < 18) // Day Time
		{
			foreach (GameObject lamp in LampLights)
			{
				lamp.SetActive(false);
			}
			
			SunRays.GetComponent<Light>().color = new Color32(0x0FF, 0x0C4, 0x070, 0x00);
		}
		else // Night Time
		{
			foreach (GameObject lamp in LampLights)
			{
				lamp.SetActive(true);
			}
			
			SunRays.GetComponent<Light>().color = new Color32(0x000, 0x009, 0x076, 0x00);
		}

	}
}

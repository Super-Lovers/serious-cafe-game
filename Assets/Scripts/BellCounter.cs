using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BellCounter : MonoBehaviour
{
	private AudioSource _audioSource;

	private void Start()
	{
		_audioSource = GetComponent<AudioSource>();
	}

	void Update () {
		if (Input.GetKeyDown(KeyCode.B))
		{
			_audioSource.PlayOneShot(_audioSource.clip);
		}
	}
}

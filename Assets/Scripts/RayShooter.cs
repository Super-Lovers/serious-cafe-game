using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RayShooter : MonoBehaviour
{
	private Camera _playerCamera;
	public LayerMask InteractablesLayer;

	public Material HoveredButton;
	public Material ClickedButton;
	public static bool EnteredButton = false;
	private bool _isButtonClicked = false;

	public Text RadioLabel;
	private int _currentRadioIndex = 0;
	private int _currentSongIndex = 0;
	private readonly string[] _radioChannels = {"Channel 1", "Channel 2"};
	public AudioSource RadioAudioSource;
	public AudioClip[] Channel1Music;
	public AudioClip[] Channel2Music;
	
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

			if (hit.transform.name == "Previous Channel" ||
			    hit.transform.name == "Next Channel" ||
			    hit.transform.name == "Volume Down" ||
			    hit.transform.name == "Volume Up" && _isButtonClicked == false)
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
				 hit.transform.name == "Next Channel" ||
			     hit.transform.name == "Volume Down" ||
			     hit.transform.name == "Volume Up"))
			{
				interactableRenderer.material = ClickedButton;
				_currentSongIndex = 0;
			}

			// **************
			// Radio Controls
			// **************
			if (Input.GetMouseButtonDown(0) &&
			    hit.transform.name == "Next Channel")
			{
				if (_currentRadioIndex < 1)
				{
					_currentRadioIndex++;
				}
				RadioLabel.text = _radioChannels[_currentRadioIndex];
				
				if (_radioChannels[_currentRadioIndex] == "Channel 1")
				{
					RadioAudioSource.clip = Channel1Music[_currentSongIndex];
				} else if (_radioChannels[_currentRadioIndex] == "Channel 2")
				{
					RadioAudioSource.clip = Channel2Music[_currentSongIndex];
				}
				RadioAudioSource.Play();
			} else if (Input.GetMouseButtonDown(0) &&
			           hit.transform.name == "Previous Channel")
			{
				if (_currentRadioIndex > 0)
				{
					_currentRadioIndex--;
				}
				RadioLabel.text = _radioChannels[_currentRadioIndex];
				
				if (_radioChannels[_currentRadioIndex] == "Channel 1")
				{
					RadioAudioSource.clip = Channel1Music[_currentSongIndex];
				} else if (_radioChannels[_currentRadioIndex] == "Channel 2")
				{
					RadioAudioSource.clip = Channel2Music[_currentSongIndex];
				}
				RadioAudioSource.Play();
			} else if (Input.GetMouseButtonDown(0) &&
			           hit.transform.name == "Volume Down" &&
			           RadioAudioSource.volume > 0.0f)
			{
				RadioAudioSource.volume -= 0.1f;
			} else if (Input.GetMouseButtonDown(0) &&
			           hit.transform.name == "Volume Up" &&
			           RadioAudioSource.volume <= 1f)
			{
				RadioAudioSource.volume += 0.1f;
			}
		}

		if (RadioAudioSource.isPlaying == false)
		{
			// This makes sure we dont play a song in the array
			// that does not exist
			if (_currentSongIndex == 1)
			{
				_currentSongIndex--;
			} else if (_currentRadioIndex >= 0)
			{
				_currentSongIndex++;
			}
				
			// This changes to the next song if the current one is done
			// playing relative to the channel as well.
			if (_radioChannels[_currentRadioIndex] == "Channel 1")
			{
				RadioAudioSource.clip = Channel1Music[_currentSongIndex];
				Debug.Log("Current song: " + Channel1Music[_currentSongIndex]);
			} else if (_radioChannels[_currentRadioIndex] == "Channel 2")
			{
				RadioAudioSource.clip = Channel2Music[_currentSongIndex];
				Debug.Log("Current song: " + Channel2Music[_currentSongIndex]);
			}
			RadioAudioSource.Play();
		}

		// Used for testing if the clip is properly started once
		// the current one is complete
		if (Input.GetKeyDown(KeyCode.K))
		{
			RadioAudioSource.clip.UnloadAudioData();
		}
	}
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class RayShooter : MonoBehaviour
{
	private Camera _playerCamera;
	public LayerMask InteractablesLayer;
	public Material DefaultButtonMaterial;

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
	private List<int> _playedSongsIndexes = new List<int>();
	
	void Start ()
	{
		_playerCamera = GetComponentInChildren<Camera>();
	}
	
	void Update () {
		// Shotting a ray from the center of the camera, where the player is looking at
		Ray ray = new Ray(_playerCamera.transform.position, _playerCamera.transform.forward);
		RaycastHit hit;

		if (Physics.Raycast(ray, out hit, 100, InteractablesLayer))
		{
			MeshRenderer interactableRenderer = hit.transform.GetComponent<MeshRenderer>();

			if (hit.transform.name == "Previous Channel" ||
			    hit.transform.name == "Next Channel" ||
			    hit.transform.name == "Volume Down" ||
			    hit.transform.name == "Volume Up" && _isButtonClicked == false)
			{
				hit.transform.GetComponent<ButtonController>().HighlightButton();
			}

            if (Input.GetMouseButtonDown(0) && hit.transform.name == "NextDialogue")
            {
                hit.transform.GetComponentInParent<CustomerController>().UpdateDialogueIndex();
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
				int newSongIndex = Random.Range(0, 2);
				// If the list of played songs in the channel is full
				// (every song is already played once) then we reset it.
				if (_playedSongsIndexes.Count >= 2)
				{
					_playedSongsIndexes.Clear();
					//Debug.Log("Cleared List");
				}
				// If this song in the channel has already been played
				// then we want to pick another one instead of the same one
				// multiple times in a row...
				while (_playedSongsIndexes.Contains(newSongIndex))
				{
					newSongIndex = Random.Range(0, 2);
				}
				_playedSongsIndexes.Add(newSongIndex);
				
				if (_currentRadioIndex < 1)
				{
					_currentRadioIndex++;
				}
				RadioLabel.text = _radioChannels[_currentRadioIndex];
				
				if (_radioChannels[_currentRadioIndex] == "Channel 1")
				{
					RadioAudioSource.clip = Channel1Music[newSongIndex];
				} else if (_radioChannels[_currentRadioIndex] == "Channel 2")
				{
					RadioAudioSource.clip = Channel2Music[newSongIndex];
				}
				RadioAudioSource.Play();
			} else if (Input.GetMouseButtonDown(0) &&
			           hit.transform.name == "Previous Channel")
			{
				int newSongIndex = Random.Range(0, 2);
				if (_playedSongsIndexes.Count >= 2)
				{
					_playedSongsIndexes.Clear();
					//Debug.Log("Cleared List");
				}
				while (_playedSongsIndexes.Contains(newSongIndex))
				{
					newSongIndex = Random.Range(0, 2);
				}
				_playedSongsIndexes.Add(newSongIndex);
				
				if (_currentRadioIndex > 0)
				{
					_currentRadioIndex--;
				}
				RadioLabel.text = _radioChannels[_currentRadioIndex];
				
				if (_radioChannels[_currentRadioIndex] == "Channel 1")
				{
					RadioAudioSource.clip = Channel1Music[newSongIndex];
				} else if (_radioChannels[_currentRadioIndex] == "Channel 2")
				{
					RadioAudioSource.clip = Channel2Music[newSongIndex];
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
		else
		{
			List<GameObject> buttons = new List<GameObject>();
			buttons.Add(GameObject.Find("Next Channel"));
			buttons.Add(GameObject.Find("Previous Channel"));
			buttons.Add(GameObject.Find("Volume Up"));
			buttons.Add(GameObject.Find("Volume Down"));

			foreach (GameObject button in buttons)
			{
				button.GetComponent<MeshRenderer>().material = DefaultButtonMaterial;
			}
		}

		if (RadioAudioSource.isPlaying == false)
		{
			// This makes sure we dont play a song in the array
			// that does not exist
			int newSongIndex = Random.Range(0, 2);
			if (_playedSongsIndexes.Count >= 2)
			{
				_playedSongsIndexes.Clear();
				//Debug.Log("Cleared List");
			}
			while (_playedSongsIndexes.Contains(newSongIndex))
			{
				newSongIndex = Random.Range(0, 2);
			}
			
			if (newSongIndex == 1)
			{
				_currentSongIndex--;
			} else if (newSongIndex >= 0)
			{
				_currentSongIndex++;
			}
				
			// This changes to the next song if the current one is done
			// playing relative to the channel as well.
			if (_radioChannels[_currentRadioIndex] == "Channel 1")
			{
				RadioAudioSource.clip = Channel1Music[_currentSongIndex];
				//Debug.Log("Current song: " + Channel1Music[_currentSongIndex]);
			} else if (_radioChannels[_currentRadioIndex] == "Channel 2")
			{
				RadioAudioSource.clip = Channel2Music[_currentSongIndex];
				//Debug.Log("Current song: " + Channel2Music[_currentSongIndex]);
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StoryController : MonoBehaviour
{
    public GameObject BackgroundColor;
    private Animator _backgroundAnimator;
    public Text Text;

    public string[] DialogueTexts;
    private int _currentDialogueIndex = 0;

    private AudioSource _audioSource;
    private bool _isTextLoaded = false;
    private bool _canPressSpace = false;

    public GameObject Notification;

	void Start () {
        _backgroundAnimator = BackgroundColor.GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
    }
	
	void Update () {
		if (_isTextLoaded && Input.GetKeyDown(KeyCode.Space)) {
            if (_currentDialogueIndex == DialogueTexts.Length - 1)
            {
                EndStorySequence();
            } else
            {
                StartCoroutine(ShowNextText());
            }
        }
	}

    public void BeginStorySequence()
    {
        StartCoroutine(ShowNextText());

        BackgroundColor.SetActive(true);
        Text.gameObject.SetActive(true);
    }

    public void EndStorySequence()
    {
        StartCoroutine(ShowNextText());
        //_backgroundAnimator.SetBool("hideBackground", true);
        //Text.gameObject.SetActive(false);
        Invoke("HideDialogueElements", 2f);
    }

    private void HideDialogueElements()
    {
        SceneManager.LoadScene("SampleScene");
    }

    private IEnumerator ShowNextText()
    {
        _isTextLoaded = false;

        if (_currentDialogueIndex > 0)
        {
            Notification.SetActive(false);
        }

        _audioSource.Play();

        string currentText = DialogueTexts[_currentDialogueIndex];
        Text.text = "";

        foreach (char symbol in currentText)
        {
            yield return new WaitForSeconds(0.02f);
            Text.text += symbol;
            yield return new WaitForSeconds(0.02f);
        }

        if (_currentDialogueIndex == 0)
        {
            Notification.SetActive(true);
        }

        _currentDialogueIndex++;
        _isTextLoaded = true;
        _audioSource.Stop();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonController : MonoBehaviour {
    public Material HoveredButton;
    public GameObject EscapeMenu;
    public GameObject OptionsMenu;
    public static bool IsEscapeMenuVisible = false;
    private bool _isOptionsMenuVisible = false;

    public AudioSource RainAudio;

    public void HighlightButton()
    {
        GetComponent<MeshRenderer>().material = HoveredButton;
    }

    public void ToggleEscapeMenu()
    {
        PlayerController._isEscapeButtonPressed = !PlayerController._isEscapeButtonPressed;
        IsEscapeMenuVisible = !IsEscapeMenuVisible;
        EscapeMenu.SetActive(IsEscapeMenuVisible);
    }

    public void BackToMainMenu()
    {
        Debug.Log("Back to Main Menu");
    }

    public void ShowOptionsMenu()
    {
        _isOptionsMenuVisible = !_isOptionsMenuVisible;
        OptionsMenu.SetActive(_isOptionsMenuVisible);
    }

    public void UpdateVolume()
    {
        string nameOfClickedButton = EventSystem.current.currentSelectedGameObject
            .transform.parent.name;
        string nameOfVolumeSetting = EventSystem.current.currentSelectedGameObject
            .name;

        GameObject[] customers = 
            GameObject.FindGameObjectsWithTag("Customer");

        switch (nameOfClickedButton)
        {
            case "Rain Volume":
                if (nameOfVolumeSetting == "Increase")
                {
                    RainAudio.volume += 0.1f;
                }
                else
                {
                    RainAudio.volume -= 0.1f;
                }
                break;
            case "Dialogue Volume":
                if (nameOfVolumeSetting == "Increase")
                {
                    foreach (GameObject customer in customers)
                    {
                        customer.GetComponent<AudioSource>().volume += 0.1f;
                    }
                }
                else
                {
                    foreach (GameObject customer in customers)
                    {
                        customer.GetComponent<AudioSource>().volume -= 0.1f;
                    }
                }
                break;
            case "Master Volume":
                if (nameOfVolumeSetting == "Increase")
                {
                    RainAudio.volume += 0.1f;
                    foreach (GameObject customer in customers)
                    {
                        customer.GetComponent<AudioSource>().volume += 0.1f;
                    }
                }
                else
                {
                    RainAudio.volume -= 0.1f;
                    foreach (GameObject customer in customers)
                    {
                        customer.GetComponent<AudioSource>().volume -= 0.1f;
                    }
                }
                break;
        }
    }
}

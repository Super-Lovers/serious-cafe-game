using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour {
    public Material HoveredButton;

    public void HighlightButton()
    {
        GetComponent<MeshRenderer>().material = HoveredButton;
    }
}

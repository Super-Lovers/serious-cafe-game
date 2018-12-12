using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour {
    public Material DefaultButtonMaterial;

    private void Update()
    {
        if (RayShooter.EnteredButton == false)
        {
            GetComponent<MeshRenderer>().material = DefaultButtonMaterial;
        }
    }
}

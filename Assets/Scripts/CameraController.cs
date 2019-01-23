using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    private bool _changeDirection = false;
    private Camera _camera;

    void Start()
    {
        _camera = GetComponent<Camera>();

        InvokeRepeating("ChangeDirectionOfRotation", 0, 17f);
    }

    void Update () {
		if (_changeDirection)
        {
            _camera.transform.Rotate(new Vector3(0, 0.05f, 0));
        } else
        {
            _camera.transform.Rotate(new Vector3(0, -0.05f, 0));
        }
	}

    private void ChangeDirectionOfRotation()
    {
        _changeDirection = !_changeDirection;
    }
}

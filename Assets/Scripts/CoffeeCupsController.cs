using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoffeeCupsController : MonoBehaviour
{
    private Camera _playerCamera;
    public LayerMask CupSize;
    public LayerMask BaseLayer;
    public LayerMask PrimaryAndSecondary;

    private string _phase = string.Empty;
    private string _finalCup = string.Empty;
    
    private string _cupSize = string.Empty;
    private string _base = string.Empty;
    private string _primaryBase = string.Empty;
    private string _secondaryBase = string.Empty;
    
    void Start ()
    {
        _playerCamera = GetComponentInChildren<Camera>();

        _phase = "Base";
    }
	
	void Update () {
        Ray ray = new Ray(_playerCamera.transform.position, _playerCamera.transform.forward);
        RaycastHit hit;

        if (_phase != "Complete")
        {
            if (_cupSize == string.Empty)
            {
                if (Physics.Raycast(ray, out hit, 100, CupSize))
                {
                    GameObject hitObject = hit.collider.gameObject;
                    if (Input.GetMouseButtonDown(0))
                    {
                        _cupSize = hitObject.name;
                        Debug.Log(_cupSize);
                    }
                }
            }
            else
            {
                if (_phase == "Base" && _cupSize != string.Empty)
                {
                    if (Physics.Raycast(ray, out hit, 100, BaseLayer))
                    {
                        GameObject hitObject = hit.collider.gameObject;
                        if (Input.GetMouseButtonDown(0))
                        {
                            _base = hitObject.name;
                            Debug.Log(_base);
                            _phase = "Primary";
                        }
                    }
                }
                else if (_phase == "Primary" || _phase == "Secondary")
                {
                    if (Physics.Raycast(ray, out hit, 100, PrimaryAndSecondary))
                    {
                        GameObject hitObject = hit.collider.gameObject;

                        if (Input.GetMouseButtonDown(0))
                        {
                            if (_phase == "Primary")
                            {
                                _primaryBase = hitObject.name;
                                Debug.Log(_primaryBase);
                                _phase = "Secondary";
                            } else if (_phase == "Secondary")
                            {
                                _secondaryBase = hitObject.name;
                                Debug.Log(_secondaryBase);
                                _phase = "Complete";

                                _finalCup = _cupSize + " " +
                                    _base + " " +
                                    _primaryBase + " " +
                                    _secondaryBase;

                                Debug.Log(_phase);
                                Debug.Log(_finalCup);
                            }
                        }
                    }
                }
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoffeeCupsController : MonoBehaviour
{
    private Camera _playerCamera;
    public LayerMask CupSize;
    public LayerMask BaseLayer;
    public LayerMask PrimaryAndSecondary;
    public Material SelectedObjectMaterial;
    public Material DefaultObjectMaterial;
    public GameObject ParticlesForSelectables;

    private string _phase = string.Empty;
    private string _finalCup = string.Empty;
    
    private string _cupSize = string.Empty;
    private string _base = string.Empty;
    private string _primaryBase = string.Empty;
    private string _secondaryBase = string.Empty;

    private GameObject _hitObject;
    
    void Start ()
    {
        _playerCamera = GetComponentInChildren<Camera>();
        ParticlesForSelectables = Instantiate(ParticlesForSelectables);

        _phase = "Base";
    }
	
	void Update () {
        Ray ray = new Ray(_playerCamera.transform.position, _playerCamera.transform.forward);
        RaycastHit hit;

        if (_phase != "Complete")
        {
            if (_cupSize == string.Empty)
            {
                // Moving the selection of different phase objects to the
                // current phase of the coffee-making process.
                MoveParticlesTo("Cups");

                if (Physics.Raycast(ray, out hit, 100, CupSize))
                {
                    // This conditional makes sure that if we enter a new collider
                    // that we restore the default material on the previous collider
                    // because if we dont, they both might stay green unless your ray
                    // exits both of the colliders, not just one.
                    if (_hitObject != null)
                    {
                        _hitObject.GetComponent<MeshRenderer>().material = DefaultObjectMaterial;
                    }
                    _hitObject = hit.collider.gameObject;
                    _hitObject.GetComponent<MeshRenderer>().material = SelectedObjectMaterial;

                    if (Input.GetMouseButtonDown(0))
                    {
                        MoveParticlesTo("Base");

                        _cupSize = _hitObject.name;
                        Debug.Log(_cupSize);
                    }
                } else
                {
                    if (_hitObject != null)
                    {
                        _hitObject.GetComponent<MeshRenderer>().material = DefaultObjectMaterial;
                    }
                }
            }
            else
            {
                if (_phase == "Base" && _cupSize != string.Empty)
                {
                    if (Physics.Raycast(ray, out hit, 100, BaseLayer))
                    {
                        if (_hitObject != null)
                        {
                            _hitObject.GetComponent<MeshRenderer>().material = DefaultObjectMaterial;
                        }
                        _hitObject = hit.collider.gameObject;
                        _hitObject.GetComponent<MeshRenderer>().material = SelectedObjectMaterial;

                        if (Input.GetMouseButtonDown(0))
                        {
                            MoveParticlesTo("Primary and Secondary");

                            _base = _hitObject.name;
                            Debug.Log(_base);
                            _phase = "Primary";
                        }
                    } else
                    {
                        if (_hitObject != null)
                        {
                            _hitObject.GetComponent<MeshRenderer>().material = DefaultObjectMaterial;
                        }
                    }
                }
                else if (_phase == "Primary" || _phase == "Secondary")
                {
                    if (Physics.Raycast(ray, out hit, 100, PrimaryAndSecondary))
                    {
                        if (_hitObject != null)
                        {
                            _hitObject.GetComponent<MeshRenderer>().material = DefaultObjectMaterial;
                        }
                        _hitObject = hit.collider.gameObject;
                        _hitObject.GetComponent<MeshRenderer>().material = SelectedObjectMaterial;

                        if (Input.GetMouseButtonDown(0))
                        {
                            if (_phase == "Primary")
                            {
                                _primaryBase = _hitObject.name;
                                Debug.Log(_primaryBase);
                                _phase = "Secondary";
                            } else if (_phase == "Secondary")
                            {
                                _secondaryBase = _hitObject.name;
                                Debug.Log(_secondaryBase);
                                _phase = "Complete";

                                _finalCup = _cupSize + " " +
                                    _base + " " +
                                    _primaryBase + " " +
                                    _secondaryBase;

                                MoveParticlesTo("Base");

                                Debug.Log(_phase);
                                Debug.Log(_finalCup);

                                // Resetting the variables that define a complete order of coffee
                                // and allow the player to create a new coffee.
                                _cupSize = string.Empty;
                                _base = string.Empty;
                                _primaryBase = string.Empty;
                                _secondaryBase = string.Empty;
                                _phase = "Base";
                            }
                        }
                    } else
                    {
                        if (_hitObject != null)
                        {
                            _hitObject.GetComponent<MeshRenderer>().material = DefaultObjectMaterial;
                        }
                    }
                }
            }
        }
    }

    private void MoveParticlesTo(string newParentName)
    {
        Vector3 ParticlesForSelectablesPosition = ParticlesForSelectables.transform.position;
        ParticlesForSelectablesPosition = GameObject.Find(newParentName).transform.position;
        ParticlesForSelectables.transform.position = ParticlesForSelectablesPosition;
    }
}

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
    public GameObject SpotForNewCoffee;

    private string _phase = string.Empty;
    private string _finalCup = string.Empty;
    
    private string _cupSize = string.Empty;
    private string _base = string.Empty;
    private string _primaryBase = string.Empty;
    private string _secondaryBase = string.Empty;

    // All of the public fields for creating a coffee using
    // the available cup sizes, base, primary and secondary ingredients.
    public GameObject SmallCup;
    public GameObject MediumCup;
    public GameObject LargeCup;

    public GameObject BaseMilk;
    public GameObject BaseCoffee;
    public GameObject BaseTea;

    public GameObject PrimarySecondaryHoney;
    public GameObject PrimarySecondaryLemon;
    public GameObject PrimarySecondaryMint;
    public GameObject PrimarySecondaryCocoa;
    public GameObject PrimarySecondaryCoffee;

    private GameObject _spotForNewCoffee;
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

                                // This is the prompt for the player when the coffee is
                                // made and he decides whether or not he wants to hand it
                                // to the customer or not.
                                _spotForNewCoffee = Instantiate(SpotForNewCoffee, GameObject.Find("Ingredients").transform);

                                InstantiateCoffee();

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

    private void InstantiateCoffee()
    {
        GameObject newCup = null;
        // ***************
        // Instantiates the cup
        if (_cupSize == "Small")
        {
            newCup = Instantiate(SmallCup, _spotForNewCoffee.transform);
        }
        else if (_cupSize == "Medium")
        {
            newCup = Instantiate(MediumCup, _spotForNewCoffee.transform);
        }
        else if (_cupSize == "Large")
        {
            newCup = Instantiate(LargeCup, _spotForNewCoffee.transform);
        }
        // We reset the layer because by default we use the cups and ingredients
        // layers to make the coffee, but after we create the final order, we dont want
        // to be able to hover over the cup and affect its default materials anymore.
        newCup.layer = 0;

        // ***************
        // Instantiates the base
        if (_base == "Milk")
        {
            newCup = Instantiate(BaseMilk, _spotForNewCoffee.transform.GetChild(1).transform);
        }
        else if (_base == "Coffee")
        {
            newCup = Instantiate(BaseCoffee, _spotForNewCoffee.transform.GetChild(1).transform);
        }
        else if (_base == "Tea")
        {
            newCup = Instantiate(BaseTea, _spotForNewCoffee.transform.GetChild(1).transform);
        }
        newCup.layer = 0;

        // ***************
        // Instantiates the primary ingredients
        InstantiateIngredientsToBase(_primaryBase);

        // ***************
        // Instantiates the secondary ingredients
        InstantiateIngredientsToBase(_secondaryBase);

        _spotForNewCoffee = null;
    }

    private void InstantiateIngredientsToBase(string newBase)
    {
        GameObject newIngredient = null;
        switch (newBase)
        {
            case "Honey":
                newIngredient = Instantiate(PrimarySecondaryHoney, _spotForNewCoffee.transform.GetChild(1).transform.GetChild(0).transform);
                break;
            case "Lemon":
                newIngredient = Instantiate(PrimarySecondaryLemon, _spotForNewCoffee.transform.GetChild(1).transform.GetChild(0).transform);
                break;
            case "Mint":
                newIngredient = Instantiate(PrimarySecondaryMint, _spotForNewCoffee.transform.GetChild(1).transform.GetChild(0).transform);
                break;
            case "Cocoa":
                newIngredient = Instantiate(PrimarySecondaryCocoa, _spotForNewCoffee.transform.GetChild(1).transform.GetChild(0).transform);
                break;
            case "Coffee":
                newIngredient = Instantiate(PrimarySecondaryCoffee, _spotForNewCoffee.transform.GetChild(1).transform.GetChild(0).transform);
                break;
        }
        newIngredient.layer = 0;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoffeeCupsController : MonoBehaviour
{
    private Camera _playerCamera;
    public LayerMask CupSizeLayer;
    public LayerMask BaseLayer;
    public LayerMask PrimaryAndSecondary;
    public Material SelectedObjectMaterial;
    public Material DefaultObjectMaterial;
    public Material CupsDefaultObjectMaterial;
    public GameObject ParticlesForSelectables;
    public GameObject SpotForNewCoffeePrefab;

    private string _phase = string.Empty;
    public static string FinalCup = string.Empty;

    public static string CupSize = string.Empty;
    public static string Base = string.Empty;
    public static string PrimaryBase = string.Empty;
    public static string SecondaryBase = string.Empty;

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

    public static GameObject SpotForNewCoffee;
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
            if (CupSize == string.Empty)
            {
                // Moving the selection of different phase objects to the
                // current phase of the coffee-making process.
                MoveParticlesTo("Cups");

                if (Physics.Raycast(ray, out hit, 100, CupSizeLayer))
                {
                    // This conditional makes sure that if we enter a new collider
                    // that we restore the default material on the previous collider
                    // because if we dont, they both might stay green unless your ray
                    // exits both of the colliders, not just one.
                    if (_hitObject != null)
                    {
                        _hitObject.GetComponent<MeshRenderer>().material = CupsDefaultObjectMaterial;
                    }
                    _hitObject = hit.collider.gameObject;
                    _hitObject.GetComponent<MeshRenderer>().material = SelectedObjectMaterial;

                    if (Input.GetMouseButtonDown(0))
                    {
                        MoveParticlesTo("Base");
                        _phase = "Base";

                        CupSize = _hitObject.name;
                        Debug.Log(CupSize);
                    }
                } else
                {
                    if (_hitObject != null)
                    {
                        _hitObject.GetComponent<MeshRenderer>().material = CupsDefaultObjectMaterial;
                    }
                }
            }
            else
            {
                if (_phase == "Base" && CupSize != string.Empty)
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

                            Base = _hitObject.name;
                            Debug.Log(Base);
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
                                PrimaryBase = _hitObject.name;
                                Debug.Log(PrimaryBase);
                                _phase = "Secondary";
                            } else if (_phase == "Secondary")
                            {
                                SecondaryBase = _hitObject.name;
                                Debug.Log(SecondaryBase);
                                _phase = "Complete";

                                FinalCup = CupSize + " " +
                                    Base + " " +
                                    PrimaryBase + " " +
                                    SecondaryBase;

                                // This is the prompt for the player when the coffee is
                                // made and he decides whether or not he wants to hand it
                                // to the customer or not.
                                SpotForNewCoffee = Instantiate(SpotForNewCoffeePrefab, GameObject.Find("Ingredients").transform);

                                InstantiateCoffee();

                                MoveParticlesTo("Final Order(Clone)");

                                Debug.Log(_phase);
                                Debug.Log(FinalCup);

                                _phase = "Cups";
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
        if (CupSize == "Small")
        {
            newCup = Instantiate(SmallCup, SpotForNewCoffee.transform);
        }
        else if (CupSize == "Medium")
        {
            newCup = Instantiate(MediumCup, SpotForNewCoffee.transform);
        }
        else if (CupSize == "Large")
        {
            newCup = Instantiate(LargeCup, SpotForNewCoffee.transform);
        }
        // We reset the layer because by default we use the cups and ingredients
        // layers to make the coffee, but after we create the final order, we dont want
        // to be able to hover over the cup and affect its default materials anymore.
        newCup.layer = 0;

        // ***************
        // Instantiates the base
        if (Base == "Milk")
        {
            newCup = Instantiate(BaseMilk, SpotForNewCoffee.transform.GetChild(1).transform);
        }
        else if (Base == "Coffee")
        {
            newCup = Instantiate(BaseCoffee, SpotForNewCoffee.transform.GetChild(1).transform);
        }
        else if (Base == "Tea")
        {
            newCup = Instantiate(BaseTea, SpotForNewCoffee.transform.GetChild(1).transform);
        }
        newCup.layer = 0;

        // ***************
        // Instantiates the primary ingredients
        InstantiateIngredientsToBase(PrimaryBase);

        // ***************
        // Instantiates the secondary ingredients
        InstantiateIngredientsToBase(SecondaryBase);

        //SpotForNewCoffee = null;
    }

    private void InstantiateIngredientsToBase(string newBase)
    {
        GameObject newIngredient = null;
        switch (newBase)
        {
            case "Honey":
                newIngredient = Instantiate(PrimarySecondaryHoney, SpotForNewCoffee.transform.GetChild(1).transform.GetChild(0).transform);
                break;
            case "Lemon":
                newIngredient = Instantiate(PrimarySecondaryLemon, SpotForNewCoffee.transform.GetChild(1).transform.GetChild(0).transform);
                break;
            case "Mint":
                newIngredient = Instantiate(PrimarySecondaryMint, SpotForNewCoffee.transform.GetChild(1).transform.GetChild(0).transform);
                break;
            case "Cocoa":
                newIngredient = Instantiate(PrimarySecondaryCocoa, SpotForNewCoffee.transform.GetChild(1).transform.GetChild(0).transform);
                break;
            case "Coffee":
                newIngredient = Instantiate(PrimarySecondaryCoffee, SpotForNewCoffee.transform.GetChild(1).transform.GetChild(0).transform);
                break;
        }
        newIngredient.layer = 0;
    }
}

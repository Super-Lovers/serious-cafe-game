using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoffeeMachine : MonoBehaviour
{
	public GameObject CoffeeCupPrefab;
	public GameObject FirstStage;
	public GameObject SecondStage;
	public GameObject ThirdStage;
	// Spots on the basket where the completed coffee
	// requests are placed to give away to customers.
	public GameObject FirstSpot;
	public GameObject SecondSpot;
	public GameObject ThirdSpot;
	public GameObject FourthSpot;
	
	// Indicator materials
	public GameObject Indicator;
	private MeshRenderer _indicatorRenderer;
	public Material IndicatorIdle;
	public Material IndicatorWorking;
	public Material IndicatorDone;

	private bool _isFirstStageComplete = false;
	public GameObject LiquidInCupPrefab;
	private GameObject _liquidInCup;
	private bool _isSecondStageComplete = false;
	private bool _isThirdStageComplete = false;

	public static GameObject CoffeeInMaking;

	public Material MilkMaterial;
	public Material CoffeeMilkMaterial;

	private bool _canGoToNextStage = true;

	public float SecondsToFirstStageCompletion;
	public float SecondsToSecondStageCompletion;
	public float SecondsToThirdStageCompletion;

	public static bool IsBasketFull = false;

	private void Start()
	{
		_indicatorRenderer = Indicator.GetComponent<MeshRenderer>();
	}

	void Update () {
		if (UiButtonBehaviour.IsRequestComplete == false)
		{
			if (_isFirstStageComplete == false &&
			    _canGoToNextStage) // First stage
			{
				CoffeeInMaking = Instantiate(CoffeeCupPrefab);
				// Once the machine starts working we indicate immediately.
				_indicatorRenderer.material = IndicatorWorking;

				MoveCoffeeToFirstStage();
			}

			if (_isFirstStageComplete &&
			    _isSecondStageComplete == false &&
			    _canGoToNextStage) // Second stage
			{
				MoveCoffeeToSecondStage();
			}

			if (_isFirstStageComplete && _isSecondStageComplete && // Third stage
			    _isThirdStageComplete == false &&
			    _canGoToNextStage)
			{
				MoveCoffeeToThirdStage();
			}

			if (_isFirstStageComplete &&
			    _isSecondStageComplete &&
			    _isThirdStageComplete)
			{
				// If the spot in the basket is taken by another coffee cup
				// then we want to put the new cup to another spot.
				if (FirstSpot.transform.childCount <= 0)
				{
					MoveCoffeeToBasket(FirstSpot);
					UiButtonBehaviour.IsRequestComplete = true;
				} else if (SecondSpot.transform.childCount <= 0)
				{
					MoveCoffeeToBasket(SecondSpot);
					UiButtonBehaviour.IsRequestComplete = true;
				} else if (ThirdSpot.transform.childCount <= 0)
				{
					MoveCoffeeToBasket(ThirdSpot);
					UiButtonBehaviour.IsRequestComplete = true;
				} else if (FourthSpot.transform.childCount <= 0)
				{
					MoveCoffeeToBasket(FourthSpot);
					UiButtonBehaviour.IsRequestComplete = true;
					IsBasketFull = true;
				}
			}
		}
	}

	private void MoveCoffeeToFirstStage()
	{
		UpdateCoffeePosition(FirstStage, "CompleteFirstStage");
		_canGoToNextStage = false;
	}

	private void MoveCoffeeToSecondStage()
	{
		UpdateCoffeePosition(SecondStage, "CompleteSecondStage");
		_canGoToNextStage = false;
	}
	
	private void MoveCoffeeToThirdStage()
	{
		UpdateCoffeePosition(ThirdStage, "CompleteThirdStage");
		_canGoToNextStage = false;
	}
	
	private void MoveCoffeeToBasket(GameObject spot)
	{
		Vector3 coffeeInMakingPos = CoffeeInMaking.transform.position;
		coffeeInMakingPos = spot.transform.position;
		coffeeInMakingPos.y += 0.05f;
		CoffeeInMaking.transform.position = coffeeInMakingPos;

		Instantiate(CoffeeInMaking, CoffeeInMaking.transform.position, Quaternion.identity, spot.transform);

		CoffeeInMaking = null;

		_isFirstStageComplete = false;
		_isSecondStageComplete = false;
		_isThirdStageComplete = false;
		
		_indicatorRenderer.material = IndicatorDone;
		Invoke("IndicatorReset", 5f);
	}

	private void UpdateCoffeePosition(GameObject stage, string stageToComplete)
	{
		// Moving the coffee currently in process of creating to the
		// first stage of processing for the machine.
		Vector3 coffeeInMakingPos = CoffeeInMaking.transform.position;
		coffeeInMakingPos = stage.transform.position;
		// This puts the coffee cup a little under the stage tube.
		coffeeInMakingPos.y -= 0.1431f;
		CoffeeInMaking.transform.position = coffeeInMakingPos;
		
		// The stage will be complete in x seconds.
		if (stageToComplete == "CompleteFirstStage")
		{
			Invoke(stageToComplete, SecondsToFirstStageCompletion);
		} else if (stageToComplete == "CompleteSecondStage")
		{
			Invoke(stageToComplete, SecondsToSecondStageCompletion);
		} else if (stageToComplete == "CompleteThirdStage")
		{
			Invoke(stageToComplete, SecondsToThirdStageCompletion);
		}
	}

	private void CompleteFirstStage()
	{
		_isFirstStageComplete = true;
		
		// Once the milk has been successfully prepared in the cup,
		// we have to instantiate it and show it to the user.
		_liquidInCup = Instantiate(LiquidInCupPrefab, CoffeeInMaking.transform.position, Quaternion.identity,
			// The cup is a child of the latte empty object, so we get the child as the liquid's parent instead
			CoffeeInMaking.transform.GetChild(0).transform);
		
		_liquidInCup.GetComponent<MeshRenderer>().material = MilkMaterial;
		
		// This allows the code in the update function to run the next stage incrementally
		// instead of spamming the conditional.
		_canGoToNextStage = true;
	}
	private void CompleteSecondStage()
	{
		_isSecondStageComplete = true;
		
		_liquidInCup.GetComponent<MeshRenderer>().material = CoffeeMilkMaterial;
		
		_canGoToNextStage = true;
	}
	private void CompleteThirdStage()
	{
		_isThirdStageComplete = true;
		
		_canGoToNextStage = true;
	}

	private void IndicatorReset()
	{
		_indicatorRenderer.material = IndicatorIdle;
	}
}

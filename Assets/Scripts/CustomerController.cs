using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class CustomerController : MonoBehaviour
{
	public GameObject ReceiveOrderSpot;
	public GameObject EnteringRestaurantSpot;
	private Transform _playerTransform;
    private AudioSource _audioSource;
	private bool _canMove = true;

	private Text _speechBubble;
	private static string[] _coffeeBeverages = {"Latte", "Espresso"};

	private Rigidbody _rb;
    public int CurrentDialogueIndex;
    //public List<string> CustomerDialogue;

    public string CustomerName;
    public string dialogueFileName;
    private readonly List<string> dialogueList = new List<string>();
    public bool HadFirstCoffee = false;
    public bool HadSecondCoffee = false;
    public bool IsDialogueLoaded = true;

    public bool IsOrderComplete = true;

    void Start ()
	{
        FormatDialogue(dialogueFileName);

		_rb = GetComponent<Rigidbody>();
        _audioSource = GetComponent<AudioSource>();
		_speechBubble = GetComponentInChildren<Text>();
		_playerTransform = GameObject.FindWithTag("Player").transform;
		
		Vector3 currentPos = transform.position;
		currentPos = EnteringRestaurantSpot.transform.position;
		transform.position = currentPos;

        // Resets the current dialogue bubble for the new one.
        _speechBubble.text = "";
        StartCoroutine(UpdateDialogueText(dialogueList[CurrentDialogueIndex]));

        // We want the speech bubble to be visible only once the customer
        // sits down and then decides on his order
        _speechBubble.transform.parent.gameObject.SetActive(false);
		//Invoke("MoveToSeat", 6f);
	}
	
	void Update () {
		if (_canMove)
		{
			// We save the new vector3 position to the transform, thats why
			// we need to update the position parameter with the new one, otherwise
			// it wont store it and update the existing one.
			transform.position = Vector3.MoveTowards(transform.position,
				CustomerGenerator.AvailableSeat.transform.position, 0.03f);
		}

		if (transform.position == CustomerGenerator.AvailableSeat.transform.position)
		{
			_canMove = false;
			_speechBubble.transform.parent.gameObject.SetActive(true);
		}
		
        /*
		if (_playerTransform)
		{
            _speechBubble.transform.LookAt(_playerTransform);
            _speechBubble.transform.Rotate(new Vector3(0, _speechBubble.transform.rotation.y * -1, 0));
			
			// For some reason the x axis of the speechbubble flips once it
			// turns to the player, so im flipipng it back...
			//Vector3 speechBubbleScale = _speechBubble.transform.localScale;
			//speechBubbleScale.x *= -1;
			//_speechBubble.transform.localScale = speechBubbleScale;
		}
        */
	}

    public void UpdateDialogueIndex()
    {
        _audioSource.enabled = true;
        _speechBubble.text = "";
        CurrentDialogueIndex++;
        //Debug.Log(CurrentDialogueIndex);

        if (CurrentDialogueIndex > dialogueList.Count - 1)
        {
            CustomerGenerator.ExistingCustomers.Remove(gameObject);
            IsDialogueLoaded = true;

            Destroy(gameObject);
        } else
        {
            StartCoroutine(UpdateDialogueText(dialogueList[CurrentDialogueIndex]));
        }

        // Once the player reachers a dialogue point where the customer would like to rest and have a coffee, then we must trigger the order functionality that makes impossible to progress until you satisfy the customer with his desired coffee.
        if (CustomerName == "Nikolay" && CurrentDialogueIndex == 2)
        {
            GameObject.FindGameObjectWithTag("Customer").GetComponent<CustomerController>().IsOrderComplete = false;
        }

        IsDialogueLoaded = false;
    }

    public IEnumerator UpdateDialogueText(string newText)
    {
        _audioSource.enabled = true;
        _speechBubble.text = "";
        //Debug.Log("Updating dialogue text");
        foreach (char symbol in newText)
        {
            yield return new WaitForSeconds(0.02f);
            _speechBubble.text += symbol;
            yield return new WaitForSeconds(0.02f);
        }

        _audioSource.enabled = false;
        IsDialogueLoaded = true;
    }

	// You can use this if you want to make the customers teleport
	// instead of walk to their seats.
	private void MoveToSeat()
	{
		Vector3 currentPos = transform.position;
		currentPos = CustomerGenerator.AvailableSeat.transform.position;
		transform.position = currentPos;

		// Stop moving in that direction if you are already in your seat.
		Debug.Log("Moved to seat");
    }

    private void FormatDialogue(string dialogueFileString)
    {
        StreamReader file = new StreamReader(
            Application.dataPath + "/StreamingAssets/" +
            dialogueFileName + ".txt");

        using (file)
        {
            string line = file.ReadLine();

            while (line != null)
            {
                dialogueList.Add(line);
                line = file.ReadLine();
            }
        }
    }
}

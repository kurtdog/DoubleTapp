using UnityEngine;
using System.Collections;

public class GuiController : MonoBehaviour {


	public GameObject GUItext;
	public GameObject ThreeDtext;
	public GameObject Ship;
	public float offset;
	private ShipController shipController;

	//DISPLAY GUI items on game objects
	// Use this for initialization
	void Start () {
		shipController = Ship.GetComponent<ShipController>();

		GUItext = GameObject.Instantiate(GUItext) as GameObject;
		GUItext.GetComponent<GUIText>().text = "Target";

		ThreeDtext = GameObject.Instantiate(ThreeDtext) as GameObject;
		ThreeDtext.GetComponent<TextMesh>().text = "Target";

	}
	
	// Update is called once per frame
	//TODO: change to OnGUI()
	void Update () {
		if(shipController.target != null)
		{

			Display3DText();
		}
	}

	void OnGUI()
	{


	}



	void DisplayGuiText()
	{
		Vector3 targetScreenPos = Camera.main.WorldToScreenPoint(shipController.target.transform.position);
	}

	void Display3DText()
	{
		//if onscreen, display text
		//Debug.Log("display GUI at target " + shipController.target);
		//Get position of target on screen. 
		//Put the target at that position, plus a screen offset 

		// shippositon - textLenght/2 + offset
		ThreeDtext.transform.position = shipController.target.transform.position - Camera.main.transform.right*(ThreeDtext.transform.lossyScale.x/2) + Camera.main.transform.up*(ThreeDtext.transform.lossyScale.y);//,0));

		//rotate the text
		ThreeDtext.transform.rotation = Camera.main.transform.rotation;


		//otherwise, display arrow
	}

}

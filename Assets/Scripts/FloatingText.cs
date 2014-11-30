using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/* This class displays a list of strings as 3D text meshes in the scene
 * It always faces the camera by updating it's rotation
 * It updates it's position relative to the sclae of it's ParentObject
 * 
 * */
public class FloatingText : MonoBehaviour {

	//object position will be the base, or bottom of the textDisplay;
	//TODO: add boarder, public GameObject Board, that get's scaled and placed correctly

	public GameObject ParentObject;
	public GameObject EmptyTextPrefab;
	public float spacing;
	public List<GameObject> textDisplayed;

	public TextAlignment alignment;
	public TextAnchor anchor;

	// Use this for initialization
	void Start () {
        textDisplayed = new List<GameObject>();
	}

	void Update()
	{
		UpdatePosition ();
	}

	void UpdatePosition()
	{
		this.transform.position = ParentObject.transform.position + Camera.main.transform.up.normalized * ParentObject.transform.lossyScale.magnitude/2;


		//update rotation to face target
		Vector3 cameraToMe = this.transform.position - Camera.main.transform.position;
		//this.transform.forward = cameraToMe;

		this.transform.rotation = Camera.main.transform.rotation;
	}

	public void DisplayText(List<string> textItems)
	{
		//check to see if textItems.count is differen than textDisplayed.count;
		if(textItems.Count != textDisplayed.Count)
		{
			//if it is, then re-create our list of 'textDisplayed'items
			textDisplayed = new List<GameObject>();
			int i = textItems.Count;
			foreach(string textItem in textItems)
			{
				GameObject displayedText = Instantiate(EmptyTextPrefab,this.transform.position + this.transform.up.normalized*i*spacing,this.transform.rotation) as GameObject;
				displayedText.transform.parent = this.transform;
				displayedText.GetComponent<TextMesh>().alignment = alignment;
				displayedText.GetComponent<TextMesh>().anchor = anchor;
				textDisplayed.Add(displayedText);
				i--;
			}
		}
		else{ //otherwise we can just update the strings on the textItems we already have
			for(int i = 0; i < textItems.Count; i++)
			{
				textDisplayed[i].GetComponent<TextMesh>().text = textItems[i];
			}
		}
		//loop through the list of text items and display them. Each text item get's its own line
	}

}

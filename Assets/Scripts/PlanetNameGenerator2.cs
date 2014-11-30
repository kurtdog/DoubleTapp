using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

/*
[CustomEditor(typeof(PlanetNameGenerator2))]
// ^ This is the script we are making a custom editor for.
public class PlanetNameGenerator2Editor : Editor {

	public GameObject planetGenerator2;
	

	public override void OnInspectorGUI()
	{
		//Called whenever the inspector is drawn for this object.
		DrawDefaultInspector();
		//This draws the default screen. You don't need this if you want
		//to start from scratch, but I use this when I'm just adding a button or
		//some small addition and don't feel like recreating the whole inspector.
		planetGenerator2 = EditorGUILayout.ObjectField (planetGenerator2,typeof(GameObject)) as GameObject;
		
		if(GUILayout.Button("GetName")) 
		{
			Debug.Log(planetGenerator2.GetComponent<PlanetNameGenerator2>().GetName());

		}
		if(GUILayout.Button("GetSolarSystemNames")) 
		{
			foreach(string name in planetGenerator2.GetComponent<PlanetNameGenerator2>().GetSolarSystemNames(10))
			{
				Debug.Log(name);
			}
		}

	}
}
 * */
public class PlanetNameGenerator2 : MonoBehaviour {

	public int minLetters;
	public int maxLetters;
	public int minNumbers;
	public int maxNumbers;

	private  List<string> alphabet = new List<string>(){"a","b","c","d","e","f","g","h","i","j","k","l","m","n","o","p","q","r","s","t","u","v","w","x","y","z",
														"A","B","C","D","E","F","G","H","I","J","K","L","M","N","O","P","Q","R","S","T","U","V","W","X","Y","Z"};
	private  List<string> numbers = new List<string>(){"0","1","2","3","4","5","6","7","8","9"};
	// Use this for initialization
	public string GetName()
	{
		string s = "";
		int numLetters = Random.Range (minLetters, maxLetters);
		int numNumbers = Random.Range (minNumbers, maxNumbers);

		for(int i = 0; i < numLetters; i++)
		{
			s += alphabet[Random.Range(0,alphabet.Count)];
		}
		s += "-";
		for(int i = 0; i < numNumbers; i++)
		{
			s += numbers[Random.Range(0,numbers.Count)];
		}
		return s;
	}

	public List<string> GetSolarSystemNames(int numNames)
	{
		List<string> nameList = new List<string> ();
		string systemName = "";
		int numLetters = Random.Range (minLetters, maxLetters);
		int numNumbers = Random.Range (minNumbers, maxNumbers);

		//make a solar system name, that will be consistent accross all names in the planet, Acdq, AJiL, something like that
		for(int i = 0; i < numLetters; i++)
		{
			systemName += alphabet[Random.Range(0,alphabet.Count)];
		}
		systemName += "-";
		for(int j = 0; j < numNames; j++)
		{
			string planetName = systemName;
			for(int i = 0; i < numNumbers; i++)
			{
				planetName += numbers[Random.Range(0,numbers.Count)];
			}
			nameList.Add(planetName);
		}
		return nameList;
	}





}

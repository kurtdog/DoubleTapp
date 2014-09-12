using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HealthLine : MonoBehaviour {

	public GameObject Ship;
	public GameObject ShooterShip;
	Health healthScript;
	LineRenderer healthLine;
	LineRenderer leftBorder;
	LineRenderer rightBorder;
	public float lineWidth;
	public int segPerLine;
	public float xOffset;
	public float yOffset;
	public float zOffset;
	public float healthLineWidth;
	public float borderWidth;

	public Color healthColor;
	public Color healthBorderColor;
	public List<GameObject> lineList;

	Vector3 healthLineStartPos;
	Vector3 healthLineEndPos;

	void Start () {
	
		lineList = new List<GameObject>();
		healthScript = Ship.GetComponent<Health>();

		healthLine = this.GetComponent<LineRenderer>();//NewLine(healthColor,healthColor,"HealthLine");
		healthLine.SetColors(Color.green,Color.green);
		healthLine.SetWidth(lineWidth, lineWidth);
		healthLine.SetVertexCount((int)segPerLine);

		leftBorder = NewLine(healthBorderColor,healthBorderColor,"BorderLeft");
		rightBorder = NewLine(healthBorderColor,healthBorderColor,"BorderRight");
		//line.SetVertexCount(2);
	}
	
	// Update is called once per frame
	void Update () {
		DrawHealthLine();

		DrawBlueBorder();
	}

	void DrawHealthLine()
	{
		Vector3 lineOffset = Ship.transform.right*xOffset +  Ship.transform.up*yOffset + Ship.transform.forward*zOffset;
		healthLineStartPos =  ShooterShip.transform.position + lineOffset - Ship.transform.right*healthLineWidth;
		healthLineEndPos = ShooterShip.transform.position + lineOffset + Ship.transform.right*healthLineWidth*(healthScript.health/healthScript.maxHealth);

		healthLine.SetPosition(0,healthLineStartPos); // set the start point
		healthLine.SetPosition(1,healthLineEndPos );// set the end point, factor in health/maxhealth

		//Draw Directly under ship
		//line.SetPosition(0, ShooterShip.transform.position - Ship.transform.right*width + Ship.transform.up*yOffset); // set the start point
		//line.SetPosition(1, ShooterShip.transform.position + Ship.transform.right*width+ Ship.transform.up*yOffset);// set the end point
	}


	LineRenderer NewLine(Color color1, Color color2,string name)
	{
		GameObject go = new GameObject(); 
		go.name = name;
		lineList.Add(go);
		LineRenderer line = go.AddComponent<LineRenderer>();
		line = go.GetComponent<LineRenderer>();
		//line.material =  new Material(Shader.Find("Particles/Additive"));
		line.SetColors(color1, color2);
		line.SetWidth(lineWidth, lineWidth);
		line.SetVertexCount((int)segPerLine);
		
		return line;
	}

	void DrawBlueBorder()
	{

		leftBorder.SetPosition(0, healthLineStartPos - Ship.transform.right*borderWidth); // set the start point
		leftBorder.SetPosition(1, healthLineStartPos); // set the start point

		rightBorder.SetPosition(0, healthLineEndPos); // set the start point
		rightBorder.SetPosition(1, healthLineEndPos + Ship.transform.right*borderWidth); // set the start point

	}



}

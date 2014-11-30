using UnityEngine;
using System.Collections;

public class MapObject : MonoBehaviour {

	public GameObject guiPopupPrefab;
	
	void Start()
	{
        GuiPopup popupScript = guiPopupPrefab.GetComponent<GuiPopup>();

	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorToggle : MonoBehaviour {
	private GameObject cameraMaster;
	// Use this for initialization
	void Start () {
		object[] obj = GameObject.FindSceneObjectsOfType(typeof (GameObject));
		foreach (object o in obj)
		{
			GameObject g = (GameObject) o;
			if(g.transform.GetComponent<CameraPivotColor>())
				cameraMaster=g;
		}
	}
	
	// Update is called once per frame
	void Update () {
		string colorName=cameraMaster.GetComponent<CameraPivotColor>().colorSelected;
		if(colorName=="red"){
			transform.GetComponent<SpriteRenderer>().color=new Color(1,0,0);
		}
		else if(colorName=="yellow"){
			transform.GetComponent<SpriteRenderer>().color=new Color(1,1,0);
		}
		else if(colorName=="blue"){
			transform.GetComponent<SpriteRenderer>().color=new Color(0,0,1);
		}
		else if(colorName=="white"){
			transform.GetComponent<SpriteRenderer>().color=new Color(1,1,1);
		}
		else if(colorName=="black"){
			transform.GetComponent<SpriteRenderer>().color=new Color(0,0,0);
		}
		else{
			transform.GetComponent<SpriteRenderer>().color=new Color(1,1,1);
		}
	}
}

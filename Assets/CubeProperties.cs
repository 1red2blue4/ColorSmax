using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeProperties : MonoBehaviour {
	public string color;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		bool transparent=!transform.GetComponent<BoxCollider>().enabled;


		if(color=="red"){
			if(transparent)
				transform.GetComponent<MeshRenderer>().material.color=new Color(1,0,0,0.5f);
			else
				transform.GetComponent<MeshRenderer>().material.color=new Color(1,0,0,1f);
		}
		else if(color=="blue"){
			if(transparent)
				transform.GetComponent<MeshRenderer>().material.color=new Color(0,0,1,0.5f);
			else
				transform.GetComponent<MeshRenderer>().material.color=new Color(0,0,1,1f);
		}
		else if(color=="yellow"){
			if(transparent)
				transform.GetComponent<MeshRenderer>().material.color=new Color(1,1,0,0.5f);
			else
				transform.GetComponent<MeshRenderer>().material.color=new Color(1,1,0,1f);
		}
		else if(color=="white"){
			if(transparent)
				transform.GetComponent<MeshRenderer>().material.color=new Color(1,1,1,0.5f);
			else
				transform.GetComponent<MeshRenderer>().material.color=new Color(1,1,1,1f);
		}
		else if(color=="black"){
			if(transparent)
				transform.GetComponent<MeshRenderer>().material.color=new Color(0,0,0,0.5f);
			else
				transform.GetComponent<MeshRenderer>().material.color=new Color(0,0,0,1f);
		}
		else if(color=="neutral"){
			if(transparent)
				transform.GetComponent<MeshRenderer>().material.color=new Color(1,1,1,0.5f);
			else
				transform.GetComponent<MeshRenderer>().material.color=new Color(1,1,1,1f);
		}
		else{
			transform.GetComponent<MeshRenderer>().material.color=new Color(0.5f,0.5f,0.5f);
		}
	}
}

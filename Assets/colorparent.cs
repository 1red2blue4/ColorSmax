using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class colorparent : MonoBehaviour {
	public GameObject cameraPivot;
	public GameObject colorSphere;
	public Color[] colorArray;
	public int sideNumber;
	private int[] mySide;
	private Vector3 initialScale;

	// Use this for initialization
	void Start () {
		mySide=new int[31];
		initialScale=transform.localScale;
	}
	
	// Update is called once per frame
	void Update () {
		int[] testSide=new int[31];
		switch (sideNumber){
		case 1:
			testSide=cameraPivot.GetComponent<CameraPivotColor>().side1Colors;
			break;
		case 2:
			testSide=cameraPivot.GetComponent<CameraPivotColor>().side2Colors;
			break;
		case 3:
			testSide=cameraPivot.GetComponent<CameraPivotColor>().side3Colors;
			break;
		case 4:
			testSide=cameraPivot.GetComponent<CameraPivotColor>().side1Solution;
			break;
		case 5:
			testSide=cameraPivot.GetComponent<CameraPivotColor>().side2Solution;
			break;
		case 6:
			testSide=cameraPivot.GetComponent<CameraPivotColor>().side3Solution;
			break;
		}
		bool same=true;
		for(int i=0;i<31;i++){
			if(mySide[i]!=testSide[i]){
				same=false;
			}
		}
		if(!same){
			mySide=testSide;
			transform.localScale=initialScale;
			generateColors();
		}

	}

	void generateColors(){
		int x=0;
		foreach(Transform child in transform){
			Destroy(child.gameObject);
		}
		for(int i=0;i<31;i++){
			for(int j=0;j<mySide[i];j++){
				generateCube(i,new Vector3(x,0,0));
				x--;
				transform.localScale=new Vector3(transform.localScale.x*.95f,transform.localScale.y,transform.localScale.z);
			}
		}
	}

	void generateCube(int index,Vector3 pos){
		GameObject temp=(GameObject)Instantiate(colorSphere);
		temp.transform.parent=transform;
		temp.transform.localRotation=Quaternion.Euler(0,0,0);
		temp.transform.localPosition=pos;
		temp.transform.localScale=new Vector3(1,1,1);
		temp.GetComponent<MeshRenderer>().material.color=colorArray[index];


	}
}

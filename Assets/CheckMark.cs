using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckMark : MonoBehaviour {
	public int side;
	public GameObject cameraPivot;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(side==1){
			if(cameraPivot.transform.GetComponent<CameraPivotColor>().side1Highlight){
				foreach(Transform child in transform){
					child.gameObject.SetActive(true);
				}
			}
			else{
				foreach(Transform child in transform){
					child.gameObject.SetActive(false);
				}
			}
		}
		if(side==2){
			if(cameraPivot.transform.GetComponent<CameraPivotColor>().side2Highlight){
				foreach(Transform child in transform){
					child.gameObject.SetActive(true);
				}
			}
			else{
				foreach(Transform child in transform){
					child.gameObject.SetActive(false);
				}
			}
		}
		if(side==3){
			if(cameraPivot.transform.GetComponent<CameraPivotColor>().side3Highlight){
				foreach(Transform child in transform){
					child.gameObject.SetActive(true);
				}
			}
			else{
				foreach(Transform child in transform){
					child.gameObject.SetActive(false);
				}
			}
		}
	}
}

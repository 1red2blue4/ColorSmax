using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPivotColor: MonoBehaviour
{
	private Vector3 rotation;
	public GameObject target;
	private float camDistance;
	public GameObject centerCube;
	public GameObject cubePrefab;
	public string colorSelected;
	//DONT USE DARK, LIGHT, AND GRAY
	//COLOR LEGEND: These are the colors for each index from 0 to 30
	//0 Blue, 1 light blue, 2 dark blue, 3 grey blue, 
	//4 yellow, 5 light yellow, 6 dark yellow, 7 grey yellow, 
	//8 red, 9 light red, 10 dark red, 11 grey red,
	//12 green, 13 light green, 14 dark green, 15 grey green, 
	//16 orange, 17 light orange, 18 dark orange, 19 grey orange, 
	//20 purp, 21 light purp, 22 dark purp, 23 grey purp,
	//24 brown, 25 light brown, 26 dark brown, 27 grey brown, 
	//28 white, 29 black, 30 grey
	public int[] side1Colors;
	public int[] side2Colors;
	public int[] side3Colors;
	public int[] side1Solution;
	public int[] side2Solution;
	public int[] side3Solution;
	public GameObject victory;
	public GameObject lightBeam;
	private bool recalculateBeams;
	private bool recalculateBeamsLate;
	public GameObject beamParent;
	public Color[] ColorArray;
	public bool side1Highlight;
	public bool side2Highlight;
	public bool side3Highlight;
	// Use this for initialization
	void Start ()
	{
		recalculateBeams=true;
		camDistance = -9;
		rotation = new Vector3 (30,30,0);
		target = Instantiate (cubePrefab);
		side1Colors = new int[31];
		side2Colors = new int[31];
		side3Colors = new int[31];
		//generateSolution(2,2,3);
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(recalculateBeamsLate){
			recalculateBeams=true;
			recalculateBeamsLate=false;
		}
		//Camera Control
		if (Input.GetMouseButton (0)) {
			
			if (Input.mousePosition.x > (Screen.width * 2 / 3)) {
				rotation.y--;
				rotation.y--;
				recalculateBeams=true;
			}
			if (Input.mousePosition.x < (Screen.width * 1 / 3)) {
				rotation.y++;
				rotation.y++;
				recalculateBeams=true;
			}

			if (Input.mousePosition.y > (Screen.height * 2 / 3)) {
				rotation.x++;
				rotation.x++;
				recalculateBeams=true;
			}
			if (Input.mousePosition.y < (Screen.height * 1 / 3)) {
				rotation.x--;
				rotation.x--;
				recalculateBeams=true;
			}
			if(rotation.y<-180){
				rotation.y+=360;
			}
			if(rotation.y>180){
				rotation.y-=360;
			}
			if (rotation.x > 80)
				rotation.x = 80;
			if (rotation.x < -80)
				rotation.x = -80;
		}
		transform.rotation = Quaternion.Euler (rotation.x, rotation.y, rotation.z);
		//Camera Control
		target.GetComponent<CubeProperties> ().color = colorSelected;
		placeCubes ();

		if (Input.GetKeyDown (KeyCode.Alpha1)) {
			switch (colorSelected) {
			case "red":
				colorSelected = "yellow";
				break;
			case "yellow":
				colorSelected = "blue";
				break;
			case "blue":
				colorSelected = "red";//THIS LINE REMOVES WHITE AS A POSSIBLE COLOR
				break;
			case "white":
				colorSelected = "red";//THIS LINE REMOVES BLACK AS POSSIBLE COLOR
				break;
			case "black":
				colorSelected = "red";
				break;
			}

		}

			

		if (Input.GetMouseButtonDown (1)) {
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);

			if (Physics.Raycast (ray, out hit, 500) && hit.transform != centerCube.transform) {
				camDistance += .4f;
				Destroy (hit.transform.gameObject);
				transform.GetChild (0).localPosition = new Vector3 (0, 0, camDistance);
				recalculateBeamsLate=true;
			}
		}
		RaycastHit hit1;
		Ray ray1 = Camera.main.ScreenPointToRay (Input.mousePosition);

		if (Physics.Raycast (ray1, out hit1, 500)) {
			if (Input.GetMouseButtonDown (0) && target != null && target.transform.position.x < 200) {//Place Cube on Click
				target.GetComponent<BoxCollider> ().enabled = true;

				target = Instantiate (cubePrefab);
				camDistance -= .4f;
				transform.GetChild (0).localPosition = new Vector3 (0, 0, camDistance);
				recalculateBeams=true;
			}
		}

		int xMin = 1000;
		int xMax = -1000;
		int yMin = 1000;
		int yMax = -1000;
		int zMin = 1000;
		int zMax = -1000;
		object[] obj = GameObject.FindSceneObjectsOfType (typeof(GameObject));
		foreach (object o in obj) {
			GameObject g = (GameObject)o;
			if (g.transform.GetComponent<CubeProperties> () && g.transform.position.x < 200) {
				if (g.transform.position.x < xMin) {
					xMin = (int)g.transform.position.x;
				}
				if (g.transform.position.x > xMax) {
					xMax = (int)g.transform.position.x;
				}
				if (g.transform.position.y < yMin) {
					yMin = (int)g.transform.position.y;
				}
				if (g.transform.position.y > yMax) {
					yMax = (int)g.transform.position.y;
				}
				if (g.transform.position.z < zMin) {
					zMin = (int)g.transform.position.z;
				}
				if (g.transform.position.z > zMax) {
					zMax = (int)g.transform.position.z;
				}
			}
				
		}
		xMax += 1;
		yMax += 1;
		zMax += 1;
		int[] sideZ = new int[(xMax - xMin) * (yMax - yMin)];
		int[] sideY = new int[(zMax - zMin) * (xMax - xMin)];
		int[] sideX = new int[(yMax - yMin) * (zMax - zMin)];
		for (int j = 0; j < sideZ.Length; j++) {
			sideZ [j] = 1;
		}
		for (int j = 0; j < sideY.Length; j++) {
			sideY [j] = 1;
		}
		for (int j = 0; j < sideX.Length; j++) {
			sideX [j] = 1;
		}


		foreach (object o in obj) {
			GameObject g = (GameObject)o;
			if (g.transform.GetComponent<CubeProperties> () && g.transform.position.x < 200 && g.GetComponent<BoxCollider>().enabled) {
				int x = (int)g.transform.position.x - xMin;
				int y = (int)g.transform.position.y - yMin;
				int z = (int)g.transform.position.z - zMin;
				if (g.transform.GetComponent<CubeProperties> ().color == "red") {
					
					sideZ [x + y * (xMax - xMin)] *= 2;
					sideY [z + x * (zMax - zMin)] *= 2;
					sideX [y + z * (yMax - yMin)] *= 2;
				}
				if (g.transform.GetComponent<CubeProperties> ().color == "yellow") {
					sideZ [x + y * (xMax - xMin)] *= 3;
					sideY [z + x * (zMax - zMin)] *= 3;
					sideX [y + z * (yMax - yMin)] *= 3;
				}
				if (g.transform.GetComponent<CubeProperties> ().color == "blue") {
					sideZ [x + y * (xMax - xMin)] *= 5;
					sideY [z + x * (zMax - zMin)] *= 5;
					sideX [y + z * (yMax - yMin)] *= 5;
				}
				if (g.transform.GetComponent<CubeProperties> ().color == "white") {
					sideZ [x + y * (xMax - xMin)] *= 7;
					sideY [z + x * (zMax - zMin)] *= 7;
					sideX [y + z * (yMax - yMin)] *= 7;
				}
				if (g.transform.GetComponent<CubeProperties> ().color == "black") {
					sideZ [x + y * (xMax - xMin)] *= 11;
					sideY [z + x * (zMax - zMin)] *= 11;
					sideX [y + z * (yMax - yMin)] *= 11;
				}
			}
		}


		side1Colors = new int[31];
		side2Colors = new int[31];
		side3Colors = new int[31];
			
		foreach (int val in sideZ) {
			int index = 0;
			bool red = val % 2 == 0;
			bool blue = val % 5 == 0;
			bool yellow = val % 3 == 0;
			bool white = val % 7 == 0;
			bool black = val % 11 == 0;

			if (blue && !red && !yellow) {
				index = 0;
			} else if (!blue && !red && yellow) {
				index = 4;
			} else if (!blue && red && !yellow) {
				index = 8;
			} else if (blue && !red && yellow) {
				index = 12;
			} else if (!blue && red && yellow) {
				index = 16;
			} else if (blue && red && !yellow) {
				index = 20;
			} else if (blue && red && yellow) {
				index = 24;
			} else if (!blue && !red && !yellow && (white || black)) {
				index = 27;
			}
			if (white && !black) {
				index += 1;
			} else if (!white && black) {
				index += 2;
			} else if (white && black) {
				index += 3;
			}
			if(blue||red||yellow||white||black)
			side1Colors [index] += 1;


				
		}

		foreach (int val in sideY) {
			int index = 0;
			bool red = val % 2 == 0;
			bool blue = val % 5 == 0;
			bool yellow = val % 3 == 0;
			bool white = val % 7 == 0;
			bool black = val % 11 == 0;

			if (blue && !red && !yellow) {
				index = 0;
			} else if (!blue && !red && yellow) {
				index = 4;
			} else if (!blue && red && !yellow) {
				index = 8;
			} else if (blue && !red && yellow) {
				index = 12;
			} else if (!blue && red && yellow) {
				index = 16;
			} else if (blue && red && !yellow) {
				index = 20;
			} else if (blue && red && yellow) {
				index = 24;
			} else if (!blue && !red && !yellow && (white || black)) {
				index = 27;
			}
			if (white && !black) {
				index += 1;
			} else if (!white && black) {
				index += 2;
			} else if (white && black) {
				index += 3;
			}
			if(blue||red||yellow||white||black)
			side2Colors [index] += 1;
		}

		foreach (int val in sideX) {
			int index = 0;
			bool red = val % 2 == 0;
			bool blue = val % 5 == 0;
			bool yellow = val % 3 == 0;
			bool white = val % 7 == 0;
			bool black = val % 11 == 0;

			if (blue && !red && !yellow) {
				index = 0;
			} else if (!blue && !red && yellow) {
				index = 4;
			} else if (!blue && red && !yellow) {
				index = 8;
			} else if (blue && !red && yellow) {
				index = 12;
			} else if (!blue && red && yellow) {
				index = 16;
			} else if (blue && red && !yellow) {
				index = 20;
			} else if (blue && red && yellow) {
				index = 24;
			} else if (!blue && !red && !yellow && (white || black)) {
				index = 27;
			}
			if (white && !black) {
				index += 1;
			} else if (!white && black) {
				index += 2;
			} else if (white && black) {
				index += 3;
			}
			if(blue||red||yellow||white||black)
			side3Colors [index] += 1;
		}

		bool s1s1=true;
		bool s1s2=true;
		bool s1s3=true;
		bool s2s1=true;
		bool s2s2=true;
		bool s2s3=true;
		bool s3s1=true;
		bool s3s2=true;
		bool s3s3=true;
		for(int i=0;i<31;i++){
			if(side1Colors[i]!=side1Solution[i]){
				s1s1=false;
			}
			if(side1Colors[i]!=side2Solution[i]){
				s1s2=false;
			}
			if(side1Colors[i]!=side3Solution[i]){
				s1s3=false;
			}
			if(side2Colors[i]!=side1Solution[i]){
				s2s1=false;
			}
			if(side2Colors[i]!=side2Solution[i]){
				s2s2=false;
			}
			if(side2Colors[i]!=side3Solution[i]){
				s2s3=false;
			}
			if(side3Colors[i]!=side1Solution[i]){
				s3s1=false;
			}
			if(side3Colors[i]!=side2Solution[i]){
				s3s2=false;
			}
			if(side3Colors[i]!=side3Solution[i]){
				s3s3=false;
			}
		}
		if(s1s1&s2s2&s3s3){
			victory.SetActive(true);
		}
		if(s1s1&s2s3&s3s2){
			victory.SetActive(true);
		}
		if(s1s2&s2s3&s3s1){
			victory.SetActive(true);
		}
		if(s1s2&s2s1&s3s3){
			victory.SetActive(true);
		}
		if(s1s3&s2s1&s3s2){
			victory.SetActive(true);
		}
		if(s1s3&s2s2&s3s1){
			victory.SetActive(true);
		}

		side1Highlight=s1s1|s2s1|s3s1;
		side2Highlight=s1s2|s2s2|s3s2;
		side3Highlight=s1s3|s2s3|s3s3;


		if(recalculateBeams){
			foreach(Transform child in beamParent.transform){
				Destroy(child.gameObject);
			}
			int xSize=xMax-xMin;
			int ySize=yMax-yMin;
			int zSize=zMax-zMin;

			for(int x=0;x<xSize;x++){
				for(int y=0;y<ySize;y++){
					for(int z=0;z<zSize;z++){
						if(sideZ [x + y * xSize]>1){
							GameObject temp=Instantiate(lightBeam);
							temp.transform.position=new Vector3(x+xMin,y+yMin,zMin);
							temp.transform.parent=beamParent.transform;
							temp.GetComponent<ParticleSystem>().startColor=colorCalculate(sideZ [x + y * xSize]);
							if(rotation.y>90 || rotation.y<-80){
								temp.transform.position=new Vector3(x+xMin,y+yMin,zMin-1);
								temp.transform.rotation=Quaternion.Euler(180,0,0);
							}
							else{
								temp.transform.position=new Vector3(x+xMin,y+yMin,zMax);
								temp.transform.rotation=Quaternion.Euler(0,0,0);
							}

						}
						if(sideY [z + x * zSize]>1){
							GameObject temp=Instantiate(lightBeam);
							temp.transform.position=new Vector3(x+xMin,yMin,z+zMin);
							temp.transform.parent=beamParent.transform;
							temp.GetComponent<ParticleSystem>().startColor=colorCalculate(sideY [z + x * zSize]);
							if(rotation.x>0){
								temp.transform.position=new Vector3(x+xMin,yMin-1,z+zMin);
								temp.transform.rotation=Quaternion.Euler(90,0,0);
							}
							else{
								temp.transform.position=new Vector3(x+xMin,yMax,z+zMin);
								temp.transform.rotation=Quaternion.Euler(-90,0,0);
							}
						}
						if(sideX [y + z * ySize]>1){
							GameObject temp=Instantiate(lightBeam);
							temp.transform.parent=beamParent.transform;
							temp.GetComponent<ParticleSystem>().startColor=colorCalculate(sideX [y + z * ySize]);
							if(rotation.y>0){
								temp.transform.position=new Vector3(xMax,y+yMin,z+zMin);
								temp.transform.rotation=Quaternion.Euler(0,90,90);
							}
							else{
								temp.transform.position=new Vector3(xMin-1,y+yMin,z+zMin);
								temp.transform.rotation=Quaternion.Euler(0,-90,-90);
							}
						}
					}
				}
			}
		}
		recalculateBeams=false;
	}


	Color colorCalculate(int val){
		int index = 0;
		bool red = val % 2 == 0;
		bool blue = val % 5 == 0;
		bool yellow = val % 3 == 0;
		bool white = val % 7 == 0;
		bool black = val % 11 == 0;
		if (blue && !red && !yellow) {
			index = 0;
		} else if (!blue && !red && yellow) {
			index = 4;
		} else if (!blue && red && !yellow) {
			index = 8;
		} else if (blue && !red && yellow) {
			index = 12;
		} else if (!blue && red && yellow) {
			index = 16;
		} else if (blue && red && !yellow) {
			index = 20;
		} else if (blue && red && yellow) {
			index = 24;
		} else if (!blue && !red && !yellow && (white || black)) {
			index = 27;
		}
		if (white && !black) {
			index += 1;
		} else if (!white && black) {
			index += 2;
		} else if (white && black) {
			index += 3;
		}

		return ColorArray[index];


	}

	private void placeCubes ()
	{
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);

		if (Physics.Raycast (ray, out hit, 500)) {
			Vector3 offset = hit.point - hit.transform.position;
			if (Mathf.Abs (offset.x) >= Mathf.Abs (offset.y) && Mathf.Abs (offset.x) >= Mathf.Abs (offset.z)) {
				if (offset.x > 0) {
					target.transform.position = new Vector3 (hit.transform.position.x + 1, hit.transform.position.y, hit.transform.position.z);
				} else {
					target.transform.position = new Vector3 (hit.transform.position.x - 1, hit.transform.position.y, hit.transform.position.z);
				}
			}
			if (Mathf.Abs (offset.y) >= Mathf.Abs (offset.x) && Mathf.Abs (offset.y) >= Mathf.Abs (offset.z)) {
				if (offset.y > 0) {
					target.transform.position = new Vector3 (hit.transform.position.x, hit.transform.position.y + 1, hit.transform.position.z);
				} else {
					target.transform.position = new Vector3 (hit.transform.position.x, hit.transform.position.y - 1, hit.transform.position.z);
				}
			}
			if (Mathf.Abs (offset.z) > Mathf.Abs (offset.y) && Mathf.Abs (offset.z) > Mathf.Abs (offset.x)) {
				if (offset.z > 0) {
					target.transform.position = new Vector3 (hit.transform.position.x, hit.transform.position.y, hit.transform.position.z + 1);
				} else {
					target.transform.position = new Vector3 (hit.transform.position.x, hit.transform.position.y, hit.transform.position.z - 1);
				}
			}

		} else {
			target.transform.position = new Vector3 (300, 0, 0);
		}
	}

	void generateSolution(int x,int y,int z){
		int[] xyzArray=new int[x*y*z];
		//b=1,y=2,r=3,w=4
		for(int i=0;i<x*y*z;i++){
			xyzArray[i]=Random.Range(0,8);//Modulates number of blank spaces on average
			if(xyzArray[i]>4)xyzArray[i]=0;
		}

		for(int i=0;i<x*y*z;i++){//This isn't enough. Two floating blocks satisfies this
			if(xyzArray[i]>0){
				bool hasNeighbor=false;
				if(i-1>=0){
					if(xyzArray[i-1]>0){
						hasNeighbor=true;
					}
				}
				if(i+1<x*y*z){
					if(xyzArray[i+1]>0){
						hasNeighbor=true;
					}
				}
				if(i+x<x*y*z){
					if(xyzArray[i+x]>0){
						hasNeighbor=true;
					}
				}
				if(i-x>=0){
					if(xyzArray[i-x]>0){
						hasNeighbor=true;
					}
				}
				if(i+(y*x)<x*y*z){
					if(xyzArray[i+(x*y)]>0){
						hasNeighbor=true;
					}
				}
				if(i-(x*y)>=0){
					if(xyzArray[i-(x*y)]>0){
						hasNeighbor=true;
					}
				}
				if(!hasNeighbor){
					xyzArray[i]=0;
				}
			}
		}


		int[] sideZ = new int[x*y];
		int[] sideY = new int[z*x];
		int[] sideX = new int[y*z];
		for (int j = 0; j < sideZ.Length; j++) {
			sideZ [j] = 1;
		}
		for (int j = 0; j < sideY.Length; j++) {
			sideY [j] = 1;
		}
		for (int j = 0; j < sideX.Length; j++) {
			sideX [j] = 1;
		}

		for(int i=0;i<x*y*z;i++){
			int xPos=i%x;
			int yPos=(i/x)%y;
			int zPos=i/y/x;
			if(xyzArray[i]==1){//blue
				sideZ [xPos + yPos * x] *= 5;
				sideY [zPos + xPos * z] *= 5;
				sideX [yPos + zPos * y] *= 5;
			}
			if(xyzArray[i]==2){//yellow
				sideZ [xPos + yPos * x] *= 3;
				sideY [zPos + xPos * z] *= 3;
				sideX [yPos + zPos * y] *= 3;
			}
			if(xyzArray[i]==3){//red
				sideZ [xPos + yPos * x] *= 2;
				sideY [zPos + xPos * z] *= 2;
				sideX [yPos + zPos * y] *= 2;
			}
			if(xyzArray[i]==4){//white
				sideZ [xPos + yPos * x] *= 7;
				sideY [zPos + xPos * z] *= 7;
				sideX [yPos + zPos * y] *= 7;
			}
		}


		side1Solution = new int[31];
		side2Solution = new int[31];
		side3Solution = new int[31];

		foreach (int val in sideZ) {
			int index = 0;
			bool red = val % 2 == 0;
			bool blue = val % 5 == 0;
			bool yellow = val % 3 == 0;
			bool white = val % 7 == 0;
			bool black = val % 11 == 0;

			if (blue && !red && !yellow) {
				index = 0;
			} else if (!blue && !red && yellow) {
				index = 4;
			} else if (!blue && red && !yellow) {
				index = 8;
			} else if (blue && !red && yellow) {
				index = 12;
			} else if (!blue && red && yellow) {
				index = 16;
			} else if (blue && red && !yellow) {
				index = 20;
			} else if (blue && red && yellow) {
				index = 24;
			} else if (!blue && !red && !yellow && (white || black)) {
				index = 27;
			}
			if (white && !black) {
				index += 1;
			} else if (!white && black) {
				index += 2;
			} else if (white && black) {
				index += 3;
			}
			if(blue||red||yellow||white||black)
				side1Solution [index] += 1;



		}

		foreach (int val in sideY) {
			int index = 0;
			bool red = val % 2 == 0;
			bool blue = val % 5 == 0;
			bool yellow = val % 3 == 0;
			bool white = val % 7 == 0;
			bool black = val % 11 == 0;

			if (blue && !red && !yellow) {
				index = 0;
			} else if (!blue && !red && yellow) {
				index = 4;
			} else if (!blue && red && !yellow) {
				index = 8;
			} else if (blue && !red && yellow) {
				index = 12;
			} else if (!blue && red && yellow) {
				index = 16;
			} else if (blue && red && !yellow) {
				index = 20;
			} else if (blue && red && yellow) {
				index = 24;
			} else if (!blue && !red && !yellow && (white || black)) {
				index = 27;
			}
			if (white && !black) {
				index += 1;
			} else if (!white && black) {
				index += 2;
			} else if (white && black) {
				index += 3;
			}
			if(blue||red||yellow||white||black)
				side2Solution [index] += 1;
		}

		foreach (int val in sideX) {
			int index = 0;
			bool red = val % 2 == 0;
			bool blue = val % 5 == 0;
			bool yellow = val % 3 == 0;
			bool white = val % 7 == 0;
			bool black = val % 11 == 0;

			if (blue && !red && !yellow) {
				index = 0;
			} else if (!blue && !red && yellow) {
				index = 4;
			} else if (!blue && red && !yellow) {
				index = 8;
			} else if (blue && !red && yellow) {
				index = 12;
			} else if (!blue && red && yellow) {
				index = 16;
			} else if (blue && red && !yellow) {
				index = 20;
			} else if (blue && red && yellow) {
				index = 24;
			} else if (!blue && !red && !yellow && (white || black)) {
				index = 27;
			}
			if (white && !black) {
				index += 1;
			} else if (!white && black) {
				index += 2;
			} else if (white && black) {
				index += 3;
			}
			if(blue||red||yellow||white||black)
				side3Solution [index] += 1;
		}
	}
}

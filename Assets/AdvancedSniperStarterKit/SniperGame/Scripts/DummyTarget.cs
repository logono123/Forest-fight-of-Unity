using UnityEngine;
using System.Collections;

public class DummyTarget : MonoBehaviour {

	public GameObject Target;
	Vector3 positionTemp;
	Quaternion rotationTemp;
	float timeTemp;
	bool hited = false;
	
	void Start () {
		if(Target){
			positionTemp = Target.transform.position;
			rotationTemp = Target.transform.rotation;
		}
		timeTemp = Time.time;
	}
	

	void Update () {
		if(Target){
		if(!hited){
			if(Target.transform.rotation != rotationTemp){
				hited = true;
				timeTemp = Time.time;
			}
		}else{
			if(Time.time > timeTemp + (5 * Time.timeScale)){
				Target.transform.position = positionTemp;
				Target.transform.rotation = rotationTemp;
				hited = false;
			}	
		}
		}
	}
}

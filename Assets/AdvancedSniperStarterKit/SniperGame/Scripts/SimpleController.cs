using UnityEngine;
using System.Collections;

public class SimpleController : MonoBehaviour {

 	public float Speed = 3;
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.UpArrow)){
			this.transform.position += new Vector3(0,0,1) * Speed * Time.deltaTime;
		}
		if (Input.GetKey (KeyCode.DownArrow)){
			this.transform.position += new Vector3(0,0,-1) * Speed * Time.deltaTime;
		}
		if (Input.GetKey (KeyCode.LeftArrow)){
			this.transform.position += new Vector3(-1,0,0) * Speed * Time.deltaTime;
		}
		if (Input.GetKey (KeyCode.RightArrow)){
			this.transform.position += new Vector3(1,0,0) * Speed * Time.deltaTime;
		}
		
	}
}

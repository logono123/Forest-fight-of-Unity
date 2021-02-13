using UnityEngine;
using System.Collections;

public class FreeCamera : MonoBehaviour {

	public float Speed = 10;
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		float speedmult = 1;
		if (Input.GetKey (KeyCode.LeftShift)) {
			speedmult = 2;
		}
		this.transform.position += ((this.transform.forward * Input.GetAxis ("Vertical")) + (this.transform.right * Input.GetAxis ("Horizontal"))) * Speed * speedmult * Time.deltaTime;
	}
}

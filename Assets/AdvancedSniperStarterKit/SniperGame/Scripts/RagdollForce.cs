using UnityEngine;
using System.Collections;

public class RagdollForce : MonoBehaviour {

	public Vector3 startingVelocity = Vector3.zero;
	void Start () {
		foreach(Rigidbody body in this.gameObject.GetComponentsInChildren<Rigidbody>())
		{
			body.velocity = startingVelocity;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

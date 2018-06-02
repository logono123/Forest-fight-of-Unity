using UnityEngine;
using System.Collections;

public class CameraClone : MonoBehaviour {

	public Camera Source;
	public bool CopyFOV = false;
	void Start () {

		if(GetComponent<Camera>()){
        	float[] distances = new float[32];
        	distances[10] = 15;
        	GetComponent<Camera>().layerCullDistances = distances;
		}
	}

	void FixedUpdate () {
		if(Source && GetComponent<Camera>()){
			if(CopyFOV)
			GetComponent<Camera>().fieldOfView = Source.fieldOfView;
			GetComponent<Camera>().cullingMask = Source.cullingMask;

			GetComponent<Camera>().transform.position = Source.transform.position;
			GetComponent<Camera>().transform.rotation = Source.transform.rotation;
			
			GetComponent<Camera>().transform.localRotation = Source.transform.localRotation;
			GetComponent<Camera>().transform.localPosition = Source.transform.localPosition;
		}
	}
}

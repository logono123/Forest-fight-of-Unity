using UnityEngine;
using System.Collections;

public class MuzzleFlash : MonoBehaviour
{

	public float Speed = 1;
	Light lightComp;

	void Start ()
	{
		lightComp = this.GetComponent<Light> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (lightComp != null) {
			if (lightComp.intensity > 0)
				lightComp.intensity -= Speed * Time.deltaTime;

			if (lightComp.intensity < 0.05)
				lightComp.intensity = 0;
		}
	}
}

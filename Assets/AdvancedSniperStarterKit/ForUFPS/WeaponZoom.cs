using UnityEngine;
using System.Collections;

public class WeaponZoom : MonoBehaviour
{

	public Texture2D CrosshairImg, CrosshairZoom;
	public Camera FPSCamera;
	public float ThresholdFOV = 45;
	private bool IsZoom;
	private GameObject[] objectTemp;
	private bool saved = false;

	void Start ()
	{
		SaveScale ();
		if (FPSCamera == null) {
			if (gameObject.transform.parent){
				GameObject fpscam = GameObject.Find ("FPSCamera");	
				if(fpscam.GetComponent<Camera>())
					FPSCamera = fpscam.GetComponent<Camera>();
			}
		}
	}

	void SaveScale ()
	{
		Renderer[] gos = (Renderer[])this.gameObject.GetComponentsInChildren<Renderer> ();
		objectTemp = new GameObject[gos.Length];
		for (int i=0; i<gos.Length; i++) {
			objectTemp [i] = gos [i].gameObject;	
		}	
	}
	
	void Hiding (bool show)
	{
		if (!saved) {
			// must save all the parts of gun in temp once
			// so we can active it anytime.
			SaveScale ();	
			saved = true;
		}
		
		for (int i=0; i<objectTemp.Length; i++) {
			if (objectTemp [i].gameObject) {
				if (!show) {
					objectTemp [i].gameObject.SetActive (show);
				} else {
					objectTemp [i].gameObject.SetActive (show);
				}
			}
		}
	}
	
	void Update ()
	{
		if (FPSCamera) {
			if (FPSCamera.fieldOfView <= ThresholdFOV && FPSCamera.enabled) {
				IsZoom = true;
				Hiding (false);
			} else {
				IsZoom = false;
				Hiding (true);
			}
		}
	}
	
	void OnGUI ()
	{
		if (!IsZoom) {
			if (CrosshairImg) {
				GUI.color = new Color (1, 1, 1, 0.8f);		
				GUI.DrawTexture (new Rect ((Screen.width * 0.5f) - (CrosshairImg.width * 0.5f), (Screen.height * 0.5f) - (CrosshairImg.height * 0.5f), CrosshairImg.width, CrosshairImg.height), CrosshairImg);
				GUI.color = Color.white;
			}
		} else {
			if (CrosshairZoom) {
				float scopeSize = (Screen.height * 1.8f);
				GUI.DrawTexture (new Rect ((Screen.width * 0.5f) - (scopeSize * 0.5f), (Screen.height * 0.5f) - (scopeSize * 0.5f), scopeSize, scopeSize), CrosshairZoom);	
			}
		}	
	}
}

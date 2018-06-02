using UnityEngine;
using System.Collections;

public class Crosshair : MonoBehaviour {

	public Texture2D CrosshairImg;
	void Start () {
	
	}
	
	void OnGUI(){
		if(CrosshairImg){
			GUI.color = new Color(1, 1, 1, 0.8f);
			GUI.DrawTexture(new Rect((Screen.width * 0.5f) - (CrosshairImg.width * 0.5f),(Screen.height * 0.5f) - (CrosshairImg.height * 0.5f), CrosshairImg.width,CrosshairImg.height), CrosshairImg);
			GUI.color = Color.white;
		}
	}
}

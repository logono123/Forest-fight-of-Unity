using UnityEngine;
using System.Collections;

public class ScoreManager : MonoBehaviour {

	public GUISkin skin;
	public int Score;
	public float BestDistance;
	private float lastestDistance;
	private float timeTemp = 0;
	private bool showdistance = false;
	private int scorePlus = 0;
	
	void Start () {
		Score = 0;
		BestDistance = 0;
	}

	void Update () {
		if(showdistance){
			if(Time.time >= timeTemp + 3){
				showdistance = false;	
			}
		}
	}
	
	public void AddScore(int score,float distance){
		int bonus = 1;
		
		if(distance>500)
			bonus = 2;
		
		if(distance>1000)
			bonus = 5;
		
		if(distance>1500)
			bonus = 10;
		
		scorePlus = score * (((int)(distance * 0.1) + 1)) * bonus;
		Score += scorePlus;
		lastestDistance = distance;
		if(distance>BestDistance){
			BestDistance = distance;
		}
		
		showdistance = true;
		timeTemp = Time.time;
	}
	
	void OnGUI(){
		if(skin){
			GUI.skin = skin;
		}
		GUI.skin.label.fontSize = 35;
		GUI.skin.label.normal.textColor = Color.black;
		GUI.skin.label.alignment = TextAnchor.MiddleLeft;
		GUI.Label(new Rect(20,20,300,40),"SCORE "+Score);
		GUI.skin.label.fontSize = 20;
		GUI.Label(new Rect(20,60,300,40),"BEST "+Mathf.Floor(BestDistance)+" M.");
		
		if(showdistance){
			GUI.skin.label.fontSize = 35;
			GUI.skin.label.alignment = TextAnchor.MiddleCenter;
			GUI.Label(new Rect((Screen.width * 0.5f) - 150,(Screen.height*0.5f)-50,300,100),Mathf.Floor(lastestDistance)+" M.\n+"+scorePlus);
		}
	}
}

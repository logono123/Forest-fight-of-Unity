using UnityEngine;
using System.Collections;

public class HowTo : MonoBehaviour {

	string presetname = "Random Presets";
	private AS_ActionCamera actioncam;
	
	void Start(){
		actioncam = (AS_ActionCamera)GameObject.FindObjectOfType(typeof(AS_ActionCamera));
	}
	
	
	void Update(){
			
		if(!actioncam)
			return;
		
		if(Input.GetKeyDown(KeyCode.F)){
			actioncam.PresetIndex += 1;
			presetname = "Preset "+(actioncam.PresetIndex);
			
			if(actioncam.PresetIndex>=actioncam.ActionPresets.Length){
				actioncam.PresetIndex = -1;	
				presetname = "Random Presets";
			}
		}		
	}
	void OnGUI(){
		GUI.TextArea(new Rect(20,20,300,170),"\n  How to play\n\n   - Shoot :Left Mouse\n   - Zoom :Right Mouse\n   - Zoom Scale:Scroll Mouse\n   - Keep Breath :Hold Left Shift\n   - Switch Guns :Q \n   - Change Bullet Action :F \n     "+presetname);
		if(Input.GetKeyDown(KeyCode.Escape)){
			//Application.LoadLevel(0);
		}
	
	}
	
}

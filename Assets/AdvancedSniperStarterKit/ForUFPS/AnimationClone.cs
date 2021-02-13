using UnityEngine;
using System.Collections;

public class AnimationClone : MonoBehaviour {

	public Animation Source;
	void Start () {
		if(this.GetComponent<Animation>() && Source)
			this.GetComponent<Animation>().clip = Source.clip;
	}
	
	// Update is called once per frame
	void Update () {
		if(this.GetComponent<Animation>() && Source){
			this.GetComponent<Animation>().clip = Source.clip;
			
			if(Source.isPlaying){
				this.GetComponent<Animation>().Play();
			}
		}
		
	}
}

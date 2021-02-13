using UnityEngine;
using System.Collections;
[RequireComponent(typeof(AudioSource))]

public class AS_SoundOnHit : MonoBehaviour {

	private AudioSource audiosource;
	public AudioClip[] Sounds;
	void Start () {
		if(GetComponent<AudioSource>() != null && Sounds!=null && Sounds.Length>0){
			audiosource = GetComponent<AudioSource>();	
			audiosource.pitch = Time.timeScale;
			if(audiosource.pitch<0.5f){
				audiosource.pitch = 0.5f;
			}
			audiosource.PlayOneShot(Sounds[Random.Range(0,Sounds.Length)]);
		}
	}
	void Update(){
		if(audiosource != null && Sounds!=null && Sounds.Length>0){
			audiosource.pitch = Time.timeScale;
			if(audiosource.pitch<0.5f){
				audiosource.pitch = 0.5f;
			}
		
		}
	}
	

}

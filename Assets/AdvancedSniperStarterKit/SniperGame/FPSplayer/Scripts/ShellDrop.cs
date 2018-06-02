using UnityEngine;
using System.Collections;
[RequireComponent(typeof(AudioSource))]

public class ShellDrop : MonoBehaviour {
	private AudioSource audiosource;
	public GameObject ParticleHit;
	public AudioClip[] Sounds;
	
	void Start () {
		if(GetComponent<AudioSource>() != null && Sounds!=null && Sounds.Length>0){
			audiosource = GetComponent<AudioSource>();
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
	void OnCollisionEnter(Collision collision) {
		if(audiosource != null && Sounds!=null && Sounds.Length>0){
			audiosource.PlayOneShot(Sounds[Random.Range(0,Sounds.Length)]);
		}
		if(ParticleHit){
			GameObject particle = (GameObject)GameObject.Instantiate(ParticleHit,this.transform.position,this.transform.rotation);
			GameObject.Destroy(particle,3);
		}
	}
}

/// <summary>
/// Enemy spawner. auto Re-Spawning an Enemy by Random index of Objectman[]
/// </summary>
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class EnemySpawner : MonoBehaviour
{

	public GameObject[] Objectman;
	public float TimeSpawn = 3;
	public int MaxObject = 10;
	public string PlayerTag = "Player";
	public bool PlayerEnter = true;
	private float timetemp = 0;
	private int indexSpawn;
	private List<GameObject> spawnList = new List<GameObject> ();
	public bool OnActive;
	
	void Start ()
	{
		indexSpawn = Random.Range (0, Objectman.Length);
		timetemp = Time.time;
	}

	void Update ()
	{
		OnActive = false;
		if (PlayerEnter) {
			GameObject[] playersaround = GameObject.FindGameObjectsWithTag (PlayerTag);
			for (int p=0; p<playersaround.Length; p++) {
				if (Vector3.Distance (this.transform.position, playersaround [p].transform.position) < this.transform.localScale.x) {
					OnActive = true;
				}
			}
		} else {
			OnActive = true;
		}
		
		bool offlineMode = (!Network.isServer && !Network.isClient);

		if (!OnActive)
			return;
		
		ObjectExistCheck ();
		if (Objectman [indexSpawn] == null)
			return;
		

		if (ObjectsNumber < MaxObject && Time.time > timetemp + TimeSpawn) {
			timetemp = Time.time;
			GameObject obj = null;
			Vector3 spawnPoint = DetectGround (transform.position + new Vector3 (Random.Range (-(int)(this.transform.localScale.x / 2.0f), (int)(this.transform.localScale.x / 2.0f)),0, Random.Range ((int)(-this.transform.localScale.z / 2.0f), (int)(this.transform.localScale.z / 2.0f))));
			if (!offlineMode) {
				if (Network.isServer)
					obj = (GameObject)Network.Instantiate (Objectman [indexSpawn], spawnPoint, Quaternion.identity, 0);
			} else {
				obj = (GameObject)GameObject.Instantiate (Objectman [indexSpawn], spawnPoint, Quaternion.identity);
			}
			if (obj)
				spawnList.Add (obj);
			indexSpawn = Random.Range (0, Objectman.Length);
			
		}
	}
	
	private int ObjectsNumber;

	void ObjectExistCheck ()
	{
		ObjectsNumber = 0;
		foreach (var obj in spawnList) {
			if (obj != null)
				ObjectsNumber++;
		}
	}
	
	void OnDrawGizmos ()
	{
		#if UNITY_EDITOR
		Gizmos.color = Color.red;
		Gizmos.DrawSphere (transform.position, 2);
		Gizmos.DrawWireCube (transform.position, this.transform.localScale);
		Handles.Label(transform.position, "Enemy Spawner");
		#endif
	}
	
	Vector3 DetectGround (Vector3 position)
	{
		RaycastHit hit;
		if (Physics.Raycast (position, -Vector3.up, out hit, 1000.0f)) {
			return hit.point;
		}
		return position;
	}
	
}

using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {

	public GameObject Objectman;
	private float timetemp = 0;
	public float timeSpawn = 3;
	public int enemyCount = 0;
	public int radiun;
	void Start () {
		if(GetComponent<Renderer>())
  			GetComponent<Renderer>().enabled = false;
		timetemp = Time.time;
	}

	void Update () {
   		GameObject[] gos = GameObject.FindGameObjectsWithTag(Objectman.tag);

   		if(gos.Length < enemyCount){
   			if(Time.time > timetemp+timeSpawn){
   	  			timetemp = Time.time;
				
   	  			GameObject.Instantiate(Objectman, TerrainFloor(transform.position+ new Vector3(Random.Range(-radiun,radiun),this.transform.position.y,Random.Range(-radiun,radiun))), Quaternion.identity);
   	  		}
   		}
	}
	
	public RaycastHit PositionOnTerrain(Vector3 position){
		RaycastHit res = new RaycastHit();
		res.point = position;
		if(GameObject.FindObjectOfType(typeof(Terrain))){
			Terrain terrain = (Terrain)GameObject.FindObjectOfType(typeof(Terrain));
			if(terrain){
				RaycastHit hit;
        		if (Physics.Raycast(position, -Vector3.up,out hit)) {
            		res = hit;
        		}
			}else{
				Debug.Log("No Terrain");	
			}	
		}
		return res;
	}
	public Vector3 TerrainFloor(Vector3 position){
		Vector3 res = position;
		RaycastHit positionSpawn = PositionOnTerrain(position + (Vector3.up * 100));
		
		res = new Vector3(position.x,positionSpawn.point.y+1,position.z);
		
		return res;
	}
}

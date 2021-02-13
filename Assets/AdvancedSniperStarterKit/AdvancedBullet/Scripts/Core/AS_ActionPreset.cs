using UnityEngine;
using System.Collections;

public class AS_ActionPreset : MonoBehaviour
{
	[HideInInspector]
	public AS_ActionCamera ActionCam;

	
	public void Start ()
	{
		Initialize ();
	}

	public void Initialize ()
	{
		ActionCam = (AS_ActionCamera)FindObjectOfType (typeof(AS_ActionCamera));
	}
	// When shooting.
	public virtual void Shoot (GameObject bullet)
	{
		//Debug.Log("State : Shoot");
	}
	// When bullet start and detected a target on the way.
	public virtual void FirstDetected (AS_Bullet bullet, AS_BulletHiter target, Vector3 point)
	{
		if (!ActionCam) {
			return;	
		}
		ActionCam.Detected = true;
		ActionCam.ObjectLookAtRoot = target.RootObject;
		//Debug.Log("State : FirstDetected");
	}
	// When bullet flying and detected a target on the way.
	public virtual void TargetDetected (AS_Bullet bullet, AS_BulletHiter target, Vector3 point)
	{
		if (!ActionCam) {
			return;	
		}
		//Debug.Log ("Running Detect target : " + target.name + " > "+ActionCam.Detected + " time "+Time.time);
		ActionCam.Detected = true;
		ActionCam.ObjectLookAtRoot = target.RootObject;
		//Debug.Log("State : TargetDetected");
	}
	// When bullet hit target
	public virtual void TargetHited (AS_Bullet bullet, AS_BulletHiter target, Vector3 point)
	{
		if (!ActionCam) {
			return;	
		}
		//Debug.Log("Hit : "+target.name + " > "+ ActionCam.Detected +" time "+Time.time);
		ActionCam.HitTarget = true;
		//ActionCam.ObjectLookAtRoot = target.RootObject;
		ActionCam.PositionHit = point;
		//Debug.Log("State : TargetHited");
	}
	
	//When bullet has been removed
	public virtual void OnBulletDestroy ()
	{
		
		if (!ActionCam) {
			return;	
		}
		
		if (!ActionCam.HitTarget) {
			ActionCam.ClearTarget ();
			ActionCam.TimeSet (1);
		}
		//Debug.Log("State : OnBulletDestroy");
	}
	

	
	// Checking prevent the Camera stay down under Terrain
	public RaycastHit PositionOnTerrain (Vector3 position)
	{
		RaycastHit res = new RaycastHit ();
		res.point = position;
		if (GameObject.FindObjectOfType (typeof(Terrain))) {
			Terrain terrain = (Terrain)GameObject.FindObjectOfType (typeof(Terrain));
			if (terrain) {
				RaycastHit hit;
				if (Physics.Raycast (position, -Vector3.up, out hit)) {
					res = hit;
				}
			} else {
				Debug.Log ("No Terrain");	
			}	
		}
		return res;
	}

	public Vector3 TerrainFloor (Vector3 position)
	{
		Vector3 res = position;
		RaycastHit positionSpawn = PositionOnTerrain (position + (Vector3.up * 100));
		if (positionSpawn.point.y > position.y) {
			res = new Vector3 (position.x, positionSpawn.point.y + 1, position.z);
		}
		return res;
	}
	
	public Vector3 GetRandomArea(Vector3 position,float size){
		return position + new Vector3(Random.Range(-size,size),0,Random.Range(-size,size));	
	}
	
}

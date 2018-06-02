using UnityEngine;
using System.Collections;

public class AP_FastHit : AS_ActionPreset
{
	public float ZoomMulti = 1;
	public override void Shoot (GameObject bullet)
	{
		if (!ActionCam) {
			return;	
		}
		ActionCam.FOVTarget = 20;
		ActionCam.SetPosition (bullet.transform.position, false);
		base.Shoot (bullet);
	}
	
	public override void FirstDetected (AS_Bullet bullet, AS_BulletHiter target,Vector3 point)
	{
		if (!ActionCam) {
			return;	
		}
		if (!ActionCam.InAction) {
			ActionCam.Slowmotion (0.5f, 0.3f);
		}
		
		
		base.FirstDetected (bullet, target, point);
	}
	public override void TargetDetected (AS_Bullet bullet, AS_BulletHiter target,Vector3 point)
	{
		
		if (!ActionCam) {
			return;	
		}
		
		ActionCam.ObjectLookAt = target.gameObject;
		ActionCam.Follow = false;
		ActionCam.Slowmotion (0.2f, 0.2f);
		ActionCam.SetPositionDistance(target.transform.position,false);
		ActionCam.ActionBullet (2.0f);
		Debug.Log(">>>"+ActionCam.ObjectLookAt);
		base.TargetDetected (bullet, target, point);
	}
	
	public override void TargetHited (AS_Bullet bullet, AS_BulletHiter target,Vector3 point)
	{
		if (!ActionCam) {
			return;	
		}
		if(!ActionCam.Detected){
			ActionCam.SetPositionDistance(target.transform.position,false);
		}
		ActionCam.ResetFOV();
		ActionCam.ObjectFollowing = null;
		ActionCam.Follow = false;
		ActionCam.ObjectLookAt = null;
		base.TargetHited (bullet, target, point);
		
	}
}

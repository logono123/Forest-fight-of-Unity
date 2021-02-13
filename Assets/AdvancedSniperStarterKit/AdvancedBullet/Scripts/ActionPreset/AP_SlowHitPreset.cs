using UnityEngine;
using System.Collections;

public class AP_SlowHitPreset : AS_ActionPreset
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
		
		ActionCam.lookAtPosition = point;
		ActionCam.Follow = false;
		ActionCam.SlowmotionNow (0.02f, 3);
		ActionCam.SetPositionDistance(point,false);
		ActionCam.ActionBullet (3.0f);
		ActionCam.SetFOV(35,true);
		base.TargetDetected (bullet, target, point);
	}
	
	public override void TargetHited (AS_Bullet bullet, AS_BulletHiter target,Vector3 point)
	{
		if (!ActionCam) {
			return;	
		}
		if(!ActionCam.Detected){
			ActionCam.SetPositionDistance(point,false);
		}
		ActionCam.ActionBullet (3.0f);
		ActionCam.SlowmotionNow (0.01f, 3);
		
		ActionCam.ObjectFollowing = null;
		ActionCam.Follow = false;
		ActionCam.ObjectLookAt = null;
		base.TargetHited (bullet, target, point);
		
	}
	
}

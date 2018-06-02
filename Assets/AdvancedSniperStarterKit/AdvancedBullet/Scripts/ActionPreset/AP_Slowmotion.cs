using UnityEngine;
using System.Collections;

public class AP_Slowmotion : AS_ActionPreset
{
	public float ZoomMulti = 1;
	public override void Shoot (GameObject bullet)
	{
		if (!ActionCam) {
			return;	
		}
		ActionCam.ObjectFollowing = null;
		ActionCam.ObjectLookAt = null;
		ActionCam.InAction = false;
		ActionCam.Follow = false;
		ActionCam.FOVTarget = 20;
		base.Shoot (bullet);
	}
	
	public override void FirstDetected (AS_Bullet bullet, AS_BulletHiter target,Vector3 point)
	{
		if (!ActionCam) {
			return;	
		}
		if (!ActionCam.InAction) {
			ActionCam.Follow = true;
			ActionCam.SetPosition (bullet.transform.position + (bullet.transform.right * ZoomMulti) - (bullet.transform.forward * 2 * ZoomMulti), false);
			ActionCam.Slowmotion (0.1f, 1.0f);
		}
		
		
		base.FirstDetected (bullet, target, point);
	}
	public override void TargetDetected (AS_Bullet bullet, AS_BulletHiter target,Vector3 point)
	{
		
		if (!ActionCam) {
			return;	
		}
	
		base.TargetDetected (bullet, target, point);
	}
	
	public override void TargetHited (AS_Bullet bullet, AS_BulletHiter target,Vector3 point)
	{
		if (!ActionCam) {
			return;	
		}

		ActionCam.ClearTarget();
		ActionCam.ResetFOV();
		base.TargetHited (bullet, target, point);
		
	}
	
}

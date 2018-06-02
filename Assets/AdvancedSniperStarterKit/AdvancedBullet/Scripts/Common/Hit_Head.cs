using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CapsuleCollider))]

public class Hit_Head : AS_BulletHiter
{
	public int Suffix = 2;
	public float DamageMult = 3;
	public DamageManager damageManage;
	
	
	void Start(){
		if(damageManage == null){
			damageManage = this.RootObject.GetComponent<DamageManager> ();
		}
	}
	
	public override void OnHit (RaycastHit hit, AS_Bullet bullet)
	{
		float distance = Vector3.Distance (bullet.pointShoot, hit.point);
		if (damageManage) {
			int damage = (int)((float)bullet.Damage * DamageMult);
			damageManage.ApplyDamage (damage, bullet.transform.forward * bullet.HitForce, distance, Suffix);
		}
		AddAudio (hit.point);
		base.OnHit (hit, bullet);
	}
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

struct casthit
{
	public int index;
	public float distance;
	public string name;
}

[RequireComponent(typeof(Rigidbody))]
public class AS_Bullet : MonoBehaviour
{
	
	public GameObject ParticleHit;
	public bool ParticleSticky;
	public bool Homing = true;
	public int Damage = 10;
	public string DamageMethodName = "ApplyDamage";
	public string DoHitMethodName = "DoHit";
	public float MuzzleVelocity = 790;
	public float LifeTime = 3;
	public float DestroyDuration = 2;
	public int HitForce = 3000;
	public int HitCountMax = 10;
	public bool DestroyWhenHit = true;
	public float RunningRaylength = 40;
	public float FirstRaylength = 20;
	public float DetectorLength = 2000;
	public string[] IgnoreTag = {"Player"};
	public string[] DestroyerTag = {"scene"};
	public Vector3 WindSpeed;
	public LineRenderer lineRenderer;
	[HideInInspector]
	public float HitDistance;
	private bool hited = false;
	private bool firsthited = false;
	private bool destroyed = false;
	private AS_ActionPreset actionPreset;
	[HideInInspector]
	public Vector3 pointShoot;
	private const int ignoreWalkThru = ~((1 << 29) | (1 << 2) | (1 << 27) | (1 << 4) | (1 << 26));
	private Vector3 initialPosition;
	private Vector3 initialVelocity;
	private Vector3 initialDirection;
	private List<Collider> hittedList = new List<Collider> ();
	private GameObject targetLocked;
	private Vector3 targetLockedOffset;
	private Rigidbody rigidBodyComp;
	
	public void Awake ()
	{
		if(AS_SniperKit.Environment!=null)
			WindSpeed = AS_SniperKit.Environment.WindSpeed;

		rigidBodyComp = GetComponent<Rigidbody>();
		targetLocked = null;
		initialPosition = this.gameObject.transform.position;
		AS_ActionCamera actioncam = (AS_ActionCamera)GameObject.FindObjectOfType (typeof(AS_ActionCamera));

		if (actioncam != null) {
			actionPreset = actioncam.GetPresets ();
			if (actionPreset != null) {
				actionPreset.Shoot (this.gameObject);
			}
		}
		if(this.GetComponent<Renderer>())
			this.GetComponent<Renderer>().enabled = true;	
	}
	
	public void  Start ()
	{
		initialVelocity = this.transform.forward * MuzzleVelocity;
		initialDirection = this.transform.forward;
		//initialPosition = this.gameObject.transform.position;
		pointShoot = this.gameObject.transform.position;
		latestPosition = this.gameObject.transform.position;
		rigidBodyComp.mass = 1;
		rigidBodyComp.drag = 0;
		rigidBodyComp.angularDrag = 0;

		hited = false;
		firsthited = false;
		destroyed = false;
		
		//Debug.Log ("Shoot");
		if (!RayShoot (true)) {
			if (rigidBodyComp.useGravity) {
				PredictionTrajectory ();
			} else {
				FirstDetectTarget ();
			}
		}
		GameObject.Destroy (this.gameObject, LifeTime);
		rigidBodyComp.velocity = (initialVelocity);
		this.transform.forward = initialVelocity.normalized;
	}

	private bool tagCheck (string tag)
	{
		for (int i=0; i<IgnoreTag.Length; i++) {
			if (IgnoreTag [i] == tag) {
				return false;	
			}
		}
		return true;
	}

	private bool tagDestroyerCheck (string tag)
	{
		for (int i=0; i<DestroyerTag.Length; i++) {
			if (DestroyerTag [i] == tag) {
				return true;	
			}
		}
		return false;
	}

	private bool hitedCheck (Collider ob)
	{
		foreach (Collider trans in hittedList) {
			if (ob == trans) {
				return false;	
			}
		}
		hittedList.Add (ob);
		return true;
	}
	
	void FixedUpdate ()
	{
		if (rigidBodyComp) {
			
			// bullet always goes to a target like a missile
			if(targetLocked && Homing){
				Vector3 directionLocked = ((targetLocked.transform.position + targetLockedOffset) - this.transform.position).normalized;
				rigidBodyComp.velocity = directionLocked * MuzzleVelocity;	
			}
			
			// bullet always facing to a diraction it going to
			rigidBodyComp.velocity += (WindSpeed * Time.deltaTime);
			this.transform.forward = rigidBodyComp.velocity.normalized;
		}
		if (!destroyed) {
			// ray casting
			RayShoot (false);
			if (!hited)
				RunningDetectTarget ();
		}

		latestPosition = this.transform.position;
	}
	
	private float runningmMagnitude;

	void Update ()
	{
		// get running magnitude
		runningmMagnitude = (this.transform.position - latestPosition).magnitude;
		if (runningmMagnitude <= 0)
			runningmMagnitude = 0.2f;
		

	}
	
	private Vector3 latestPosition;
	private int hitcount;

	public bool RayShoot (bool first)
	{
		bool res = false;
		RaycastHit[] hits;
		List<casthit> castHits = new List<casthit> ();
		
		// calculate ray length by running magnitude.
		float raySize = runningmMagnitude;
		Vector3 direction = rigidBodyComp.velocity.normalized;

		if (first) {
			raySize = FirstRaylength;
			direction = initialDirection;
		}
		
		if (raySize <= 2.0f) {
			raySize = 2.0f;	
		}

		Vector3 pos1 = this.transform.position - (direction * raySize);
		//Vector3 pos2 = pos1 + (direction * raySize);

		if (first) {
			pos1 = initialPosition;
			//pos2 = pos1 + (direction * raySize);
		}
		
		// if you add line renderer. you can see how the ray is casting
		if (lineRenderer) {
			LineRenderer line = (LineRenderer)GameObject.Instantiate (lineRenderer, pos1, Quaternion.identity);
			line.SetPosition (0, pos1);
			line.SetPosition (1, pos1 + (raySize * direction));
			line.name = "laser " + raySize + " direction " + direction;
			GameObject.Destroy (line, 10);
		}


		// shoot ray to casting all objects
		int castcount = 0;
		RaycastHit[] casterhits = Physics.RaycastAll (pos1, direction, raySize, ignoreWalkThru);
		for (int i=0; i<casterhits.Length; i++) {
			if (casterhits [i].collider && Vector3.Dot ((casterhits [i].point - initialPosition).normalized, initialDirection) > 0.5f) {
				// checking a target tags. make sure it hit only taged object.
				if (tagCheck (casterhits [i].collider.tag) && casterhits [i].collider.gameObject != this.gameObject) {
					// checking for make sure it not hit the same object 2 time.
					if (hitedCheck (casterhits [i].collider)) {
						castcount++;
						casthit casted = new casthit ();
						casted.distance = Vector3.Distance (initialPosition, casterhits [i].point);
						casted.index = i;
						casted.name = casterhits [i].collider.name;
						castHits.Add (casted);
					}
				}
			}
		}

		// and sorted first to the last by distance
		hits = new RaycastHit[castcount];
		castHits.Sort ((x,y) => x.distance.CompareTo (y.distance));

		for (int i=0; i<castHits.Count; i++) {
			hits [i] = casterhits [castHits [i].index];
		}

		for (var i = 0; i < hits.Length && hitcount < HitCountMax; i++) {
			RaycastHit hit = hits [i];
	
			if (first) {
				firsthited = true;	
			} else {
				hited = true;	
			}

			GameObject hitparticle = null;
			GameObject flowparticle = null;
			targetLocked = null;
			float hitparticlelifetime = 3;
			float flowparticlelifetime = 3;

			if (hit.collider.GetComponent<Rigidbody>()) {
				hit.collider.GetComponent<Rigidbody>().AddForce (direction * HitForce, ForceMode.Force);
				
			}
			
			// get bullet hiter component.
			AS_BulletHiter bulletHit = hit.collider.gameObject.GetComponent<AS_BulletHiter> ();
							
			if (bulletHit != null) {
				// get particle from hiter object
				if (bulletHit.ParticleHit) {
					hitparticle = (GameObject)Instantiate (bulletHit.ParticleHit, hit.point, hit.transform.rotation);
				}
				if (bulletHit.ParticleFlow) {
					flowparticle = (GameObject)Instantiate (bulletHit.ParticleFlow, this.transform.position, hit.transform.rotation);
					flowparticle.transform.SetParent(this.transform);
				}
				// using action preset
				if (actionPreset && !firsthited) {
					actionPreset.TargetHited (this, bulletHit, hit.point);
				}
				// get particle life time from hiter
				hitparticlelifetime = bulletHit.ParticleLifeTime;
				flowparticlelifetime = bulletHit.ParticleParticleFlowLifeTime;
				bulletHit.OnHit (hit, this);
			} else {
				if (ParticleHit) {
					hitparticle = (GameObject)Instantiate (ParticleHit, hit.point, hit.transform.rotation);
				}
			}
			
			// call do damage function on an object that's got hit.
			hit.collider.SendMessageUpwards (DamageMethodName, (float)Damage, SendMessageOptions.DontRequireReceiver);
			this.SendMessageUpwards (DoHitMethodName, SendMessageOptions.DontRequireReceiver);	
			
			// just setup particle after hit to properly
			if (hitparticle != null) {
				hitparticle.transform.forward = hit.normal;
				if (ParticleSticky)
					hitparticle.transform.parent = hit.collider.transform;
				GameObject.Destroy (hitparticle, hitparticlelifetime);
			}
			if (flowparticle != null) {
				GameObject.Destroy (flowparticle, flowparticlelifetime);
			}
			
			// check a hit number if bullet is still hit under max count.
			res = true;
			hitcount++;
			if (DestroyWhenHit || hitcount >= HitCountMax || tagDestroyerCheck (hit.collider.tag)) {
				destroyed = true;

			}

		}
		if (destroyed) {
			if (actionPreset) {
				// using action preset
				actionPreset.OnBulletDestroy ();
			}
			// completely removed
			GameObject.Destroy (this.gameObject, DestroyDuration);
		}	

		return res;
	}
	
	private bool targetdetected = false;

	public void RunningDetectTarget ()
	{
		// detection any object while a bullet is running
		RaycastHit[] camerahits;
		camerahits = Physics.RaycastAll (transform.position, transform.forward, RunningRaylength);
		
		for (var i = 0; i < camerahits.Length; i++) {
			RaycastHit hitcam = camerahits [i];
			if (hitcam.collider) {
				if (tagCheck (hitcam.collider.tag) && hitcam.collider.gameObject != this.gameObject) {
					if (hitcam.collider.GetComponent<AS_BulletHiter> ()) {
						AS_BulletHiter bulletHit = hitcam.collider.gameObject.GetComponent<AS_BulletHiter> ();
						if (actionPreset && !firsthited && !targetdetected) {
							actionPreset.TargetDetected (this, bulletHit, hitcam.point);
							targetdetected = true;
							targetLocked = null;
						}
					}
				}
			}
		}
	}
	
	public void FirstDetectTarget ()
	{
		// detect first object at start
		RaycastHit[] camerahits;
		camerahits = Physics.RaycastAll (transform.position, transform.forward, DetectorLength);
		for (var i = 0; i < camerahits.Length; i++) {
			RaycastHit hitcam = camerahits [i];
			if (hitcam.collider) {
				if (tagCheck (hitcam.collider.tag) && hitcam.collider.gameObject != this.gameObject) {
					if (hitcam.collider.GetComponent<AS_BulletHiter> ()) {
						AS_BulletHiter bulletHit = hitcam.collider.gameObject.GetComponent<AS_BulletHiter> ();
						if (actionPreset && !firsthited) {
							actionPreset.FirstDetected (this, bulletHit, hitcam.point);
						}
					}
				}
			}
		}
	}
	
	void PredictionTrajectory ()
	{	
		// predict a line of running bullet.
		Vector3 gravity = Vector3.zero;
		if (rigidBodyComp.useGravity) {
			gravity = Physics.gravity;
		}
		int numSteps = (int)DetectorLength;
		float timeDelta = 1.0f / initialVelocity.magnitude;
 
		Vector3 position = initialPosition;
		Vector3 velocity = initialVelocity;
		Vector3 lastpos = initialPosition;
		bool targetdetected = false;

		for (int i = 0; i < numSteps && !targetdetected; ++i) {
			position += velocity * timeDelta + 0.5f * gravity * timeDelta * timeDelta;
			velocity += (gravity * timeDelta) + (WindSpeed * timeDelta);
			targetdetected = RayPrediction (lastpos, position, initialPosition, timeDelta);
			lastpos = position;
		}
	}

	bool RayPrediction (Vector3 lastpos, Vector3 currentpos, Vector3 initialPosition, float delta)
	{
		// ray casting with a line of prediction
		RaycastHit[] hits;
		Vector3 dir = (currentpos - lastpos);
		dir.Normalize ();

		hits = Physics.RaycastAll (lastpos, dir, 1);
		
		for (var i = 0; i < hits.Length; i++) {
			RaycastHit hit = hits [i];
			AS_BulletHiter bulletHit = hit.collider.gameObject.GetComponent<AS_BulletHiter> ();
			if (bulletHit) {
				targetLocked = bulletHit.gameObject;
				targetLockedOffset = hit.point - bulletHit.gameObject.transform.position;
				if (actionPreset && !firsthited) {
					actionPreset.FirstDetected (this, bulletHit, hit.point);
					return true;
				}
			}	
			
		}
		return false;
	}

}

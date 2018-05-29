using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class follow_farscene : NetworkBehaviour
{
    public Transform target ;
    public Vector3 offset = new Vector3(0, 11.98111f, -14.10971f);
    private float smoothing = 2;
    
	void Update () {
        Vector3 targetPosition = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothing * Time.deltaTime);
        transform.LookAt(target);
	}
}

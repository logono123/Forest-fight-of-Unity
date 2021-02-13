using UnityEngine;
using System.Collections;

public class AS_SniperManager : MonoBehaviour
{

	void Awake ()
	{
		AS_SniperKit.Environment = (AS_Environment)GameObject.FindObjectOfType (typeof(AS_Environment));	
	}

	void Start ()
	{
	
	}
}

public static class AS_SniperKit
{
	public static AS_Environment Environment;
}


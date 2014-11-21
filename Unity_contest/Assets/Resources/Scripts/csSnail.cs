using UnityEngine;
using System.Collections;

public class csSnail : MonoBehaviour {

	GameObject manager,ss;
	public static Vector3 inVector,outVector;

	public static float speed=1f;
	public static bool on = false;

	// Use this for initialization
	void Start () 
	{
		manager=GameObject.Find("Manager"); 
		ss=GameObject.Find("SS");

		//위치 확인
		Debug.DrawRay(transform.position,new Vector3(1f,0f,0f)*10f,Color.yellow,10f);
		Debug.DrawRay(transform.position,new Vector3(-1f,0f,0f)*10f,Color.red,10f);
		Debug.DrawRay(transform.position,new Vector3(0f,0f,1f)*10f,Color.white,10f);
		Debug.DrawRay(transform.position,new Vector3(0f,0f,-1f)*10f,Color.gray,10f);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void Shooting_Raycast(char outSign)
	{

	}

}

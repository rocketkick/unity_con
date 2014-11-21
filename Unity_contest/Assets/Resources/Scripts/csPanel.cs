using UnityEngine;
using System.Collections;

public class csPanel : MonoBehaviour {
	// bool raycast_active = false;
	int rail_num;
	Vector3 pos;
	GameObject manager;
	
	// Use this for initialization
	void Start () {
		manager=GameObject.Find("Manager");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void Click()
	{
		Debug.Log("Panel click!!"+this.gameObject.name);
		manager.SendMessage("Choice_Panel",this.gameObject);
	}

	void End()
	{
		manager.SendMessage("Game_Over");
		Debug.Log("bang");
	}
} 

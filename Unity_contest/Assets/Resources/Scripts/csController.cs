using UnityEngine;
using System.Collections;

public class csController : MonoBehaviour {
//	GameObject manager ;
	bool mouse_active = true;
	// Use this for initialization
	void Awake() {
//		manager = GameObject.Find("Manager");
	}
	
	// Update is called once per frame
	void Update () 
	{


		Vector3 rot=transform.TransformDirection(0,0,1);
		Vector3 pos=this.gameObject.camera.ScreenToWorldPoint(Input.mousePosition);
		RaycastHit input_hit;

		if(Input.GetMouseButtonDown(0) && mouse_active == true)
		{
			mouse_active = false;

			if(Physics.Raycast(pos,rot,out input_hit,50))
			{
				if(input_hit.collider.tag == "PANEL")
				{
					input_hit.collider.SendMessage("Click");
				}else if(input_hit.collider.tag == "RAIL")
				{
					input_hit.collider.SendMessage("R_Click");
				}else if(input_hit.collider.tag == "START")
				{}
			}
		}
		mouse_active = true;

	}
}

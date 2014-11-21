using UnityEngine;
using System.Collections;

public class csStart : MonoBehaviour {
	GameObject manager; 
	
	// Use this for initialization
	void Start () {
		manager=GameObject.Find("Manager"); 
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void Check_Start(Vector3 outVector)
	{
		//use Start name -> know raycast direction
		//일단 지금은 디폴트(0,0,-1)로 raycast 발사
		
		Vector3 rot = outVector; 
		Vector3 pos = this.gameObject.transform.position; 
		//train이 발사한 레일에 대한 진행값과 레일 분류 
		RaycastHit input_hit; 
		if(Physics.Raycast(pos,rot,out input_hit,1)) 
		{ 
			if(input_hit.collider.tag=="RAIL") 
			{ 
				Debug.Log("Check_Rail"+outVector);
				input_hit.collider.SendMessage("Chain_Rail"); 
			} 
		}else
		{
			Debug.Log("Train raycast me!!"+this.gameObject.name); 
			manager.SendMessage("Game_Over",outVector);
		}
		//이전 진행 방향벡트를 이용해 현 레일로 진행 가능한지 체크 
		//check(trainDirection); 
	}
} 

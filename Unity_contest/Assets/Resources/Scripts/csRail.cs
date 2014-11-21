using UnityEngine; 
using System.Collections; 


public class csRail : MonoBehaviour { 
	// bool raycast_active = false; 
	// bool choise = false; 
	GameObject manager,ss,rail; 
	private static Vector3 outVector;
	
	// Use this for initialization 
	void Start () { 
		manager=GameObject.Find("Manager"); 
		ss=GameObject.Find("SS");
		rail=this.gameObject;
		
	} 
	
	// Update is called once per frame 
	void Update () { 
		
	} 
	void R_Click() 
	{ 
		Debug.Log("Rail click!!"+this.gameObject.name); 
		manager.SendMessage("Choice_Rail",this.gameObject); 
	} 
	
	
	void Check_Rail(Vector3 outVector) 
	{ 
		
		//기차 진행 방향을 받음
		Vector3 rot = outVector; 
		Vector3 pos = this.gameObject.transform.position; 
		//train이 발사한 레일에 대한 진행값과 레일 분류 
		RaycastHit input_hit;
		Debug.Log("chain reaction!!!");
		if(Physics.Raycast(pos,rot,out input_hit,10)) 
		{ 
			if(input_hit.collider.tag=="RAIL") 
			{ 
				input_hit.collider.SendMessage("Chain_Rail"); 
			} 
			//End 지점 검색 코딩!!!
			if(input_hit.collider.tag=="GOAL")
			{
				input_hit.collider.SendMessage("Goal",outVector);
			}
		}else
		{
			Debug.Log("Train raycast me!!"+this.gameObject.name);
			Debug.Log(outVector);
			manager.SendMessage("Game_Over",outVector); 
			//이전 진행 방향벡트를 이용해 현 레일로 진행 가능한지 체크 
			//check(trainDirection); 
		}
	} 
	
	//start에서 outVector로 보낸걸 inVector로 받음
	void Chain_Rail() 
	{  
		ss.SendMessage("Move_Train",rail.name);
	} 
	
}  

using UnityEngine;
using System.Collections;

public class csGoal : MonoBehaviour {
	GameObject train; 
	
	// Use this for initialization
	void Start () {
		train=GameObject.Find("Train"); 
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void Goal(Vector3 outVector){
		//적당한 벡터로 기차가 들어왔는지 확인후 
		train.SendMessage ("Goal", outVector);
	}
} 

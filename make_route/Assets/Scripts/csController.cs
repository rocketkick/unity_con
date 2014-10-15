using UnityEngine;
using System.Collections;

public class csController : MonoBehaviour {

	//start, goal route
	public Transform[] twoPointRoute;
	//nomal route
	public Transform[] threePointRoute;
	public Transform Tr;
	
	//Tr direction
	public enum Direction {Forward,Reverse};
	private Direction characterDirection;
	//path controll
	private float pathPostion=0;
	private Vector3 floorPosition;	
	private RaycastHit hit;
	private float speed = .4f;
	private float rayLenght = 3;
	//need?
	public static int choicePath;
	
	private float lookAheadAmount = .01f;
	private float ySpeed=0;
	private float gravity=.5f;

	void OnDrawGizmos(){
		iTween.DrawPath(twoPointRoute,Color.blue);
		iTween.DrawPath(threePointRoute,Color.black);
		//iTween.DrawPath(myBezier,Color.red);
	}
	// Use this for initialization
	void Start () {

		//myBezier = new Bezier(new Vector3(1,2,0),Random.insideUnitSphere * 2f, Random.insideUnitSphere * 2f,new Vector3(0,2,1));
		            
		//plop the character pieces in the "Ignore Raycast" layer so we don't have false raycast data:	
		foreach (Transform child in twoPointRoute) {
			child.gameObject.layer=2;
		}
		foreach (Transform child in threePointRoute) {
			child.gameObject.layer=2;
		}

		//iTween.MoveToBezier(Tr("time",3, "transition","easeInOutQuint","bezier",Vector3(1,2,0),Vector3(0,2,0),Vector3(0,2,1)));
	}
	// Update is called once per frame
	void Update () {
		MoveTrain();
		FindFloorAndRoatation();
		MoveCharacter();
	}

	void MoveTrain(){
		characterDirection=Direction.Forward;
		pathPostion += Time.deltaTime * speed;

		float temps = pathPostion + (Time.deltaTime * speed);

		if(temps > 1){
			//change path 
			choicePath = 1;
			pathPostion=0;
		}
		//if((temps >0.5)&&(rayShoot==true)){}

	}
	void FindFloorAndRoatation(){
		float pathPercent = pathPostion%1f;
		Vector3 lookTarget;

				
		if(pathPercent-lookAheadAmount>=0 && pathPercent+lookAheadAmount <=1){


			if(choicePath==0){
			Vector3 coordinateOnPath = iTween.PointOnPath(threePointRoute,pathPercent);
			if(characterDirection==Direction.Forward){
				lookTarget = iTween.PointOnPath(threePointRoute,pathPercent+lookAheadAmount);
			}else{
				lookTarget = iTween.PointOnPath(threePointRoute,pathPercent-lookAheadAmount);
				}
				//look;
				Tr.LookAt(lookTarget);


				if (Physics.Raycast(coordinateOnPath,-Vector3.up,out hit, rayLenght)){
					Debug.DrawRay(coordinateOnPath, -Vector3.up * hit.distance);
					floorPosition=hit.point;
				}


			}


			else if(choicePath==1){
				Vector3 coordinateOnPath = iTween.PointOnPath(twoPointRoute,pathPercent);
				if(characterDirection==Direction.Forward){
					lookTarget = iTween.PointOnPath(twoPointRoute,pathPercent+lookAheadAmount);
				}else{
					lookTarget = iTween.PointOnPath(twoPointRoute,pathPercent-lookAheadAmount);
				}
				//look;
				Tr.LookAt(lookTarget);
		
				if (Physics.Raycast(coordinateOnPath,-Vector3.up,out hit, rayLenght)){
					Debug.DrawRay(coordinateOnPath, -Vector3.up * hit.distance);
					floorPosition=hit.point;
				}
			}

			float yRot = Tr.eulerAngles.y;
			if(yRot<0f){yRot=0f;}
			Tr.eulerAngles=new Vector3(0,yRot,0);
		}
		/* original location
		if (Physics.Raycast(coordinateOnPath,-Vector3.up,out hit, rayLenght)){
			Debug.DrawRay(coordinateOnPath, -Vector3.up * hit.distance);
			floorPosition=hit.point;
		}*/
	}
	
	void MoveCharacter(){
		//add gravity:
		ySpeed += gravity * Time.deltaTime;
		
		//apply gravity:
		Tr.position=new Vector3(floorPosition.x,Tr.position.y,floorPosition.z);
		
		//floor checking:
		if(Tr.position.y<floorPosition.y){
			ySpeed=0;
			//jumpState=0;
			Tr.position=new Vector3(floorPosition.x,floorPosition.y,floorPosition.z);
		}
	}

}

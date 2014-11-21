using UnityEngine;
using System.Collections;

public class csManager : MonoBehaviour {
	
	public static GameObject panel, rail, template, start, goal, ss; 
	public static GameObject r1,r2,r3,r4,r5,r6,r7; 
	public static int stat=0; 
	public static float speed = 1f;
	//ss 진행방향 U: up, D: down, L: left, R: right 
	public static string ssDirection="";
	//ss 현 위치 0~49
	public static int ssLocation;
	public static GameObject[] queue = new GameObject[5]; 
	public static GameObject[] stack = new GameObject[50]; 
	public static int startTime, delayTime; 
	public static Vector3 START,P1,P2,END;
	//ss 진행방향 벡트값 
	public char inVector, outVector; 
	public static bool checkKey = false;
	public static bool moveKey = false;
	public static bool gameOver = false;
	
	
	GameObject panel_bank; 
	GameObject rail_bank; 
	GameObject queue_bank; 
	
	
	//private bool ss_on = false; 
	
	public static Bezier myBezier;
	private float t = 0f;


	void Awake() 
	{ 
		for(int s=0;s<49;s++) 
		{ stack[s]=null; 
		} 
		
		panel=Resources.Load("Prefabs/Panel") as GameObject; 
		template=Resources.Load("Prefabs/Panel") as GameObject; 
		start=Resources.Load("Prefabs/Start") as GameObject; 
		goal=Resources.Load("Prefabs/Goal") as GameObject;
		
		ss=Resources.Load("Prefabs/SS") as GameObject; 
		
		r1 = Resources.Load("Prefabs/R1") as GameObject; 
		r2 = Resources.Load("Prefabs/R2") as GameObject; 
		r3 = Resources.Load("Prefabs/R3") as GameObject; 
		r4 = Resources.Load("Prefabs/R4") as GameObject; 
		r5 = Resources.Load("Prefabs/R5") as GameObject; 
		r6 = Resources.Load("Prefabs/R6") as GameObject; 
		r7 = Resources.Load("Prefabs/R7") as GameObject; 
		
		
		panel_bank=GameObject.Find("Panel_Bank"); 
		rail_bank=GameObject.Find("Rail_Bank"); 
		queue_bank=GameObject.Find("Queue_Bank"); 
		
		
		//패널 기본 위치 지정 
		int posX = 0; 
		int posZ = 0; 
		
		
		//배경 패널 배치 
		for(int i=0;i<7;++i) 
		{ 
			for(int j=0;j<7;++j) 
			{ 
				stat++; 
				GameObject temp1=Instantiate(panel,new Vector3(posX,0,posZ),transform.rotation) as GameObject; 
				temp1.transform.parent=panel_bank.transform;
				temp1.name = stat+""; //패널의 위치값으로 이름을 지정 
				//temp.name=panel.name; 
				posX=posX+4; 
			} 
			posZ=posZ+4; 
			posX=0; 
		} 
		
		
		/*레일 배경 배치 
  int railX = -5; 
  int railZ = 11; 
  for(int k=0;k<5;++k) 
  { 
   GameObject temp2=Instantiate(template, new Vector3(railX,0,railZ),transform.rotation) as GameObject; 
   temp2.transform.parent=panel_bank.transform; 
   temp2.name=template.name; 
   railZ--; 
  } 
  */
	} 
	// Use this for initialization 
	void Start ()  
	{ 
		int railX = -6; 
		int railZ = 20; 
		speed = 0.5f;
		for(int k=0;k<5;++k) 
		{ 
			//queue에 rail을 랜덤 생성해서 리턴 받음 
			rail = make_rail();
			
			//큐에 5개의 레일을 배치하고 대기 
			queue[k]=Instantiate(rail, new Vector3(railX,1,railZ),transform.rotation) as GameObject;
			queue[k].transform.parent=queue_bank.transform; 
			queue[k].name=rail.name; 
			railZ=railZ-4; 
		} 
		
		//임시로 시작지점과 진행 방향을 지정 
		GameObject temp3=Instantiate(start, new Vector3(4f,1f,8f),transform.rotation) as GameObject; 
		temp3.name=start.name;
		outVector = 'D';
		ssLocation = 31;
		
		GameObject temp4=Instantiate(ss, new Vector3(4f,2f,8f),transform.rotation) as GameObject; 
		temp4.name =ss.name; 
		ss = temp4; 
		
		GameObject temp5=Instantiate(goal, new Vector3(0f,1f,4f),transform.rotation) as GameObject; 
		temp5.name =goal.name; 
		
		//inVector = new Vector3(0, 0, -1);
		//outVector = new Vector3(0, 0, -1);
		
		StartCoroutine("Countdown"); 
	}  
	
	
	// Update is called once per frame 
	void Update () { 
		if(checkKey == true)
		{
			checkSS();
		}
		if(moveKey == true) 
		{ 
			//ssRayShoot(); 
			//StartCoroutine("ssRayShoot");
			moveSS();
		}





	} 
	/*
	void ssRayShoot() 
	{ 
		moveKey = false; 
		//이동전 다음 레일로 이동 가능한지 체크
		//ss.SendMessage("Shooting_Raycast",outVector);

	}
	*/

	void Switch_On()
	{
		//moveKey = true;
	}
	
	IEnumerator Countdown(){ 
		yield return new WaitForSeconds(2f); 
		checkKey = true;
		moveKey = true; 
	}
	
	
	
	void Game_Over(){
		//Crash_ss(outVec);
		Debug.Log("Game Over");
	}
	
	
	
	//레일이 없는 곳을 클릭 했을시 레일 배치
	void Choice_Panel(GameObject choice_temp) 
	{ 
		Vector3 temp; 
		GameObject demo; 
		//선택된 패널 이름 
		Debug.Log(choice_temp.name); 
		//동작 설명 
		Debug.Log(" 큐에 마지막 레일을 해당 위치에 배치 "); 
		/* queue[4]의 레일을 직접 이동하고 신규 생성 하는 경우 */ 
		//클릭 지점으로 해당 레일 이동 
		//x축으로 -1 이동해서 생성해야 해서 코드가 길어짐 
		demo = queue[4]; 
		temp = new Vector3(choice_temp.transform.position.x,choice_temp.transform.position.y+1,choice_temp.transform.position.z); 
		demo.transform.position=temp; 
		
		//패널을 클릭한 경우이므로 패널에 레일이 비어 있는지 확인 필요 없이 스택에 추가 
		stack[int.Parse(choice_temp.name)]=demo; 
		demo.transform.parent=rail_bank.transform;

		/* 레일 설치 여부 스택 확인
		for( int jj=0;jj<50;jj++)
		{
			if(stack[jj] != null)
			Debug.Log(jj+" "+stack[jj].name);
		}
		*/

		/*큐를 하나씩 이동하고 queue[0]에 신규 레일 생성*/ 
		for( int i=4;i>0;i--) 
		{  
			queue[i] = queue[i-1]; 
			queue[i-1].transform.Translate(0,0,-4); 
		} 
		
		//queue에 랜덤 rail을 생성해서 리턴 받음 
		rail = make_rail(); 
		
		//큐에 1개의 레일을 배치하고 대기 
		queue[0]=Instantiate(rail, queue[0].transform.position,transform.rotation) as GameObject;

		queue[0].transform.parent=queue_bank.transform; 
		queue[0].name=rail.name; 
		//queue[0]을 queue[1]의 위치로 마지막으로 이동 
		queue[0].transform.Translate(0,0,4);
	} 
	
	void Choice_Rail(GameObject choice_temp){ 
		//선택 오브젝트 위치 확인
		Vector3 rot=new Vector3(0,-1,0); 
		Vector3 pos=choice_temp.transform.position; 

		RaycastHit input_hit; 
		Physics.Raycast(pos,rot,out input_hit,10);
		//선택 오브젝트 위치 저장
		string tempName = input_hit.collider.name;

		Debug.Log(choice_temp.name);
		//진행 확인용 임시 변수 선택된 오브젝트 파괴전 임시 저장

		GameObject tempObject = choice_temp;
		//선택된 오브젝트 파괴 
		Debug.Log(" 선택 오브젝트 파괴 "); 
		Destroy(choice_temp); 
		
		/*************************************************/ 
		/***** 파괴 패널티 코딩 ****************************/ 
		/*************************************************/ 
		
		Debug.Log(" 큐에 마지막 레일을 해당 위치에 배치 "); 
		queue[4].transform.position = choice_temp.transform.position; 
		queue[4].transform.parent=rail_bank.transform; 
		queue[4].transform.position=choice_temp.transform.position; 


		stack[int.Parse(tempName)]=queue[4];

		//설치 확인 
		Debug.Log(tempName+" "+tempObject.name +" replace: "+queue[4].name); 
		
		/*큐를 하나씩 이동하고 queue[0]에 신규 레일 생성*/ 
		for( int i=4;i>0;i--) 
		{  
			queue[i] = queue[i-1]; 
			queue[i-1].transform.Translate(0,0,-4); 
		} 
		
		//queue에 랜덤 rail을 랜덤 생성해서 리턴 받음 
		rail = make_rail(); 
		
		//큐에 1개의 레일을 배치하고 대기 
		queue[0]=Instantiate(rail, queue[0].transform.position,transform.rotation) as GameObject;
		queue[0].transform.parent=queue_bank.transform; 
		queue[0].name=rail.name; 
		//queue[0]을 마지막으로 이동 
		queue[0].transform.Translate(0,0,4);
	} 
	
	//레일을 랜덤 생성
	GameObject make_rail() 
	{ 
		int t = Random.Range(0,7); 
		GameObject rail=null; 
		
		if(t==0) 
			rail=r1; 
		else if(t==1) 
			rail=r2; 
		else if(t==2) 
			rail=r3; 
		else if(t==3) 
			rail=r4; 
		else if(t==4) 
			rail=r5; 
		else if(t==5) 
			rail=r6; 
		else if(t==6) 
			rail=r7; 
		return rail; 
	} 







	//현재 레일과 outVector 값을 비교해서 진행할 위치에 벡터 값을 확정
	//이동 함수를 Update()에 할당
	void checkSS()
	{
		//로컬 변수 선언
		int nextRail;
		char inVector;

		/*********************
		**Start point 시 코딩**
		**********************/ 
		
		

		//이동 칸이 벽인지 확인 하고 아닐시 해당 내용 적용
		if(outVector=='R')
		{
			//다음 레일 번호를 임시 저장 공간에 저장
			nextRail = ssLocation+1;
			//R 진행 방향에 벽
			if(ssLocation%7==0)
			{
				Debug.Log("R is Wall!");
				//벽 반동 코딩!!
			}
			//R에 벽이 아닐시
			//else if(stack[nextRail].
		}






		if(outVector=='L')
		{
			//R 진행 방향에 벽
			if(ssLocation%7==1)
			{
				Debug.Log("R is Wall!");
				//벽 반동 코딩!!
			}
		}
		if(outVector=='U')
		{
			//R 진행 방향에 벽
			if(ssLocation-43>0)
			{
				Debug.Log("R is Wall!");
				//벽 반동 코딩!!
			}
		}
		if(outVector=='D')
		{
			//R 진행 방향에 벽
			if(ssLocation-7<=0)
			{
				Debug.Log("R is Wall!");
				//벽 반동 코딩!!
			}
		}


			nextRail=ssLocation+1;


	


		//if(outVector=='U')
		START = ss.transform.position;




		checkKey = false;
	}
	//moveSS에 필요한 이동 명령
	//myBezier=new Bezier(ss.transform.position,new Vector3(0f,0f,10f),new Vector3(0f,0f,0f),new Vector3(0f,0f,-1f));

	//벡터 확인하여 다음 움직일 위치 지정 
	void moveSS()
	{

		myBezier=new Bezier(new Vector3(4f,3f,8f),new Vector3(0f,0f,1f),new Vector3(-1f,0f,0f),new Vector3(6f,2f,10f));

		Vector3 vec = myBezier.GetPointAtTime( t );
		ss.transform.position = vec;
			
		t += 0.1f * Time.deltaTime;
		if( t > 1f )
		{	
			t = 0f;
			//moveKey = false;
		}
		Debug.Log(t);
		vec = myBezier.GetPointAtTime( t );
		ss.transform.LookAt(vec);
	}
} 

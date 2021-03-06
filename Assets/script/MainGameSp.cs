﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGameSp : MonoBehaviour {

	int[,,]stage= new int[14,7,7];//ステージ全体の状態を格納する配列

	int[] dropB = new int[2];//落とす箱の色とかを格納する配列 4つあるのは過去に4つ同時に落としてた名残
	int[,] dropS = new int[2,7];//次に落とす箱を格納する配列 4つ使ってた時期が懐かしいですね直そうね


	bool coolclear = false;//箱が消える反応を確認しましたか？それならtrueにします

	bool Droping = true;//プレイヤーが箱を落とせるか確認するbool

	int posX=0,posZ=0,posY=0;//主にupdate関数内で箱を置く場所を確認するために変数

	float LookTower = 0;//マウスやタップした画面を上下に動かすと上下する変数

	int stageTall;//ステージに最高何段積んであるか確認した変数
	float CameraTall;//カメラの高さ 登るんすよこのカメラ

	GameObject TopCam;

	public GameObject[] Boxpre;//フィールドの横に出てくる今落とそうとしてる箱の色を示すアレ

	public GameObject[] BoxsetMark;//箱を置く場所を示す半透明の箱

	GameObject flooa;

	GameObject modeChecker;

	GameObject TurnTable;//タンテイボウ。これから落とす箱の色がここで回る？？？


	public GameObject setflash;
	public GameObject[] Crash; //クラアアアアアアアアアッシュ！！！！！箱が弾けるエフェクト！！！
	public GameObject Bubble;
	public Material[] CrashMat;
	public GameObject[] Fallpati;



	public GameObject UIBar;

	public GameObject Breakon;

	GameObject GameoverUI;

	RectTransform ComboBar;


	bool IsGameover= false;//ゲームオーバーか確認

	public static int score = 0;//獲得した合計スコア

	int scoreCount = 0;//箱を消した際に獲得するスコアを計算し代入される変数
	float scoreRate = 1; //連鎖時のスコアボーナス 連鎖するほどモリモリ増える

	public static int ActiveCombo = 0;	//アクティブコンボ回数
	bool ActiveComboStart = false;//アクティブコンボ成立中か判定
	public float ActiveComboLimit = 5;//アクティブコンボのリミット コンボを重ねるごとに時間がシビアになる
	float ComboLimiter;
	float ComboLimitCount = 0;//アクティブコンボ中の制限時間を数える

	float scoreComboRate = 1f;//アクティブコンボのスコアボーナス つなぎやすい分レートは連鎖より控えめ

	static int DelBox = 0;//消した箱の数 消した数が多いほどランクが上がるとかなんとか
	public static int DelBoxLevel = 1;//箱を一定数消すごとに上がるレベル ルールによってはこれでスコアが増えるとかなんとか

	public float DoubleDelRate = 1.45f;



	float OjamaCount;//フィールド下からせり上がってくる箱が出てくる時間を測る 箱を消すと減るのは仕様

	public float OjamaLimit;//箱が出てくる時間の指定 短いほど無理ゲー



	GameObject[,] SetBoxcoler=new GameObject[2,8];//フィールド横に出てくる箱のオブジェクト 向こう７回分の落とすハコがみえーる

	GameObject[] mark=new GameObject[4];//箱を置く場所を示す半透明のオブジェクト 2つで十分ですよ。タブン

	GameObject[,,] BoxStorage= new GameObject[14,7,7];//フィールド全体の箱のオブジェクトを管理する もっとまともな方法ありますかね？
	public GameObject COMBOTEXT;


	private int LookShift = 0;
	bool leftseting = true;
	bool rightseting = true;

	private int[,] makerLim = new int[5,2] {//箱を置く場所を示す場所の2つ目の箱を指示するためだけの配列 これが楽だったもので・・・
		{0,1},{1,0},{0,-1},{-1,0},{0,0}
	};
		
	private int makerSpin=0;//半透明の箱がどの方向を向いてるか

	private int makerTa = 1;//半透明の箱が上下左右どこを向いてるか 2が上で4が下

	private Vector3 colorSet = new Vector3 (4f, 1f, -4f);//フィールド横の箱のデフォルトの位置を示す

	private Vector3[] setB = new Vector3[4] {//セットボックス 半透明の箱の2つ目の場所を指示する 楽チン！
		new Vector3(0f,0f,1f),
		new Vector3(1f,0f,0f),
		new Vector3(0f,0f,-1f),
		new Vector3(-1f,0f,0f)
	};

	private Vector3 defaltSet = new Vector3 (-3f, 1f, -3f);

	public int DelBoxCheak = 3;


	// Use this for initialization
	void Start () {
		posX = 2;
		posZ = 2;
		ComboBar = UIBar.GetComponent<RectTransform>();
		TurnTable = GameObject.Find ("Tableup");
		GameoverUI = GameObject.Find ("GAMEOVERui");
		flooa = GameObject.Find ("flooa");
		modeChecker = GameObject.Find ("Imageground");

		TopCam = GameObject.Find ("TopCamera");
		TopCam.SetActive (false);

		CameraTall = TopCam.transform.position.y;

		modeChecker.SetActive (false);

		GameoverUI.SetActive (false);

		ComboLimiter = ActiveComboLimit;
		ComboLimitCount = ComboLimiter;

		//置いていく箱の色をランダムで代入
		dropB [0] = Random.Range (1, 5);
		dropB [1] = Random.Range (1, 5);


		for (int i = 0; i < 7; i++) {
			dropS [0,i] = Random.Range (1, 5);
			dropS [1,i] = Random.Range (1, 5);
		}
		COMBOTEXT.SetActive (false);

		OjamaCount = 0;


		//最初に半透明の箱と横の奴をセット
		for (int t = 0; t < 8; t++) {
			for (int i = 0; i < 2; i++) {
				if (dropB [i] != 0) {
					if (t > 0) {
						SetBoxcoler [i, t] = (GameObject)Instantiate (
							Boxpre [dropS [i,t-1] - 1],
							new Vector3 (TurnTable.transform.position.x, TurnTable.transform.position.y, TurnTable.transform.position.z),
							Quaternion.identity);
				//		SetBoxcoler [i, t].transform.parent = TurnTable.transform;
						SetBoxcoler [i, t].transform.localPosition = new Vector3 (1.6f , 9.3f + (0.12f + t * 0.18f), 152f- (2 * i * 0.07f));
						SetBoxcoler [i, t].transform.localScale = new Vector3 (0.12f,0.12f,0.12f);
						SetBoxcoler [i, t].transform.localRotation = new Quaternion (0.0f, 25.0f, 0.0f, 0.0f);
					} else {
						SetBoxcoler [i, t] = (GameObject)Instantiate (
							Boxpre [dropB [i] - 1],
							new Vector3 (TurnTable.transform.position.x, TurnTable.transform.position.y, TurnTable.transform.position.z),
							Quaternion.identity);
						SetBoxcoler [i, t].transform.parent = TurnTable.transform;
						SetBoxcoler [i, t].transform.localPosition = new Vector3 (0.125f - (2 * i * 0.125f), 0.136f , 0f);
						SetBoxcoler [i, t].transform.localScale = new Vector3 (0.25f, 0.25f, 0.25f);
						SetBoxcoler [i, t].transform.localRotation = new Quaternion (0.0f, 0.0f, 0.0f, 0.0f);
						mark [i] = (GameObject)Instantiate (
							BoxsetMark [dropB [i] - 1],
							setB [i * 2],
							Quaternion.identity);
						mark [i].transform.parent = flooa.transform;
					}




				}
			}
		}

	}

	
	// Update is called once per frame
	void Update () {
		
		if (IsGameover) {
			if (Input.anyKeyDown) {
				Application.LoadLevel("Start");
			}
		}
		ComboBar.anchorMax = new Vector2(1 - ComboLimitCount / ComboLimiter,ComboBar.anchorMax.y);

		if (Droping) {//プレイヤーが操作しても大丈夫か確認
			if (Input.GetButton ("Fire1")) {//マウスをクリックか画面をタップ中か確認 そのままだとスマホで不具合起こしそう
				modeChecker.SetActive(true);
//				mark[0].SetActive(false);
//				mark[1].SetActive(false);
				LookTower-=Input.GetAxis ("Mouse Y");//マウスを上下に動かす
//				flooa.transform.Rotate (new Vector3 (0f, Input.GetAxis("Mouse X") * 5f, 0f));
				if (LookTower < 0)LookTower = 0;
				if (LookTower > 12)LookTower = 12;
				if(Input.GetAxisRaw ("Vertical") > 0){
					TopCam.SetActive (true);
				}
				if(Input.GetAxisRaw ("Vertical") < 0){
					TopCam.SetActive (false);
				}

				if (Input.GetAxisRaw ("Horizontal") > 0 && rightseting) {
					flooa.transform.Rotate (new Vector3 (0f, 90f, 0f));
					rightseting = false;
					LookShift--;

				} 
				if (Input.GetAxisRaw ("Horizontal") < 0 && leftseting) {
					flooa.transform.Rotate (new Vector3 (0f, -90f, 0f));
					leftseting = false;
					LookShift++;
				}
				if (LookShift > 3)LookShift = 0;
				if (LookShift < 0)LookShift = 3;
				if(Input.GetAxisRaw ("Horizontal") == 0 ){
					leftseting = true;
					rightseting = true;
				}

			} else {
				modeChecker.SetActive (false);
				Quaternion deltaflooa =flooa.transform.rotation;
//				TopCam.SetActive (false);
//				mark[0].SetActive(true);
//				mark[1].SetActive(true);
				LookTower = 0;
			//	flooa.transform.rotation =new Quaternion(deltaflooa.x,deltaflooa.y /( Time.deltaTime *3),deltaflooa.z,0);
//				flooa.transform.rotation =new Quaternion(0,0,0,0);
				if (Input.anyKeyDown) {
					posSet ();
				}
				markTrace ();
			}
			for (int Y = 0; Y < 12; Y++) {
				for (int X = 0; X < 7; X++) {
					for (int Z = 0; Z < 7; Z++) {
						if (LookTower  > 12 - Y) {
							if (BoxStorage [Y, X, Z]) {
								BoxStorage [Y, X, Z].SetActive (false);
							}
						} else {
							if (BoxStorage [Y, X, Z]) {
								BoxStorage [Y, X, Z].SetActive (true);
							}
						}
					}
				}
			}
		}
		OjamaCount += Time.deltaTime;
		if (ActiveComboStart) {
			ComboLimitCount += Time.deltaTime;
			if (ComboLimitCount > ComboLimiter) {
				ActiveComboStart = false;
				ActiveCombo = 0;
				ComboLimitCount = 0;
				ComboLimiter = ActiveComboLimit;
				ComboLimitCount = ActiveComboLimit;
				scoreComboRate = 1f;
				COMBOTEXT.SetActive (false);
			}
		}
	}

	void GameOver(){
		Droping = false;
		ComboLimitCount = ComboLimitCount + ComboLimiter;
		IsGameover = true;
		GameoverUI.SetActive (true);
	}

	void OnGUI() {
		
//		GUI.Label (new Rect(120,10,200,30),stageTall + " Tall");
	}

	void posSet() {

		int Limnum ;

		if (makerTa % 2 == 0) {
			Limnum = 4;
		} else {
			Limnum = makerSpin;
		}


		if (Input.GetButton ("Jump")) {
			
			if (ComboLimitCount <= ComboLimiter) {
				ComboLimitCount = ComboLimitCount + ComboLimiter / 5f ;
			}

				switch (makerTa) {
				case 2:
				if (dropB [0] != 0) {
					for(int i=0;i<13;i++){
						if(stage[i,posX,posZ]==0){
							stage [ i,posX, posZ] =dropB [0];
							BoxStorage[i,posX,posZ] = (GameObject)Instantiate (
								Boxpre[stage [i, posX, posZ]-1],
								new Vector3((float)posX,(float)i,(float)posZ),
								flooa.transform.rotation);
							BoxStorage[i,posX,posZ].transform.parent = flooa.transform;
							BoxStorage[i,posX,posZ].transform.localPosition = defaltSet + new Vector3((float)posX,(float)i,(float)posZ);
							BoxStorage[i,posX,posZ].transform.localRotation = new Quaternion (0.0f, 0.0f, 0.0f, 0.0f);
							StartCoroutine(SetingFLASH(i,posX,posZ,dropB[0]-1));
							break;
						}
					}
				}
				if (dropB [1] != 0) {
					for(int i=0;i<13;i++){
						if(stage[i,posX,posZ]==0){
							stage [ i,posX, posZ] =dropB [1];
							BoxStorage[i,posX,posZ] = (GameObject)Instantiate (
								Boxpre[stage [i, posX, posZ]-1],
								new Vector3((float)posX,(float)i,(float)posZ),
								flooa.transform.rotation);
							BoxStorage[i,posX,posZ].transform.parent = flooa.transform;
							BoxStorage[i,posX,posZ].transform.localPosition = defaltSet + new Vector3((float)posX,(float)i,(float)posZ);
							BoxStorage[i,posX,posZ].transform.localRotation = new Quaternion (0.0f, 0.0f, 0.0f, 0.0f);
							StartCoroutine(SetingFLASH(i,posX,posZ,dropB[1]-1));
							break;
						}
					}
				}
					break;
				case 4:
				if (dropB [1] != 0) {
					for(int i=0;i<13;i++){
						if(stage[i,posX,posZ]==0){
							stage [ i,posX, posZ] =dropB [1];
							BoxStorage[i,posX,posZ] = (GameObject)Instantiate (
								Boxpre[stage [i, posX, posZ]-1],
								new Vector3((float)posX,(float)i,(float)posZ),
								flooa.transform.rotation);
							BoxStorage[i,posX,posZ].transform.parent = flooa.transform;
							BoxStorage[i,posX,posZ].transform.localPosition = defaltSet + new Vector3((float)posX,(float)i,(float)posZ);
							BoxStorage[i,posX,posZ].transform.localRotation = new Quaternion (0.0f, 0.0f, 0.0f, 0.0f);
							StartCoroutine(SetingFLASH(i,posX,posZ,dropB[1]-1));
							break;
						}
					}
				}
				if (dropB [0] != 0) {
					for(int i=0;i<13;i++){
						if(stage[i,posX,posZ]==0){
							stage [ i,posX, posZ] =dropB [0];
							BoxStorage[i,posX,posZ] = (GameObject)Instantiate (
								Boxpre[stage [i, posX, posZ]-1],
								new Vector3((float)posX,(float)i,(float)posZ),
								flooa.transform.rotation);
							BoxStorage[i,posX,posZ].transform.parent = flooa.transform;
							BoxStorage[i,posX,posZ].transform.localPosition = defaltSet + new Vector3((float)posX,(float)i,(float)posZ);
							BoxStorage[i,posX,posZ].transform.localRotation = new Quaternion (0.0f, 0.0f, 0.0f, 0.0f);
							StartCoroutine(SetingFLASH(i,posX,posZ,dropB[0]-1));
							break;
						}
					}
				}

					break;
				default:
				if (dropB [0] != 0) {
					for(int i=0;i<13;i++){
						if(stage[i,posX,posZ]==0){
							stage [ i,posX, posZ] =dropB [0];
							BoxStorage[i,posX,posZ] = (GameObject)Instantiate (
								Boxpre[stage [i, posX, posZ]-1],
								new Vector3((float)posX,(float)i,(float)posZ),
								flooa.transform.rotation);
							BoxStorage[i,posX,posZ].transform.parent = flooa.transform;
							BoxStorage[i,posX,posZ].transform.localPosition = defaltSet + new Vector3((float)posX,(float)i,(float)posZ);
							BoxStorage[i,posX,posZ].transform.localRotation = new Quaternion (0.0f, 0.0f, 0.0f, 0.0f);
							StartCoroutine(SetingFLASH(i,posX,posZ,dropB[0]-1));
							break;
						}
					}
				}
				if (dropB [1] != 0) {
					for (int i = 0; i < 13; i++) {
						if(stage[i,posX+makerLim[makerSpin,0],posZ+makerLim[makerSpin,1]]==0){
							stage [ i,posX+makerLim[makerSpin,0], posZ+makerLim[makerSpin,1]] =dropB [1];
							BoxStorage[i,posX+makerLim[makerSpin,0],posZ+makerLim[makerSpin,1]] = (GameObject)Instantiate (
								Boxpre[stage [i, posX+makerLim[makerSpin,0], posZ+makerLim[makerSpin,1]]-1],
								new Vector3((float)posX,(float)i,(float)posZ)+setB[makerSpin],
								flooa.transform.rotation);
							BoxStorage[i,posX+makerLim[makerSpin,0],posZ+makerLim[makerSpin,1]].transform.parent = flooa.transform;
							BoxStorage[i,posX+makerLim[makerSpin,0],posZ+makerLim[makerSpin,1]].transform.localPosition = defaltSet + new Vector3((float)posX+makerLim[makerSpin,0],(float)i,(float)posZ+makerLim[makerSpin,1]);
							BoxStorage[i,posX+makerLim[makerSpin,0],posZ+makerLim[makerSpin,1]].transform.localRotation = new Quaternion (0.0f, 0.0f, 0.0f, 0.0f);
							StartCoroutine(SetingFLASH(i,posX+makerLim[makerSpin,0],posZ+makerLim[makerSpin,1],dropB[1]-1));
							break;
						}
					}
				}
					break;
				}

			Cdrop2 ();
			StartCoroutine("stagecheck");
		}
		if(Input.GetButton ("Fire2")){

			Tspin ();

		}
		if(Input.GetButton ("Fire3")){
			
			Dspin2 ();

		}
		if (Input.GetAxisRaw ("Vertical") != 0 || Input.GetAxisRaw ("Horizontal") != 0) {



			switch(LookShift){
			case 0:
				Horizontalmove ((int)Input.GetAxisRaw ("Horizontal"),Limnum);
				Verticalmove ((int)Input.GetAxisRaw ("Vertical")*-1,Limnum);
				break;
			case 1:
				Horizontalmove ((int)Input.GetAxisRaw ("Vertical"),Limnum);
				Verticalmove ((int)Input.GetAxisRaw ("Horizontal"),Limnum);
				break;
			case 2:
				Horizontalmove ((int)Input.GetAxisRaw ("Horizontal")*-1,Limnum);
				Verticalmove ((int)Input.GetAxisRaw ("Vertical"),Limnum);
				break;
			case 3:
				Horizontalmove ((int)Input.GetAxisRaw ("Vertical")*-1,Limnum);
				Verticalmove ((int)Input.GetAxisRaw ("Horizontal")*-1,Limnum);
				break;
			default:
				break;
			}



		}
	}

	void Horizontalmove(int Raw,int Lim){
		posZ += Raw;
		if(posZ+makerLim[Lim,1] < 0 || posZ < 0){
			posZ++;
		}else if(posZ+makerLim[Lim,1] > 6 || posZ > 6){
			posZ--;
		}
	}

	void Verticalmove(int Raw,int Lim){
		posX += Raw;
		if(posX+makerLim[Lim,0] < 0 || posX < 0){
			posX++;
		}else if(posX+makerLim[Lim,0] > 6 || posX > 6){
			posX--;
		}
	}

	void PullBox(int X,int Y,int Z){
		
	}



	void Dspin2 (){
		if (makerSpin < 3) {
			makerSpin++;
		} else {
			makerSpin = 0;
		}
		if (posX + makerLim [makerSpin, 0] < 0  && makerTa % 2 != 0) {
			posX++;
		}
		if (posX + makerLim [makerSpin, 0] > 6 && makerTa % 2 != 0) {
			posX--;
		}
		if (posZ + makerLim [makerSpin, 1] < 0 && makerTa % 2 != 0) {
			posZ++;
		}
		if (posZ + makerLim [makerSpin, 1] > 6 && makerTa % 2 != 0) {
			posZ--;
		}

	}

	void Tspin(){
		if (makerTa < 4) {
			makerTa++;
		} else {
			makerTa = 1;
		}

		if (makerTa % 2 != 0) {
			makerSpin += 2;
			if (makerSpin > 3) {
				makerSpin-=4;
			} 
		}

		if (posX + makerLim [makerSpin, 0] < 0) {
			posX++;
		}
		if (posX + makerLim [makerSpin, 0] > 6) {
			posX--;
		}
		if (posZ + makerLim [makerSpin, 1] < 0) {
			posZ++;
		}
		if (posZ + makerLim [makerSpin, 1] > 6) {
			posZ--;
		}

	}

	void Cdrop2(){
		dropB [0] = dropS [0,0];
		dropB [1] = dropS [1,0];
		for (int i = 0; i < 6; i++) {
			dropS [0, i] = dropS [0, i + 1];
			dropS [1, i] = dropS [1, i + 1];
		}
		dropS [0,6] = Random.Range (1, 5);
		dropS [1,6] = Random.Range (1, 5);
		for (int t = 0; t < 8; t++) {
			for (int i = 0; i < 2; i++) {
				Destroy	(SetBoxcoler [i, t]);
				Destroy	(mark [i]);
				if (dropB [i] != 0) {
/*					SetBoxcoler [i, t] = (GameObject)Instantiate (
						Boxpre [dropB [i] - 1],
						new Vector3 (TurnTable.transform.position.x, TurnTable.transform.position.y, TurnTable.transform.position.z),
						Quaternion.identity);*/
					if (t > 0) {
						SetBoxcoler [i, t] = (GameObject)Instantiate (
							Boxpre [dropS [i,t-1] - 1],
							new Vector3 (TurnTable.transform.position.x, TurnTable.transform.position.y, TurnTable.transform.position.z),
							Quaternion.identity);
//						SetBoxcoler [i, t].transform.parent = TurnTable.transform;
						SetBoxcoler [i, t].transform.localPosition = new Vector3 (1.6f , 9.3f + (0.12f + t * 0.18f), 152f- (2 * i * 0.07f));
						SetBoxcoler [i, t].transform.localScale = new Vector3 (0.12f, 0.12f, 0.12f);
						SetBoxcoler [i, t].transform.localRotation = new Quaternion (0.0f, 25.0f, 0.0f, 0.0f);
					} else {
						SetBoxcoler [i, t] = (GameObject)Instantiate (
							Boxpre [dropB [i] - 1],
							new Vector3 (TurnTable.transform.position.x, TurnTable.transform.position.y, TurnTable.transform.position.z),
							Quaternion.identity);
						SetBoxcoler [i, t].transform.parent = TurnTable.transform;
						SetBoxcoler [i, t].transform.localPosition = new Vector3 (0.125f - (2 * i * 0.125f), 0.136f , 0f);
						SetBoxcoler [i, t].transform.localScale = new Vector3 (0.25f, 0.25f, 0.25f);
						SetBoxcoler [i, t].transform.localRotation = new Quaternion (0.0f, 0.0f, 0.0f, 0.0f);
					}
				}
			}
		}

		mark [0] = (GameObject)Instantiate (
			BoxsetMark [dropB [0] - 1],
			new Vector3 ((float)posX, (float)posY, (float)posZ),
			Quaternion.identity);
		mark [1] = (GameObject)Instantiate (
			BoxsetMark [dropB [1] - 1],
			setB [makerSpin],
			Quaternion.identity);
		mark [0].transform.parent = flooa.transform;
		mark [1].transform.parent = flooa.transform;

		StartCoroutine ("SyeikuCamera");
	}

	void markTrace(){

		for (posY = 0; stage [posY, posX, posZ] != 0; posY++) {
		}

		if (makerTa % 2 == 0) {
			switch (makerTa) {
			case 2:
				if (dropB [0] != 0) {
					mark [0].transform.localPosition = new Vector3 ((float)posX - 3, (float)posY + 1, (float)posZ - 3);
				}
				if (dropB [1] != 0) {
					mark [1].transform.localPosition = new Vector3 ((float)posX - 3, (float)posY + 2, (float)posZ - 3);
				}
				break;
			case 4:
				if (dropB [0] != 0) {
					mark [0].transform.localPosition = new Vector3 ((float)posX - 3, (float)posY + 2, (float)posZ - 3);
				}
				if (dropB [1] != 0) {
					mark [1].transform.localPosition = new Vector3 ((float)posX - 3, (float)posY + 1, (float)posZ - 3);
				}
				break;
			default:
				break;
			}

		} else {
			if (dropB [0] != 0) {
				mark [0].transform.localPosition = new Vector3 ((float)posX - 3, (float)posY + 1, (float)posZ - 3);
			}
			for (posY = 0; stage [posY, posX + makerLim [makerSpin, 0], posZ + makerLim [makerSpin, 1]] != 0; posY++) {
			}

			if (dropB [1] != 0) {
				mark [1].transform.localPosition = new Vector3 ((float)posX + makerLim [makerSpin, 0] - 3, (float)posY + 1, (float)posZ + makerLim [makerSpin, 1] - 3);
			}
		}

	}

	IEnumerator stagecheck(){//ハコを消すかどうかの判定をする

		do{
			bool[,,]Cstage = new bool[13,7,7];
			coolclear=false;
			int DDcolC;
			int BoxInCo;

			bool DDC = false;
			for (int Y = 0; Y < 12; Y++) {
				for (int X = 0; X < 7; X++) {
					for (int Z = 0; Z < 7; Z++) {
						for (int Xc = X + 1; Xc < 7; Xc++) {
							if (stage [Y, X, Z] == stage [Y, Xc, Z] && stage [Y, X, Z] >= 1 && stage [Y, X, Z] <= 4 && Xc-X >= DelBoxCheak+1) {
								DDcolC = stage[Y,X,Z];
								BoxInCo=0;

								BoxInCo = stage[Y,X+1,Z];

								for(int i=X+1;i<Xc;i++){
									if(BoxInCo == stage[Y,i,Z]){
										BoxInCo = stage[Y,i,Z];
									}else{
										DDC = true;
									}
									if(DDcolC==stage[Y,i,Z]){
										DDC =true;
									}
								}


//								if(DelBoxCheak(CDcolC,D))DDC = true;
								if(DDC == false){
									for (int Ca = X; Ca <= Xc; Ca++) {
										Cstage [Y, Ca, Z] = true;
									}
									coolclear = true;
								}
							}else if(stage [Y, Xc, Z]==0)break;
							DDC= false;
						}
						DDC= false;
						for (int Zc = Z + 1; Zc < 7; Zc++) {
							if (stage [Y, X, Z] == stage [Y, X, Zc] && stage [Y, X, Z] >= 1 && stage [Y, X, Z] <= 4 && Zc-Z >= DelBoxCheak+1) {
								DDcolC = stage[Y,X,Z];
								BoxInCo = stage[Y,X,Z+1];

								for(int i=Z+1;i<Zc;i++){
									if(BoxInCo == stage[Y,X,i]){
										BoxInCo = stage[Y,X,i];
									}else{
										DDC = true;
									}
									if(DDcolC==stage[Y,X,i]){
										DDC =true;
									}
								}
								if(DDC == false){
									for (int Ca = Z; Ca <= Zc; Ca++) {
										Cstage [Y, X, Ca] = true;

									}
									coolclear = true;
								}

							}else if(stage [Y, X, Zc]==0)break;
							DDC= false;
						}
						DDC= false;
						for (int Yc = Y + 1; Yc < 14; Yc++) {
							if (stage [Y, X, Z] == stage [Yc, X, Z] && stage [Y, X, Z] >= 1 && stage [Y, X, Z] <= 4 && Yc-Y >=DelBoxCheak+1) {
								DDcolC = stage[Y,X,Z];
								BoxInCo = stage[Y+1,X,Z];

								for(int i=Y+1;i<Yc;i++){
									if(BoxInCo == stage[i,X,Z]){
										BoxInCo = stage[i,X,Z];
									}else{
										DDC = true;
									}
									if(DDcolC==stage[i,X,Z]){
										DDC =true;
									}
								}

								if(DDC == false){
									for (int Ca = Y; Ca <= Yc; Ca++) {
										Cstage [Ca, X, Z] = true;

									}
									coolclear = true;
								}

							}else if(stage [Yc, X, Z]==0)break;
							DDC= false;
						}
						DDC= false;
					}
				}
			}
			if(coolclear == true){
				float DoubleRate = DoubleDelRate;
				int sCt = 0;
				for (int Y = 0; Y < 13; Y++) {
					for (int X = 0; X < 7; X++) {
						for (int Z = 0; Z < 7; Z++) {
							if(Cstage[Y,X,Z]){
								
							//	DelWhiteDown(X,Y,Z);
							//	DelWhiteMulch(X,Y,Z);
								DoubleRate = DoubleRate * DoubleDelRate;

								BoxStorage[Y,X,Z].GetComponent<Renderer>().material=CrashMat[stage[Y,X,Z]-1];
								BoxStorage[Y,X,Z].layer = 10;
								DelBox++;
								sCt++;
							} 
						}
					}
				}

				scoreCount =(int)((DoubleRate * 10f * scoreRate) * scoreComboRate) + scoreCount;
				for(int i = 0;i<sCt;i++){
					scoreComboRate = scoreComboRate + 1.01f;
				}



				scoreRate = scoreRate * 1.7f;
				mark[0].SetActive(false);
				mark[1].SetActive(false);
				Droping =false;
				ActiveComboStart = false;

				yield return new WaitForSeconds(0.5f);

				for (int Y = 0; Y < 13; Y++) {
					for (int X = 0; X < 7; X++) {
						for (int Z = 0; Z < 7; Z++) {
							if(Cstage[Y,X,Z]){
								StartCoroutine(CRASHBOX(X,Y,Z,stage[Y,X,Z]-1));
								stage[Y,X,Z]=0;
							} 
						}
					}
				}

				ComboLimitCount = 0;

				ActiveCombo++;
				ActiveComboStart = true;
				COMBOTEXT.SetActive (true);

				ComboLimiter = ComboLimiter * 0.95f;

				mark[0].SetActive(true);
				mark[1].SetActive(true);
				if(DelBox >= DelBoxLevel * 50){
					StartCoroutine("partFire");
					DelBoxLevel++;

					DelWhiteLevel();
				}

				Droping = true;
			}

		for (int X = 0; X < 7; X++) {
			for (int Z = 0; Z < 7; Z++) {
					bool Ydrop;
					do{
						Ydrop = false;
				for (int Y = 0; Y < 13; Y++) {
					if (stage [Y, X, Z] == 0) {
							if(stage[Y+1,X,Z]!=0){
								stage [Y, X, Z] = stage [Y+1, X, Z];
								stage [Y+1, X, Z] = 0;
									Ydrop = true;
							}
					}
						Destroy (BoxStorage [Y, X, Z]);
							if(stage [Y, X, Z] != 0){
								BoxStorage[Y,X,Z] = (GameObject)Instantiate (
									Boxpre[stage [Y, X, Z]-1],
									new Vector3((float)X,(float)Y,(float)Z),
									flooa.transform.rotation);
								BoxStorage[Y,X,Z].transform.parent = flooa.transform;
								BoxStorage[Y,X,Z].transform.localPosition = defaltSet + new Vector3((float)X,(float)Y,(float)Z);
								BoxStorage[Y,X,Z].transform.localRotation = new Quaternion (0.0f, 0.0f, 0.0f, 0.0f);
							}
						
							
				}
					}while(Ydrop == true);
			}
		}
			score +=scoreCount;
			OjamaCount -= scoreCount / 300;
			if(OjamaCount<0)OjamaCount=0;
			scoreCount = 0;


		}while(coolclear==true);
		scoreRate=1f;
		if (OjamaCount>OjamaLimit) {
			for (int X = 0; X < 7; X++) {
				for (int Z = 0; Z < 7; Z++) {
					for (int Y = 13; Y > 0; Y--) {
						stage [Y, X, Z] = stage [Y-1, X, Z];
						stage [Y-1, X, Z] = 0;
						Destroy (BoxStorage [Y-1, X, Z]);
						if (stage [Y, X, Z] != 0) {
							BoxStorage [Y, X, Z] = (GameObject)Instantiate (
								Boxpre [stage [Y, X, Z] - 1],
								new Vector3 ((float)X, (float)Y, (float)Z),
								flooa.transform.rotation);
							BoxStorage[Y,X,Z].transform.parent = flooa.transform;
							BoxStorage[Y,X,Z].transform.localPosition = defaltSet + new Vector3((float)X,(float)Y,(float)Z);
							BoxStorage[Y,X,Z].transform.localRotation = new Quaternion (0.0f, 0.0f, 0.0f, 0.0f);
						}

					}
				}
			}
			for (int X = 0; X < 7; X++) {
				for (int Z = 0; Z < 7; Z++) {
					stage [0, X, Z] = 5;
					if (stage [0, X, Z] != 0) {
						BoxStorage[0,X,Z] = (GameObject)Instantiate (
							Boxpre[stage [0, X, Z]-1],
							new Vector3((float)X,(float)0,(float)Z),
							flooa.transform.rotation);
						BoxStorage[0,X,Z].transform.parent = flooa.transform;
						BoxStorage[0,X,Z].transform.localPosition = defaltSet + new Vector3((float)X,(float)0,(float)Z);
						BoxStorage[0,X,Z].transform.localRotation = new Quaternion (0.0f, 0.0f, 0.0f, 0.0f);
					}
						
				}
			}
			OjamaCount -= OjamaLimit;
		}
		stageTall = 0;
		for (int y = 0; y < 13; y++) {
			for (int x=0; x < 7; x++) {
				for (int z=0; z < 7; z++) {
					if (stage [y, x, z] != 0) {
						stageTall++;
						x = 7;
						break;
					}
				}
			}
		}
		if (stageTall == 0)stageTall = 1;
		TopCam.transform.position =new Vector3(
			TopCam.transform.position.x,
			CameraTall + 1 * stageTall,
			TopCam.transform.position.z);
		if(stageTall>=12)GameOver ();

	}

	IEnumerator partFire(){
		GameObject BreakDown = (GameObject)Instantiate (Breakon);
		yield return new WaitForSeconds (1.0f);
		Destroy (BreakDown);
	}

	IEnumerator CRASHBOX(int X, int Y, int Z, int i){
		GameObject CRASH = (GameObject)Instantiate (Crash[i]);
		GameObject BUBBLE = (GameObject)Instantiate (Bubble);
		CRASH.transform.position = BoxStorage [Y, X, Z].transform.position;
		BUBBLE.transform.position = BoxStorage [Y, X, Z].transform.position;
		yield return new WaitForSeconds (0.8f);
		Destroy (CRASH);
		Destroy (BUBBLE);
	}

	IEnumerator SetingFLASH(int Y,int X,int Z,int i){
		GameObject FLASH ;
		GameObject FALL;
		FLASH = (GameObject)Instantiate (setflash);
		FALL = (GameObject)Instantiate (Fallpati[i]);
//		FLASH.transform.Rotate(new Vector3 (-90f, 0f, 0f));
		FLASH.transform.position = BoxStorage[Y,X,Z].transform.position;
		FALL.transform.position = BoxStorage[Y,X,Z].transform.position/* + new Vector3(0.0f,7.0f,0.0f)*/;
		yield return new WaitForSeconds (0.8f);
		Destroy (FLASH);
//		Destroy (FALL);
	}

	IEnumerator SyeikuCamera(){
		float Cx=Camera.main.transform.position.x;
		float Cy=Camera.main.transform.position.y;
		float Cz=Camera.main.transform.position.z;
		Camera.main.transform.position = new Vector3(Cx,Cy+Random.Range(-0.05f,0.05f),Cz);
		yield return new WaitForSeconds (0.01f);
		Camera.main.transform.position = new Vector3(Cx,Cy,Cz);
	}
	void DelWhiteLevel(){
		for (int Y = 0; Y < 13; Y++) {
			for (int X = 0; X < 7; X++) {
				for (int Z = 0; Z < 7; Z++) {
					if (stage [Y, X, Z] == 5) {
						stage [Y, X, Z] = 0;
					}
				}
			}
		}
	}



}


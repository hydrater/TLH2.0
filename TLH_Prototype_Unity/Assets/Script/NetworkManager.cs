using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class NetworkManager : Photon.MonoBehaviour
{
	const string VERSION = "Prototype";
	bool isReady = false;

	//Connect to server upon game start
	void Awake() 
	{
		SceneManager.activeSceneChanged += onSceneChange;
        DontDestroyOnLoad(transform.gameObject);
		if (!PhotonNetwork.connected)
					PhotonNetwork.ConnectUsingSettings(VERSION);
		
    }

	//Function is called every time the scene is changed
	public void onSceneChange(Scene previousScene, Scene newScene)
    {
		switch(SceneManager.GetActiveScene().name)
		{
			default:

			break;

			case "Lobby":
				photonView.RPC("updatePlayerDisplay", PhotonTargets.All);
				GameObject.Find("ReadyBtn").GetComponent<Button>().onClick.AddListener(readyButton);
				GameObject.Find("LeaveBtn").GetComponent<Button>().onClick.AddListener(leaveButton);
			break;

			case "PrototypeLevel":
				Cursor.lockState =  CursorLockMode.Locked;
				Cursor.visible = false;
				Transform spawnPoint = GameObject.Find("spawnPoint").transform;
				PhotonNetwork.Instantiate("PlayerTest", spawnPoint.position, spawnPoint.rotation, 0);
				//gameObject.SetActive(false);
			break;
		}

    }

    //Linked to the button
    public void enterRoom()
    {
		PhotonNetwork.playerName = GameObject.Find("Username").GetComponent<InputField>().text;
		if(PhotonNetwork.playerName.Length < 3)
		{
			GameObject.Find("Error Log").GetComponent<Text>().text = "Your name is too short!";
			return;
		}
		string roomName = GameObject.Find("Room").GetComponent<InputField>().text;
		if (roomName == "")
		{
			GameObject.Find("Error Log").GetComponent<Text>().text = "You need to enter a room name!";
			return;
		}
		RoomOptions roomOptions = new RoomOptions();
		roomOptions.MaxPlayers = 6;
		PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, TypedLobby.Default);
    }

    //Change scene once connected to room
	void OnJoinedRoom()
	{
		SceneManager.LoadScene("Lobby");
	}

	void OnPhotonJoinRoomFailed()
	{
		//ROOM IS FULL ERROR
	}

	void OnPhotonPlayerDisconnected(PhotonPlayer other)
	{
		updatePlayerDisplay();
		photonView.RPC("lobbyReady", PhotonTargets.All, isReady, PhotonNetwork.playerName);
	}


    //For debugging purposes and convenient keyboard controls
    bool tabfocus = false;
	void Update()
	{
		if(Input.GetKeyUp(KeyCode.Tab))
		{
			if (tabfocus) 
			{
				GameObject.Find ("Room").GetComponent<InputField> ().Select ();
				tabfocus = !tabfocus;
			}
			else
			{
				GameObject.Find("Username").GetComponent<InputField>().Select();
				tabfocus = !tabfocus;
			}
		}
		if (Input.GetKeyUp(KeyCode.Return))
			enterRoom();
	}

	[PunRPC]
	void updatePlayerDisplay()
	{
		Transform playerList = GameObject.Find("Player List").transform;
		for(int i = PhotonNetwork.playerList.Length - 1; i >= 0; --i)
		{
			playerList.transform.GetChild(i).gameObject.SetActive(true);
			playerList.transform.GetChild(i).GetChild(0).GetComponent<Text>().text = PhotonNetwork.playerList[i].name;
		}
		playerList.transform.GetChild(PhotonNetwork.playerList.Length).gameObject.SetActive(false);
	}

	public void readyButton()
	{
		isReady = !isReady;
		photonView.RPC("lobbyReady", PhotonTargets.All, isReady, PhotonNetwork.playerName);
	}

	public void leaveButton()
	{
		PhotonNetwork.LeaveRoom();
		SceneManager.LoadScene("_Title");
		SceneManager.activeSceneChanged -= onSceneChange;
		Destroy(gameObject);
	}

	private IEnumerator gameStart;

	//Bugs cannot unready, cannot check all
	[PunRPC]
	void lobbyReady(bool ready, string username)
	{
		byte readyAmt = 0;
		Transform playerList = GameObject.Find("Player List").transform;
		for(int i = PhotonNetwork.playerList.Length - 1; i >= 0; --i)
		{
			if(playerList.transform.GetChild(i).GetChild(0).GetComponent<Text>().text == username)
				playerList.transform.GetChild(i).GetComponent<Image>().color = ready ? Color.green : Color.red;
			if (playerList.transform.GetChild(i).GetComponent<Image>().color == Color.green)
				++readyAmt;
		}
		if (readyAmt == PhotonNetwork.playerList.Length)
		{
			gameStart = gameStartTimer();
			StartCoroutine(gameStart);
		}
		else
		{
			if (gameStart != null)
			{
				StopCoroutine(gameStart);
				GameObject.Find("TimerTxt").GetComponent<Text>().text = "";
			}
		}
	}

	IEnumerator gameStartTimer()
	{
		Text timer = GameObject.Find("TimerTxt").GetComponent<Text>();
		for (sbyte i = 5; i >= 0; --i)
		{
			timer.text = string.Format("0:0{0}", i.ToString());
			yield return new WaitForSeconds(1f);
		}
		//display loading scene
		SceneManager.LoadSceneAsync("PrototypeLevel");
	}
}

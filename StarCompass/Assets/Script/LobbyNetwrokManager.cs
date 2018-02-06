using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LobbyNetwrokManager : Photon.MonoBehaviour {
    public static LobbyNetwrokManager instance = null;

    private List<GameObject> roomPrefabs = new List<GameObject>();

    public GameObject roomPrefab;

    public InputField roomName;
    public InputField maxCount;
    public InputField playerName;
    public GameObject errorButton;
    public GameObject lobbyPanel;
    public GameObject roomPanel;
    public GameObject[] map;
    int index;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
          //  DontDestroyOnLoad(gameObject.transform);
        }else if(instance != null)
        {
            Destroy(gameObject);
        }
    }
    private void Start()

    {
        PhotonNetwork.automaticallySyncScene = true;
        PhotonNetwork.ConnectUsingSettings("1.0");
        playerName.text = PlayerPrefs.GetString("UserName");
        map[index].SetActive(true);
    }
    private void Update()
    {
          
    }
    public void ButtonEvents(string EVENT)
    {
        switch (EVENT)
        {
            case "CreateRoom":
                if (PhotonNetwork.JoinLobby())
                    if (playerName == null)
                    {
                        playerName.text = "Player";
                    }
                {
                    if (byte.Parse(maxCount.text) %2  == 0)
                    {
                        RoomOptions ro = new RoomOptions();
                        ro.MaxPlayers = byte.Parse(maxCount.text);
                        PhotonNetwork.CreateRoom(roomName.text, ro, TypedLobby.Default);   
                    }
                    else
                    {
                        errorButton.SetActive(true);
                    }
                }
                break;
            case "ExitRoom":
                break;
            case "RefreshButton":
                if (PhotonNetwork.JoinLobby())
                    RefreshRoomList();
                break;
            case "JoinRandomRoom":
                if (PhotonNetwork.JoinLobby())
                    JoinRandomRoomButton();
                break;
            case "ErrorButton":
                errorButton.SetActive(false);
                break;
            case "QuitGame":
                Application.Quit();
                break;
        }
    }

    void RefreshRoomList()
    {
        if(roomPrefabs.Count > 0)
        {
            for(int i = 0; i < roomPrefabs.Count; i++)
            {
                Destroy(roomPrefabs[i]);
            }

            roomPrefabs.Clear();
        }
        for(int i =0;i < PhotonNetwork.GetRoomList().Length; i++)
        {
            GameObject g = Instantiate(roomPrefab);
            g.transform.SetParent(roomPrefab.transform.parent);

            g.GetComponent<RectTransform>().localScale = roomPrefab.GetComponent<RectTransform>().localScale;
            g.GetComponent<RectTransform>().localPosition = new Vector3(roomPrefab.GetComponent<RectTransform>().localPosition.x, roomPrefab.GetComponent<RectTransform>().localPosition.y - (i * 50), roomPrefab.GetComponent<RectTransform>().localPosition.z);
            g.transform.Find("roomName").GetComponent<Text>().text = PhotonNetwork.GetRoomList()[i].Name;
            g.transform.Find("roomCount").GetComponent<Text>().text = PhotonNetwork.GetRoomList()[i].PlayerCount + "/" + PhotonNetwork.GetRoomList()[i].MaxPlayers;

            g.transform.Find("JoinButton").GetComponent<Button>().onClick.AddListener(() => { JoinRoom(g.transform.Find("roomName").GetComponent<Text>().text); });
            g.SetActive(true);
            roomPrefabs.Add(g);
        }
    }

    void JoinRandomRoomButton()
    {
        if(PhotonNetwork.GetRoomList().Length > 0)
        {
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            Debug.Log("There are no room");
        }
    }

    void JoinRoom(string roomName)
    {
        Debug.Log(roomName+"jR");
        bool available = false;
        foreach (RoomInfo RI in PhotonNetwork.GetRoomList())
        {
            if(roomName == RI.Name)
            {
                PhotonNetwork.JoinRoom(roomName);
               // available = true;
               // break;
            }
            else
            {
                available = false;
            }
        }
       /* if (available)
        {
            PhotonNetwork.JoinRoom(roomName);
        }
        else
        {
            Debug.Log("Can't Join Room");
        }*/
    }
    private void OnGUI()
    {
        GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
    }
    void OnJoinedLobby()
    {
        Debug.Log("Lobby");
        Invoke("RefreshRoomList", 0.1f);
    }
    void OnPhotonJoinRoomFailed()
    {
        Debug.Log("Join Fail");
    }
    void OnJoinedRoom()
    {
        Debug.Log("Join Room");

        //lobbyPanel.SetActive(false);
        //roomPanel.SetActive(true);    
    }
    void OnCreatedRoom()
    {
        PhotonNetwork.LoadLevel(map[index].name);
        // PhotonNetwork.LoadLevel(index + 1);
        PhotonNetwork.player.NickName = playerName.text;
        PlayerPrefs.SetString("UserPlayer", playerName.text);
        Debug.Log("Create Room");
    }
    public void leftMap()
    {
        map[index].SetActive(false);
        index--;
        if (index < 0)
            index = map.Length - 1;

        map[index].SetActive(true);
    }
    public void rightMap()
    {
        map[index].SetActive(false);
        index++;
        if (index == map.Length)
            index = 0;

        map[index].SetActive(true);
    }
}

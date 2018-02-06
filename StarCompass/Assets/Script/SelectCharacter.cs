using System.Collections;
using System.Collections.Generic;
using UnityEngine; 
public class SelectCharacter : Photon.MonoBehaviour {
    
    public MainGameManager mgn;
    public GameObject Team;
    public GameObject team1Charcter;
    public GameObject team2Charcter;
    public GameObject BackButton;
    public GameObject[] Player;
    public GameObject ischosen;
    // count 已選數
    public Transform[] spawnPoint;
    public GameObject menu;
    public int teamAIndex, teamBIndex;
    int count;
    private void Start()
    {
        count = PhotonNetwork.room.MaxPlayers / 2;
    }
    private void Update()
    {
        Debug.Log(count);
    }
    public void SelectTeamAChar()
    {
        if (PunTeams.PlayersPerTeam[PunTeams.Team.red].Count < count)
        {
            PhotonNetwork.player.SetTeam(PunTeams.Team.red);
            Team.SetActive(false);
            team1Charcter.SetActive(true);
        }
        else
        {
            Debug.Log("error");
        }
    }
    public void SelectTeamBChar()
    {
        if (PunTeams.PlayersPerTeam[PunTeams.Team.blue].Count < count)
        {
            PhotonNetwork.player.SetTeam(PunTeams.Team.blue);
            Team.SetActive(false);
            team2Charcter.SetActive(true);
        }
    }
    public void aBackToSelection()
    {
        PhotonNetwork.player.SetTeam(PunTeams.Team.none);
        team1Charcter.SetActive(false);
        Team.SetActive(true);    
    }
    public void bBackToSelection()
    {
        PhotonNetwork.player.SetTeam(PunTeams.Team.none);
        team2Charcter.SetActive(false);
        Team.SetActive(true);
    }
 
    public void spawnTeamAPlayer()
    {
        if (mgn.teamAPlayer1 == null)
        {
            GameObject _myPlayer = PhotonNetwork.Instantiate("character_captain2", spawnPoint[0].position, spawnPoint[0].rotation, 0) as GameObject;
            GameObject _myCamera = PhotonNetwork.Instantiate("Camera", spawnPoint[1].position, spawnPoint[1].rotation, 0) as GameObject;
            _myPlayer.GetComponent<CharacterMovement>().cLookPos = _myCamera.GetComponentInChildren<CameraLookPos>();
            _myPlayer.GetComponent<OnPlatformMovement>().camera = _myCamera.transform.GetChild(0).GetComponentInChildren<Camera>();
            _myCamera.transform.GetComponentInChildren<CameraLookPos>().inputManager = _myPlayer.GetComponent<InputManager>();
            _myCamera.transform.GetComponentInChildren<CameraLookPos>().characterMovement = _myPlayer.GetComponent<CharacterMovement>();
            _myCamera.gameObject.GetComponentInChildren<CameraLookPos>().cameraFollowTar = _myPlayer.transform.GetChild(0).gameObject;
            _myCamera.gameObject.GetComponentInChildren<CameraLookPos>().cameraFollowTar2 = _myPlayer.transform.GetChild(1).GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(0).gameObject;
            _myCamera.gameObject.GetComponentInChildren<CameraLookPos>().stateManager = _myPlayer.GetComponent<StateManager>();
            _myCamera.gameObject.GetComponentInChildren<CameraLookPos>().onPlatformMovement = _myPlayer.GetComponent<OnPlatformMovement>();
            menu.SetActive(false);
        }
        else
        {
            ischosen.SetActive(true);
        }
    }
    public void spawnTeamAPlayer2()
    {
        if (mgn.teamAPlayer2 == null)
        {
            PhotonNetwork.Instantiate("RedTeamCaptain", spawnPoint[0].position, Quaternion.identity, 0);
            teamAIndex += 1;
            menu.SetActive(false);
        }
        else
        {
            ischosen.SetActive(true);
        }
    }
    public void spawnTeamBPlayer1()
    {
        if (mgn.teamBPlayer1 == null)
        {
            GameObject _myPlayer = PhotonNetwork.Instantiate("character_captainB", spawnPoint[2].position, spawnPoint[2].rotation, 0) as GameObject;
            GameObject _myCamera = PhotonNetwork.Instantiate("Camera", spawnPoint[3].position,spawnPoint[2].rotation,0) as GameObject;
            _myPlayer.GetComponent<CharacterMovement>().cLookPos = _myCamera.GetComponentInChildren<CameraLookPos>();
            _myPlayer.GetComponent<OnPlatformMovement>().camera = _myCamera.transform.GetChild(0).GetComponentInChildren<Camera>();
            _myCamera.transform.GetComponentInChildren<CameraLookPos>().inputManager = _myPlayer.GetComponent<InputManager>();
            _myCamera.transform.GetComponentInChildren<CameraLookPos>().characterMovement = _myPlayer.GetComponent<CharacterMovement>();
            _myCamera.gameObject.GetComponentInChildren<CameraLookPos>().cameraFollowTar = _myPlayer.transform.GetChild(0).gameObject;
            _myCamera.gameObject.GetComponentInChildren<CameraLookPos>().cameraFollowTar2 = _myPlayer.transform.GetChild(1).GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(0).gameObject;
            _myCamera.gameObject.GetComponentInChildren<CameraLookPos>().stateManager = _myPlayer.GetComponent<StateManager>();
            _myCamera.gameObject.GetComponentInChildren<CameraLookPos>().onPlatformMovement = _myPlayer.GetComponent<OnPlatformMovement>();
            menu.SetActive(false);
        }
        else
        {
            ischosen.SetActive(true);
        }
    }
    public void okButton()
    {
        ischosen.SetActive(false);
    }
    public void OnLeftRoom()
    {
        //  PhotonNetwork.Disconnect();
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel(0);
    }
    public void OnLeftRoomInGame()
    {
        //  PhotonNetwork.Disconnect();
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel(0);
        Time.timeScale = 1;
    }

}

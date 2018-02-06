using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class roomPanelContoller : Photon.PunBehaviour {
    public GameObject lobbyPanel;
    public GameObject roomPanel;
    public Button backButton;
    public Text roomName;
    public GameObject[] Team1;
    public GameObject[] Team2;
    public Button readyButton;
    public Text promptMessage;
    PhotonView pView;
    int teamSize;
    Text[] texts;
    ExitGames.Client.Photon.Hashtable costomProperties;
    private void OnEnable()
    {
        pView = GetComponent<PhotonView>();
        if (!PhotonNetwork.connected)
            return;
        roomName.text = PhotonNetwork.room.name;
        promptMessage.text = "";

        backButton.onClick.RemoveAllListeners();
        backButton.onClick.AddListener(delegate ()
        {
            PhotonNetwork.LeaveRoom();
            lobbyPanel.SetActive(true);
            roomPanel.SetActive(false);
        });

        teamSize = PhotonNetwork.room.MaxPlayers / 2;
        DisableTeamPanel();
        UpdateTeamPanel(false);

        for (int i = 0; i < teamSize; i++)
        {
            if (!Team1[i].activeSelf)
            {
                Team1[i].SetActive(true);
                texts = Team1[i].GetComponentsInChildren<Text>();
                texts[0].text = PhotonNetwork.playerName;
                if (PhotonNetwork.isMasterClient) texts[1].text = "Master";
                else texts[1].text = "Not Ready";
                costomProperties = new ExitGames.Client.Photon.Hashtable()
                {
                    {"Team","Team1" },
                    {"TeamNum",i},
                    {"isReady" ,false}
                };
                PhotonNetwork.player.SetCustomProperties(costomProperties);
                break;
            } else if (!Team2[i].activeSelf)
            {
                Team2[i].SetActive(true);
                texts = Team2[i].GetComponentsInChildren<Text>();
                if (PhotonNetwork.isMasterClient) texts[1].text = "Master";
                else texts[1].text = "Not Ready";
                costomProperties = new ExitGames.Client.Photon.Hashtable()
                {
                    {"Team2","Team2" },
                    {"TeamNum",i },
                    {"isReady",false}
                };
                PhotonNetwork.player.SetCustomProperties(costomProperties);
                break;
            }
        }
        ReadyButtonControl();
    }
    public override void OnPhotonPlayerPropertiesChanged(object[] playerAndUpdatedProps)
    {
        DisableTeamPanel();
        UpdateTeamPanel(true);
    }
    public override void OnMasterClientSwitched(PhotonPlayer newMasterClient)
    {
        ReadyButtonControl();
    }
    public override void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
    {
        DisableTeamPanel();
        UpdateTeamPanel(true);

    }
    void DisableTeamPanel()
    {
        for (int i = 0; i < Team1.Length; i++)
        {
            Team1[i].SetActive(false);
        }
        for (int i = 0; i < Team2.Length; i++)
        {
            Team2[i].SetActive(false);
        }
    }
    void UpdateTeamPanel(bool isUpdateSelf)
    {
        GameObject go;
        foreach (PhotonPlayer p in PhotonNetwork.playerList)
        {
            if (!isUpdateSelf && p.IsLocal) continue;
            costomProperties = p.CustomProperties;
            if (costomProperties["Team"].Equals("Team1"))
            {
                go = Team1[(int)costomProperties["TeamNum"]];
                go.SetActive(true);
                texts = go.GetComponentsInChildren<Text>();
            }
            else
            {
                go = Team2[(int)costomProperties["TeamNum"]];
                go.SetActive(true);
                texts = go.GetComponentsInChildren<Text>();
            }
            texts[0].text = p.name;
            if (p.IsMasterClient)
                texts[1].text = "Master";
            else if ((bool)costomProperties["isReady"])
            {
                texts[1].text = "is Ready";
            }
            else { texts[1].text = "Not Readye"; }
        }
    }
    void ReadyButtonControl()
    {
        if (PhotonNetwork.isMasterClient)
        {
            readyButton.GetComponentInChildren<Text>().text = "Game Start";
            readyButton.onClick.RemoveAllListeners();
            readyButton.onClick.AddListener(delegate ()
            {
                ClickStartGameButton();
            });
        }
        else
        {
            if ((bool)PhotonNetwork.player.customProperties["isReady"])
                readyButton.GetComponentInChildren<Text>().text = "Not Ready";
            else
                readyButton.GetComponentInChildren<Text>().text = "Is Ready";
            readyButton.onClick.RemoveAllListeners();
            readyButton.onClick.AddListener(delegate ()
            {
                ClickReadyButton();
            });
        }
    }
    public void ClickSwitchButton()
    {
        costomProperties = PhotonNetwork.player.customProperties;
        if ((bool)costomProperties["isReady"]){
            promptMessage.text = "Can't Change Team!";
            return;
        }
        bool isSwitched = false;
        if (costomProperties["Team"].ToString().Equals("Team1"))
        {
            for (int i = 0; i < teamSize; i++)
            {
                if (!Team2[i].activeSelf)
                {
                    isSwitched = true;
                    Team1[(int)costomProperties["TeamNum"]].SetActive(false);
                    texts = Team2[i].GetComponentsInChildren<Text>();
                    texts[0].text = PhotonNetwork.playerName;
                    if (PhotonNetwork.isMasterClient) texts[1].text = "Master";
                    else texts[1].text = "Not Ready";
                    Team2[i].SetActive(true);
                    costomProperties = new ExitGames.Client.Photon.Hashtable()
                    {
                        {"Team2","Team2" },
                        {"TeamNum",i }
                    };
                    PhotonNetwork.player.SetCustomProperties(costomProperties);
                    break;
                }
            }
        } else if (costomProperties["Team"].ToString().Equals("Team2"))
        {
            for (int i = 0; i < teamSize; i++)
            {
                if (!Team1[i].activeSelf)
                {
                    isSwitched = true;
                    Team2[(int)(costomProperties["TeamNum"])].SetActive(false);
                    texts = Team1[i].GetComponentsInChildren<Text>();
                    texts[0].text = PhotonNetwork.playerName;
                    if (PhotonNetwork.isMasterClient) texts[1].text = "Master";
                    else texts[1].text = "Not Ready";
                    Team1[i].SetActive(true);
                    costomProperties = new ExitGames.Client.Photon.Hashtable()
                    {
                        {"Team","Team1" },
                        {"TeamNum",i }
                    };
                    PhotonNetwork.player.SetCustomProperties(costomProperties);
                    break;
                }
            }
        }
        if (!isSwitched)
            promptMessage.text = "Team is Fulled";
        else
            promptMessage.text = "";
    }
    public void ClickReadyButton()
    {
        bool isReady = (bool)PhotonNetwork.player.customProperties["isReady"];
        costomProperties = new ExitGames.Client.Photon.Hashtable() { { "isReady", !isReady } };
        PhotonNetwork.player.SetCustomProperties(costomProperties);
        Text readyButtonText = readyButton.GetComponentInChildren<Text>();
        if (isReady) readyButtonText.text = "Ready";
        else readyButtonText.text = "Disable";
    }
    public void ClickStartGameButton()
    {
        foreach (PhotonPlayer p in PhotonNetwork.playerList)
        {
            if (p.IsLocal) continue;
            if ((bool)p.customProperties["isReady"] == false)
            {
                promptMessage.text =  "some player not ready";
                return;
            }
        }
        promptMessage.text = "";
        PhotonNetwork.room.open = false;
        pView.RPC("LoadGameScene", PhotonTargets.All, "GameScene");
    }
    [PunRPC]
    public void LoadGameScene(string sceneName) {
        PhotonNetwork.LoadLevel(sceneName);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Text;
using Photon.Realtime;

public class RoomSceneManager : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    [SerializeField]
    Text textRoomName;
    [SerializeField]
    Text textPlayerList;
    [SerializeField]
    Button buttonStartGame;
    void Start()
    {
        if(PhotonNetwork.CurrentRoom == null)
        {
            SceneManager.LoadScene("Lobby Scene");
        }
        else
        {
            textRoomName.text = PhotonNetwork.CurrentRoom.Name;
            UpdatePlayerlist();
        }
        buttonStartGame.interactable = PhotonNetwork.IsMasterClient;

    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        buttonStartGame.interactable = PhotonNetwork.IsMasterClient;
    }

    public void UpdatePlayerlist()
    {
        StringBuilder sb = new StringBuilder();
        foreach(var kvp in PhotonNetwork.CurrentRoom.Players)
        {
            sb.AppendLine("->" + kvp.Value.NickName);
        }
        textPlayerList.text = sb.ToString();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        UpdatePlayerlist();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdatePlayerlist();
    }
    public void OnClickStartGame()
    {
        SceneManager.LoadScene("GameScene");
    }
    public void OnClickLeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("Lobby Scene");
    }

}
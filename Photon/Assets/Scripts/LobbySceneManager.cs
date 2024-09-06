using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Realtime;
using System.Text;

public class LobbySceneManager : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    [SerializeField]
    InputField inputRoomName;
    [SerializeField]
    InputField inputPlayerName;
    [SerializeField]
    Text textRoomList;
    void Start()
    {
        if(PhotonNetwork.IsConnected == false)
        {
            SceneManager.LoadScene("StartScene");
        }
        else
        {
            if(PhotonNetwork.CurrentLobby == null)
            {
                PhotonNetwork.JoinLobby();
            }
            PhotonNetwork.JoinLobby();
        }
    }
    
    public override void OnConnectedToMaster()
    {
        print("Connected to Master!");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        print("Lobby joined.");
    }

    public string GetRoomName()
    {
        string roomName = inputRoomName.text;
        return roomName.Trim();
    }

    public string GetPlayerName()
    {
        string playerName = inputPlayerName.text;
        return playerName.Trim();
    }

    public void OnClickCreateRoom()
    {
        string roomName = GetRoomName();
        string playerName = GetPlayerName();
        if(roomName.Length > 0  && playerName.Length > 0)
        {
            PhotonNetwork.CreateRoom(roomName);
            PhotonNetwork.LocalPlayer.NickName = playerName;
        }
        else
        {
            print("Invalid RoomName or PlayerName!");
        }
    }

    public void OnClickJoinRoom()
    {
        string roomName = GetRoomName();
        string playerName = GetPlayerName();
        if(roomName.Length > 0 && playerName.Length > 0)
        {
            PhotonNetwork.JoinRoom(roomName);
            PhotonNetwork.LocalPlayer.NickName = playerName;
        }
        else
        {
            print("Invalid RoomName or PlayerName!");
        }
    }

    public override void OnJoinedRoom()
    {
        print("Room Joined!");
        SceneManager.LoadScene("RoomScene");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        print("update");
        StringBuilder sb = new StringBuilder();
        foreach(RoomInfo roomInfo in roomList)
        {
            if(roomInfo.PlayerCount >= 1)
            {
                sb.AppendLine("-> " + roomInfo.Name);
            }
        }
        textRoomList.text = sb.ToString();
    }
}

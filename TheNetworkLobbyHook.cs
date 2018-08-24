using UnityEngine;
using Prototype.NetworkLobby;
using System.Collections;
using UnityEngine.Networking;

/*
 *	Author James Power 20067779 
 * 
*/

//Attaches to LobbyManager in LobbyScene
//Connects the player colour and player name selected in the lobby scene to the colour and name of the respective player in game

public class TheNetworkLobbyHook : LobbyHook 
{
	public override void OnLobbyServerSceneLoadedForPlayer(NetworkManager manager, GameObject lobbyPlayer, GameObject gamePlayer)
    {
        LobbyPlayer lobby = lobbyPlayer.GetComponent<LobbyPlayer>();

        SetLocalPlayer localPlayer = gamePlayer.GetComponent<SetLocalPlayer>();

        localPlayer.playerName = lobby.playerName;
        localPlayer.playerColor = lobby.playerColor;
	}
}

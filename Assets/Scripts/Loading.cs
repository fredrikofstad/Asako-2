using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class Loading : MonoBehaviourPunCallbacks
{
    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to server");
        SceneManager.LoadScene("Lobby");
    }
}

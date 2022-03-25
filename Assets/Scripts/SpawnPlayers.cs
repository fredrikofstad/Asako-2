using UnityEngine;
using Photon.Pun;
using Cinemachine;

public class SpawnPlayers : MonoBehaviour
{
    [SerializeField] private GameObject[] playerPrefabs;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private CinemachineVirtualCamera playerCam;
    [SerializeField] private bool testing;


    private GameObject player;

    private void Start()
    {
        if (testing) return;

        GameObject playerPrefab = playerPrefabs[(int)GameManager.instance.characterSelected];

        player = PhotonNetwork.Instantiate(playerPrefab.name, spawnPoint.position, spawnPoint.rotation);
        playerCam.Follow = player.transform;
    }

}

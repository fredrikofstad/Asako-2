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

        float randomRange = Random.Range(-2f, 2f);
        Vector3 spawnPosition = new Vector3(spawnPoint.position.x + randomRange, spawnPoint.position.y, spawnPoint.position.z);

        GameObject playerPrefab = playerPrefabs[(int)GameManager.instance.characterSelected];

        player = PhotonNetwork.Instantiate(playerPrefab.name, spawnPosition, spawnPoint.rotation);
        playerCam.Follow = player.transform;
    }

}

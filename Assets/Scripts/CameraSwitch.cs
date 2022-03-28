using UnityEngine;
using Cinemachine;

public class CameraSwitch : MonoBehaviour

{
    private CinemachineVirtualCamera playerCam;

    public void Start()
    {
        playerCam = GameObject.FindWithTag("PlayerCam").GetComponent<CinemachineVirtualCamera>();
    }
    public void SwitchCamera(CinemachineVirtualCamera second)
    {

        if (playerCam.Priority > second.Priority)
        {
            playerCam.Priority = 0;
            second.Priority = 1;
        }
        else
        {
            playerCam.Priority = 1;
            second.Priority = 0;
        }

    }
}
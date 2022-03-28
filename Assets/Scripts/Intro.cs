using System.Collections;
using UnityEngine;
using Cinemachine;

public class Intro : MonoBehaviour
{
    [SerializeField] private Animator takarazukaSprite;
    [SerializeField] private GameObject backBorder;
    [SerializeField] private CinemachineVirtualCamera playerCam;

    private void Start()
    {
        StartCoroutine(DropSprite());
    }

    private IEnumerator DropSprite()
    {
        yield return new WaitForSeconds(10);

        takarazukaSprite.SetTrigger("Drop");
        yield return new WaitForSeconds(2);
        playerCam.Priority = 10;
        backBorder.SetActive(false);
    }
}

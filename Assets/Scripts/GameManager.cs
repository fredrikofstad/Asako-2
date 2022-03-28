using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public enum Character
    {
        ASAKO,
        PANDA
    }

    public Character characterSelected;
    public GameObject player;
    public bool isAudioPlaying;

    private void Awake()
    {
        if (GameManager.instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        #if UNITY_EDITOR
        if (Application.isPlaying)
            UnityEditor.SceneVisibilityManager.instance.Show(gameObject, false);
        #endif
        DontDestroyOnLoad(gameObject);
    }

    public void SelectCharacter(int selection)
    {
        characterSelected = (Character)selection;
    }
}

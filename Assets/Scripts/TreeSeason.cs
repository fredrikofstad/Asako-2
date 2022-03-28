using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeSeason : MonoBehaviour
{
    public enum Season
    {
        Spring,
        Summer,
        Autumn,
        Winter
    }
    public Season currentSeason = Season.Autumn;
    public Texture[] textures;

    void Start()
    {
        GetComponent<Renderer>().material.mainTexture = textures[(int)currentSeason];
    }

}

using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public int NoOfPlayers;
    private string[] AIColors = new string[] { "Red", "Green", "Pink" };


    public Platform[] platforms;

    private void Start()
    {
        Invoke("DelayStart", 0.15f);
    }


    public void DelayStart()
    {
        if (NoOfPlayers < 2 || NoOfPlayers > 4) return;

        string[] playerColors = new string[NoOfPlayers];
        playerColors[0] = "Blue"; // Default Player Color
        for (int i = 1; i < NoOfPlayers; i++)
        {
            playerColors[i] = AIColors[i - 1];
        }


        foreach (Platform platform in platforms)
        {
            platform.SetupPlatform(playerColors);
        }

        PlayerInstantiator.Instance.InstantiatePlayers(playerColors, platforms[0]);
    }

}

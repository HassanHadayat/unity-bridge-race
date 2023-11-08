using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    [Range(2, 4)]
    public int NoOfPlayers;
    private string[] AIColors = new string[] { "Red", "Green", "Pink" };
    private string[] overallPlayerColors;
    public string[] PlayerColors
    {
        get { return overallPlayerColors; }
        private set { overallPlayerColors = value; }
    }

    public Platform[] platforms;

    private void Start()
    {
        //Invoke("DelayStart", 0.15f);
        DelayStart();
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

        PlayerColors = playerColors;


        // Setup all Platforms
        for (int i = 0; i < platforms.Length; i++)
        {
            if (i == 0)
                platforms[0].SetupPlatform(this, true);
            else
                platforms[i].SetupPlatform(this);
        }

        PlayerInstantiator.Instance.InstantiatePlayers(playerColors, platforms[0]);
    }
}

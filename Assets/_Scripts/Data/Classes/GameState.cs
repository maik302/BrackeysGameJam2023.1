using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameState {

    public int PlayerScore;
    public int PlayerMaxHealth;
    public int PlayerPowerLevel;
    public int WaveReached;

    private GameState(int playerScore, int playerMaxHealth, int playerPowerLevel, int waveReached) {
        PlayerScore = playerScore;
        PlayerMaxHealth = playerMaxHealth;
        PlayerPowerLevel = playerPowerLevel;
        WaveReached = waveReached;
    }
}

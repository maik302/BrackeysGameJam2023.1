using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameState {

    public int PlayerScore;
    public int PlayerMaxHealth;
    public int PlayerCurrentHealth;
    public int PlayerPowerLevel;
    public int PlayerCurrentPower;
    public int WaveReached;

    private GameState(int playerScore, int playerMaxHealth, int playerCurrentHealth, int playerPowerLevel, int playerCurrentPower, int waveReached) {
        PlayerScore = playerScore;
        PlayerMaxHealth = playerMaxHealth;
        PlayerCurrentHealth = playerCurrentHealth;
        PlayerPowerLevel = playerPowerLevel;
        PlayerCurrentPower = playerCurrentPower;
        WaveReached = waveReached;
    }
}

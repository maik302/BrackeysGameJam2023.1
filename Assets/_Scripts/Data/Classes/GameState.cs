using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameState {

    public int PlayerScore;
    public int PlayerMaxHealth;
    public int PlayerCurrentHealth;
    public int PlayerMaxPower;
    public int PlayerCurrentPower;
    public int WaveReached;

    public int MaxHealthModifiersCount;
    public int MaxPowerModifiersCount;
    public int ScoreModifiersCount;

    public GameState(
        int playerScore,
        int playerMaxHealth,
        int playerCurrentHealth,
        int playerMaxPower,
        int playerCurrentPower,
        int waveReached,
        int maxHealthModifiersCount,
        int maxPowerModifiersCount,
        int scoreModifiersCount) {
        PlayerScore = playerScore;
        PlayerMaxHealth = playerMaxHealth;
        PlayerCurrentHealth = playerCurrentHealth;
        PlayerMaxPower = playerMaxPower;
        PlayerCurrentPower = playerCurrentPower;
        WaveReached = waveReached;
        MaxHealthModifiersCount = maxHealthModifiersCount;
        MaxPowerModifiersCount = maxPowerModifiersCount;
        ScoreModifiersCount = scoreModifiersCount;
    }
}

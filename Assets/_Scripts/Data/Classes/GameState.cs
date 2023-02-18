using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class GameState : IComparable {

    public int PlayerScore;
    public int PlayerMaxHealth;
    [HideInInspector]
    public int PlayerCurrentHealth;
    public int PlayerMaxPower;
    [HideInInspector]
    public int PlayerCurrentPower;
    public int WaveReached;

    [HideInInspector]
    public int MaxHealthModifiersCount;
    [HideInInspector]
    public int MaxPowerModifiersCount;
    [HideInInspector]
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
        PlayerCurrentHealth = playerMaxHealth;
        PlayerMaxPower = playerMaxPower;
        PlayerCurrentPower = playerMaxPower;
        WaveReached = waveReached;
        MaxHealthModifiersCount = maxHealthModifiersCount;
        MaxPowerModifiersCount = maxPowerModifiersCount;
        ScoreModifiersCount = scoreModifiersCount;
    }

    public string ToJson() {
        return JsonUtility.ToJson(this);
    }

    public static GameState FromJson(string json) {
        return JsonUtility.FromJson<GameState>(json);
    }

    public int CompareTo(object obj) {
        
        if (obj == null) {
            return 1;
        }

        GameState otherGameState = obj as GameState;
        if (otherGameState != null) {
            return this.PlayerScore.CompareTo(otherGameState.PlayerScore);
        } else {
            throw new ArgumentException("Compared object is not a GameState instance");
        }
    }
}

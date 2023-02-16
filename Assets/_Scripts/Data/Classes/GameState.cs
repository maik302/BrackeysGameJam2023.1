using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameState {

    [SerializeField]
    public int PlayerScore { get; set; }
    [SerializeField]
    public int PlayerMaxHealth { get; set; }
    [SerializeField]
    public int PlayerPowerLevel { get; set; }
    [SerializeField]
    public int WaveReached { get; set; }

    private GameState(int playerScore, int playerMaxHealth, int playerPowerLevel, int waveReached) {
        PlayerScore = playerScore;
        PlayerMaxHealth = playerMaxHealth;
        PlayerPowerLevel = playerPowerLevel;
        WaveReached = waveReached;
    }

    // TODO: Move this to the GameManager class
    public GameState CopyKeepingModifier(GameModifiers modifier = GameModifiers.None) {
        return modifier switch {
            GameModifiers.MaxHealth => new GameState(0, PlayerMaxHealth, 1, 1),
            GameModifiers.Power => new GameState(0, 3, PlayerPowerLevel, 1),
            GameModifiers.Score => new GameState(PlayerScore, 3, 1, 1),
            GameModifiers.None => new GameState(0, 3, 1, 1),
            _ => new GameState(0, 3, 1, 1),
        };
    }

    public GameState Copy() {
        return new GameState(PlayerScore, PlayerMaxHealth, PlayerPowerLevel, WaveReached);
    }
}

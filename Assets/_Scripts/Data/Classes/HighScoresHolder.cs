using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Serialization;

[System.Serializable]
public class HighScoresHolder {

    [SerializeField]
    List<GameState> HighScoreGameStates;

    public HighScoresHolder() {
        HighScoreGameStates = new List<GameState>();
    }

    public HighScoresHolder(List<GameState> initialHighScores) {
        HighScoreGameStates = initialHighScores;
    }

    public void Add(GameState gameState) {
        HighScoreGameStates.Add(gameState);
    }

    public void SortDescending() {
        HighScoreGameStates.OrderByDescending(gameState => gameState);
    }

    public List<GameState> Take(int n) {
        return HighScoreGameStates.Take(n).ToList<GameState>();
    }

    public string ToJson() {
        return JsonUtility.ToJson(this);
    }

    public static HighScoresHolder FromJson(string json) {
        return JsonUtility.FromJson<HighScoresHolder>(json);
    }

    public int Count() {
        return HighScoreGameStates.Count;
    }

    public GameState Get(int index) {
        return HighScoreGameStates[index];
    }
}

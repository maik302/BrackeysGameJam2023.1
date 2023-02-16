using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerPrefsUtils {

    public static HighScoresHolder GetHighScoresFromPlayerPrefs() {
        return HighScoresHolder.FromJson(PlayerPrefs.GetString(PlayerPrefsKeys.HighScoresKey));
    }

    public static void SaveHighScoresToPlayerPrefs(HighScoresHolder highScoresHolder) {
        PlayerPrefs.SetString(PlayerPrefsKeys.HighScoresKey, highScoresHolder.ToJson());
    }
}

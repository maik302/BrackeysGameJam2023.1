using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoresContainerController : MonoBehaviour {

    [SerializeField]
    GameObject _scoreRow0;
    [SerializeField]
    GameObject _scoreRow1;
    [SerializeField]
    GameObject _scoreRow2;
    [SerializeField]
    GameObject _scoreRow3;
    [SerializeField]
    GameObject _scoreRow4;
    [SerializeField]
    GameObject _emptyScoresContainer;

    // Start is called before the first frame update
    void Start() {
        SetUpSavedHighScores();
    }

    void SetUpSavedHighScores() {
        var highScoresHolder = PlayerPrefsUtils.GetHighScoresFromPlayerPrefs();
        if (highScoresHolder != null && highScoresHolder.Count() > 0) {
            _emptyScoresContainer.SetActive(false);

            var highScoresCount = highScoresHolder.Count();
            highScoresHolder.SortDescending();

            // Row 0
            if (highScoresCount == 1) {
                _scoreRow0.SetActive(true);
                SetUpScoreRow(_scoreRow0, highScoresHolder.Get(0));
            } else {
                _scoreRow0.SetActive(false);
            }

            // Row 1
            if (highScoresCount == 2) {
                _scoreRow0.SetActive(true);
                _scoreRow1.SetActive(true);
                SetUpScoreRow(_scoreRow0, highScoresHolder.Get(0));
                SetUpScoreRow(_scoreRow1, highScoresHolder.Get(1));
            } else {
                _scoreRow1.SetActive(false);
            }

            // Row 2
            if (highScoresCount == 3) {
                _scoreRow0.SetActive(true);
                _scoreRow1.SetActive(true);
                _scoreRow2.SetActive(true);
                SetUpScoreRow(_scoreRow0, highScoresHolder.Get(0));
                SetUpScoreRow(_scoreRow1, highScoresHolder.Get(1));
                SetUpScoreRow(_scoreRow2, highScoresHolder.Get(2));
            } else {
                _scoreRow2.SetActive(false);
            }

            // Row 3
            if (highScoresCount == 4) {
                _scoreRow0.SetActive(true);
                _scoreRow1.SetActive(true);
                _scoreRow2.SetActive(true);
                _scoreRow3.SetActive(true);
                SetUpScoreRow(_scoreRow0, highScoresHolder.Get(0));
                SetUpScoreRow(_scoreRow1, highScoresHolder.Get(1));
                SetUpScoreRow(_scoreRow2, highScoresHolder.Get(2));
                SetUpScoreRow(_scoreRow3, highScoresHolder.Get(3));
            } else {
                _scoreRow3.SetActive(false);
            }

            // Row 4
            if (highScoresCount == 5) {
                _scoreRow0.SetActive(true);
                _scoreRow1.SetActive(true);
                _scoreRow2.SetActive(true);
                _scoreRow3.SetActive(true);
                _scoreRow4.SetActive(true);
                SetUpScoreRow(_scoreRow0, highScoresHolder.Get(0));
                SetUpScoreRow(_scoreRow1, highScoresHolder.Get(1));
                SetUpScoreRow(_scoreRow2, highScoresHolder.Get(2));
                SetUpScoreRow(_scoreRow3, highScoresHolder.Get(3));
                SetUpScoreRow(_scoreRow4, highScoresHolder.Get(4));
            } else {
                _scoreRow4.SetActive(false);
            }
        } else {
            _scoreRow0.SetActive(false);
            _scoreRow1.SetActive(false);
            _scoreRow2.SetActive(false);
            _scoreRow3.SetActive(false);
            _scoreRow4.SetActive(false);
            _emptyScoresContainer.SetActive(true);
        }
    }

    void SetUpScoreRow(GameObject scoreRow, GameState scoredGameState) {
        var scoreRowController = scoreRow.transform.GetComponent<ScoreRowController>();
        if (scoreRowController != null) {
            scoreRowController.SetUpScoreRow(scoredGameState);
        } 
    }
}

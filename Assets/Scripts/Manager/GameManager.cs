using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
	public TMP_Text scoreText;
	public Button resetScore;
	private int _score = 0;
	private const string ScoreKey = "Score"; 

	private void Awake() {
		LoadScore();
		resetScore?.onClick.AddListener(ResetScore);
	}

	public void AddScore(int points) {
		_score += points;
		UpdateScoreText();
		SaveScore();
	}

	private void UpdateScoreText() 
	{
		scoreText?.text = "Score: " + _score.ToString();
	}

	public int GetScore() => _score

	private void SaveScore() {
		PlayerPrefs.SetInt(ScoreKey, _score); 
		PlayerPrefs.Save(); 
	}

	private void LoadScore() {
		_score = PlayerPrefs.GetInt(ScoreKey, 0); 
		UpdateScoreText(); 
	}

	private void ResetScore() {
		_score = 0;
		UpdateScoreText(); 
		SaveScore(); 
	}
}

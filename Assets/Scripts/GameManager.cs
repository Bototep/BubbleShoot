using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance;
	public TMP_Text scoreText;
	public Button resetScore;

	private int _score = 0;

	private const string ScoreKey = "Score"; // The key to store score in PlayerPrefs

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else
		{
			Destroy(gameObject);
		}

		DontDestroyOnLoad(gameObject);

		// Load the saved score when the game starts
		LoadScore();

		if (resetScore != null)
		{
			resetScore.onClick.AddListener(ResetScore);
		}
	}

	public void AddScore(int points)
	{
		_score += points;
		UpdateScoreText();
		SaveScore();
	}

	private void UpdateScoreText()
	{
		if (scoreText != null)
		{
			scoreText.text = "Score: " + _score.ToString();
		}
	}

	public int GetScore()
	{
		return _score;
	}

	private void SaveScore()
	{
		PlayerPrefs.SetInt(ScoreKey, _score); // Save the score using PlayerPrefs
		PlayerPrefs.Save(); // Ensure the data is written to disk
	}

	private void LoadScore()
	{
		_score = PlayerPrefs.GetInt(ScoreKey, 0); // Load the score, default to 0 if not found
		UpdateScoreText(); // Update the UI text with the loaded score
	}

	private void ResetScore()
	{
		_score = 0; // Reset score to 0
		UpdateScoreText(); // Update the UI text to show the reset score
		SaveScore(); // Save the reset score to PlayerPrefs
	}
}

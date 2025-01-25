using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance;
	public TMP_Text scoreText;
	public Button resetScore;

	private int _score = 0;

	private const string ScoreKey = "Score"; 

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
		PlayerPrefs.SetInt(ScoreKey, _score); 
		PlayerPrefs.Save(); 
	}

	private void LoadScore()
	{
		_score = PlayerPrefs.GetInt(ScoreKey, 0); 
		UpdateScoreText(); 
	}

	private void ResetScore()
	{
		_score = 0; 
		UpdateScoreText(); 
		SaveScore(); 
	}
}

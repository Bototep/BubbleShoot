using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance;
	public TMP_Text scoreText;
	public TMP_Text gameOverText;
	public Button resetScore;

	private int _score;
	private const string ScoreKey = "Score";

	private readonly Dictionary<GameObject, float> bubbleStayTimers = new Dictionary<GameObject, float>();
	private const float TimeToGameOver = 5f; 

	void Awake()
	{
		if (Instance == null) Instance = this;
		else Destroy(gameObject);
		DontDestroyOnLoad(gameObject);
		LoadScore();
		if (resetScore != null) resetScore.onClick.AddListener(ResetScore);
	}

	public void AddScore(int points)
	{
		_score += points;
		UpdateScoreText();
		SaveScore();
	}

	void UpdateScoreText()
	{
		if (scoreText != null) scoreText.text = "Score: " + _score;
	}

	public int GetScore() => _score;

	void SaveScore()
	{
		PlayerPrefs.SetInt(ScoreKey, _score);
		PlayerPrefs.Save();
	}

	void LoadScore()
	{
		_score = PlayerPrefs.GetInt(ScoreKey, 0);
		UpdateScoreText();
	}

	void ResetScore()
	{
		_score = 0;
		UpdateScoreText();
		SaveScore();
	}

	private void GameOver()
	{
		Time.timeScale = 0f;
		if (gameOverText != null) gameOverText.gameObject.SetActive(true);
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.layer == LayerMask.NameToLayer("Bubble"))
		{
			Rigidbody2D bubbleRb = other.GetComponent<Rigidbody2D>();
			if (bubbleRb != null && bubbleRb.velocity.y < 0)
			{
				GameOver(); 
			}

			if (!bubbleStayTimers.ContainsKey(other.gameObject))
			{
				bubbleStayTimers[other.gameObject] = 0f;
			}
		}
	}

	private void OnTriggerStay2D(Collider2D other)
	{
		if (other.gameObject.layer == LayerMask.NameToLayer("Bubble"))
		{
			if (bubbleStayTimers.ContainsKey(other.gameObject))
			{
				bubbleStayTimers[other.gameObject] += Time.deltaTime;

				if (bubbleStayTimers[other.gameObject] >= TimeToGameOver)
				{
					GameOver(); 
				}
			}
		}
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		if (other.gameObject.layer == LayerMask.NameToLayer("Bubble"))
		{
			if (bubbleStayTimers.ContainsKey(other.gameObject))
			{
				bubbleStayTimers.Remove(other.gameObject);
			}
		}
	}
}

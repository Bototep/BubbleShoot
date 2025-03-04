using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
	public TMP_Text scoreText;
	public TMP_Text gameOverText;
	public Button restartButton;
	public Button resetScore;

	public PlayerShooting playerShooting;
	private int _score;
	private const string ScoreKey = "Score";

	private readonly Dictionary<GameObject, float> bubbleStayTimers = new Dictionary<GameObject, float>();
	private const float TimeToGameOver = 10f;

	void Awake()
	{
		// Find any existing GameManager in the scene
		GameManager existingManager = FindObjectOfType<GameManager>();

		if (existingManager != null && existingManager != this)
		{
			Destroy(gameObject); 
			return;
		}

		Time.timeScale = 1f; 
		LoadScore();

		if (resetScore != null) resetScore.onClick.AddListener(ResetScore);
		if (restartButton != null) restartButton.onClick.AddListener(Restart);
	}


	public void AddScore(int points)
	{
		_score += points;
		UpdateScoreText();
		SaveScore();
	}

	void UpdateScoreText()
	{
		if (scoreText != null) scoreText.text = _score.ToString();
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
		playerShooting.GameOver();
		Time.timeScale = 0f;
		if (gameOverText != null) gameOverText.gameObject.SetActive(true);
		if (restartButton != null) restartButton.gameObject.SetActive(true);
	}

	private void Restart()
	{
		Time.timeScale = 1f;
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		playerShooting.RestartGame();

		if (SkillManager.Instance != null)
		{
			SkillManager.Instance.ResetSkills();
		}
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
			bubbleStayTimers.Remove(other.gameObject);
		}
	}
}

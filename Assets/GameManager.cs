using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int leftPlayerScore = 0;
    public int rightPlayerScore = 0;

    private TextMeshProUGUI leftScoreText;
    private TextMeshProUGUI rightScoreText;

    [Header("Audio Settings")]
    public AudioClip goalClip;
    private AudioSource audioSource;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        leftScoreText = GameObject.FindGameObjectWithTag("LeftScore").GetComponent<TextMeshProUGUI>();
        rightScoreText = GameObject.FindGameObjectWithTag("RightScore").GetComponent<TextMeshProUGUI>();

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    public void GoalScored(bool isLeftGoal)
    {
        if (isLeftGoal)
        {
            rightPlayerScore++;
            Debug.Log("Right player scores! Score: "
                      + leftPlayerScore + " - " + rightPlayerScore);
        }
        else
        {
            leftPlayerScore++;
            Debug.Log("Left player scores! Score: "
                      + leftPlayerScore + " - " + rightPlayerScore);
        }

        PlayGoalSound();

        UpdateScoreTexts();

        // Invoke(nameof(ResetBall), goalClip.length);
        ResetBall();
    }

    private void ResetBall()
    {
        Ball ball = Object.FindAnyObjectByType<Ball>();
        if (ball != null)
        {
            ball.ResetPositionAndLaunch();
        }
    }

    private void UpdateScoreTexts()
    {
        leftScoreText.text = leftPlayerScore.ToString();
        rightScoreText.text = rightPlayerScore.ToString();
    }

    private void PlayGoalSound()
    {
        if (goalClip == null || audioSource == null)
        {
            Debug.LogWarning("Goal sound clip or AudioSource is missing!");
            return;
        }

        audioSource.PlayOneShot(goalClip);
    }

}

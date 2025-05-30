using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int score = 0;
    public int health = 100;
    public Text scoreText;
    public Slider healthSlider;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        UpdateUI();
    }

    public void AddScore(int amount)
    {
        score += amount;
        UpdateUI();
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        UpdateUI();

        if (health <= 0)
        {
            Debug.Log("Player Dead");
        }
    }

    void UpdateUI()
    {
        scoreText.text = "Score: " + score;
        healthSlider.value = health;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerStats : MonoBehaviour
{
    [Header("Main UI")]
    [SerializeField] private TextMeshProUGUI killsText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private TextMeshProUGUI timeText;
    [Min(0), SerializeField] private int startingMoney = 0, maxMoney = 0, scoreToGive = 0;
    [SerializeField] private bool stopTimeCountup = false;

    [Header("Lose Screen")]
    [SerializeField] private TextMeshProUGUI lsKillsText;
    [SerializeField] private TextMeshProUGUI lsScoreText;
    [SerializeField] private TextMeshProUGUI lsMoneyText;
    [SerializeField] private TextMeshProUGUI lsTimeText;

    //Internal Variables
    private int kills = 0, money = 0, score = 0;
    private float seconds = 0, minutes = 0, hours = 0;
    private string tempTime = "";

    // Start is called before the first frame update
    private void Start()
    {
        money = startingMoney;

        if (maxMoney == 0)
        {
            maxMoney = startingMoney;
        }
    }

    // Update is called once per frame
    private void Update()
    {
        UpdateText();
    }

    private void UpdateText()
    {
        if (Time.deltaTime == 0)
        {
            lsKillsText.text = $"{kills}";
            lsScoreText.text = $"{score}";
            lsMoneyText.text = $"${money}";
            lsTimeText.text = tempTime;
        }
        else
        {
            killsText.text = $"Kills: {kills}";
            scoreText.text = $"Score: {score}";
            moneyText.text = $"Money: ${money}";
        }
        if (stopTimeCountup)
        {
            return;
        }
        seconds += Time.deltaTime;
        if (seconds >= 60)
        {
            minutes++;
            seconds = 0;
        }
        else if (minutes >= 60)
        {
            hours++;
            minutes = 0;
        }
        timeText.text = $"Time: {hours}:{minutes}:{(int)seconds}";
        tempTime = $"{hours}:{minutes}:{(int)seconds}";
        Debug.Log(timeText.text);
    }

    public void AddKill()
    {
        kills++;
    }

    public void AddMoney(int money)
    {
        if (money <= maxMoney)
        {
            this.money += money;
        }
    }

    public void AddScore(int score)
    {
        this.score += score;
    }
}

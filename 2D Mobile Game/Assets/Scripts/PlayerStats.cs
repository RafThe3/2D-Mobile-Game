using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI killsText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private TextMeshProUGUI timeText;
    [Min(0), SerializeField] private int startingMoney = 0, maxMoney = 0, scoreToGive = 0;

    //Internal Variables
    private int kills = 0, money = 0, score = 0;
    float seconds = 0, minutes = 0, hours = 0;

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
        //killsText.text = $"{kills}";
        //scoreText.text = $"{score}";
        //moneyText.text = $"{money}";
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
        timeText.text = $"{hours}:{minutes}:{(int)seconds}";
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

    public void AddCustomScore(int score)
    {
        this.score += score;
    }
    
    public void AddDefaultScore()
    {
        score += scoreToGive;
    }
}

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
    [Min(0), SerializeField] private int startingMoney = 0, maxMoney = 0;

    [Header("Lose Screen")]
    [SerializeField] private TextMeshProUGUI lsKillsText;
    [SerializeField] private TextMeshProUGUI lsScoreText;
    [SerializeField] private TextMeshProUGUI lsMoneyText;
    [SerializeField] private TextMeshProUGUI lsTimeText;

    [Header("Win Screen")]
    [SerializeField] private TextMeshProUGUI wsKillsText;
    [SerializeField] private TextMeshProUGUI wsScoreText;
    [SerializeField] private TextMeshProUGUI wsMoneyText;
    [SerializeField] private TextMeshProUGUI wsTimeText;

    [Header("Merchant UI")]
    [SerializeField] private TextMeshProUGUI merchantMoneyText;

    //Internal Variables
    private int kills = 0, money = 0, score = 0;
    private float seconds = 0, minutes = 0, hours = 0;
    private string tempTime = "";
    private MerchantUI merchant;
    private Player player;
    private Gun gun;
    private const int maxInt = 10000;

    // Start is called before the first frame update
    private void Start()
    {
        money = PlayerPrefs.GetInt("Money");

        if (maxMoney == 0)
        {
            maxMoney = startingMoney;
        }
    }

    private void Awake()
    {
        merchant = FindObjectOfType<MerchantUI>();
        player = FindObjectOfType<Player>();
        gun = FindObjectOfType<Gun>();
    }

    // Update is called once per frame
    private void Update()
    {
        PlayerPrefs.SetInt("Money", money);

        //Test
        if (Input.GetKey(KeyCode.M))
        {
            AddMoney(10);
        }
        if (Input.GetKey(KeyCode.N))
        {
            SubtractMoney(10);
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            SubtractMoney(68);
        }

        UpdateText();
    }

    private void UpdateText()
    {
        if (money < 0)
        {
            money = 0;
        }

        if (Time.deltaTime == 0)
        {
            lsKillsText.text = $"{kills}";
            lsScoreText.text = $"{score}";
            lsMoneyText.text = $"${money}";
            lsTimeText.text = tempTime;

            //wsKillsText.text = $"{kills}";
            //wsScoreText.text = $"{score}";
            //wsMoneyText.text = $"${money}";
            //wsTimeText.text = tempTime;

            merchantMoneyText.text = $"${money}";
        }
        else
        {
            killsText.text = $"Kills: {kills}";
            scoreText.text = $"Score: {score}";
            moneyText.text = $"${money}";
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

    public int GetMoney()
    {
        return money;
    }

    public void SubtractMoney(int money)
    {
        if (money > 0 && this.money - money >= 0)
        {
            this.money -= money;
        }
    }

    public void AddScore(int score)
    {
        this.score += score;
    }

    public void GiveReward(string reward)
    {
        if (money - merchant.GetRewardPrice(1) >= 0 && reward == "Reload")
        {
            FastReload();
            SubtractMoney(merchant.GetRewardPrice(1));
            merchant.AddRewardPrice("Reload", merchant.GetRewardPrice(1) / 2);
        }
        else if (money - merchant.GetRewardPrice(2) >= 0 && reward == "Dash")
        {
            FastDash();
            SubtractMoney(merchant.GetRewardPrice(2));
            merchant.AddRewardPrice("Dash", merchant.GetRewardPrice(2) / 2);
        }
        else if (money - merchant.GetRewardPrice(3) >= 0 && reward == "Shoot")
        {
            FastShoot();
            SubtractMoney(merchant.GetRewardPrice(3));
            merchant.AddRewardPrice("Shoot", merchant.GetRewardPrice(3) / 2);
        }
        else if (money - merchant.GetRewardPrice(4) >= 0 && reward == "Run")
        {
            FastWalk();
            SubtractMoney(merchant.GetRewardPrice(4));
            merchant.AddRewardPrice("Run", merchant.GetRewardPrice(4) / 2);
        }
    }

    private void FastReload()
    {
        gun.SubtractReloadSpeed(0.15f);
    }

    private void FastDash()
    {
        player.SubtractDashInterval(0.15f);
    }

    private void FastWalk()
    {
        player.AddSpeed(2);
    }

    private void FastShoot()
    {
        gun.AddShootSpeed(0.025f);
    }
}

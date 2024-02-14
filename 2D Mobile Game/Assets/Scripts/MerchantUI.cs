using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MerchantUI : MonoBehaviour
{
    [SerializeField] private Canvas merchantUI;
    [SerializeField] private TextMeshProUGUI fastReloadText;
    [SerializeField] private TextMeshProUGUI fastDashText;
    [SerializeField] private TextMeshProUGUI fastShootText;
    [SerializeField] private TextMeshProUGUI fastRunText;

    [Min(0), SerializeField] private int fastReloadPrice = 1;
    [Min(0), SerializeField] private int fastDashPrice = 1;
    [Min(0), SerializeField] private int fastShootPrice = 1;
    [Min(0), SerializeField] private int fastRunPrice = 1;

    private void Start()
    {
        merchantUI.enabled = false;
        fastDashPrice = PlayerPrefs.GetInt("Dash");
        fastReloadPrice = PlayerPrefs.GetInt("Reload");
        fastRunPrice = PlayerPrefs.GetInt("Run");
        fastShootPrice = PlayerPrefs.GetInt("Shoot");
    }

    private void Update()
    {
        PlayerPrefs.SetInt("Reload", fastReloadPrice);
        PlayerPrefs.SetInt("Dash", fastDashPrice);
        PlayerPrefs.SetInt("Shoot", fastShootPrice);
        PlayerPrefs.SetInt("Run", fastRunPrice);

        fastDashText.text = $"${PlayerPrefs.GetInt("Dash")}";
        fastReloadText.text = $"${PlayerPrefs.GetInt("Reload")}";
        fastRunText.text = $"${PlayerPrefs.GetInt("Run")}";
        fastShootText.text = $"${PlayerPrefs.GetInt("Shoot")}";
    }

    public void OpenUI()
    {
        merchantUI.enabled = true;
        Time.timeScale = 0;
    }

    public void CloseUI()
    {
        merchantUI.enabled = false;
        Time.timeScale = 1;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="reward">1 - reload, 2 - dash, 3 - shoot, 4 - run</param>
    /// <returns></returns>
    public int GetRewardPrice(int reward)
    {
        if (reward == 1)
        {
            return fastReloadPrice;
        }
        else if (reward == 2)
        {
            return fastDashPrice;
        }
        else if (reward == 3)
        {
            return fastShootPrice;
        }
        else if (reward == 4)
        {
            return fastRunPrice;
        }
        else
        {
            return 0;
        }
    }

    public void AddRewardPrice(string reward, int price)
    {
        if (reward == "Reload")
        {
            fastReloadPrice += price;
        }
        else if (reward == "Shoot")
        {
            fastShootPrice += price;
        }
        else if (reward == "Run")
        {
            fastRunPrice += price;
        }
        else if (reward == "Dash")
        {
            fastDashPrice += price;
        }
    }

    public void SetRewardPrice(string reward, int price)
    {
        if (reward == "Reload")
        {
            fastReloadPrice = price;
        }
        else if (reward == "Shoot")
        {
            fastShootPrice = price;
        }
        else if (reward == "Run")
        {
            fastRunPrice = price;
        }
        else if (reward == "Dash")
        {
            fastDashPrice = price;
        }
    }
}

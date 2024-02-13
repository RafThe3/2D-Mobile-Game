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
        fastDashText.text = $"${fastDashPrice}";
        fastReloadText.text = $"${fastReloadPrice}";
        fastRunText.text = $"${fastRunPrice}";
        fastShootText.text = $"${fastShootPrice}";
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
}

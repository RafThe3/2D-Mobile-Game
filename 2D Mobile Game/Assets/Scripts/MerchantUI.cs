using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MerchantUI : MonoBehaviour
{
    [SerializeField] private Canvas merchantUI;

    private void Start()
    {
        merchantUI.enabled = false;
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
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Merchant : MonoBehaviour
{
    [Min(0), SerializeField] private float interactDistance = 1;

    [SerializeField] private TextMeshProUGUI interactButtonText;
    [SerializeField] private Button interactButton;

    private Vector3 interactTextScale;

    private void Start()
    {
        interactTextScale = interactButtonText.transform.localScale;
    }

    private void Update()
    {
        Vector3 playerPosition = FindObjectOfType<Player>().transform.position - transform.position;
        bool playerIsClose = playerPosition.magnitude < interactDistance;
        bool hasInteracted = Input.GetKeyDown(KeyCode.F);

        UpdateText(playerIsClose);

        if (playerIsClose)
        {
            FlipSprite();
            if (hasInteracted)
            {
                FindObjectOfType<MerchantUI>().OpenUI();
            }
        }
    }

    private void UpdateText(bool playerIsClose)
    {
        const string keyControlsInteractionText = "Press F to Interact";
        const string mobileControlsInteractionText = "Press [Interact] to Interact";
        Vector3 originalScale = gameObject.transform.localScale.x < 0 ? new(-interactTextScale.x, interactTextScale.y, interactTextScale.z)
                                : interactTextScale;
        interactButtonText.transform.localScale = originalScale;

        interactButtonText.text = FindObjectOfType<Player>().AllowsKeyControls() ? keyControlsInteractionText : mobileControlsInteractionText;
        interactButtonText.enabled = playerIsClose;
        interactButton.gameObject.SetActive(playerIsClose);
    }

    private void FlipSprite()
    {
        GameObject player = GameObject.FindWithTag("Player");
        transform.localScale = new Vector2(-player.transform.localScale.x, 1);
    }
}

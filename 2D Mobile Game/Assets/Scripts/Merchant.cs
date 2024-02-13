using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Merchant : MonoBehaviour
{
    [Min(0), SerializeField] private float interactDistance = 1;

    private void Update()
    {
        Vector3 playerPosition = FindObjectOfType<Player>().transform.position - transform.position;
        bool playerIsClose = playerPosition.magnitude < interactDistance;
        bool hasInteracted = Input.GetKeyDown(KeyCode.F);

        if (playerIsClose)
        {
            FlipSprite();
            if (hasInteracted)
            {
                FindObjectOfType<MerchantUI>().OpenUI();
            }
        }
    }

    private void FlipSprite()
    {
        GameObject player = GameObject.FindWithTag("Player");
        transform.localScale = new Vector2(-player.transform.localScale.x, 1);
    }
}

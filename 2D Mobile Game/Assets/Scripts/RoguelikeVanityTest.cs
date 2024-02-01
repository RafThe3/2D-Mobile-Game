using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoguelikeVanityTest : MonoBehaviour
{
    //needs to be set to "true" otherwise vanities will never be visible, even if their respective bool is set to "true"
    [SerializeField] private bool vanityEnabled;
    [Min(0), SerializeField] private int vanityNumber;

    [SerializeField] private GameObject[] vanity;

    void Start()
    {
       vanity = GameObject.FindGameObjectsWithTag("Vanity");
    }

    void Update()
    {
        UpdateVanity();
    }

    private void UpdateVanity()
    {
        if (!vanityEnabled)
        {
            return;
        }
        else
        {
            vanity[vanityNumber].GetComponent<SpriteRenderer>().enabled = vanityNumber == 1 ? vanity[1].GetComponent<SpriteRenderer>().enabled = true
                : vanityNumber == 2 ? vanity[2].GetComponent<SpriteRenderer>().enabled = true
                : vanityNumber == 3 ? vanity[2].GetComponent<SpriteRenderer>().enabled = true
                : vanityNumber == 4 ? vanity[2].GetComponent<SpriteRenderer>().enabled = true
                : true;
        }
    }
}

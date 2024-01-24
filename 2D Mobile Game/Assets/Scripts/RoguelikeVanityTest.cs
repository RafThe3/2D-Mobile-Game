using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoguelikeVanityTest : MonoBehaviour
{
    public bool vanityEnabled;

    public bool hasNecklace;
    public bool hasPropeller;
    public bool hasShield;
    public GameObject necklace;
    public GameObject propeller;
    public GameObject shield;

    void Start()
    {
        vanityEnabled = false;
        necklace = GameObject.Find("NecklaceVanity");
        propeller = GameObject.Find("PropellerVanity");
        shield = GameObject.Find("ShieldVanity");
    }
    void Update()
    {
        if(!vanityEnabled)
        {
            necklace.GetComponent<SpriteRenderer>().enabled = false;
            propeller.GetComponent<SpriteRenderer>().enabled = false;
            shield.GetComponent<SpriteRenderer>().enabled = false;
        }
        else
        {
            if(hasNecklace)
            {
                necklace.GetComponent<SpriteRenderer>().enabled = true;
            }
            if (hasPropeller)
            {
                propeller.GetComponent<SpriteRenderer>().enabled = true;
            }
            if (hasShield)
            {
                shield.GetComponent<SpriteRenderer>().enabled = true;
            }

        }
    }
}

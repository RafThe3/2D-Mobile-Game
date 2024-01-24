using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoguelikeVanityTest : MonoBehaviour
{
    //needs to be set to "true" otherwise vanities will never be visible, even if their respective bool is set to "true"
    public bool vanityEnabled;

    //bools that check for object in question, if set to "true" will display vanity associated with that object
    #region Vanity Bools
    public bool hasNecklace;
    public bool hasPropeller;
    public bool hasShield;
    public bool hasSmile;
    #endregion

    //gameobject variables for each vanity
    #region Vanity GameObjects
    GameObject necklace;
    GameObject propeller;
    GameObject shield;
    GameObject smile;
    #endregion

    void Start()
    {
        //finds and sets respective gameobject variables to the associated GameObject
        vanityEnabled = false;
        necklace = GameObject.Find("NecklaceVanity");
        propeller = GameObject.Find("PropellerVanity");
        shield = GameObject.Find("ShieldVanity");
        smile = GameObject.Find("SmileVanity");
    }
    void Update()
    {
        //checks if the vanityEnabled bool is set to false. if it is, all vanities will NOT show
        if(!vanityEnabled)
        {
            necklace.GetComponent<SpriteRenderer>().enabled = false;
            propeller.GetComponent<SpriteRenderer>().enabled = false;
            shield.GetComponent<SpriteRenderer>().enabled = false;
            smile.GetComponent<SpriteRenderer>().enabled = false;
        }
        else
        {
            //bad code that just shows vanities if their respective bool is set to "true". if there's a better way to do this please let me know
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
            if(hasSmile)
            {
                smile.GetComponent<SpriteRenderer>().enabled = true;
            }
        }
    }
}

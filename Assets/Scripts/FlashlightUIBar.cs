using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FlashlightUIBar : MonoBehaviour
{
    public Flashlight flashlight;
    public int barToDissepear;

    private void Update()
    {
        if (flashlight.battery < barToDissepear)
        {
            gameObject.GetComponent<Image>().enabled = false;
        }

        if (flashlight.battery > barToDissepear)
        {
            gameObject.GetComponent<Image>().enabled = true;
        }
    }

}

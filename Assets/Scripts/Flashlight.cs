using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour
{
    [Header("Flashlight")]
    public GameObject flashlight;
    public bool flashing = false;
    public int flashOn = 0;
    public int battery = 100;

    private float batteryDrainTimer = 0f;
    private float batteryDrainTick = 10f;

    private void Update()
    {
        if(flashing)
        {
            batteryDrainTimer += Time.deltaTime;
            if (batteryDrainTimer >= batteryDrainTick)
            {
                battery -= 1;
                batteryDrainTimer = 0f;
            }
        }
    }
}

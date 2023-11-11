using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryManager : MonoBehaviour
{
    public int maxBattery = 8;
    public int currentBattery;
    public int blinkTimes;

    private int currnetBlinkTimes;

    public BatteryFill batteryBar;
    public GameObject flashLight;

    void Start()
    {
        currentBattery = maxBattery;
        batteryBar.SetMaxHealth(maxBattery);
        currnetBlinkTimes = blinkTimes;
    }

    private void Update()
    {
        if (currentBattery == 0)
        {
            flashLight.SetActive(false);
        }
    }

    public void DecreaseBattery()
    {
        if (currentBattery <= 0)
        {
            currentBattery = 0;
            return;
        }
        currentBattery--;
        Debug.Log("Current Battery: " + currentBattery);
        batteryBar.SetHealth(currentBattery);
    }

    public void IncreaseBattery(int amount)
    {
        currentBattery += amount;
        batteryBar.SetHealth(currentBattery);
        currnetBlinkTimes = blinkTimes;
        if (!flashLight.activeSelf)
            flashLight.SetActive(true);
    }

    
    private void Blink()
    {
        if (flashLight.activeSelf)
            flashLight.SetActive(false);
        else 
            flashLight.SetActive(true);

        if (currnetBlinkTimes > 0)
        {
            currnetBlinkTimes--;
            Invoke(nameof(Blink), 0.2f);
        }
        else
            flashLight.SetActive(false);
    }
}

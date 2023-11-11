using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public int batteryAmount;
    public bool isBattery;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isBattery)
        {
            if (collision.gameObject.GetComponent<BatteryManager>() != null)
                collision.gameObject.GetComponent<BatteryManager>().IncreaseBattery(batteryAmount);
        }

        this.gameObject.SetActive(false);
    }
}

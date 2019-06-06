using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class RunStats
{
    private static Text distanceHUDTextBox = GameObject.Find("Distance").gameObject.GetComponent<Text>();
    private static Text moneyHUDTextBox = GameObject.Find("Money").gameObject.GetComponent<Text>();
    private static Text distanceMenuTextBox = GameObject.Find("DistanceTextBox").gameObject.GetComponent<Text>();
    private static Text moneyMenuTextBox = GameObject.Find("MoneyTextBox").gameObject.GetComponent<Text>();
    private static string seed = null;
    public static string Seed
    {
        get
        {
            return seed;
        }
        set
        {
            seed = value;
            Text tmp = GameObject.Find("SeedTextBox").gameObject.GetComponent<Text>();
            if(tmp != null)
            {
                tmp.text = seed;
            }
        }
    }

    public static void Reset()
    {
        distanceHUDTextBox = GameObject.Find("Distance").gameObject.GetComponent<Text>();
        moneyHUDTextBox = GameObject.Find("Money").gameObject.GetComponent<Text>();

        distanceMenuTextBox = GameObject.Find("DistanceTextBox").gameObject.GetComponent<Text>();
        moneyMenuTextBox = GameObject.Find("MoneyTextBox").gameObject.GetComponent<Text>();
        Money = 0;
        Distance = 0;
        
    }
    private static int money = 0;
    public static int Money
    {
        get
        {
            return money;
        }
        set
        {
            money = value;
            if(moneyHUDTextBox == null || moneyMenuTextBox == null)
            {
                GameObject.Find("DeloreanFBX").gameObject.GetComponent<Controls>().Reset();
            }
            moneyHUDTextBox.text = moneyMenuTextBox.text = money.ToString();
        }
    }
    private static float distance = 0;
    public static float Distance
    {
        get
        {
            return distance;
        }
        set
        {
            distance = value;
            if(distanceHUDTextBox == null || distanceMenuTextBox == null)
            {
                GameObject.Find("DeloreanFBX").gameObject.GetComponent<Controls>().Reset();
            }
            distanceHUDTextBox.text = distanceMenuTextBox.text = Mathf.Floor(distance).ToString();
        }
    }
}

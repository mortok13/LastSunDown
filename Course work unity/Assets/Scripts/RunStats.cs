using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RunStats
{
    private static Text distanceTextBox = GameObject.Find("Distance").gameObject.GetComponent<Text>();
    private static Text moneyTextBox = GameObject.Find("Money").gameObject.GetComponent<Text>();
    public static string seed = null;
    public static string Seed
    {
        get
        {
            return seed;
        }
        set
        {
            Seed = value;
        }
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
            moneyTextBox.text = money.ToString();
        }
    }
    private static int distance = 0;
    public static int Distance
    {
        get
        {
            return distance;
        }
        set
        {
            distance = value;
            distanceTextBox.text = distance.ToString();
        }
    }
    public static Time time = null;
}

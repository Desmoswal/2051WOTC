using UnityEngine;
using System;

public class DataTranslator : MonoBehaviour {

    private const string KILLS_TAG = "[KILLS]";
    private const string DEATHS_TAG = "[DEATHS]";

    public static string ValuesToData(int kills, int deaths)
    {
        return KILLS_TAG + kills + "/" + DEATHS_TAG + deaths;
    }

    public static int DataToKills(string data)
    {
        return int.Parse(DataToValue(data, KILLS_TAG));
    }
	
    public static int DataToDeaths(string data)
    {
        return int.Parse(DataToValue(data, DEATHS_TAG));
    }

    private static string DataToValue(string data, string tag)
    {
        string[] values = data.Split('/');
        foreach (string value in values)
        {
            if (value.StartsWith(tag))
            {
                return value.Substring(tag.Length);
            }
        }

        Debug.LogError(tag + " not found in " + data);
        return "";
    }
}

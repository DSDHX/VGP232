using System.Collections.Generic;
using UnityEngine;

public enum SupportedLanguage { En, Fr, Sp }

[System.Serializable]
public class LocData
{
    public string locKey;
    public string en;
    public string fr;
    public string sp;
}

[CreateAssetMenu(fileName = "LanguageData", menuName = "GameData/Language")]
public class Language : ScriptableObject
{
    public SupportedLanguage currentLanguage = SupportedLanguage.En;
    public List<LocData> locDataList = new List<LocData>();

    public string GetText(string key)
    {
        LocData data = locDataList.Find(x => x.locKey == key);
        if (data == null)
        {
            return key;
        }

        switch (currentLanguage)
        {
            case SupportedLanguage.Fr: return data.fr;
            case SupportedLanguage.Sp: return data.sp;
            case SupportedLanguage.En:
            default: return data.en;
        }
    }
}
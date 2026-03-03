using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardList", menuName = "GameData/Card List")]
public class CardList : ScriptableObject
{
    [Tooltip("The relative path of the card within the Resources folder, for example: Cards/Dragon")]
    public List<string> cardResourcePaths;

    private Dictionary<string, CardData> loadedCards = new Dictionary<string, CardData>();

    public CardData GetCard(string path)
    {
        if (loadedCards.ContainsKey(path) && loadedCards[path] != null)
        {
            return loadedCards[path];
        }

        CardData card = Resources.Load<CardData>(path);
        if (card != null)
        {
            loadedCards[path] = card;
        }
        else
        {
            Debug.LogError($"Card data not found in Resources/{path}!");
        }

        return card;
    }
}
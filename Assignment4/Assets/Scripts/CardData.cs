using UnityEngine;

public enum CardType { Monster, Spell, Trap } [CreateAssetMenu(fileName = "NewCard", menuName = "GameData/Card Data")]
public class CardData : ScriptableObject
{
    [Header("Localization Keys")]
    public string nameLocKey;
    public string descLocKey; [Header("Card Attributes")]
    public CardType type;
    public int cost;
    public int att;
    public int def;
    public Sprite image;
}
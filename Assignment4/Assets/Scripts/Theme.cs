using UnityEngine;
using TMPro;

[CreateAssetMenu(fileName = "Theme", menuName = "GameData/Theme")]
public class Theme : ScriptableObject
{
    [Header("Regular Style")]
    public TMP_FontAsset regularFontType;
    public Color regularFontColor = Color.black;
    public Sprite regularButtonStyle;

    [Header("Special Style")]
    public TMP_FontAsset specialFontType;
    public Color specialFontColor = Color.white;
    public Sprite specialFontStyle;
}
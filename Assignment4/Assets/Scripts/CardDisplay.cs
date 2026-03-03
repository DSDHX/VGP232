using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardDisplay : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descText;
    public TextMeshProUGUI statsText;
    public Image cardImage;
    public Image cardBackground;

    private CardData currentCard;

    public void Setup(CardData card, Theme theme, Language language)
    {
        currentCard = card;

        ApplyLocText(language);
        ApplyTheme(theme);
    }

    private void ApplyLocText(Language lang)
    {
        if (currentCard == null || lang == null)
        {
            return;
        }

        nameText.text = lang.GetText(currentCard.nameLocKey);
        descText.text = lang.GetText(currentCard.descLocKey);

        statsText.text = $"Cost: {currentCard.cost}  Att: {currentCard.att}  Def: {currentCard.def}" +
                         $"\nType: {currentCard.type}";
        cardImage.sprite = currentCard.image;
    }

    private void ApplyTheme(Theme theme)
    {
        if (theme == null)
        {
            return;
        }

        nameText.font = theme.specialFontType;
        nameText.color = theme.specialFontColor;

        descText.font = theme.regularFontType;
        descText.color = theme.regularFontColor;
        statsText.font = theme.regularFontType;
        statsText.color = theme.regularFontColor;

        if (cardBackground != null)
        {
            cardBackground.sprite = theme.specialFontStyle;
        }
    }

    public void OnClickCard()
    {
        MainScreenManager.Instance.ShowCardDetail(currentCard);
    }
}
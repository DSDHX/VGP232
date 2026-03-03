using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainScreenManager : MonoBehaviour
{
    public static MainScreenManager Instance;

    [Header("Databases")]
    public CardList cardDatabase;
    public Theme activeTheme;
    public Language activeLanguage;

    [Header("Screens")]
    public GameObject mainMenuScreen;
    public GameObject playScreen;
    public GameObject showCardsScreen;
    public GameObject cardDetailScreen;

    [Header("Main Menu UI")]
    public Button playButton;
    public Button showCardsButton;
    public Button quitButton;
    public TextMeshProUGUI playText, showCardsText, quitText;

    [Header("Card List Setup")]
    public Transform cardListContainer;
    public GameObject cardPrefab;

    [Header("Card Detail Setup")]
    public CardDisplay detailCardDisplay;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        ApplyThemeToMenu();
        ShowMainMenu();
    }

    private void ApplyThemeToMenu()
    {
        playButton.image.sprite = activeTheme.regularButtonStyle;
        showCardsButton.image.sprite = activeTheme.regularButtonStyle;
        quitButton.image.sprite = activeTheme.regularButtonStyle;

        playText.font = activeTheme.regularFontType;
        playText.color = activeTheme.regularFontColor;
        showCardsText.font = activeTheme.regularFontType;
        showCardsText.color = activeTheme.regularFontColor;
        quitText.font = activeTheme.regularFontType;
        quitText.color = activeTheme.regularFontColor;
    }

    public void ShowMainMenu()
    {
        CloseAllScreens();
        mainMenuScreen.SetActive(true);
    }

    public void OnClickPlay()
    {
        CloseAllScreens();
        playScreen.SetActive(true);
    }

    public void OnClickShowCards()
    {
        CloseAllScreens();
        showCardsScreen.SetActive(true);

        foreach (Transform child in cardListContainer)
        {
            Destroy(child.gameObject);
        }

        foreach (string path in cardDatabase.cardResourcePaths)
        {
            CardData card = cardDatabase.GetCard(path);
            if (card != null)
            {
                GameObject newCard = Instantiate(cardPrefab, cardListContainer);
                CardDisplay display = newCard.GetComponent<CardDisplay>();
                display.Setup(card, activeTheme, activeLanguage);

                Button btn = newCard.GetComponent<Button>();
                if (btn == null) btn = newCard.AddComponent<Button>();
                btn.onClick.AddListener(() => display.OnClickCard());
            }
        }
    }

    public void ShowCardDetail(CardData card)
    {
        CloseAllScreens();
        cardDetailScreen.SetActive(true);
        detailCardDisplay.Setup(card, activeTheme, activeLanguage);
    }

    public void OnClickQuit()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    private void CloseAllScreens()
    {
        mainMenuScreen.SetActive(false);
        playScreen.SetActive(false);
        showCardsScreen.SetActive(false);
        cardDetailScreen.SetActive(false);
    }
}
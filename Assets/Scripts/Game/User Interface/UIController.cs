using UnityEngine;

public enum UIMode
{
    Normal,
    Shop
}

public enum UICursorMode
{
    Normal,
    Farmer,
    Modification,
    Fertilizer
}

public class UIController : MonoBehaviour
{
    public static UIController Instance { get; private set; }

    [SerializeField]
    Texture2D _cursorNormalTexture;

    [SerializeField]
    Texture2D _cursorFarmerTexture;

    [SerializeField]
    Texture2D _cursorModificationTexture;

    [SerializeField]
    Texture2D _cursorFertilizerTexture;

    public float totalMoney;

    public UIMode uiMode;
    public UICursorMode cursorMode;

    [SerializeField]
    GameObject _exitButton;


    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void OnEnable()
    {
        ShopButtons.OnFarmerButtonClicked += OnFarmerButtonClicked;
        ShopButtons.OnModificationButtonClicked += OnModificationButtonClicked;
        ShopButtons.OnFertilizerButtonClicked += OnFertilizerButtonClicked;
    }

    private void OnDisable()
    {
        ShopButtons.OnFarmerButtonClicked -= OnFarmerButtonClicked;
        ShopButtons.OnModificationButtonClicked -= OnModificationButtonClicked;
        ShopButtons.OnFertilizerButtonClicked -= OnFertilizerButtonClicked;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // HACK: Just make sure to set this incase the user exitted to menu.
        Time.timeScale = 1;

        totalMoney = 0;
        uiMode = UIMode.Normal;
        cursorMode = UICursorMode.Normal;

        UpdateGameCursor();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SubtractMoneyClamped(float amount)
    {
        totalMoney -= amount;

        if (totalMoney <= 0.0f)
        {
            totalMoney = 0;
        }
    }

    public void ResetUIAndCursorMode()
    {
        uiMode = UIMode.Normal;
        cursorMode = UICursorMode.Normal;
        UpdateGameCursor();
        _exitButton.SetActive(false);
    }

    private void OnFarmerButtonClicked()
    {
        uiMode = UIMode.Shop;
        cursorMode = UICursorMode.Farmer;
        UpdateGameCursor();
        _exitButton.SetActive(true);
    }

    private void OnModificationButtonClicked()
    {
        uiMode = UIMode.Shop;
        cursorMode = UICursorMode.Modification;
        UpdateGameCursor();
        _exitButton.SetActive(true);
    }

    private void OnFertilizerButtonClicked()
    {
        uiMode = UIMode.Shop;
        cursorMode = UICursorMode.Fertilizer;
        UpdateGameCursor();
        _exitButton.SetActive(true);
    }

    public void UpdateGameCursor()
    {
        switch (cursorMode)
        {
            case UICursorMode.Normal:
                Cursor.SetCursor(_cursorNormalTexture, new Vector2(0, 0), CursorMode.Auto);
                break;
            case UICursorMode.Farmer:
                Cursor.SetCursor(_cursorFarmerTexture, new Vector2(0, 0), CursorMode.Auto);
                break;
            case UICursorMode.Modification:
                Cursor.SetCursor(_cursorModificationTexture, new Vector2(0, 0), CursorMode.Auto);
                break;
            case UICursorMode.Fertilizer:
                Cursor.SetCursor(_cursorFertilizerTexture, new Vector2(0, 0), CursorMode.Auto);
                break;
            default:
                break;
        }
    }
}

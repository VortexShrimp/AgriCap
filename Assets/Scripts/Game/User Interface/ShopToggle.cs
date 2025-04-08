using UnityEngine;

public class ShopToggle : MonoBehaviour
{
    public delegate void OnShopToggledDelegate(bool isOpen);
    public static OnShopToggledDelegate OnShopToggled;


    [SerializeField]
    GameObject _panel;

    bool _isPaused;
    bool _isOpen;

    [SerializeField]
    GameObject _exitButton;

    void Start()
    {
        _isPaused = false;
        _isOpen = false;
    }

    void OnEnable()
    {
        PauseToggle.OnPauseStateChanged += OnPausedStateChanged;
        ShopButtons.OnAnyShopButtonClicked += OnAnyShopButtonClicked;
    }

    void OnDisable()
    {
        PauseToggle.OnPauseStateChanged -= OnPausedStateChanged;
        ShopButtons.OnAnyShopButtonClicked += OnAnyShopButtonClicked;
    }

    public void TogglePanel()
    {
        if (_isPaused == true)
        {
            return;
        }

        if (_panel != null)
        {
            _panel.SetActive(!_panel.activeSelf);
            _isOpen = _panel.activeSelf;

            OnShopToggled?.Invoke(_isOpen);

            _exitButton.SetActive(_isOpen);
        }
    }

    private void OnPausedStateChanged(bool newState)
    {
        _isPaused = newState;

        // Close the shop if it was open when the game was paused.
        if (_isOpen == true && _isPaused == true)
        {
            _panel.SetActive(!_panel.activeSelf);
            _isOpen = _panel.activeSelf;

            OnShopToggled?.Invoke(_isOpen);
        }
    }

    private void OnAnyShopButtonClicked()
    {
        TogglePanel();
    }
}

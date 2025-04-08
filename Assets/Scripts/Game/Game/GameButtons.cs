using System.Collections;
using TMPro;
using UnityEngine;

public enum GameButtonState
{
    Enabled,
    Disabled
}

public class GameButtons : MonoBehaviour
{
    public delegate void OnMouseOverButtonDelegate(float farmerPrice, float modificationPrice, float fertilizerPrice);
    public static OnMouseOverButtonDelegate OnMouseOverButton;

    public delegate void OnMouseExitButtonDelegate();
    public static OnMouseExitButtonDelegate OnMouseExitButton;

    public delegate void OnUpgradeAppliedDelegate();
    public static OnUpgradeAppliedDelegate OnUpgradeApplied;

    [SerializeField]
    TextMeshProUGUI _moneyText;

    [SerializeField]
    float _income;
    float _earnedTotal;

    [SerializeField]
    float _incomeThreshold;

    [SerializeField]
    GameButtonState _state;

    [SerializeField]
    float timeSeconds;

    [SerializeField]
    float _farmerPrice;

    [SerializeField]
    float _modificationPrice;

    [SerializeField]
    float _fertilizerPrice;

    bool _canInteract;
    bool _hasFarmer;
    bool _useFarmer;
    bool _isShopOpen;

    bool _hasModification;
    bool _hasFertilizer;

    Transform _loadingIcon;

    GameObject _iconBox;
    GameObject _infoBox;
    GameObject _progressBox;
    GameObject _highlightBox;

    GameObject _widgetText;
    GameObject _infoText;

    GameObject _farmerImage;
    GameObject _modificationImage;
    GameObject _fertilizerImage;

    const float DISABLED_ALPHA = 0.6f;

    private void OnEnable()
    {
        ShopToggle.OnShopToggled += OnShopToggled;
    }

    private void OnDisable()
    {
        ShopToggle.OnShopToggled -= OnShopToggled;
    }

    private void Start()
    {
        _earnedTotal = 0;
        _canInteract = true;
        _hasFarmer = false;
        _useFarmer = true;
        _isShopOpen = false;

        _hasModification = false;
        _hasFertilizer = false;

        _loadingIcon = transform.Find("Loading Icon");

        _iconBox = transform.Find("Icon Box").gameObject;
        _infoBox = transform.Find("Info Box").gameObject;
        _progressBox = transform.Find("Progress Box").gameObject;
        _highlightBox = transform.Find("Highlight Box").gameObject;

        _widgetText = transform.Find("Widget Text").gameObject;
        _infoText = transform.Find("Info Text").gameObject;
        var infoTextRenderer = _infoText.GetComponent<TMP_Text>();
        if (infoTextRenderer != null)
        {
            infoTextRenderer.text = $"R{_incomeThreshold}";
        }

        _farmerImage = transform.Find("Farmer Image").gameObject;
        _modificationImage = transform.Find("Modification Image").gameObject;
        _fertilizerImage = transform.Find("Fertilizer Image").gameObject;

        // Some widgets are enabled/disabled so update that.
        ApplyState();
    }

    private void Update()
    {
        if (_state == GameButtonState.Disabled)
        {
            CheckIfStateUpdateNeeded();
            return;
        }

        if (UIController.Instance.uiMode != UIMode.Shop)
        {
            _highlightBox.SetActive(false);
        }

        if (_hasFarmer == true && _useFarmer == true)
        {
            StartCoroutine(UseFarmerTimerSeconds(_hasModification == false ? timeSeconds : timeSeconds * 0.5f));
            StartCoroutine(MoveLoadingIcon(_hasModification == false ? timeSeconds : timeSeconds * 0.5f));
        }
    }

    private void OnMouseOver()
    {
        if (_state == GameButtonState.Disabled)
        {
            return;
        }

        if (_isShopOpen == true)
        {
            return;
        }

        if (UIController.Instance.uiMode == UIMode.Shop)
        {
            OnMouseOverButton?.Invoke(_farmerPrice, _modificationPrice, _fertilizerPrice);

            switch (UIController.Instance.cursorMode)
            {
                case UICursorMode.Farmer:
                    if (_hasFarmer == false)
                    {
                        _highlightBox.SetActive(true);

                        if (Input.GetKeyDown(KeyCode.Mouse0) == true)
                        {
                            if (_farmerPrice <= UIController.Instance.totalMoney)
                            {
                                UIController.Instance.SubtractMoneyClamped(_farmerPrice);
                                UpdateMoney();

                                _hasFarmer = true;
                                _farmerImage.SetActive(true);

                                UIController.Instance.ResetUIAndCursorMode();
                                OnUpgradeApplied?.Invoke();
                            }
                        }
                    }
                    break;
                case UICursorMode.Modification:
                    if (_hasModification == false)
                    {
                        _highlightBox.SetActive(true);

                        if (Input.GetKeyDown(KeyCode.Mouse0) == true)
                        {
                            if (_modificationPrice <= UIController.Instance.totalMoney)
                            {
                                UIController.Instance.SubtractMoneyClamped(_modificationPrice);
                                UpdateMoney();

                                _hasModification = true;
                                _modificationImage.SetActive(true);

                                UIController.Instance.ResetUIAndCursorMode();
                                OnUpgradeApplied?.Invoke();
                            }
                        }
                    }
                    break;
                case UICursorMode.Fertilizer:
                    if (_hasFertilizer == false)
                    {
                        _highlightBox.SetActive(true);

                        if (Input.GetKeyDown(KeyCode.Mouse0) == true)
                        {
                            if (_fertilizerPrice <= UIController.Instance.totalMoney)
                            {
                                UIController.Instance.SubtractMoneyClamped(_fertilizerPrice);
                                UpdateMoney();

                                _hasFertilizer = true;
                                _fertilizerImage.SetActive(true);

                                UIController.Instance.ResetUIAndCursorMode();
                                OnUpgradeApplied?.Invoke();
                            }
                        }
                    }
                    break;
                default:
                    break;
            }

            return;
        }

        // Make sure they don't have a farmer, have clicked and can interact.
        if (_hasFarmer == false && _canInteract == true && Input.GetKeyDown(KeyCode.Mouse0) == true)
        {
            // Adds money.
            StartCoroutine(CanInteractTimerSeconds(_hasModification == false ? timeSeconds : timeSeconds * 0.5f));
            StartCoroutine(MoveLoadingIcon(_hasModification == false ? timeSeconds : timeSeconds * 0.5f));
        }
    }

    private void OnMouseExit()
    {
        if (UIController.Instance.uiMode == UIMode.Shop)
        {
            _highlightBox.SetActive(false);

            if (_isShopOpen == false)
            {
                OnMouseExitButton?.Invoke();
            }
        }
    }

    private void CheckIfStateUpdateNeeded()
    {
        if (UIController.Instance.totalMoney >= _incomeThreshold)
        {
            _state = GameButtonState.Enabled;
            ApplyState();
        }
    }

    private void OnShopToggled(bool isOpen)
    {
        _isShopOpen = isOpen;
    }

    IEnumerator TimerSeconds(float durationSeconds)
    {
        float elapsedSeconds = 0.0f;
        while (elapsedSeconds < durationSeconds)
        {
            elapsedSeconds += Time.deltaTime;
            yield return null;
        }
    }

    private void AddMoney()
    {
        float income = _hasFertilizer == false ? _income : _income * 2.0f;

        UIController.Instance.totalMoney = UIController.Instance.totalMoney + income;
        _moneyText.text = "R" + UIController.Instance.totalMoney;

        // Track the total.
        _earnedTotal += income;

        var infoTextRenderer = _infoText.GetComponent<TMP_Text>();
        if (infoTextRenderer != null)
        {
            infoTextRenderer.text = $"R{_earnedTotal}";
        }
    }

    private void UpdateMoney()
    {
        _moneyText.text = "R" + UIController.Instance.totalMoney;
    }

    private void ApplyState()
    {
        if (_iconBox == null || _progressBox == null || _infoBox == null || _widgetText == null)
        {
            Debug.Log("One of the widget components is null.");
            return;
        }

        var loadingIconRenderer = _loadingIcon.GetComponent<SpriteRenderer>();
        if (loadingIconRenderer != null)
        {
            var color = loadingIconRenderer.color;
            color.a = _state == GameButtonState.Enabled ? 1 : DISABLED_ALPHA;
            loadingIconRenderer.color = color;
        }

        var iconRenderer = _iconBox.GetComponent<SpriteRenderer>();
        if (iconRenderer != null)
        {
            var color = iconRenderer.color;
            color.a = _state == GameButtonState.Enabled ? 1 : DISABLED_ALPHA;
            iconRenderer.color = color;
        }

        var infoRenderer = _infoBox.GetComponent<SpriteRenderer>();
        if (infoRenderer != null)
        {
            var color = infoRenderer.color;
            color.a = _state == GameButtonState.Enabled ? 1 : DISABLED_ALPHA;
            infoRenderer.color = color;
        }

        var progressRenderer = _progressBox.GetComponent<SpriteRenderer>();
        if (progressRenderer != null)
        {
            var color = progressRenderer.color;
            color.a = _state == GameButtonState.Enabled ? 1 : DISABLED_ALPHA;
            progressRenderer.color = color;
        }

        var textRenderer = _widgetText.GetComponent<TMP_Text>();
        if (textRenderer != null)
        {
            var color = textRenderer.color;
            color.a = _state == GameButtonState.Enabled ? 1 : DISABLED_ALPHA;
            textRenderer.color = color;
        }

        var infoTextRenderer = _infoText.GetComponent<TMP_Text>();
        if (infoTextRenderer != null)
        {
            infoTextRenderer.text = $"R{(_state == GameButtonState.Enabled ? _earnedTotal : _incomeThreshold)}";
        }
    }

    IEnumerator CanInteractTimerSeconds(float durationSeconds)
    {
        _canInteract = false;

        yield return TimerSeconds(durationSeconds);

        AddMoney();

        _canInteract = true;
    }

    IEnumerator UseFarmerTimerSeconds(float durationSeconds)
    {
        _useFarmer = false;

        yield return TimerSeconds(durationSeconds);

        AddMoney();

        _useFarmer = true;
    }

    IEnumerator MoveLoadingIcon(float time)
    {
        Vector3 start = _loadingIcon.position;
        Vector3 end = start + (Vector3.right * 3.6f);

        float timeElapsed = 0;

        while (timeElapsed < time)
        {
            _loadingIcon.position = Vector3.Lerp(start, end, timeElapsed / time);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        _loadingIcon.position = start;
    }
}

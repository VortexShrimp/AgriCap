using TMPro;
using UnityEngine;

public class TitleTextController : MonoBehaviour
{
    TextMeshProUGUI _textRenderer;

    void Start()
    {
        _textRenderer = transform.GetComponent<TextMeshProUGUI>();
    }

    void OnEnable()
    {
        ShopToggle.OnShopToggled += OnShopToggled;

        ShopButtons.OnFarmerButtonClicked += OnFarmerButtonClicked;
        ShopButtons.OnModificationButtonClicked += OnModificationButtonClicked;
        ShopButtons.OnFertilizerButtonClicked += OnFertilizerButtonClicked;

        GameButtons.OnMouseOverButton += OnMouseOverButton;
        GameButtons.OnMouseExitButton += OnMouseExitButton;
        GameButtons.OnUpgradeApplied += OnUpgradeApplied;
    }

    void OnDisable()
    {
        ShopToggle.OnShopToggled -= OnShopToggled;

        ShopButtons.OnFarmerButtonClicked -= OnFarmerButtonClicked;
        ShopButtons.OnModificationButtonClicked -= OnModificationButtonClicked;
        ShopButtons.OnFertilizerButtonClicked -= OnFertilizerButtonClicked;

        GameButtons.OnMouseOverButton -= OnMouseOverButton;
        GameButtons.OnMouseExitButton -= OnMouseExitButton;
        GameButtons.OnUpgradeApplied -= OnUpgradeApplied;
    }

    void OnShopToggled(bool isOpen)
    {
        if (isOpen == true)
        {
            _textRenderer.text = "Welcome to the Shop!";
        }
        else
        {
            if (UIController.Instance.uiMode == UIMode.Shop)
            {
                switch (UIController.Instance.cursorMode)
                {
                    case UICursorMode.Farmer:
                        _textRenderer.text = $"Farmer upgrade.";
                        break;
                    case UICursorMode.Modification:
                        _textRenderer.text = $"Genetic Modification upgrade.";
                        break;
                    case UICursorMode.Fertilizer:
                        _textRenderer.text = $"Fertilizer upgrade.";
                        break;
                    default:
                        break;
                }
            }
            else
            {
                _textRenderer.text = "Purchase upgrades in the shop.";
            }
        }
    }

    private void OnFarmerButtonClicked()
    {

    }

    private void OnModificationButtonClicked()
    {

    }

    private void OnFertilizerButtonClicked()
    {

    }

    private void OnMouseOverButton(float farmerPrice, float modificationPrice, float fertilizerPrice)
    {
        switch (UIController.Instance.cursorMode)
        {
            case UICursorMode.Farmer:
                _textRenderer.text = $"Farmer price R{farmerPrice}.";
                break;
            case UICursorMode.Modification:
                _textRenderer.text = $"Genetic Modification price R{modificationPrice}.";
                break;
            case UICursorMode.Fertilizer:
                _textRenderer.text = $"Fertilizer price R{fertilizerPrice}.";
                break;
            default:
                break;
        }
    }

    private void OnMouseExitButton()
    {
        switch (UIController.Instance.cursorMode)
        {
            case UICursorMode.Farmer:
                _textRenderer.text = $"Farmer upgrade.";
                break;
            case UICursorMode.Modification:
                _textRenderer.text = $"Genetic Modification upgrade.";
                break;
            case UICursorMode.Fertilizer:
                _textRenderer.text = $"Fertilizer upgrade.";
                break;
            default:
                break;
        }
    }

    private void OnUpgradeApplied()
    {
        _textRenderer.text = "Purchase upgrades in the shop.";
    }
}

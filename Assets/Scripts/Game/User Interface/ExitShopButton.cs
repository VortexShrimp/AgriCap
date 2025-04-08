using UnityEngine;

public class ExitShopButton : MonoBehaviour
{
    private void Start()
    {
        transform.gameObject.SetActive(false);
    }

    public void ExitShop()
    {
        if (UIController.Instance.uiMode == UIMode.Shop)
        {
            UIController.Instance.uiMode = UIMode.Normal;
            UIController.Instance.cursorMode = UICursorMode.Normal;
            UIController.Instance.UpdateGameCursor();
        }
    }

    private void OnEnable()
    {
        ShopToggle.OnShopToggled += OnShopToggled;
    }

    private void OnDisable()
    {
        ShopToggle.OnShopToggled -= OnShopToggled;
    }

    private void OnShopToggled(bool isOpen)
    {
        Debug.Log("called");
        transform.gameObject.SetActive(isOpen);
    }
}

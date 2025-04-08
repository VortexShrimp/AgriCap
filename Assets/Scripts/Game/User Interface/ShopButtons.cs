using UnityEngine;

public class ShopButtons : MonoBehaviour
{
    public delegate void OnFarmerButtonClickedDelegate();
    public static OnFarmerButtonClickedDelegate OnFarmerButtonClicked;

    public delegate void OnModificationButtonClickedDelegate();
    public static OnModificationButtonClickedDelegate OnModificationButtonClicked;

    public delegate void OnFertilizerButtonClickedDelegate();
    public static OnFertilizerButtonClickedDelegate OnFertilizerButtonClicked;

    public delegate void OnAnyShopButtonClickedDelegate();
    public static OnAnyShopButtonClickedDelegate OnAnyShopButtonClicked;

    [SerializeField]
    GameObject _exitButton;


    public void OnFarmerButtonClick()
    {
        OnFarmerButtonClicked?.Invoke();
        OnAnyShopButtonClicked?.Invoke();
    }

    public void OnModificationButtonClick()
    {
        OnModificationButtonClicked?.Invoke();
        OnAnyShopButtonClicked?.Invoke();
    }

    public void OnFertilizerButtonClick()
    {
        OnFertilizerButtonClicked?.Invoke();
        OnAnyShopButtonClicked?.Invoke();
    }
}

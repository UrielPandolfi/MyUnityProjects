using System;
using UnityEngine;
using UnityEngine.Purchasing;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Purchasing.Extension;

public class ShopController : MonoBehaviour
{
    public static ShopController instance;
    private void Awake()
    {
        instance = this;
    }
    
    [SerializeField]
    private TextMeshProUGUI crystalsCounter;
    [SerializeField]
    private TextMeshProUGUI rewardedAdsCounter;
    [SerializeField]
    private TextMeshProUGUI rewardedAdsCooldownText;
    [SerializeField]
    private TextMeshProUGUI rewardedAdsText;
    [SerializeField]
    private TextMeshProUGUI SoldOutText;
    [SerializeField]
    private Button rewardedAdsButton;
    private float rewardedAdsCooldown;
    public GameObject noAdsPurchased;
    public string smallPackageID, mediumPackageID, bigPackageID, noAdsID;
    public int crystals;
    private int currentRewardedAds;
    
    void Start()
    {
        if(PlayerPrefs.HasKey("NoAds"))
        {
            RemoveAds();
        }
        if(PlayerPrefs.HasKey("Crystals"))
        {
            crystals = PlayerPrefs.GetInt("Crystals");
            crystalsCounter.text = crystals.ToString();
        }
        else
        {
            AddCrystals(100);
        }

        if(PlayerPrefs.HasKey("RewardedAds"))
        {
            currentRewardedAds = PlayerPrefs.GetInt("RewardedAds");
            rewardedAdsCounter.text = currentRewardedAds.ToString() + "/25";
            if(currentRewardedAds >= 25)
            {
                RewardedAdsSoldOut();
            }
        }
        else
        {
            currentRewardedAds = 0;
            rewardedAdsCounter.text = "0/25";
        }
    }

    void Update()
    {
        if(rewardedAdsCooldown > 0)
        {
            rewardedAdsCooldown -= Time.deltaTime;

            int minutos = Mathf.FloorToInt(rewardedAdsCooldown / 60f);  // Hacemos los minutos dividiendo por 60 y redondeandolo hacia abajo con floor to int, HACIA ABAJO SIEMPRE REDONDEAMOS
            int segundos = Mathf.FloorToInt(rewardedAdsCooldown % 60f); // Y sacamos  el resto que serian los segundos
            
            string tiempoFormateado = string.Format("{0:00}:{1:00}", minutos, segundos);
            rewardedAdsCooldownText.text = tiempoFormateado;
        }
        else
        {
            RewardedAdsCooldownOff();
        }
    }

    public void RewardedAdsCooldownOn()
    {
        // Cuando se apreta el boton para ver un ad, se comprueba si hay un ad para mostrar
        // y si lo hay se muestra y luego se llama a esta función
        
        // Cambiamos este texto por el del contador
        rewardedAdsText.enabled = false; 
        rewardedAdsCooldownText.enabled = true;

        rewardedAdsCooldown = 90;

        rewardedAdsButton.interactable = false;

        // Agregamos uno al contador del limite de ads y lo ponemos en el ui
        currentRewardedAds++;
        PlayerPrefs.SetInt("RewardedAds", currentRewardedAds);
        PlayerPrefs.Save();
        rewardedAdsCounter.text = (currentRewardedAds.ToString() + "/25");
    }

    public void RewardedAdsCooldownOff()
    {
        if(currentRewardedAds < 25)
        {
            rewardedAdsButton.interactable = true;
            rewardedAdsText.enabled = true; 
            rewardedAdsCooldownText.enabled = false;
        }
        else
        {
            RewardedAdsSoldOut();
        }
    }

    public void RewardedAdsSoldOut()
    {
        rewardedAdsButton.interactable = false;
        rewardedAdsText.enabled = false; 
        rewardedAdsCooldownText.enabled = false;
        SoldOutText.enabled = true;
    }

    public void OnPurchaseComplete(Product product)    // Esta función se llama automaticamente cuando se conpleta una compra
    {

        print("Purchase complete " + product.definition.id);

        if(product.definition.id == smallPackageID)
        {
            AddCrystals(120);
        }
        if(product.definition.id == mediumPackageID)
        {
            AddCrystals(320);
        }
        if(product.definition.id == bigPackageID)
        {
            AddCrystals(850);
        }
        if(product.definition.id == noAdsID)
        {
            RemoveAds();
        }
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureDescription failureReason)
    {
        Debug.Log("Purchase failed in product " + product.definition.id + "because" + failureReason);
    }

    public void AddCrystals(int amount)
    {
        crystals += amount;
        PlayerPrefs.SetInt("Crystals", crystals);
        PlayerPrefs.Save();
        // Updatear el UI
        crystalsCounter.text = crystals.ToString();
    }

    public void RemoveCrystals(int amount)
    {
        crystals -= amount;
        PlayerPrefs.SetInt("Crystals", crystals);
        PlayerPrefs.Save();
        // Updatear el UI
        crystalsCounter.text = crystals.ToString();
    }

    private void RemoveAds()
    {
        PlayerPrefs.SetInt("NoAds", 1); // Establece el valor a 1 para indicar que los anuncios deben eliminarse
        PlayerPrefs.Save();             // Asegúrate de guardar los cambios en PlayerPrefs

        AdsController.instance.DestroyBannerAd();
        noAdsPurchased.SetActive(true);
    }

    private void ShowAds()
    {
        PlayerPrefs.SetInt("NoAds", 0); // Establece el valor a 1 para indicar que los anuncios deben eliminarse
        PlayerPrefs.Save();             // Asegúrate de guardar los cambios en PlayerPrefs
        noAdsPurchased.SetActive(false);
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        print("initialize failed " + error);
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        print("initialize failed " + error + message);
    }





    
}

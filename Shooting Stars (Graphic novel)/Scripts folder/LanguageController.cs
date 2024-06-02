using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization;

public class LanguageController : MonoBehaviour
{
    public Button englishButton, portugueseButton, spanishButton, acceptButton;

    private void Start()
    {
        acceptButton.interactable = false;
    }

    public void SetLanguage(string language)
    {
        Color highlightedColor = new Color(0f, 1f, 0.937f, 1f);

        PlayerPrefs.SetString("selectedLanguage", language);
        
        acceptButton.interactable = true;

        ResetButtons();

        switch(language)
        {
            case "en":
                englishButton.image.color = highlightedColor;
                break;

            case "es":
                spanishButton.image.color = highlightedColor;
                break;

            case "pt":
                portugueseButton.image.color = highlightedColor;
                break;
        }

        StartCoroutine(SetLocale(language));
    }

    public static IEnumerator SetLocale(string language)
    {
        // Espera hasta que las configuraciones de localización estén listas
        yield return LocalizationSettings.InitializationOperation;

        Locale selectedLocale = null;
        foreach (var locale in LocalizationSettings.AvailableLocales.Locales)  // ponemos una variable tipo var para que sea universal y comprobamos a que idioma es igual la string
        {
            if (locale.Identifier.Code == language)
            {
                selectedLocale = locale;
                break;
            }
        }

        LocalizationSettings.SelectedLocale = selectedLocale;
    }

    private void ResetButtons()
    {
        englishButton.image.color = Color.white;
        spanishButton.image.color = Color.white;
        portugueseButton.image.color = Color.white;
    }

    public void Accept()
    {
        Destroy(gameObject);
    }
}

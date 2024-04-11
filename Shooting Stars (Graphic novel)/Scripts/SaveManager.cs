using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text.RegularExpressions;

public class SaveManager : MonoBehaviour
{
    public static string SAVED_GAME = "savedGame";

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            ClearData();
        }
    }

    public static void SaveGame(DataHolder data)    // Esta función la utilizamos siempre que el juego se guarde
    {
        // Pasamos el scriptable object a json para guardarlo en los playerPrefs
        PlayerPrefs.SetString(SAVED_GAME, JsonUtility.ToJson(data));
    }

    public static DataHolder LoadGame()
    {
        // Pasamos el scriptable guardado desde json a scriptable object de nuevo
        return JsonUtility.FromJson<DataHolder>(PlayerPrefs.GetString(SAVED_GAME)); // Tomamos el string guardado en SAVED_GAME y lo pasamos a scriptable
    }

    public static bool IsGameSaved()
    {
        return PlayerPrefs.HasKey(SAVED_GAME);
    }

    public static void SaveTextReview(string reviewText, string language)
    {
        string filePath = Path.Combine(Application.persistentDataPath, "reviewText_" + language + ".txt");
        // Usando File.AppendAllText
        File.AppendAllText(filePath, reviewText);
    }

    public static string LoadTextReview(string language)
    {
        string filePath = Path.Combine(Application.persistentDataPath, "reviewText_" + language + ".txt");

        if(File.Exists(filePath))   // Comprobamos si existe el archivo
        {
            return File.ReadAllText(filePath);  // De ser asi lo devolvemos
        }
        
        return "";  // De no serlo devolvemos "";
    }

    public static void ReplaceTextName(string oldName, string newName)
    {
        ReplaceNameInFile("en", oldName, newName);
        ReplaceNameInFile("es", oldName, newName);
        ReplaceNameInFile("pt", oldName, newName);
    }

    private static void ReplaceNameInFile(string language, string oldName, string newName)
    {
        string filePath = Path.Combine(Application.persistentDataPath, "reviewText_" + language + ".txt");

        if (File.Exists(filePath))  // Si el archivo existe hacemos el reemplazo
        {
            string content = File.ReadAllText(filePath);
            content = Regex.Replace(content, oldName, newName, RegexOptions.IgnoreCase);
            File.WriteAllText(filePath, content);
        }
    }
    
    public static void ClearData()
    {
        // Borramos la data
        PlayerPrefs.DeleteKey(SAVED_GAME);
        
        // Borramos el review text
        DeleteFile("en");
        DeleteFile("es");
        DeleteFile("pt");

        OptionsMenuController.instance.reviewText.text = "";
    }

    private static void DeleteFile(string language)
    {
        string filePath = Path.Combine(Application.persistentDataPath, "reviewText_" + language + ".txt");
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
    }
}

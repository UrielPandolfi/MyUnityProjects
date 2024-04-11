using System.Collections;
using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class TextBarController : MonoBehaviour
{
    [Header("Textos del panel")]
    public TextMeshProUGUI barText;
    public TextMeshProUGUI personNameText;
    [Header("Escena Actual")]
    public StoryScene currentScene;
    private int sentenceIndex = -1;
    public State currentState = State.COMPLETED;
    private bool textBarIsON = true;
    [HideInInspector]
    public bool automodeON;
    public bool recentLoadedGame = false;
    public float automodeTime = 5f;
    public float automodeCounter;
    private int adCounter;
    [Header("Referencias")]
    private Animator animator;
    public CamController camController;
    public GameObject spritesPrefab;
    public RectTransform namePanelRect;
    private Dictionary<Speaker, SpriteController> spritesControllers = new Dictionary<Speaker, SpriteController>();

    public enum State
    {
        PLAYING, COMPLETED
    }
    
    private void Awake()
    {
        animator = GetComponent<Animator>();
        automodeON = false;
        automodeCounter = automodeTime;
        adCounter = 0;
    }

    private void Update()
    {
        if(automodeON && currentState == State.COMPLETED) // El contador solo correra si esta en automodeON y en completed
        {
            if(automodeCounter <= 0f)
            {
                if(!IsLastSentence())
                {
                    GameManager.instance.TouchScreen();
                    automodeCounter = automodeTime;
                }
                else
                {
                    GameManager.instance.TouchScreen();
                    automodeCounter = automodeTime;
                }
            }
            else
            {
                automodeCounter -= Time.deltaTime;
            }
        }
    }

    public void PlayScene(StoryScene scene) // Esta función la utilizamos siempre que inicie una nueva StoryScene
    {
        currentScene = scene;   // Asignamos la nueva StoryScene como la actual
        PlayNextSentence();     // Llamamos a la función para que empiece a leer las sentencias con su dialo y el nombre del hablante
    }

    public void PlayNextSentence()  // Esta función la utilizamos para pasar a la siguiente sentencia
    {
        currentState = State.PLAYING;   // Ponemos el estado en PLAYING
        automodeCounter = automodeTime;
        GameManager.instance.SaveData();

        if(!currentScene.sentences[++sentenceIndex].desaparecerBarraDeTexto)
        {
            StartCoroutine(TypeText(currentScene.sentences[sentenceIndex])); // Llamamos a la coroutine para que haga el efecto de aparicion del texto
        }

        if(currentScene.sentences[sentenceIndex].speaker != null)
        {
            personNameText.text = currentScene.sentences[sentenceIndex].speaker.speakerName; // Cambiamos el nombre al del actual hablante
        }
        AdjustNameWidht();

        ActSpeakers(currentScene.sentences[sentenceIndex]);

        if(currentScene.sentences[sentenceIndex].desaparecerBarraDeTexto == false)
        {
            InstantShow();
        }
        else
        {
            InstantHide();
        }

        AdCounter();

    }

    IEnumerator TypeText(StoryScene.Sentence sentence)   // Función encargada de hacer el efecto de aparición del texto
    {
        string playerName = PlayerPrefs.GetString("player_Name");
        string selectedLanguage = PlayerPrefs.GetString("selectedLanguage", "en"); // Se pone default en ingles si no hay ninguno
        string dialogoTexto = "";

        switch(selectedLanguage)
        {
            case "en":
                dialogoTexto = Regex.Replace(sentence.dialogoIngles, "sofia", playerName, RegexOptions.IgnoreCase);
                break;
            case "es":
                dialogoTexto = Regex.Replace(sentence.dialogoEspañol, "sofia", playerName, RegexOptions.IgnoreCase);
                break;
            case "pt":
                dialogoTexto = Regex.Replace(sentence.dialogoPortugues, "sofia", playerName, RegexOptions.IgnoreCase);
                break;
        }
        dialogoTexto = Regex.Replace(dialogoTexto, "sofia", playerName, RegexOptions.IgnoreCase);

        if(!recentLoadedGame)           // Si el juego no se cargo recien agregamos el texto, para prevenir que se duplique un dialogo
        {
            WriteTextReview(dialogoTexto);
            SaveTextReviews(sentence);
        }

        barText.text = "";              // Comenzamos con el texto vacio
        int letterIndex = 0;            // Creamos una variable para ir agregando letra por letra

        while(currentState == State.PLAYING)        // Este while se encargara de ir agregando de a una letra hasta que se termine la palabra
        {
            barText.text += dialogoTexto[letterIndex];
            yield return new WaitForSeconds(0.01f);

            if(++letterIndex == dialogoTexto.Length)        // Aqui se le suma uno al indice y se chequea si es del mismo tamaño que la palabra para saber si terminó
            {
                currentState = State.COMPLETED;     // De ser asi ponemos el state en COMPLETED
            }
        }

        recentLoadedGame = false; // Después de la primera ejecución, establecer a false
    }


    private void WriteTextReview(string dialogo)
    {
      
        string text = dialogo;

        string name = "";

        if(currentScene.sentences[sentenceIndex].speaker.name != null)
            {
                name = currentScene.sentences[sentenceIndex].speaker.speakerName;
            }

        OptionsMenuController.instance.reviewText.text += name == "" ? text + "\n" : name + ": " + text + "\n";   // si hay nombre lo ponemos y si no, no
        Debug.Log("Se agrego texto");
    }

    private void SaveTextReviews(StoryScene.Sentence sentence)
    {
        string text = sentence.dialogoIngles;
        string language = "EN";
        SaveManager.SaveTextReview(sentence.speaker.speakerName == "" ? text + "\n" : sentence.speaker.speakerName + ": " + text + "\n", language);

        text = sentence.dialogoEspañol;
        language = "ES";
        SaveManager.SaveTextReview(sentence.speaker.speakerName == "" ? text + "\n" : sentence.speaker.speakerName + ": " + text + "\n", language);

        text = sentence.dialogoPortugues;
        language = "PT";
        SaveManager.SaveTextReview(sentence.speaker.speakerName == "" ? text + "\n" : sentence.speaker.speakerName + ": " + text + "\n", language);
    }

    public void SkipText()
    {
        currentState = State.COMPLETED;
        StoryScene.Sentence sentence = currentScene.sentences[sentenceIndex];
        string selectedLanguage = PlayerPrefs.GetString("selectedLanguage", "en"); // Se pone default en ingles si no hay ninguno
        switch(selectedLanguage)
        {
            case "en":
                barText.text = currentScene.sentences[sentenceIndex].dialogoIngles;
                break;
            case "es":
                barText.text = currentScene.sentences[sentenceIndex].dialogoEspañol;
                break;
            case "pt":
                barText.text = currentScene.sentences[sentenceIndex].dialogoPortugues;
                break;
        }
        camController.SetCamPosition();
    }

    private void ActSpeakers(StoryScene.Sentence sentence)
    {
        List<StoryScene.Sentence.accion> actions = sentence.acciones;
        for(int i=0; i<actions.Count; i++)
        {
            ActSpeaker(actions[i]);
        }
    }

    private void ActSpeaker(StoryScene.Sentence.accion action)
    {
        SpriteController spriteController = null;
        if(action.speakerDeLaAccion != null)
        {
            if(!spritesControllers.ContainsKey(action.speakerDeLaAccion)) // Si el personaje este nunca hablo o aparecio
            {                                                           // instanciamos su prefab, tomaoms su componente sprite controller y lo añadimos la diccionario
                spriteController = Instantiate(action.speakerDeLaAccion.prefabCharacterController, spritesPrefab.transform).GetComponent<SpriteController>();
                spritesControllers.Add(action.speakerDeLaAccion, spriteController);
            }
            else    // De no ser asi simplemente tomoamos el sprite controller del diccionario
            {
                spriteController = spritesControllers[action.speakerDeLaAccion];
            }
            Sprite currentSprite = action.speakerDeLaAccion.sprites[action.spriteIndex];
            switch(action.tipoDeAccion)
            {
                case StoryScene.Sentence.accion.Tipo.Aparecer:
                
                    //  Seteamos el character
                    spriteController.Setup(currentSprite);
                    if(action.coordenadas != Vector2.zero)
                    {
                        spriteController.SetCharacterPosition(action.coordenadas);
                    }
                    spriteController.Show();

                    //  Guardamos los datos del character
                    action.speakerDeLaAccion.currentSprite = currentSprite; 
                    action.speakerDeLaAccion.currentPosition = action.coordenadas;
                    GameManager.instance.SaveCharacter(action.speakerDeLaAccion);
                    return;

                case StoryScene.Sentence.accion.Tipo.Desaparecer:
                    spriteController.Hide();

                    //  Lo sacamos del dataholder
                    GameManager.instance.RemoveCharacter(action.speakerDeLaAccion);
                    break;
            }
        }
        
        if(action.tipoDeAccion == StoryScene.Sentence.accion.Tipo.MoverCamara)
        {
            Debug.Log("Se movio la camara");
            if(action.coordenadas != Vector2.zero)
            {
                GameManager.instance.SaveCamPosition(action.coordenadas);
                camController.MoveCam(action.coordenadas);
                
            }
        }
        
        // Siempre cambiamos el sprite sea cual sea la accion hasta en NONE, y aparte si no es movecam movemos al character hasta la posición.
        if(spriteController != null && action.tipoDeAccion != StoryScene.Sentence.accion.Tipo.MoverCamara && action.tipoDeAccion != StoryScene.Sentence.accion.Tipo.Nada)
        {
            spriteController.SwitchSprite(action.speakerDeLaAccion.sprites[action.spriteIndex]);
            action.speakerDeLaAccion.currentSprite = action.speakerDeLaAccion.sprites[action.spriteIndex]; 

            if(action.coordenadas != Vector2.zero)
            {
                spriteController.SetCharacterPosition(action.coordenadas);
                action.speakerDeLaAccion.currentPosition = action.coordenadas;
            }
            
            GameManager.instance.SaveCharacter(action.speakerDeLaAccion);
        }

    }

    public void SetSpeakers(List<Speaker> speakers)
    {
        for(int i = 0; i < speakers.Count; i++)
        {
            SetSpeaker(speakers[i]);
        }
    }

    private void SetSpeaker(Speaker character)
    {
        SpriteController spriteController = null;
        int index = GetSentenceIndex();
        if(!spritesControllers.ContainsKey(character)) // Si el personaje este nunca hablo o aparecio
        {   // instanciamos su prefab, tomamos su componente sprite controller y lo añadimos la diccionario
            spriteController = Instantiate(character.prefabCharacterController, spritesPrefab.transform).GetComponent<SpriteController>();
            spritesControllers.Add(character, spriteController);
        }
        else    // De no ser asi simplemente tomoamos el sprite controller del diccionario
        {
            spriteController = spritesControllers[character];
        }

        spriteController.Setup(character.currentSprite);
        spriteController.SetCharacterPosition(character.currentPosition);
        spriteController.Show();
    }

    void AdjustNameWidht()
    {
        float preferredWidth = personNameText.preferredWidth;
        Vector2 size = namePanelRect.sizeDelta;
        if(preferredWidth == 0f)
        {
            namePanelRect.sizeDelta = new Vector2(0f, size.y); 
        }
        else
        {
            namePanelRect.sizeDelta = new Vector2(preferredWidth + 30f, size.y); 
        }
        // Tomamos el tamaño del texto y se lo pasamos al panel rect sin mas...
        
    }

    void AdCounter()
    {
        adCounter++;
        if(adCounter >= 25)
        {
            AdsController.instance.ShowInterstitialAd();
            adCounter = 0;
        }
    }

        public void Show()
    {
        if(textBarIsON == false)
        {
            AdjustNameWidht();
            animator.SetTrigger("Show");
            textBarIsON = true;
        }
    }

    public void InstantShow()
    {
        if(textBarIsON == false)
        {
            animator.SetTrigger("InstantShow");
            textBarIsON = true;
        }
    }

    public void Hide()
    {
        if(textBarIsON == true)
        {
            animator.SetTrigger("Hide");
            textBarIsON = false;
        }
    }

    public void InstantHide()
    {
        if(textBarIsON == true)
        {
            animator.SetTrigger("InstantHide");
            textBarIsON = false;
        }
    }


    public void ClearText(StoryScene scene)
    {
        barText.text = "";
        personNameText.text = scene.sentences[0].speaker.speakerName;
    }

    public bool IsCompleted()
    {
        return currentState == State.COMPLETED;
    }

    public bool IsLastSentence()
    {
        return sentenceIndex + 1 == currentScene.sentences.Count;
    }

    public int GetSentenceIndex()
    {
        return sentenceIndex;
    }

    public void SetSentenceIndex(int index)
    {
        sentenceIndex = index;
    }
}

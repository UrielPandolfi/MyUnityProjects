using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public TextBarController textBarController;
    public GameScene currentScene;
    public SpriteSwitcher backgroundController;
    public ChooseController chooseController;
    private DataHolder dataHolder;
    public ControladorTactil playerTouch;
    private EventSystem eventSystem;
    private State state = State.IDLE;

    public enum State
    {
        IDLE, ANIMATE, CHOOSE
    }
 
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        if(playerTouch == null)
        {
            playerTouch = new ControladorTactil();
        }
        playerTouch.TouchInput.Touch.started += context => OnTouchStart(context);
        playerTouch.TouchInput.Touch.started += context => ParticlesTouch(context);
        eventSystem = EventSystem.current;
    }

    private void Start()
    {
        if(!PlayerPrefs.HasKey("currentSeason"))    // Si nunca se jugo al juego, entonces ponemos el player prefs
        {
            PlayerPrefs.SetString("currentSeason", SceneManager.GetActiveScene().name);
            PlayerPrefs.Save(); // Siempre poner playerprefs.save cuando son cosas importantes o poco frecuentes
        }

        if(SaveManager.IsGameSaved())   // Si hay una partida guardada la cargamos y ya.
        {
            LoadData();
        }
        else
        {
            // Inicializa para un nuevo juego
            dataHolder = new DataHolder
            {
                scene = null, // O asigna un valor inicial si es necesario
                sentenceIndex = 0, // O el valor inicial que desees
                music = null, // O asigna una pista inicial si es necesario
                charactersOnScene = new List<Speaker>(), // Inicializa la lista
                camPosition = new Vector2(0f, 0f)
            };
        }
        
        if(currentScene is StoryScene)  // Hacemos esto para iniciar con el fondo de la primera escena siempre
        {
            StoryScene scene = currentScene as StoryScene;
            textBarController.PlayScene(scene);
            backgroundController.SetImage(scene.Background);
            PlaySound(scene.sentences[textBarController.GetSentenceIndex()]);
        }
    }

    private void LoadData()
    {
        dataHolder = SaveManager.LoadGame();    // Llamamos a la función para cargar la data
        textBarController.recentLoadedGame = true;
        currentScene = dataHolder.scene;        // Cargamos la escena que se dejó el juego
        textBarController.SetSentenceIndex(dataHolder.sentenceIndex);   // Nos ubicamos en la sentencia que se dejó
        AudioController.instance.PlaySound(dataHolder.music, null);  // Reproducimos la ultima canción que se ejecutó
        textBarController.SetSpeakers(dataHolder.charactersOnScene);
        CamController.instance.MoveCam(dataHolder.camPosition);
    }

    private void OnEnable()
    {
        playerTouch.Enable();
    }

    private void OnDisable()
    {
        playerTouch.Disable();
    }

    public void OnTouchStart(InputAction.CallbackContext context)
    {
        PointerEventData pointerData = new PointerEventData(eventSystem)
        {
            position = Touchscreen.current.primaryTouch.position.ReadValue()
        };

        // Lista para almacenar los resultados del raycast
        List<RaycastResult> results = new List<RaycastResult>();

        // Hacer el raycast
        eventSystem.RaycastAll(pointerData, results);

        Debug.Log(results.Count);
        // Comprueba si el toque está sobre un objeto de UI
        if (results.Count == 0)
        {
            // Si no está sobre un objeto de UI, llama a TouchScreen
            TouchScreen();
        }
    }

    void ParticlesTouch(InputAction.CallbackContext context)
    {
        PointerEventData pointerData = new PointerEventData(eventSystem)
        {
            position = Touchscreen.current.primaryTouch.position.ReadValue()
        };

        ParticlesController.instance.GenerateParticles(pointerData.position);
    }

    public void TouchScreen()
    {
        if(state == State.IDLE && textBarController.IsCompleted())
            {
                if(textBarController.IsLastSentence())
                {
                    PlayScene((currentScene as StoryScene).nextStoryScene); // Le ponemos as storyScene ya que si es la utlima sentencia si o si es una storyScene, y pasamos el nextStoryScene, que puede ser una chooseScene
                }
                else
                {
                    textBarController.PlayNextSentence();
                    PlaySound((currentScene as StoryScene).sentences[textBarController.GetSentenceIndex()]); // Recordar siempre pasarlo como alguna clase que herede gamescene, en este caso storyscene
                }
            }
            else if(!textBarController.IsCompleted())
            {
                textBarController.SkipText();
            }

        
    }

    public void PlayScene(GameScene scene)
    {
        StartCoroutine(SwitchScene(scene));
    }

    IEnumerator SwitchScene(GameScene scene)
    {
        state = State.ANIMATE;
        if(currentScene is StoryScene)
        {
            textBarController.Hide();   // Guardamos la barra de dialogo sin importar a que cambiemos
        }
        currentScene = scene;
        yield return new WaitForSeconds(1f);
        if(scene is StoryScene)
        {
            textBarController.SetSentenceIndex(-1);
            StoryScene storyScene = scene as StoryScene;

            Sprite currentBackground = backgroundController.GetImage();
            if(currentBackground != storyScene.Background)
            {
                backgroundController.SwitchImage(storyScene.Background);
                yield return new WaitForSeconds(1f);
            }

            PlaySound(storyScene.sentences[0]); // Ponemos 0 ya que siempre arranque una storyscene arranca desde la sentence 0
            textBarController.ClearText(storyScene);
            textBarController.Show();

            yield return new WaitForSeconds(1f);
            
            textBarController.PlayScene(storyScene);
            state = State.IDLE;
        }
        else if(scene is ChooseScene)   // Si es una chooseScene ponemos las opciones, ya luego cambiara de escena a una storyScene dependiendo que boton toque
        {
            state = State.CHOOSE;
            chooseController.SetupChoose(scene as ChooseScene);
        }
    }

    public void SaveData()
    {
        int currentIndex = textBarController.GetSentenceIndex();    // Tomamos el index actual
        dataHolder.scene = currentScene;                            // Guardamos la escena actual
        dataHolder.sentenceIndex = currentIndex;                    // Guardamos el index actual
        if((currentScene as StoryScene).sentences[++currentIndex].musica != null)  // Si hay musica en esta sentencia se guarda
        {
            dataHolder.music = (currentScene as StoryScene).sentences[currentIndex].musica;
        }
        
        SaveManager.SaveGame(dataHolder);   // Y llamamos a la función para guardar el daraHolder
    }

    public void SaveCharacter(Speaker character)
    {
        dataHolder.charactersOnScene.Add(character);
        Debug.Log("Se guardó el character " + character.name);
    }

    public void RemoveCharacter(Speaker character)
    {
         if (dataHolder.charactersOnScene.Contains(character))
        {
            dataHolder.charactersOnScene.Remove(character);
            Debug.Log("Se removió el character " + character.name);
        }
        else
        {
            Debug.Log("El character a remover no se encuentra en la escena: " + character.name);
        }
    }

    public void SaveCamPosition(Vector2 coords)
    {
        dataHolder.camPosition = coords;
    }

    private void PlaySound(StoryScene.Sentence sentence)
    {
        AudioController.instance.PlaySound(sentence.musica, sentence.sonido);
    }
}

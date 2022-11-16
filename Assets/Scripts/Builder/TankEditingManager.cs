using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TankEditingManager : MonoBehaviour
{
    string currentScene;

    [SerializeField] TankSaveManager save;

    [SerializeField]
    List<string> sceneList = new List<string>();

    static TankEditingManager _instance;

    static TankEditingManager Instance { get { return _instance; } }

    void Update()
    {
        if(Input.GetKeyUp(KeyCode.Return))
        {
            if(ConveyorSystemManager.Instance() != null)
            {
                foreach(ConveyorSystemManager m in FindObjectsOfType<ConveyorSystemManager>())
                {
                    m.StopAllCoroutines();
                    Destroy(m.gameObject);
                }
            }
            SwitchScene();
        }
    }

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

        DontDestroyOnLoad(gameObject);
        

        

    }
    void Start()
    {
        currentScene = SceneManager.GetActiveScene().name;
        if (SceneManager.GetActiveScene().name == "BuildTest")
        {
            Debug.Log("Loading in to tank editor, loading in tank");
            save.LoadEditModeTank();
            save.TankToBuiltMode();
        }
    }

    void SwitchScene()
    {
        if(SceneManager.GetActiveScene().name == "BuildTest")
        {
            save.SaveTransform();
        }
        if(sceneList.IndexOf(currentScene) < sceneList.Count - 1)
        {
            SceneManager.LoadScene(sceneList[sceneList.IndexOf(currentScene) + 1], LoadSceneMode.Single);
        }
        else
        {
            SceneManager.LoadScene(sceneList[0], LoadSceneMode.Single);
        }
        
    }

    void OnEnable()
    {
        //Tell our 'OnLevelFinishedLoading' function to start listening for a scene change as soon as this script is enabled.
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    void OnDisable()
    {
        //Tell our 'OnLevelFinishedLoading' function to stop listening for a scene change as soon as this script is disabled. Remember to always have an unsubscription for every delegate you subscribe to!
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        
        if (currentScene == "BuildTest" && SceneManager.GetActiveScene().name != "BuildTest")
        {
            if(ConveyorSystemManager.Instance() == null)
            {
                new GameObject().AddComponent<ConveyorSystemManager>();
            }
            Debug.Log("Switching in from tank editor, loading in tank");
            save.LoadGameModeTank();
            save.TankToGameMode();
        }
        else if ((currentScene == "TankTest" && SceneManager.GetActiveScene().name != "TankTest"))
        {
            Debug.Log("Switching in to tank editor, loading in tank");
            save.LoadEditModeTank();
            save.TankToBuiltMode();
        }
        currentScene = SceneManager.GetActiveScene().name;
    }
}

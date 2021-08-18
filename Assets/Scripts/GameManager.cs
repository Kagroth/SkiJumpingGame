using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
public class GameManager : MonoBehaviour
{
    [System.Serializable]
    public class SceneUI {
        public string name;
        public GameObject uiPrefab;
    }

    private GameObject currentUI;
    private UIManager currentUIManager;

    public SceneUI[] sceneUIPrefabs; 

    public HillData[] allHills;

    public static GameObject hillPrefab;
    public GameObject skiJumperPrefab;
    private GameObject hill;
        
    private List<SkiJumper> allJumpers;

    private WorldCupData worldCupData;

    private void Awake() {
        DontDestroyOnLoad(this.gameObject);    
    }

    void Start()
    {
        allJumpers = SkiJumperDatabase.LoadSkiJumpers();
        SceneManager.sceneLoaded += InitScene;
        InitScene();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitScene() {
        string currentScene = SceneManager.GetActiveScene().name;

        if (currentScene.Equals("MainMenu")) {
            Debug.Log("Main Menu scene loading");
        }
        else if (currentScene.Equals("RandomCompetition")) {
            Debug.Log("Random Competition scene loading");

            int hillIndex = Random.Range(0, allHills.Length);
            HillData hillData = allHills[hillIndex];
            hillPrefab = Resources.Load<GameObject>("Hills/" + hillData.name);
            hill = Instantiate(hillPrefab);

            GameObject player = Instantiate(skiJumperPrefab);
            PlayerController pc = player.GetComponent<PlayerController>();

            worldCupData = new WorldCupData(allJumpers);
            worldCupData.SetParticipants(allJumpers);
            worldCupData.CreateRandomCompetition(hillData);
        }
        else if (currentScene.Equals("Competition")) {
            string hillToLoad = worldCupData.GetCurrentCompetition().GetHillName();
            hillPrefab = Resources.Load<GameObject>("Hills/" + hillToLoad);
            
            hill = Instantiate(hillPrefab);
            GameObject player = Instantiate(skiJumperPrefab);
            PlayerController pc = player.GetComponent<PlayerController>();
        }

        LoadSceneUI();
    }

    // method for sceneLoaded event
    public void InitScene(Scene scene, LoadSceneMode mode) {
        InitScene();
    }
    
    public void LoadSceneUI() {
        string currentSceneName = SceneManager.GetActiveScene().name;
        
        SceneUI UIToLoad = sceneUIPrefabs.Where(sceneUI => sceneUI.name.Equals(currentSceneName)).First();

        currentUI = Instantiate(UIToLoad.uiPrefab, Vector3.zero, Quaternion.identity);
        currentUIManager = currentUI.GetComponent<UIManager>();
        currentUIManager.hill = hill;
        currentUIManager.Init(this);
    }

    public WorldCupData GetWorldCupData() {
        return worldCupData;
    }
}

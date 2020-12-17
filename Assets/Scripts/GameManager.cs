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
        
    void Start()
    {
        InitScene();
        LoadSceneUI();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitScene() {
        string currentScene = SceneManager.GetActiveScene().name;

        if (currentScene.Equals("RandomCompetition")) {
            int hillIndex = Random.Range(0, allHills.Length);
            Debug.Log("Hill index to load" + hillIndex);
            HillData hillData = allHills[hillIndex];
            hillPrefab = Resources.Load<GameObject>("Hills/" + hillData.name);
            hill = Instantiate(hillPrefab);
            GameObject player = Instantiate(skiJumperPrefab);
            PlayerController pc = player.GetComponent<PlayerController>();
            WorldCupData.PushCompetition(hillData);
        }
        else if (currentScene.Equals("Competition")) {
            string hillToLoad = WorldCupData.GetCurrentCompetition().GetHillName();
            hillPrefab = Resources.Load<GameObject>("Hills/" + hillToLoad);
            
            hill = Instantiate(hillPrefab);
            GameObject player = Instantiate(skiJumperPrefab);
            PlayerController pc = player.GetComponent<PlayerController>();
        }
    }
    
    public void LoadSceneUI() {
        string currentSceneName = SceneManager.GetActiveScene().name;
        
        SceneUI UIToLoad = sceneUIPrefabs.Where(sceneUI => sceneUI.name.Equals(currentSceneName)).First();

        currentUI = Instantiate(UIToLoad.uiPrefab, Vector3.zero, Quaternion.identity);
        currentUIManager = currentUI.GetComponent<UIManager>();
        currentUIManager.hill = hill;
        currentUIManager.Init(this);
    }
}

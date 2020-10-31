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
        if (SceneManager.GetActiveScene().name.Equals("Competition")) {
            if (!hillPrefab) {
                hillPrefab = Resources.Load<GameObject>("Hills/Fly-HS215");                
            }
            
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
        currentUIManager.Init();
    }
}

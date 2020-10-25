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
        
    void Start()
    {
        LoadSceneUI();

        if (!hillPrefab) {
            hillPrefab = Resources.Load<GameObject>("Hills/Fly-HS215");
        }

        GameObject hill = Instantiate(hillPrefab);
        GameObject player = Instantiate(skiJumperPrefab);
        PlayerController pc = player.GetComponent<PlayerController>();
        currentUIManager.InitWindMeter();
        currentUIManager.hillInfo.text = hillPrefab.name.Replace("-", " ");
        pc.lastScoreText = currentUIManager.lastScore;
        pc.bestScoreText = currentUIManager.bestScore;
        pc.landingText = currentUIManager.landingType;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H)) {
            currentUIManager.ToggleHelpPanel();
        }
        else if (Input.GetKeyDown(KeyCode.Escape)) {
            SceneManager.LoadScene("MainMenu");
        }
    }

    public void DisplayJumpResult() {
        // currentUIManager.ToggleJumpResultPanel();
    }

    public void LoadSceneUI() {
        string currentSceneName = SceneManager.GetActiveScene().name;
        
        SceneUI UIToLoad = sceneUIPrefabs.Where(sceneUI => sceneUI.name.Equals(currentSceneName)).First();

        currentUI = Instantiate(UIToLoad.uiPrefab, Vector3.zero, Quaternion.identity);
        currentUIManager = currentUI.GetComponent<UIManager>();
        currentUIManager.Init();
    }
}

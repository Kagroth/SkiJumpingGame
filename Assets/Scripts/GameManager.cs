using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public static GameObject hillPrefab;
    public GameObject skiJumperPrefab;
    
    private UIManager uIManager;
    
    void Start()
    {
        hillPrefab = Resources.Load<GameObject>("Hills/Fly-HS215");
        GameObject hill = Instantiate(hillPrefab);
        GameObject player = Instantiate(skiJumperPrefab);
        PlayerController pc = player.GetComponent<PlayerController>();
        uIManager = GetComponent<UIManager>();

        uIManager.hillInfo.text = hillPrefab.name.Replace("-", " ");
        pc.lastScoreText = uIManager.lastScore;
        pc.bestScoreText = uIManager.bestScore;
        pc.landingText = uIManager.landingType;
        pc.SetUIManager(uIManager);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H)) {
            uIManager.ToggleHelpPanel();
        }
        else if (Input.GetKeyDown(KeyCode.Escape)) {
            SceneManager.LoadScene("MainMenu");
        }
    }

    public void DisplayJumpResult() {
        uIManager.ToggleJumpResultPanel();
    }
}

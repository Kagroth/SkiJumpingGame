using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject gameplayPanel;
    
    public Text hillInfo;
    public Text lastScore;
    public Text bestScore;
    public Text landingType;

    public GameObject helpPanel;

    private bool helpPanelShow = false;

    private void Awake() {
        helpPanel.SetActive(helpPanelShow);    
    }

    public void ToggleHelpPanel() {
        helpPanelShow = !helpPanelShow;
        helpPanel.SetActive(helpPanelShow);
        gameplayPanel.SetActive(!helpPanelShow);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

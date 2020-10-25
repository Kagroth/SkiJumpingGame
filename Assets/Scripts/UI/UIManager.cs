using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public virtual void Init() {

    }

    public GameObject gameplayPanel;
    
    public Text hillInfo;
    public Text lastScore;
    public Text bestScore;
    public Text landingType;

    public GameObject helpPanel;

    public WindMeterUI windMeter;

    private bool helpPanelShow = false;
    private bool jumpResultsPanelShow = false;

    private void Awake() {
        helpPanel.SetActive(helpPanelShow);    
        // jumpResultsPanel.SetActive(jumpResultsPanelShow);
    }

    public void ToggleHelpPanel() {
        helpPanelShow = !helpPanelShow;
        helpPanel.SetActive(helpPanelShow);
        gameplayPanel.SetActive(!helpPanelShow);
    }

    public void InitWindMeter() {
        windMeter.Init();
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
public class MainMenuUIManager : UIManager
{
    public View[] views;

    private View currentView;

    public override void Init()
    {
        currentView = views.Where(view => view.name.Equals("MainMenuView")).First();
        currentView.Show();
    }

    public void SwitchView(string viewToShowName) {
        View viewToShow = views.Where(view => view.name.Equals(viewToShowName)).First();
        currentView.SwitchView(viewToShow);
        currentView = viewToShow;
    }
    public void StartRandomCompetition() {
        WorldCupData.CreateRandomCompetition();
        
        SceneManager.LoadScene("RandomCompetition");
    }

    public void StartQuickWorldCup() {
        WorldCupData.CreateQuickWorldCup();

        SceneManager.LoadScene("WorldCup");
    }

    private void Start() {
        
    }

    private void Update() {
        
    }
}

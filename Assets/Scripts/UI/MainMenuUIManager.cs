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

    public override void Init(GameManager gameManager)
    {
        base.Init(gameManager);
        this.Init();
    }

    public void SwitchView(string viewToShowName) {
        View viewToShow = views.Where(view => view.name.Equals(viewToShowName)).First();
        currentView.SwitchView(viewToShow);
        currentView = viewToShow;
    }
    public void StartRandomCompetition() {
        SceneManager.LoadScene("RandomCompetition");
    }

    public void StartRandomKOCompetition() {
        // WorldCupData.CreateRandomCompetition();

        SceneManager.LoadScene("KOCompetition");
    }

    public void StartQuickWorldCup() {
        // WorldCupData.CreateQuickWorldCup(gameManager.allHills.ToList());

        SceneManager.LoadScene("WorldCup");
    }

    private void Start() {
        
    }

    private void Update() {
        
    }
}

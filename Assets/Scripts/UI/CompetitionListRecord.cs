using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CompetitionListRecord : MonoBehaviour
{
    public Text position;
    public Text hillName;
    public Text competitionType;

    private bool completed;

    public void SetData(int pos, string newHillName, string newType) {
        position.text = pos.ToString();
        hillName.text = newHillName;
        // competitionType.text = newType;
        SetColor(Color.black);
    }

    public void Complete() {
        completed = true;

        SetColor(Color.green);
    }

    public void SetColor(Color newColor) {
        position.color        = newColor;
        hillName.color        = newColor;
        competitionType.color = newColor;
    }

    private void Awake() {
        completed = false;
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

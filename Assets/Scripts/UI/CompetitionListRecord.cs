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
        competitionType.text = newType;
        completed = false;
    }

    public void Complete() {
        completed = true;
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

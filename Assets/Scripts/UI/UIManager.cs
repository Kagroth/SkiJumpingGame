using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject hill;
    public Text hillInfo;
    public Text landingType;
    
    public virtual void Init() {

    }
    
    public virtual void Init(GameManager gameManagerToSet) {
        gameManager = gameManagerToSet;
    }

    
}

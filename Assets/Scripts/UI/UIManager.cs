using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject hill;
    protected Text hillInfo;
    protected Text landingType;
    
    public virtual void Init() {

    }
    
    public virtual void Init(GameManager gameManagerToSet) {
        gameManager = gameManagerToSet;
    }

    
}

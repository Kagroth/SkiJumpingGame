using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static string currentInputMode;

    public const string SKI_JUMPER = "SKI_JUMPER";
    public const string JUMP_FINISHED = "JUMP_FINISHED";

    public const string COMPETITION_UI = "COMPETITION_UI";

    public static void SetInputMode(string inputMode) {
        currentInputMode = inputMode;
        Debug.Log("Zmieniam sterowanie na: " + inputMode);
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkiJumperSimulatorController : MonoBehaviour
{
    public GameObject hill;
    private SkiJumperSimulator skiJumperSimulator;
    private SkiJumperComputer[] skiJumpers; 


    // Start is called before the first frame update
    void Start()
    {
        skiJumperSimulator = new SkiJumperSimulator();
        skiJumpers = new SkiJumperComputer[1];
        skiJumperSimulator.SetHill(hill);
        SimulateAllJumpers();    
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) {
            SimulateAllJumpers();
        }
    }

    private void SimulateAllJumpers() {
        foreach(SkiJumperComputer skiJumper in skiJumpers) {
            skiJumperSimulator.SimulateJump(skiJumper);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;
using UnityEditor;
using System.Linq;

public class SkiJumperSimulatorController : MonoBehaviour
{
    public GameObject hill;
    private SkiJumperSimulator skiJumperSimulator;
    private List<SkiJumperComputer> skiJumpers; 

    private void Awake() {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        LoadSkiJumpers();
        skiJumperSimulator = new SkiJumperSimulator();

        skiJumperSimulator.SetHill(hill);
        SimulateAllJumpers();   
        
        foreach (SkiJumperComputer skiJumperComputer in skiJumpers) {
            Debug.Log(string.Format("Wynik skoczka {0}:  {1}m {2}pkt", 
                                    skiJumperComputer.lastname, 
                                    skiJumperComputer.jumpResultData.jumpDistance, 
                                    skiJumperComputer.jumpResultData.jumpPoints));
        }         
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) {
            SimulateAllJumpers();
            foreach (SkiJumperComputer skiJumperComputer in skiJumpers) {
                Debug.Log(string.Format("Wynik skoczka {0}:  {1}m {2}pkt", 
                                        skiJumperComputer.lastname, 
                                        skiJumperComputer.jumpResultData.jumpDistance, 
                                        skiJumperComputer.jumpResultData.jumpPoints));
            } 
        }
    }

    private void SimulateAllJumpers() {
        foreach(SkiJumperComputer skiJumper in skiJumpers) {
            skiJumperSimulator.SimulateJump(skiJumper);
        }
    }

    public void LoadSkiJumpers() {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/skijumpers.dat", FileMode.Open);
        skiJumpers = (List<SkiJumperComputer>) bf.Deserialize(file);
        file.Close();
    }

    [MenuItem("Ski Jumpers/Generate ski jumpers file")]
    public static void GenerateSkiJumpersFile() {
        List<SkiJumperComputer> skiJumpersToSave = new List<SkiJumperComputer>();
        
        skiJumpersToSave.Add(new SkiJumperComputer("Janusz", "Kowalski", "Polska"));
        skiJumpersToSave.Add(new SkiJumperComputer("Andrzej", "Nowak", "Polska"));
        skiJumpersToSave.Add(new SkiJumperComputer("Mariusz", "Klamkowski", "Polska"));

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/skijumpers.dat");
        bf.Serialize(file, skiJumpersToSave);
        file.Close();

        Debug.Log("Skoczkowie zapisani");
    }
}

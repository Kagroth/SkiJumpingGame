using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;
using UnityEditor;
using System.Linq;

public class SkiJumperDatabase
{
    public static List<SkiJumper> LoadSkiJumpers() {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/skijumpers.dat", FileMode.Open);
        List<SkiJumper> skiJumpers = (List<SkiJumper>) bf.Deserialize(file);
        file.Close();

        return skiJumpers;
    }

    [MenuItem("Ski Jumpers/Generate ski jumpers file")]
    public static void GenerateSkiJumpersFile() {
        List<SkiJumper> skiJumpersToSave = new List<SkiJumper>();
        
        skiJumpersToSave.Add(new SkiJumper("Janusz", "Kowalski", true));
        skiJumpersToSave.Add(new SkiJumper("Andrzej", "Nowak", true));
        skiJumpersToSave.Add(new SkiJumper("Mariusz", "Klamkowski", true));

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/skijumpers.dat");
        bf.Serialize(file, skiJumpersToSave);
        file.Close();

        Debug.Log("Skoczkowie zapisani");
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;
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

    public static void GenerateSkiJumpersFile() {
        List<SkiJumper> skiJumpersToSave = new List<SkiJumper>();

        GenerateSkiJumpersList(skiJumpersToSave);

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/skijumpers.dat");
        bf.Serialize(file, skiJumpersToSave);
        file.Close();

        Debug.Log("Skoczkowie zapisani");
    }

    private static void GenerateSkiJumpersList(List<SkiJumper> skiJumpersList) {
        // Austria
        skiJumpersList.Add(new SkiJumper("Richard Kraus", "Austria", true));
        skiJumpersList.Add(new SkiJumper("Barnabas Von Grimmelshausen", "Austria", true));
        skiJumpersList.Add(new SkiJumper("Benjamin Zellweger", "Austria", true));
        skiJumpersList.Add(new SkiJumper("Hartwig Laurenz", "Austria", true));
        skiJumpersList.Add(new SkiJumper("Eckart Gros", "Austria", true));
        skiJumpersList.Add(new SkiJumper("Gert Bernhard", "Austria", true));
        skiJumpersList.Add(new SkiJumper("Gotthold Grosser", "Austria", true));
        skiJumpersList.Add(new SkiJumper("Dominik Mein", "Austria", true));

        // Bułgaria
        skiJumpersList.Add(new SkiJumper("Angel Chavdarov", "Bulgaria", true));
        skiJumpersList.Add(new SkiJumper("Rayko Simeonov", "Bulgaria", true));

        // Czechy
        skiJumpersList.Add(new SkiJumper("Kornel Kader", "Czechy", true));
        skiJumpersList.Add(new SkiJumper("Julius Klimek", "Czechy", true));
        skiJumpersList.Add(new SkiJumper("Vavrinec Laska", "Czechy", true));
        skiJumpersList.Add(new SkiJumper("Vitezslav Zitnik", "Czechy", true));
        skiJumpersList.Add(new SkiJumper("Vlasitimir Kozel", "Czechy", true));

        // Estonia
        skiJumpersList.Add(new SkiJumper("Meelis Tamm", "Estonia", true));
        skiJumpersList.Add(new SkiJumper("Andres Kukk", "Estonia", true));

        // Finlandia
        skiJumpersList.Add(new SkiJumper("Viljami Ruotsalainen", "Finlandia", true));
        skiJumpersList.Add(new SkiJumper("Jalmari Linna", "Finlandia", true));
        skiJumpersList.Add(new SkiJumper("Aatu Virtanen", "Finlandia", true));
        skiJumpersList.Add(new SkiJumper("Teuvo Jarvinen", "Finlandia", true));
        skiJumpersList.Add(new SkiJumper("Manu Karppinen", "Finlandia", true));

        // Japonia
        skiJumpersList.Add(new SkiJumper("Osamu Tamura", "Japonia", true));
        skiJumpersList.Add(new SkiJumper("Ryoichi Tamura", "Finlandia", true));
        skiJumpersList.Add(new SkiJumper("Kazuo Sugiyama", "Finlandia", true));
        skiJumpersList.Add(new SkiJumper("Kyou Yamauchi", "Finlandia", true));
        skiJumpersList.Add(new SkiJumper("Hayato Tanaka", "Finlandia", true));
        skiJumpersList.Add(new SkiJumper("Takahiro Minami", "Finlandia", true));

        // Kanada
        skiJumpersList.Add(new SkiJumper("Cayden Cairns", "Kanada", true));
        skiJumpersList.Add(new SkiJumper("Emerson Jones", "Kanada", true));

        // Kazachstan
        skiJumpersList.Add(new SkiJumper("Azat Bulat", "Kazachstan", true));
        skiJumpersList.Add(new SkiJumper("Rustam Erasyl", "Kazachstan", true));

        // Niemcy
        skiJumpersList.Add(new SkiJumper("Matthias Gunther", "Niemcy", true));
        skiJumpersList.Add(new SkiJumper("Sebastian Armbruster", "Niemcy", true));
        skiJumpersList.Add(new SkiJumper("Edmund Seidel", "Niemcy", true));
        skiJumpersList.Add(new SkiJumper("Ferdi Riese", "Niemcy", true));
        skiJumpersList.Add(new SkiJumper("Tim Schuttmann", "Niemcy", true));
        skiJumpersList.Add(new SkiJumper("Aaron Loewe", "Niemcy", true));
        skiJumpersList.Add(new SkiJumper("August Geier", "Niemcy", true));
        skiJumpersList.Add(new SkiJumper("Wendelin Schlosser", "Niemcy", true));

        // Norwegia
        skiJumpersList.Add(new SkiJumper("Mats Halvorsen", "Norwegia", true));
        skiJumpersList.Add(new SkiJumper("Alexander Iversen", "Norwegia", true));
        skiJumpersList.Add(new SkiJumper("Fredrik Andersen", "Norwegia", true));
        skiJumpersList.Add(new SkiJumper("Halle Dahl", "Norwegia", true));
        skiJumpersList.Add(new SkiJumper("Torger Hansen", "Norwegia", true));
        skiJumpersList.Add(new SkiJumper("Bjarte Stenberg", "Norwegia", true));
        skiJumpersList.Add(new SkiJumper("Ebbe Ernst Antonsen", "Norwegia", true));
        skiJumpersList.Add(new SkiJumper("Andor Torgils Vang", "Norwegia", true));

        // Polska
        skiJumpersList.Add(new SkiJumper("Przemysław Popławski", "Polska", true));
        skiJumpersList.Add(new SkiJumper("Tymoteusz Kowalski", "Polska", true));
        skiJumpersList.Add(new SkiJumper("Jakub Wasilewski", "Polska", true));
        skiJumpersList.Add(new SkiJumper("Kamil Zima", "Polska", true));
        skiJumpersList.Add(new SkiJumper("Adam Czajka", "Polska", true));
        skiJumpersList.Add(new SkiJumper("Kornel Pokorny", "Polska", true));
        skiJumpersList.Add(new SkiJumper("Krystian Ząbek", "Polska", true));

        // Rosja
        skiJumpersList.Add(new SkiJumper("Evgeni Orlov", "Rosja", true));
        skiJumpersList.Add(new SkiJumper("Dmitrij Krupin", "Rosja", true));
        skiJumpersList.Add(new SkiJumper("Yury Naumov", "Rosja", true));
        skiJumpersList.Add(new SkiJumper("Vladimir Viktorov", "Rosja", true));

        // Słowenia
        skiJumpersList.Add(new SkiJumper("Jernej Gasper", "Słowenia", true));
        skiJumpersList.Add(new SkiJumper("Bojan Plesko", "Słowenia", true));
        skiJumpersList.Add(new SkiJumper("Tijan Strnad", "Słowenia", true));
        skiJumpersList.Add(new SkiJumper("Grega Korosec", "Słowenia", true));
        skiJumpersList.Add(new SkiJumper("Vinko Zupancic", "Słowenia", true));
        skiJumpersList.Add(new SkiJumper("Drago Babic", "Słowenia", true));
        skiJumpersList.Add(new SkiJumper("Borut Babic", "Słowenia", true));

        // Szwajcaria
        skiJumpersList.Add(new SkiJumper("Gido Kernen", "Szwajcaria", true));
        skiJumpersList.Add(new SkiJumper("Niklas Zahnd", "Szwajcaria", true));
        skiJumpersList.Add(new SkiJumper("Jost Held", "Szwajcaria", true));
        skiJumpersList.Add(new SkiJumper("Leonardo Habrerli", "Szwajcaria", true));

        // Stany Zjednoczone
        skiJumpersList.Add(new SkiJumper("Royston Tennison", "Stany Zjednoczone", true));
        skiJumpersList.Add(new SkiJumper("Cedric Bateson", "Stany Zjednoczone", true));

        // Francja
        skiJumpersList.Add(new SkiJumper("Lothaire Lyon", "Francja", true));
        skiJumpersList.Add(new SkiJumper("Emeric Thayer", "Francja", true));
        skiJumpersList.Add(new SkiJumper("Jean-Francois Pierre", "Francja", true));
        skiJumpersList.Add(new SkiJumper("Matheo Gerard", "Francja", true));
    }
}

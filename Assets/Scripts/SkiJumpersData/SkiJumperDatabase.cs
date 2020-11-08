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
        skiJumpersList.Add(new SkiJumper("Richard Kraus", Country.AUSTRIA, true));
        skiJumpersList.Add(new SkiJumper("Barnabas Von Grimmelshausen", Country.AUSTRIA, true));
        skiJumpersList.Add(new SkiJumper("Benjamin Zellweger", Country.AUSTRIA, true));
        skiJumpersList.Add(new SkiJumper("Hartwig Laurenz", Country.AUSTRIA, true));
        skiJumpersList.Add(new SkiJumper("Eckart Gros", Country.AUSTRIA, true));
        skiJumpersList.Add(new SkiJumper("Gert Bernhard", Country.AUSTRIA, true));
        skiJumpersList.Add(new SkiJumper("Gotthold Grosser", Country.AUSTRIA, true));
        skiJumpersList.Add(new SkiJumper("Dominik Mein", Country.AUSTRIA, true));

        // Bułgaria
        skiJumpersList.Add(new SkiJumper("Angel Chavdarov", Country.BULGARIA, true));
        skiJumpersList.Add(new SkiJumper("Rayko Simeonov", Country.BULGARIA, true));

        // Czechy
        skiJumpersList.Add(new SkiJumper("Kornel Kader", Country.CZECH_REPUBLIC, true));
        skiJumpersList.Add(new SkiJumper("Julius Klimek", Country.CZECH_REPUBLIC, true));
        skiJumpersList.Add(new SkiJumper("Vavrinec Laska", Country.CZECH_REPUBLIC, true));
        skiJumpersList.Add(new SkiJumper("Vitezslav Zitnik", Country.CZECH_REPUBLIC, true));
        skiJumpersList.Add(new SkiJumper("Vlasitimir Kozel", Country.CZECH_REPUBLIC, true));

        // Estonia
        skiJumpersList.Add(new SkiJumper("Meelis Tamm", Country.ESTONIA, true));
        skiJumpersList.Add(new SkiJumper("Andres Kukk", Country.ESTONIA, true));

        // Finlandia
        skiJumpersList.Add(new SkiJumper("Viljami Ruotsalainen", Country.FINLAND, true));
        skiJumpersList.Add(new SkiJumper("Jalmari Linna", Country.FINLAND, true));
        skiJumpersList.Add(new SkiJumper("Aatu Virtanen", Country.FINLAND, true));
        skiJumpersList.Add(new SkiJumper("Teuvo Jarvinen", Country.FINLAND, true));
        skiJumpersList.Add(new SkiJumper("Manu Karppinen", Country.FINLAND, true));

        // Japonia
        skiJumpersList.Add(new SkiJumper("Osamu Tamura", Country.JAPAN, true));
        skiJumpersList.Add(new SkiJumper("Ryoichi Tamura", Country.JAPAN, true));
        skiJumpersList.Add(new SkiJumper("Kazuo Sugiyama", Country.JAPAN, true));
        skiJumpersList.Add(new SkiJumper("Kyou Yamauchi", Country.JAPAN, true));
        skiJumpersList.Add(new SkiJumper("Hayato Tanaka", Country.JAPAN, true));
        skiJumpersList.Add(new SkiJumper("Takahiro Minami", Country.JAPAN, true));

        // Kanada
        skiJumpersList.Add(new SkiJumper("Cayden Cairns", Country.CANADA, true));
        skiJumpersList.Add(new SkiJumper("Emerson Jones", Country.CANADA, true));

        // Kazachstan
        skiJumpersList.Add(new SkiJumper("Azat Bulat", Country.KAZAKHSTAN, true));
        skiJumpersList.Add(new SkiJumper("Rustam Erasyl", Country.KAZAKHSTAN, true));

        // Niemcy
        skiJumpersList.Add(new SkiJumper("Matthias Gunther", Country.GERMANY, true));
        skiJumpersList.Add(new SkiJumper("Sebastian Armbruster", Country.GERMANY, true));
        skiJumpersList.Add(new SkiJumper("Edmund Seidel", Country.GERMANY, true));
        skiJumpersList.Add(new SkiJumper("Ferdi Riese", Country.GERMANY, true));
        skiJumpersList.Add(new SkiJumper("Tim Schuttmann", Country.GERMANY, true));
        skiJumpersList.Add(new SkiJumper("Aaron Loewe", Country.GERMANY, true));
        skiJumpersList.Add(new SkiJumper("August Geier", Country.GERMANY, true));
        skiJumpersList.Add(new SkiJumper("Wendelin Schlosser", Country.GERMANY, true));

        // Norwegia
        skiJumpersList.Add(new SkiJumper("Mats Halvorsen", Country.NORWAY, true));
        skiJumpersList.Add(new SkiJumper("Alexander Iversen", Country.NORWAY, true));
        skiJumpersList.Add(new SkiJumper("Fredrik Andersen", Country.NORWAY, true));
        skiJumpersList.Add(new SkiJumper("Halle Dahl", Country.NORWAY, true));
        skiJumpersList.Add(new SkiJumper("Torger Hansen", Country.NORWAY, true));
        skiJumpersList.Add(new SkiJumper("Bjarte Stenberg", Country.NORWAY, true));
        skiJumpersList.Add(new SkiJumper("Ebbe Ernst Antonsen", Country.NORWAY, true));
        skiJumpersList.Add(new SkiJumper("Andor Torgils Vang", Country.NORWAY, true));

        // Polska
        skiJumpersList.Add(new SkiJumper("Przemysław Popławski", Country.POLAND, true));
        skiJumpersList.Add(new SkiJumper("Tymoteusz Kowalski", Country.POLAND, true));
        skiJumpersList.Add(new SkiJumper("Jakub Wasilewski", Country.POLAND, true));
        skiJumpersList.Add(new SkiJumper("Kamil Zima", Country.POLAND, true));
        skiJumpersList.Add(new SkiJumper("Adam Czajka", Country.POLAND, true));
        skiJumpersList.Add(new SkiJumper("Kornel Pokorny", Country.POLAND, true));
        skiJumpersList.Add(new SkiJumper("Krystian Ząbek", Country.POLAND, true));

        // Rosja
        skiJumpersList.Add(new SkiJumper("Evgeni Orlov", Country.RUSSIA, true));
        skiJumpersList.Add(new SkiJumper("Dmitrij Krupin", Country.RUSSIA, true));
        skiJumpersList.Add(new SkiJumper("Yury Naumov", Country.RUSSIA, true));
        skiJumpersList.Add(new SkiJumper("Vladimir Viktorov", Country.RUSSIA, true));

        // Słowenia
        skiJumpersList.Add(new SkiJumper("Jernej Gasper", Country.SLOVENIA, true));
        skiJumpersList.Add(new SkiJumper("Bojan Plesko", Country.SLOVENIA, true));
        skiJumpersList.Add(new SkiJumper("Tijan Strnad", Country.SLOVENIA, true));
        skiJumpersList.Add(new SkiJumper("Grega Korosec", Country.SLOVENIA, true));
        skiJumpersList.Add(new SkiJumper("Vinko Zupancic", Country.SLOVENIA, true));
        skiJumpersList.Add(new SkiJumper("Drago Babic", Country.SLOVENIA, true));
        skiJumpersList.Add(new SkiJumper("Borut Babic", Country.SLOVENIA, true));

        // Szwajcaria
        skiJumpersList.Add(new SkiJumper("Gido Kernen", Country.SWITZERLAND, true));
        skiJumpersList.Add(new SkiJumper("Niklas Zahnd", Country.SWITZERLAND, true));
        skiJumpersList.Add(new SkiJumper("Jost Held", Country.SWITZERLAND, true));
        skiJumpersList.Add(new SkiJumper("Leonardo Habrerli", Country.SWITZERLAND, true));

        // Stany Zjednoczone
        skiJumpersList.Add(new SkiJumper("Royston Tennison", Country.USA, true));
        skiJumpersList.Add(new SkiJumper("Cedric Bateson", Country.USA, true));

        // Francja
        skiJumpersList.Add(new SkiJumper("Lothaire Lyon", Country.FRANCE, true));
        skiJumpersList.Add(new SkiJumper("Emeric Thayer", Country.FRANCE, true));
        skiJumpersList.Add(new SkiJumper("Jean-Francois Pierre", Country.FRANCE, true));
        skiJumpersList.Add(new SkiJumper("Matheo Gerard", Country.FRANCE, true));
    }
}

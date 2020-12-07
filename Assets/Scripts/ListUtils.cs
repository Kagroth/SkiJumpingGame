using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ListUtils
{
    public static void LogList(List<CompetitionResult> listToLog, string listName) {
        string listStr = "";
        listStr += "Lista startowa - " + listName +": \n"; 
        listStr += "Liczba zawodników: " + listToLog.Count.ToString() + "\n";

        foreach (CompetitionResult cr in listToLog) {
            listStr += cr.skiJumper.skiJumperName + "\n";
        }

        Debug.Log(listStr);
    }

    public static void CopyList(List<CompetitionResult> sourceList, List<CompetitionResult> destinationList) {
        destinationList.Clear();

        foreach (CompetitionResult cr in sourceList) {
            destinationList.Add(cr);
        }
    }

    public static bool ListReferenceEquals(List<CompetitionResult> listOne, List<CompetitionResult> listTwo) {
        return Object.ReferenceEquals(listOne, listTwo);
    }

    public static bool ListContentEquals(List<CompetitionResult> listOne, List<CompetitionResult> listTwo) {
        if (listOne.Count != listTwo.Count) {
            return false;
        }

        for (int index = 0; index < listOne.Count; index++) {
            if (listOne.Contains(listTwo[index])) {
                continue;
            }

            return false;
        }

        return true;
    }
}

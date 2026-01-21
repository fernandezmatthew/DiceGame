using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class YahtzeeSaveHandler {
    private string dataDirPath;

    public YahtzeeSaveHandler() {
        dataDirPath = Path.Combine(Application.persistentDataPath, "YahtzeeGameData");
    }
    // we want file name to be dataX starting at X = 0;
    public void Save(int[] rollScores, int[] gameScores) {

        int[] dateTime = new int[6];
        DateTime now = DateTime.Now;
        dateTime[0] = now.Month;
        dateTime[1] = now.Day;
        dateTime[2] = now.Year;
        dateTime[3] = now.Hour;
        dateTime[4] = now.Minute;
        dateTime[5] = now.Second;

        YahtzeeGameData data = new YahtzeeGameData(rollScores, gameScores, dateTime);

        string fileName = "";
        foreach (var n in dateTime) {
            fileName += n.ToString() + "_";
        }
        fileName = fileName.Substring(0, fileName.Length - 1);
        fileName += ".yahtzee";

        string fullPath = Path.Combine(dataDirPath, fileName);

        try {
            Directory.CreateDirectory(dataDirPath);
            string dataString = JsonUtility.ToJson(data, true);

            using (FileStream stream = new FileStream(fullPath, FileMode.Create)) {
                using (StreamWriter writer = new StreamWriter(stream)) {
                    writer.Write(dataString);
                    Debug.Log("File saved to: " + fullPath);
                }
            }
        }
        catch (Exception e) {
            Debug.Log("Error when trying to save file " + fullPath);
        }
    }
}

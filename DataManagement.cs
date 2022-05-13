using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public class DataManagement : MonoBehaviour
{
    private const string path = @"D:\Unity\2018.3.10f1\Flappy Bird ML_Agents\DATA";

    Model model;

    float lstTime;

    float a = 0, b = 0;

    private void Start()
    {
        lstTime = Time.time;
    }
    private void Update()
    {
        if(lstTime+300  < Time.time)
        {
            lstTime = Time.time;

            model = GameObject.Find("Player").GetComponent<Model>();

            float score = model.overallScore;
            float deaths = model.overallDeaths;
            StartCoroutine(Sort(score-a, deaths-b));

            a = score;
            b = deaths;
        }
    }
    IEnumerator Sort(float score, float deaths)
    {

        int index = 0;

        float average_score = (score / deaths);
        model.reward = average_score;


        while (true)
        {
            if (File.Exists($@"{path}\data(improved){index}.txt"))
            {
                index += 1;
                continue;
            }

            else
            {
                using (StreamWriter writer = File.CreateText(@$"{path}\data(improved){index}.txt"))
                {
                    writer.WriteLine($"Average score: {average_score.ToString("F3")}");
                    writer.WriteLine($"Score: {score}, Deaths: {deaths}");
                    writer.WriteLine($"\nHigh score: {model.highScore}");
                    break;
                }
            }
        }
        yield return null;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine.UI;

public class Model : Agent
{
    private Vector3 Plocation = new Vector3(-7.35f, 0, 0);
    private Vector3 Clocation = new Vector3(0, 0, -10);
    private Vector3 Glocation = new Vector3(0, -6, 0);
    [SerializeField] private float flypower = 5f, speed = 5f;

    [HideInInspector] public float overallScore = 0, overallDeaths = 0, highScore = 38, reward=1;

    public Text scoreText, deathText, overallScoreText;
    public Camera cam; 
    
    ObstacleGenereation obsg;

    private int score = 0;

    private void Update()
    {
        this.transform.position += Vector3.right * Time.deltaTime * speed;
    }
    public override void OnEpisodeBegin()
    {
        score = 0;
        scoreText.text = "Score: 0";

        obsg = GameObject.Find("EventSystem").GetComponent<ObstacleGenereation>();
        obsg.counter = 0;

        foreach (var i in obsg.CubeList1)
        {
            Destroy(i.gameObject);
        }
        foreach (var i in obsg.CubeList2)
        {
            Destroy(i.gameObject);
        }
        foreach (var i in obsg.Gaplist)
        {
            Destroy(i.gameObject);
        }

        obsg.CubeList1.Clear();
        obsg.CubeList2.Clear();
        obsg.Gaplist.Clear();

        this.transform.position = Plocation;
        GameObject.Find("Main Camera").GetComponent<Transform>().position = Clocation;
        GameObject.Find("Ground").GetComponent<Transform>().position = Glocation;
        GameObject.Find("Ground (1)").GetComponent<Transform>().position = new Vector3(0, 6.21f, 0);

        for (int i = 0; i <= 5; i++)
        {
            obsg.generate_obstacle();
        }
    }


    public override void OnActionReceived(ActionBuffers actions)
    {
        int jmp = actions.DiscreteActions[0];

        Debug.Log(jmp);

        if (jmp == 1)
        {
            this.GetComponent<Rigidbody>().velocity = Vector3.zero;
            this.transform.localPosition += Vector3.up * Time.deltaTime * flypower;
        }

    }
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(obsg.CubeList1[score].transform.localPosition);
        sensor.AddObservation(obsg.CubeList1[score].transform.localScale);
        sensor.AddObservation(obsg.CubeList2[score].transform.localPosition);
        sensor.AddObservation(obsg.CubeList2[score].transform.localScale);
        sensor.AddObservation(obsg.Gaplist[score].transform.localPosition);
        sensor.AddObservation(obsg.Gaplist[score].transform.localScale);

        sensor.AddObservation(this.transform.localPosition);
        sensor.AddObservation(this.transform.localScale);

        sensor.AddObservation(GameObject.Find("Ground").GetComponent<Transform>().position);
        sensor.AddObservation(GameObject.Find("Ground (1)").GetComponent<Transform>().position);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<int> continousActions = actionsOut.DiscreteActions;
        continousActions[0] = Input.GetKey(KeyCode.Space) ?  1: 0;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "obstacle" || collision.collider.tag == "BAD")
        {
            cam.backgroundColor = Color.red;
            overallDeaths += 1;

            deathText.text = $"Deaths: {overallDeaths}";

            if(score > highScore)
            {
                highScore = score;
            }

            SetReward(-10);
        }
        if(collision.collider.tag == "BAD")
        {
            SetReward(-10);
        }
        EndEpisode();
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Trigger")
        {
            overallScore += 1;
            score += 1;
            overallScoreText.text = $"Total score: {overallScore}";

            scoreText.text = $"Score: {score}";
            SetReward(1);

            cam.backgroundColor = Color.green;
            obsg.generate_obstacle();

            Debug.Log("Score");
        }
    }
}

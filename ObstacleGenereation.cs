using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleGenereation : MonoBehaviour
{
    [SerializeField] private float obsatacle_width = 0.5f, obstacle_height = 5f;
    [SerializeField] private float obstacle_gap = 5f;
    [SerializeField] private float distance_between_obstacles = 10f;
    [SerializeField] private Vector2 barrier;

    [HideInInspector] public int counter = 0;
    [HideInInspector] public int gapLocIndex = 0;

    [HideInInspector] public List<GameObject> CubeList1 = new List<GameObject>();
    [HideInInspector] public List<GameObject> CubeList2 = new List<GameObject>();
    [HideInInspector] public List<GameObject> Gaplist = new List<GameObject>();


    public void generate_obstacle()
    {
        // top cube
        GameObject cubeObject = GameObject.CreatePrimitive(PrimitiveType.Cube);

        cubeObject.tag = "obstacle";

        cubeObject.transform.localPosition = new Vector3(counter * distance_between_obstacles, (obstacle_gap), 0);
        cubeObject.transform.localScale = new Vector3(obsatacle_width, obstacle_height - obstacle_gap, 1);
        cubeObject.GetComponent<MeshRenderer>().material.color = Color.grey;


        //bottom cube
        GameObject cubeObject2 = GameObject.CreatePrimitive(PrimitiveType.Cube);

        cubeObject2.tag = "obstacle";

        cubeObject2.transform.localPosition = new Vector3(counter * distance_between_obstacles, -(obstacle_gap), 0);
        cubeObject2.transform.localScale = new Vector3(obsatacle_width, obstacle_height - obstacle_gap, 1);
        cubeObject2.GetComponent<MeshRenderer>().material.color = Color.grey;

        // Set parent
        cubeObject2.transform.SetParent(cubeObject.transform);


        //set random y to parent 
        float height = Random.Range(barrier.x, barrier.y);
        Vector3 cur_loc = cubeObject.transform.position;
        cubeObject.transform.position = new Vector3(cur_loc.x, height, cur_loc.z);

        //Place trigger detection cube in gaps
        GameObject triggerObject = GameObject.CreatePrimitive(PrimitiveType.Cube);

        triggerObject.tag = "Trigger";

        triggerObject.transform.localPosition = new Vector3(counter * distance_between_obstacles, height-obstacle_gap,0);
        triggerObject.transform.localScale = new Vector3(obsatacle_width, obstacle_gap/2, 1);

        triggerObject.GetComponent<BoxCollider>().isTrigger = true;
        triggerObject.GetComponent<MeshRenderer>().enabled = false;

        CubeList1.Add(cubeObject);
        CubeList2.Add(cubeObject2);
        Gaplist.Add(triggerObject);

        counter += 1;
        
    }



}

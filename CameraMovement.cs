using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    // Start is called before the first frame update
    Camera cam;
    [SerializeField] private float flyspeed = 5f;
    private void Start()
    {
        cam = this.GetComponent<Camera>();
    }
    void Update()
    {
        cam.transform.position += Vector3.right * Time.deltaTime * flyspeed;
    }
}

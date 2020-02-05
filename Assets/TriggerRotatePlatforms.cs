using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerRotatePlatforms : MonoBehaviour
{
    public float RotationSpeed;
    public GameObject[] RotatableObjects;
    private Quaternion rotation0;
    private Quaternion rotation1;
    private bool active;
    private bool finished;
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("entered");
        active = true;
    }

    private void Start()
    {
        rotation0 = RotatableObjects[0].transform.rotation;
        rotation1 = RotatableObjects[1].transform.rotation;
    }

    private void Update()
    {
        if (!active)
        {

        }
        if (active && !finished)
        {
            var ro = RotatableObjects;

            ro[0].transform.rotation = Quaternion.Slerp(ro[0].transform.rotation, rotation1, RotationSpeed * Time.deltaTime);
            ro[1].transform.rotation = Quaternion.Slerp(ro[1].transform.rotation, rotation0, RotationSpeed * Time.deltaTime);

            if(ro[0].transform.rotation == rotation1)
            {
                finished = true;
            }
        }
    }
}

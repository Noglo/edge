using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform Target;

    [SerializeField]
    private float smooth;

    [SerializeField]
    private float camZ = 0;
    [SerializeField]
    private float camX = 0;
    [SerializeField]
    private float camY = 0;

    // Update is called once per frame
    void LateUpdate()
    {
        float xDist = Target.transform.position.x - camX;
        float yDist = Target.transform.position.y - camY;
        float zDist = Target.transform.position.z - camZ;

        //lerp to follow player
        transform.position = Vector3.Lerp(transform.position, new Vector3(xDist, yDist, zDist), smooth * Time.deltaTime);
    }
}

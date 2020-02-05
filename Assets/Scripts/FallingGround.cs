using System.Collections;
using UnityEngine;

public class FallingGround : MonoBehaviour
{
    private bool isFalling;
    private float fallingSpeed = 12;
    private float fallingDelay = 1;

    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.up, out hit, .5f, LayerMask.GetMask("Default")))
        {
#if UNITY_EDITOR
            Debug.DrawRay(transform.position, Vector3.up, Color.red);
#endif
            StartCoroutine(Fall());
        }
        else
        {
#if UNITY_EDITOR
            Debug.DrawRay(transform.position, Vector3.up, Color.white);
#endif
        }

        if (isFalling)
        {
            transform.Translate(Vector3.down * Time.deltaTime * fallingSpeed, Space.World);
        }
    }

    IEnumerator Fall()
    {
        var t = 0.0f;

        while (t < fallingDelay)
        {
            t += Time.deltaTime;

            if (t < fallingDelay)
            {
                yield return null;
            }
        }

        isFalling = true;
    }
}

using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float TumblingDuration = 0.3f;
    public float FallingSpeed = 0.3f;

    private bool isTumbling = false;
    private bool isGrounded = false;

    // round up by Y when the fall is over
    private bool isFalling = false;

    private void Update()
    {
        isGrounded = Raycasting(Vector3.down);

        if (!isGrounded && !isTumbling)
        {
            isFalling = true;
            Fall();
            return;
        }

        if (isFalling)
        {
            Vector3 position = transform.position;
            position.y = Mathf.Round(position.y);
            transform.position = position;
            isFalling = false;
        }
        var dir = Vector3.zero;

        if (SwipeInput.swipedUp)
        {
            Swiped(Vector3.forward, ref dir, Raycasting(Vector3.forward));
        }
        if (SwipeInput.swipedDown)
        {
            Swiped(Vector3.back, ref dir, Raycasting(Vector3.back));
        }
        if (SwipeInput.swipedLeft)
        {
            Swiped(Vector3.left, ref dir, Raycasting(Vector3.left));
        }
        if (SwipeInput.swipedRight)
        {
            Swiped(Vector3.right, ref dir, Raycasting(Vector3.right));
        }

        if (dir != Vector3.zero && !isTumbling)
        {
            StartCoroutine(Tumble(dir, RaycastingClimbing(dir), !RaycastingClimbing(dir)));
        }
    }

    private void Swiped(Vector3 direction, ref Vector3 dir, bool blocked)
    {
            if (!blocked)
            {
                dir = direction;
            }
            else if (blocked && !isTumbling)
            {
                StartCoroutine(Tumble(direction, true, !RaycastingClimbing(direction)));
            }
    }

    #region Raycast
    private bool Raycasting(Vector3 direction)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, direction, out hit, 1f, LayerMask.GetMask("Default")))
        {
#if UNITY_EDITOR
            Debug.DrawRay(transform.position, direction * 2, Color.red);
#endif
            return true;
        }
        else
        {
#if UNITY_EDITOR
            Debug.DrawRay(transform.position, direction * 2, Color.white);
#endif
            return false;
        }
    }

    private bool RaycastingClimbing(Vector3 direction)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up, direction, out hit, 1, LayerMask.GetMask("Default")))
        {
#if UNITY_EDITOR
            Debug.DrawRay(transform.position + Vector3.up, direction * 2, Color.yellow);
#endif
            return true;
        }
        else
        {
#if UNITY_EDITOR
            Debug.DrawRay(transform.position + Vector3.up, direction * 2, Color.blue);
#endif
            return false;
        }
    }
    #endregion

    #region Movement
    private void Fall()
    {
        transform.Translate(Vector3.down * Time.deltaTime * FallingSpeed, Space.World);
    }

    /// <param name="climbing">Defines will the player is willing to climb the wall</param>
    /// <param name="success">Defines will the player actually climb the wall</param>
    IEnumerator Tumble(Vector3 direction, bool climbing = false, bool success = true)
    {
        isTumbling = true;

        var rotAxis = Vector3.Cross(Vector3.up, direction);
        var pivot = (transform.position + (climbing ? Vector3.up : Vector3.down) * 0.5f) + direction * .5f;

        var startRotation = transform.rotation;
        var endRotation = Quaternion.AngleAxis(climbing ? 180f : 90, rotAxis) * startRotation;

        var startPosition = transform.position;
        var endPosition = transform.position + direction + (climbing ? Vector3.up : Vector3.zero);

        var rotSpeed = 90.0f / TumblingDuration;
        var t = 0.0f;

        while (t < TumblingDuration)
        {
            var divider = success ? 2 : 1;
            divider = climbing ? divider : 1;
            t += Time.deltaTime / divider;
            
            if (t < TumblingDuration)
            {
                transform.RotateAround(pivot, rotAxis, rotSpeed * Time.deltaTime);
                yield return null;
            }
            else
            {
                endPosition.y = Mathf.Round(transform.position.y);

                transform.rotation = endRotation;
                transform.position = success ? endPosition : startPosition + Vector3.up;
            }
        }

        isTumbling = false;
    }
    #endregion
}
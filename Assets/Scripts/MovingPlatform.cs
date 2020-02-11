using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public enum PlatformType{
        Endless,
        OnInteract
    }
    
    [Header("Way Points")]
    public Transform[] wayPoints;

    [Header("Movement")]
    public PlatformType Type;
    [SerializeField] private float speed = 10f;
    [SerializeField] private float timeDelay = 2f;

    [Header ("Gizmos")]
    [SerializeField] private float sphRadius = 1f;

    private float timer;
    private Vector3 nextPos;
    private int wayDir;
    private int curPointNum;
    private bool onPlatform;

    void Start(){
        transform.position = wayPoints[0].position;
    }

    void Update()
    {
        SetNextPos();
        if (Type == PlatformType.Endless){
            EndlessMove();
        }
        else{
            OnInteractMove();
        }
    }

    void FixedUpdate() {
        if (timer >= timeDelay)
        {
            PlayerOnPlatform();
        }
        else
        {
            timer += Time.deltaTime;
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos() {
        for (int i = 0; i < wayPoints.Length; i++){
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(wayPoints[i].position, sphRadius);
            if (i >= 1){
                Gizmos.color = Color.green;
                Gizmos.DrawLine(wayPoints[i - 1].position, wayPoints[i].position);
            }
        }
    }
#endif

    private void SetNextPos(){ // Присваивает nextPos позицию следующего пункта.
        if (transform.position == wayPoints[0].position && nextPos != wayPoints[1].position){
            nextPos = wayPoints[1].position;
            curPointNum = 1;
            wayDir = 1;
            onPlatform = false;
            timer = 0;
        }
        else if (transform.position == wayPoints[wayPoints.Length - 1].position && nextPos != wayPoints[wayPoints.Length - 2].position){
            nextPos = wayPoints[curPointNum - 1].position;
            curPointNum = wayPoints.Length - 2;
            wayDir = -1;
            onPlatform = false;
            timer = 0;

        }
        else if(transform.position == wayPoints[curPointNum].position && (nextPos != wayPoints[curPointNum + 1].position || nextPos != wayPoints[curPointNum - 1].position)){
            curPointNum += wayDir;
            nextPos = wayPoints[curPointNum].position;
            if (Type == PlatformType.Endless)
            {
                timer = 0;
            }
        }
    }

    private void EndlessMove(){ // Движение для платформы, двигающейся бесконечно.
        if (timer >= timeDelay){
            transform.position = Vector3.MoveTowards(transform.position, nextPos, speed * Time.deltaTime);
        }
        else{
            timer += Time.deltaTime;
        }
        
    }

    private void OnInteractMove(){ // Движение для платформы, реагирующей на воздействие игрока.
        if (onPlatform){
            transform.position = Vector3.MoveTowards(transform.position, nextPos, speed * Time.deltaTime);
        }
    }
    
    private void PlayerOnPlatform(){ // Находится ли игрок на платформе.
        RaycastHit hit;
        if (Physics.BoxCast(transform.position, transform.lossyScale * 0.4f, transform.up, out hit, transform.rotation, transform.lossyScale.y * 1.3f)){
            if (hit.collider.CompareTag("Player")){
                onPlatform = true;
            }
        }
    }
}

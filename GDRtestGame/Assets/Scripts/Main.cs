using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    Touch touch;
    Rigidbody2D circle;
    LineRenderer lr;
    bool isMoving=false;
    Vector3 startPos, touchPos, whereToMove;
    float previousDistanceToTouchPos, currentDistanceToTouchPos;
    public int moveSpeed=6; 
    public int coinsTook,coinsAmount=0;
    public Text coinsTookText, CongratsTxt;
    public GameObject[] coins;
    public GameObject menu;

    void Start(){
        circle = GetComponent<Rigidbody2D>();
        lr = GetComponent<LineRenderer>();
        coins = GameObject.FindGameObjectsWithTag("Coin");
        coinsAmount=coins.Length;
        menu.SetActive(false);
        CongratsTxt.enabled = false;
    }

    //When circle collide with coin increase var value and if collect all show Window with congrats
    private void OnCollisionEnter2D(Collision2D collision){
        if (collision.gameObject.CompareTag("Coin")){
            coinsTook++;
            coinsTookText.text=coinsTook.ToString();
            Destroy(collision.gameObject);
            if (coinsAmount==coinsTook){
                CongratsTxt.enabled=true;
                menu.SetActive(true);
            }
        }
    }

    //if circle is desctorying by smth it shows windows with restart option
    public void OnDestroy(){
        menu.SetActive(true);
    }

    void Update()
    {
        //while moving calculate first distance to target, at the beginning it will be larger than second distance (previous)
        if (isMoving)
            currentDistanceToTouchPos=(touchPos-transform.position).magnitude;
        //main logic of moving
        if (Input.touchCount>0){
            //get first touch data
            touch=Input.GetTouch(0);
            //catch only "toching" data. Get direction, draw trajectory line and set moving speed
            if (touch.phase==TouchPhase.Began){
                previousDistanceToTouchPos=0;
                currentDistanceToTouchPos=0;
                isMoving=true;
                lr.enabled=true;
                startPos=gameObject.transform.position;
                lr.SetPosition(0, startPos);
                touchPos=Camera.main.ScreenToWorldPoint(touch.position);
                touchPos.z=0;
                lr.SetPosition(1, touchPos);
                whereToMove = (touchPos-transform.position).normalized;
                circle.velocity=new Vector2 (whereToMove.x * moveSpeed, whereToMove.y * moveSpeed);
            }
        }
        
        //stop when reach target point
        if (currentDistanceToTouchPos>previousDistanceToTouchPos){
            lr.enabled=false;
            isMoving=false;
            circle.velocity=Vector2.zero;
        }
            
        //second disntace calculation. Needs to stop when reach target point
        if (isMoving)
            previousDistanceToTouchPos=(touchPos-transform.position).magnitude;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    //DungeonGenerator dungeon;

    //public Vector2Int playerPosition;

    MapDrawer MD;
    GameObject gameManager;

    public Image healthBar;
    public float speed = 3f;
    public int health = 20;
    public int maxHealth = 20;

    //private new CameraScript camera;
    private Transform canvasTransform;
    private Rigidbody2D rB;
    private Animator anim;
    private Vector2 inputMovement;
    private Vector2 moveVelocity;
    private float mapk;
    private int prevX;
    private int prevY;

    public void Start()
    {
        rB = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        canvasTransform = GameObject.Find("UI").transform;
        healthBar.fillAmount = 1f;
        gameManager = GameObject.Find("GameManager");
        MD = gameManager.GetComponent<MapDrawer>();

        mapk = MD.mapk;
        Image toInstance = Instantiate(healthBar, new Vector3(140, 69, 0), Quaternion.identity) as Image;
        toInstance.transform.SetParent(canvasTransform);
        //camera = GetComponent<CameraScript>();
    }

    public void Update()
    {
        Combat();
        //Move();
        HealthBarChange();
    }

    private void Move()
    {
        inputMovement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        moveVelocity = inputMovement * speed;
        if (inputMovement.x != 0 || inputMovement.y != 0)
        {
            if (inputMovement.y > 0)
            {
                anim.SetBool("runFrontA1", true);
                anim.SetBool("runBackA1", false);
                anim.SetBool("runLeftA1", false);
                anim.SetBool("runRightA1", false);
                //anim.SetTrigger("runFrontTrig");
            }
            else if (inputMovement.y < 0)
            {
                anim.SetBool("runBackA1", true);
                anim.SetBool("runFrontA1", false);
                anim.SetBool("runLeftA1", false);
                anim.SetBool("runRightA1", false);
            }
            else if (inputMovement.x < 0)
            {
                anim.SetBool("runLeftA1", true);
                anim.SetBool("runFrontA1", false);
                anim.SetBool("runBackA1", false);
                anim.SetBool("runRightA1", false);
            }
            else if (inputMovement.x > 0)
            {
                anim.SetBool("runRightA1", true);
                anim.SetBool("runFrontA1", false);
                anim.SetBool("runBackA1", false);
                anim.SetBool("runLeftA1", false);
            }

            prevX = Mathf.FloorToInt(rB.position.x / mapk + 0.5f);
            prevY = Mathf.FloorToInt(rB.position.y / mapk + 0.5f);
            MapManager.map[prevX, prevY].hasPlayer = false;
            rB.MovePosition(rB.position + moveVelocity * Time.deltaTime);
            prevX = Mathf.FloorToInt(rB.position.x / mapk + 0.5f);
            prevY = Mathf.FloorToInt(rB.position.y / mapk + 0.5f);
            MapManager.map[prevX, prevY].hasPlayer = true;


        }
        else
        {
            anim.SetBool("runFrontA1", false);
            anim.SetBool("runBackA1", false);
            anim.SetBool("runLeftA1", false);
            anim.SetBool("runRightA1", false);

        }
    }

    private void Combat()
    {
        if (Input.GetMouseButtonDown(0))
        {
            anim.SetTrigger("isAttackA1");


            Debug.Log(mapk);
            Debug.Log(transform.position);
            //Debug.Log(MapManager.map[(int)transform.position.x, (int)transform.position.y].hasPlayer);
            int x = Mathf.FloorToInt(rB.position.x / mapk + 0.5f);
            int y = Mathf.FloorToInt(rB.position.y / mapk + 0.5f);
            Debug.Log(x);
            Debug.Log(y);
            Debug.Log(MapManager.map[x,y].hasPlayer);

        }
            

    }

    private void HealthBarChange()
    {
        healthBar.fillAmount = health / maxHealth;
    }

    public void FixedUpdate()
    {
        Move();
    }

}

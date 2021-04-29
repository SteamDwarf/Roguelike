using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    DungeonGenerator dungeon;

    //public Vector2Int playerPosition;

    MapDrawer MD;
    GameObject gameManager;

    public Image healthBar;
    public float speed = 3f;
    public int health = 20;
    public int maxHealth = 20;
    public int noiseRange;
    public float hitRange;
    public float damage;
    public Transform hitPos;

    //private new CameraScript camera;
    private Transform canvasTransform;
    private Rigidbody2D rB;
    private Animator anim;
    private GameObject door;
    private DoorOpening doorScript;
    private Vector2 inputMovement;
    private Vector2 moveVelocity;

    private float mapk;
    private int prevX;
    private int prevY;
    private int curX;
    private int curY;
    private int negRangeNoise;
    private int posRangeNoise;
    private int mapWidth;
    private int mapHeight;

    public void Start()
    {
        rB = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        canvasTransform = GameObject.Find("UI").transform;
        healthBar.fillAmount = 1f;
        gameManager = GameObject.Find("GameManager");
        MD = gameManager.GetComponent<MapDrawer>();
        dungeon = gameManager.GetComponent<DungeonGenerator>();

        mapk = MD.mapk;
        mapWidth = dungeon.mapWidth;
        mapHeight = dungeon.mapHeight;
        Image toInstance = Instantiate(healthBar, new Vector3(140, 69, 0), Quaternion.identity) as Image;
        toInstance.transform.SetParent(canvasTransform);
        //camera = GetComponent<CameraScript>();

        negRangeNoise = -(noiseRange / 2);
        posRangeNoise = noiseRange / 2;
    }

    public void Update()
    {
        Combat();
        //Move();
        HealthBarChange();
        Actions();
    }
    public void FixedUpdate()
    {
        Move();
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
            //Debug.Log(MapManager.map[prevX, prevY].hasPlayer);
            rB.MovePosition(rB.position + moveVelocity * Time.deltaTime);
            curX = Mathf.FloorToInt(rB.position.x / mapk + 0.5f);
            curY = Mathf.FloorToInt(rB.position.y / mapk + 0.5f);
            MapManager.map[curX, curY].hasPlayer = true;
            //Debug.Log(MapManager.map[curX, curY].hasPlayer);
            NoiseFOVCheck(curX, curY);
            

        }
        else
        {
            anim.SetBool("runFrontA1", false);
            anim.SetBool("runBackA1", false);
            anim.SetBool("runLeftA1", false);
            anim.SetBool("runRightA1", false);

        }
    }

    private void Actions()
    {
        if (Input.GetKeyDown("f") && doorScript != null)
        {
            doorScript.ChangeDoorState();
        }
    }

    private void Combat()
    {
        if (Input.GetMouseButtonDown(0))
        {
            anim.SetTrigger("isAttackA1");
            MakeDamage.Action(hitPos.position, hitRange, 6, damage, true);


            /*Debug.Log(mapk);
            Debug.Log(transform.position);*/
            //Debug.Log(MapManager.map[(int)transform.position.x, (int)transform.position.y].hasPlayer);
            int x = Mathf.FloorToInt(rB.position.x / mapk + 0.5f);
            int y = Mathf.FloorToInt(rB.position.y / mapk + 0.5f);
            /*Debug.Log(x);
            Debug.Log(y);*/
            Debug.Log(MapManager.map[x, y].hasPlayer);

        }

        if(Input.GetMouseButtonDown(1))
        {
            int x = Mathf.FloorToInt(rB.position.x / mapk + 0.5f);
            int y = Mathf.FloorToInt(rB.position.y / mapk + 0.5f);

            for (int i = x - 4; i < x + 4; i++)
            {
                for (int j = y - 4; j < y + 4; j++)
                {
                    Debug.Log(MapManager.map[x, y].hasPlayer);
                }
            }
        }

    }

    private void HealthBarChange()
    {
        healthBar.fillAmount = health / maxHealth;
    }

    void NoiseFOVCheck(int x, int y)
    {
        //Debug.Log(negRangeNoise);
        //Debug.Log(posRangeNoise);
        //List<Vector2Int> cords = new List<Vector2Int>();
        for (int i = -4; i <= 4; i++)
        {
            for (int j = -4; j <= 4; j++)
            {
                if (x + i < 0 || x + i > mapWidth)
                    break;
                if (y + j < 0 || y + j > mapHeight)
                    continue;

                int xCord = x + i;
                int yCord = y + j;
                try
                {
                    if (MapManager.map[xCord, yCord].hasEnemy == true)
                    {
                        Enemy enemy = MapManager.map[xCord, yCord].enemy.GetComponent<Enemy>();
                        enemy.SawPlayer(transform.position);
                        //enemy.sawPlayer = true;
                        //enemy.currentPlayerPosition = transform.position;
                        //enemy.savedPlayerPosition = transform.position;
                        //Debug.Log("“ут враг");
                        
                        //Debug.Log(MapManager.map[xCord, yCord].enemy.name);
                    }
                }
                catch (System.Exception)
                {
                    continue;
                }
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "Door")
        {
            door = collision.gameObject;
            doorScript = door.GetComponent<DoorOpening>();
        }
    }
}

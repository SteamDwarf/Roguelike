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

    //public Image staminaBar;
    public Image staminaBar;
    public Image healthBar;
    public List<GameObject> hitBoxes;
    //public Transform hitPos;
    public float speed;
    public float maxHealth;
    public int maxStamina;
    public int noiseRange;
    public float hitRange;
    public int damage;
    public float strength;
    public bool isDied;
    

    //private new CameraScript camera;
    //private Transform canvasTransform;
    private Rigidbody2D rB;
    //private Animator anim;
    private PlayerAnimator anim;
    private GameObject door;
    private DoorOpening doorScript;
    private Vector2 inputMovement;
    private Vector2 moveVelocity;

    private float mapk;
    private int prevX;
    private int prevY;
    private int curX;
    private int curY;
    private int mapWidth;
    private int mapHeight;
    private float health;
    private float stamina;
    private float staminaPerAttack;
    private float defence;
    private bool isDefending;


    public void Start()
    {
        rB = GetComponent<Rigidbody2D>();
        //anim = GetComponent<Animator>();
        anim = GetComponent<PlayerAnimator>();
        //canvasTransform = GameObject.Find("UI").transform;

        staminaBar = GameObject.FindGameObjectWithTag("StaminaBar").GetComponent<Image>();
        healthBar = GameObject.FindGameObjectWithTag("HealthBar").GetComponent<Image>();
        staminaBar.fillAmount = 1f;
        healthBar.fillAmount = 1f;

        gameManager = GameObject.Find("GameManager");
        MD = gameManager.GetComponent<MapDrawer>();
        dungeon = gameManager.GetComponent<DungeonGenerator>();
        
        //////////////////////////////////////////////////////////////////////
        ///HIT BOX не берется/////////////////////////////////////////////

        mapk = MD.mapk;
        mapWidth = dungeon.mapWidth;
        mapHeight = dungeon.mapHeight;
        health = maxHealth;
        stamina = maxStamina;
        defence = 1;
        staminaPerAttack = 30;
        isDied = false;

        foreach (var hitBox in hitBoxes)
        {
            HitBox hB = hitBox.GetComponent<HitBox>();
            hB.damage = damage;
            hB.thrust = strength;
            hB.owner = "Player";
        }
    }

    public void Update()
    {
        Combat();
        StatBarChange();
        Actions();
        StaminaRefresh();
        NoiseFOVCheck(curX, curY);
    }
    public void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        inputMovement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        moveVelocity = inputMovement * speed;
        if ((inputMovement.x != 0 || inputMovement.y != 0))
        {
            //anim.curState = "Run";
            anim.isMoving = true;
            anim.curState = "Run";

            if (inputMovement.y > 0)
                anim.faceTo = "Back";
            else if (inputMovement.y < 0)
                anim.faceTo = "Front";
            else if (inputMovement.x > 0)
                anim.faceTo = "Right";
            else if (inputMovement.x < 0)
                anim.faceTo = "Left";
            /*if (inputMovement.y > 0)
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
            }*/

            prevX = Mathf.FloorToInt(rB.position.x / mapk + 0.5f);
            prevY = Mathf.FloorToInt(rB.position.y / mapk + 0.5f);
            MapManager.map[prevX, prevY].hasPlayer = false;
            //Debug.Log(MapManager.map[prevX, prevY].hasPlayer);
            rB.MovePosition(rB.position + moveVelocity * Time.deltaTime);
            curX = Mathf.FloorToInt(rB.position.x / mapk + 0.5f);
            curY = Mathf.FloorToInt(rB.position.y / mapk + 0.5f);
            MapManager.map[curX, curY].hasPlayer = true;
            //Debug.Log(MapManager.map[curX, curY].hasPlayer);
        }
        else
        {
            anim.isMoving = false;
            //anim.curState = "Idle";
            /*anim.SetBool("runFrontA1", false);
            anim.SetBool("runBackA1", false);
            anim.SetBool("runLeftA1", false);
            anim.SetBool("runRightA1", false);*/

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
        if(Input.GetMouseButtonDown(0) && stamina >= staminaPerAttack && anim.isActing == false)
        {
            stamina -= 30;
            StartCoroutine(Attacking());
        }
        /*if (Input.GetMouseButtonDown(0) && anim.isAttacking == false &&  stamina >= staminaPerAttack)
        {
            stamina -= 30;
            StartCoroutine(Attacking());
        }*/

        if(Input.GetMouseButtonDown(1) && anim.isActing == false)
        {
            //defence = 2;
            isDefending = true;
            StartCoroutine(Blocking());
            
            /*int x = Mathf.FloorToInt(rB.position.x / mapk + 0.5f);
            int y = Mathf.FloorToInt(rB.position.y / mapk + 0.5f);

            for (int i = x - 4; i < x + 4; i++)
            {
                for (int j = y - 4; j < y + 4; j++)
                {
                    Debug.Log(MapManager.map[x, y].hasPlayer);
                }
            }*/
        }

        if (Input.GetMouseButton(1))
        {
            anim.isActing = true;
            anim.act = "Blocking";
        }
            
        if(Input.GetMouseButtonUp(1))
        {
            anim.isActing = false;
            isDefending = false;
            /*anim.isBlocking = false;
            anim.curState = "Idle";
            defence = 1;
            anim.animIsBlocked = false;*/
        }

    }

    private IEnumerator Attacking()
    {
        anim.isActing = true;
        anim.act = "Attack_1";
        yield return new WaitForSeconds(0.5f);
        anim.isActing = false;
        /*anim.curAttack = "Attack_1";
        anim.isAttacking = true;
        yield return new WaitForSeconds(0.5f);
        anim.isAttacking = false;*/
    }

    private IEnumerator Blocking()
    {
        anim.isActing = true;
        anim.act = "Block";
        yield return new WaitForSeconds(0.5f);
        anim.act = "Blocking";
        /*anim.isBlocking = true;
        anim.curState = "Block";
        yield return new WaitForSeconds(0.5f);
        anim.curState = "Blocking";*/
    }

    private IEnumerator Hurting()
    {
        anim.isActing = true;
        anim.act = "Hurt";
        yield return new WaitForSeconds(0.5f);
        anim.act = "";
        anim.isActing = false;
        /*anim.curState = "Hurt";
        anim.isHurting = true;
        yield return new WaitForSeconds(0.5f);
        anim.isHurting = false;*/
    }

    private void StatBarChange()
    {
        staminaBar.fillAmount = stamina / maxStamina;
        healthBar.fillAmount = health / maxHealth;
        //Debug.Log(staminaBar.fillAmount);
    }

    private void StaminaRefresh()
    {
        if(stamina < maxStamina)
        {
            if (anim.curState == "Idle")
                stamina += Time.deltaTime * 20;
            else if (anim.curState == "Run")
                stamina += Time.deltaTime * 10;
        }
       
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
                        //Debug.Log("Тут враг");
                        
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

    public void GetDamage(float damage)
    {
        //Debug.Log("Игрок получил дамаг");
        StartCoroutine(Hurting());
        if(isDefending)
        {
           // Debug.Log("Игрок защищается");
            float staminaSub = damage * 10;
            if(staminaSub > stamina)
            {
                stamina = 0;
                staminaSub -= stamina;
                health -= staminaSub / 10;
            }
            else
                stamina -= staminaSub;
           // Debug.Log(stamina);
        }
        else 
            health -= damage;
    }
}

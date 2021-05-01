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
    public int maxHealth;
    public int maxStamina;
    public int noiseRange;
    public float hitRange;
    public int damage;
    

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
        staminaPerAttack = 30;

        foreach (var hitBox in hitBoxes)
        {
            hitBox.GetComponent<HitBox>().damage = damage;
        }
    }

    public void Update()
    {
        Combat();
        StatBarChange();
        Actions();
        StaminaRefresh();
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
            NoiseFOVCheck(curX, curY);
            

        }
        else
        {
            anim.curState = "Idle";
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
        if (Input.GetMouseButtonDown(0) && anim.isAttacking == false &&  stamina >= staminaPerAttack)
        {
            //Debug.Log("Атакую!");
            //Debug.Log(stamina);
            stamina -= 30;
            StartCoroutine(Attacking());
            //anim.SetTrigger("isAttackA1");
            //MakeDamage.Action(hitPos.position, hitRange, 6, damage, true);


            /*Debug.Log(mapk);
            Debug.Log(transform.position);*/
            //Debug.Log(MapManager.map[(int)transform.position.x, (int)transform.position.y].hasPlayer);
            /*int x = Mathf.FloorToInt(rB.position.x / mapk + 0.5f);
            int y = Mathf.FloorToInt(rB.position.y / mapk + 0.5f);*/
            /*Debug.Log(x);
            Debug.Log(y);*/
            //Debug.Log(MapManager.map[x, y].hasPlayer);

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

    private IEnumerator Attacking()
    {
        anim.curAttack = "Attack_1";
        anim.isAttacking = true;
        yield return new WaitForSeconds(0.5f);
        anim.isAttacking = false;
    }

    private IEnumerator Hurting()
    {
        anim.curState = "Hurt";
        anim.isHurting = true;
        yield return new WaitForSeconds(0.5f);
        anim.isHurting = false;
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
        StartCoroutine(Hurting());
        health -= damage;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    DungeonGenerator DG;
    GameObject gameManager;

    public Image healthBar;
    public Transform attackPos;
    public float attackRange;
    public float defaultSpeed;
    
    public int health = 20;
    public int maxHealth = 20;
    public int moveRange;
    public float startWaitTime;
    public Vector2 currentPlayerPosition;
    public Vector2 target;
    public bool sawPlayer;
    public float startAgroTime;


    private Rigidbody2D rB;
    private Animator anim;
    private Vector2 moveVelocity;
    private Vector2 startPosition;
    
    private int xCord;
    private int yCord;
    private string faceTo;
    private float mapk;
   
    private string enemyName;
    private float currentAgroTime;
    private float speed;
    private string currentAnimation;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager");
        rB = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        DG = gameManager.GetComponent<DungeonGenerator>();
        mapk = DG.mapk;
        enemyName = gameObject.name.Split('(')[0];
        startPosition = transform.position;
        speed = defaultSpeed;
        faceTo = "Right";
    }

    // Update is called once per frame
    void Update()
    {  
        if (sawPlayer)
        {
            target = currentPlayerPosition;
            currentAnimation = "Run";
            speed = defaultSpeed * 2;
            currentAgroTime -= Time.deltaTime;
            /*if(Mathf.Abs(Vector2.Distance(transform.position, currentPlayerPosition)) < 5)
            {
                xPlayerCord = Mathf.FloorToInt(currentPlayerPosition.x / mapk + 0.5f);
                yPlayerCord = Mathf.FloorToInt(currentPlayerPosition.y / mapk + 0.5f);

                Debug.Log(MapManager.map[xPlayerCord, yPlayerCord].hasPlayer);
                if (MapManager.map[xPlayerCord, yPlayerCord].hasPlayer)
                    Attack();
                else
                    sawPlayer = false;
            }*/
        }
        else
        {
            target = startPosition;
            currentAnimation = "Walk";
            speed = defaultSpeed;
        }

        if(currentAgroTime <= 0)
        {
            sawPlayer = false;
        }

        Move(target, speed, currentAnimation);

        /*if (sawPlayer)
        {
            target = currentPlayerPosition;
            Move(target, speed * 5);
            anim.Play("Run" + enemyName);
            //StartCoroutine("LookingPlayer");

            if(Mathf.Abs(Vector2.Distance(transform.position, currentPlayerPosition)) < 5)
            {
                Debug.Log("Враг впритык");
                xPlayerCord = Mathf.FloorToInt(transform.position.x / mapk + 0.5f);
                yPlayerCord = Mathf.FloorToInt(transform.position.y / mapk + 0.5f);
                
                if(MapManager.map[xPlayerCord, yPlayerCord].hasPlayer)
                    Attack();
                else
                {
                    sawPlayer = false;
                    target = startPosition;
                }
                    
            }
        }
        else
            //anim.Play("Idle" + enemyName);
        {
            if (Mathf.Abs(Vector2.Distance(startPosition, transform.position)) > 1)
            {
                Move(startPosition, speed * 5);
                anim.Play("Walk" + enemyName);

            }
            else
                anim.Play("Idle" + enemyName);
        }*/

        
    }

    /*private IEnumerator LookingPlayer()
    {
        Debug.Log("Корутин стартовал");
        yield return new WaitForSeconds(5f);
        //if(savedPlayerPosition == currentPlayerPosition)
            sawPlayer = false;
        else
            //savedPlayerPosition = currentPlayerPosition;
    }*/

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Attack();
        }
    }

    /*private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject hitObj = collision.gameObject;
        
        if(hitObj.tag == "Wall" || hitObj.tag == "Enemy")
        {
            goBackX = !goBackX;
            Flip();
        }
    }*/

    void Flip()
    {
        Vector2 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }

    void Move(Vector2 target, float moveSpeed, string animation)
    {
        //moveVelocity = (target - rB.position) * moveSpeed;
        moveVelocity = (target - rB.position) * moveSpeed;
        if(target.x < transform.position.x && faceTo == "Right")
        {
            faceTo = "Left";
            Flip();
        }
        else if(target.x > transform.position.x && faceTo == "Left")
        {
            faceTo = "Right";
            Flip();
        }

        anim.Play(animation + enemyName);


        rB.MovePosition(rB.position +  moveVelocity * Time.deltaTime);

        xCord = Mathf.FloorToInt(transform.position.x / mapk + 0.5f);
        yCord = Mathf.FloorToInt(transform.position.y / mapk + 0.5f);
        MapManager.map[xCord, yCord].hasEnemy = false;
        MapManager.map[xCord, yCord].enemy = null;
        transform.position = Vector2.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
        xCord = Mathf.FloorToInt(transform.position.x / mapk + 0.5f);
        yCord = Mathf.FloorToInt(transform.position.y / mapk + 0.5f);
        MapManager.map[xCord, yCord].hasEnemy = true;
        MapManager.map[xCord, yCord].enemy = this.gameObject;

    }

    private void Attack()
    {
        Debug.Log("Враг атакует");
    }

    public void SawPlayer(Vector2 playerPos)
    {
        currentPlayerPosition = playerPos;
        currentAgroTime = startAgroTime;
        sawPlayer = true;
        Debug.Log("SawPlayer:" + sawPlayer);
    }

    /*private void NoiseChecking()
    {
        for (int i = -4; i <= 4; i++)
        {
            for (int j = -4; j <= 4; j++)
            {
                int x = Mathf.FloorToInt(transform.position.x / mapk) + i;
                int y = Mathf.FloorToInt(transform.position.y / mapk) + j;
                if(MapManager.map[x , y].hasPlayer)
                {
                    Debug.Log("Игрок тут");
                }
            }
        }
    }*/
}

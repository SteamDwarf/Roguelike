using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    DungeonGenerator DG;
    GameObject gameManager;

    public Image healthBar;
    public float defaultSpeed;
    public int health;
    public int maxHealth;
    public Vector2 currentPlayerPosition;
    public bool sawPlayer;
    public float startAgroTime;
    /*public Transform attackPos;
    public float attackRange;*/

    protected Rigidbody2D rB;
    protected Animator anim;
    protected Vector2 moveVelocity;
    protected Vector2 startPosition;
    protected Vector2 target;

    protected int xCord;
    protected int yCord;
    protected string faceTo;
    protected float mapk;
    protected string enemyName;
    protected float currentAgroTime;
    protected float speed;
    protected string currentAnimation;

    // Start is called before the first frame update
    protected void Start()
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
    protected void Update()
    {
        DefaultBehavior();
        Move(target, speed, currentAnimation);
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

    void Flip()
    {
        Vector2 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }

    protected void Move(Vector2 target, float moveSpeed, string animation)
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

    /*protected void Move()
    {
        //moveVelocity = (target - rB.position) * moveSpeed;
        moveVelocity = (target - rB.position) * speed;
        if (target.x < transform.position.x && faceTo == "Right")
        {
            faceTo = "Left";
            Flip();
        }
        else if (target.x > transform.position.x && faceTo == "Left")
        {
            faceTo = "Right";
            Flip();
        }

        anim.Play(currentAnimation + enemyName);


        rB.MovePosition(rB.position + moveVelocity * Time.deltaTime);

        xCord = Mathf.FloorToInt(transform.position.x / mapk + 0.5f);
        yCord = Mathf.FloorToInt(transform.position.y / mapk + 0.5f);
        MapManager.map[xCord, yCord].hasEnemy = false;
        MapManager.map[xCord, yCord].enemy = null;
        transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
        xCord = Mathf.FloorToInt(transform.position.x / mapk + 0.5f);
        yCord = Mathf.FloorToInt(transform.position.y / mapk + 0.5f);
        MapManager.map[xCord, yCord].hasEnemy = true;
        MapManager.map[xCord, yCord].enemy = this.gameObject;

    }*/

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

    protected void DefaultBehavior()
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

        if (currentAgroTime <= 0)
        {
            sawPlayer = false;
        }
    }

}

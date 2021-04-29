using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    DungeonGenerator DG;
    GameObject gameManager;

    public Image healthBar;
    public Vector2 currentPlayerPosition;
    public List<Transform> attackPoses;
    public List<float> attackRanges;
    public List<int> attackDamages;
    

    public float defaultSpeed;
    public int maxHealth;
    public bool sawPlayer;
    public float startAgroTime;

    protected Rigidbody2D rB;
    protected Animator anim;
    protected Vector2 moveVelocity;
    protected Vector2 startPosition;
    protected Vector2 target;
    protected List<Attack> enemyAttacks;


    protected int xCord;
    protected int yCord;
    protected string faceTo;
    protected float mapk;
    protected string enemyName;
    protected float currentAgroTime;
    protected float speed;
    protected int health;
    protected string currentAnimation;
    protected bool isAttack = false;
    protected float attackTime;

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

        enemyAttacks = new List<Attack>();
        CreateAttacksList();
    }

    // Update is called once per frame
    protected void Update()
    {
        if (!isAttack && attackTime <= 0)
        {
            DefaultBehavior();
            Move(target, speed);
        }
        else
            attackTime -= Time.deltaTime;
            
        //anim.Play(currentAnimation + enemyName);
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

    protected void Move(Vector2 target, float moveSpeed)
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

            if (Vector2.Distance(transform.position, target) < 0.1)
                currentAnimation = "Idle";
        }

        anim.Play(currentAnimation + enemyName);

        if (currentAgroTime <= 0)
        {
            sawPlayer = false;
        }
    }

    private void CreateAttacksList()
    {
        int attacksCount = attackPoses.Count;

        for (int i = 0; i < attacksCount; i++)
        {
            string name = attackPoses[i].name.Substring(0, attackPoses[i].name.Length - 3);
            Vector2 pos = attackPoses[i].position;
            float range = attackRanges[i];
            int damage = attackDamages[i];

            Attack attack = new Attack(name, pos, range, damage);
            enemyAttacks.Add(attack);
        }
    }

    void Flip()
    {
        Vector2 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }

    protected virtual void MakeAttack()
    {
        Debug.Log("Враг атакует");
    }

    public void SawPlayer(Vector2 playerPos)
    {
        currentPlayerPosition = playerPos;
        currentAgroTime = startAgroTime;
        sawPlayer = true;
        //Debug.Log("SawPlayer:" + sawPlayer);
    }

   

}

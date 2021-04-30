using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum EnemyState
{
    idle,
    walk,
    run, 
    attack
}

public class Enemy : MonoBehaviour
{
    DungeonGenerator DG;
    GameObject gameManager;

    public Image healthBar;
    public Vector2 currentPlayerPosition;
    public List<Transform> attackPoses;
    public List<float> attackRanges;
    public List<int> attackDamages;
    public EnemyState currentState;
    

    public float defaultSpeed;
    public int maxHealth;
    public bool sawPlayer;
    public float startAgroTime;
    public int maxStamina;
    public float attackRadius;

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
    protected float stamina;
    protected string currentAnimation;
    protected float attackTime;
    protected string curAttack;

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
        currentState = EnemyState.idle;
        stamina = maxStamina;

        enemyAttacks = new List<Attack>();
        CreateAttacksList();
    }

    // Update is called once per frame
    protected void Update()
    {
        if(currentState != EnemyState.attack)
        {
            DefaultBehavior();
            Move(target, speed);
        }

        attackTime -= Time.deltaTime;
        AnimPlay();
        RefreshStamina();
    }


    protected void Move(Vector2 target, float moveSpeed)
    {
        if(Vector2.Distance(target, transform.position) > attackRadius)
        {
            moveVelocity = (target - rB.position) * moveSpeed;
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

            rB.MovePosition(rB.position + moveVelocity * Time.deltaTime);

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
    }

    protected void DefaultBehavior()
    {
        if (sawPlayer)
        {
            target = currentPlayerPosition;
            currentState = EnemyState.run;
            speed = defaultSpeed * 2;
            currentAgroTime -= Time.deltaTime;
        }
        else
        {
            target = startPosition;
            currentState = EnemyState.walk;
            speed = defaultSpeed;

            if (Vector2.Distance(transform.position, target) < 0.1)
                currentState = EnemyState.idle;
        }

        //anim.Play(currentAnimation + enemyName);

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

    protected void RefreshStamina()
    {
        if (stamina < maxStamina && currentState != EnemyState.attack)
            stamina += 0.1f;
    }

    protected IEnumerator Attacking()
    {
        //Debug.Log("Корутин стартовал");
        currentState = EnemyState.attack;
        yield return new WaitForSeconds(0.5f);
        currentState = EnemyState.idle;
    }

    void Flip()
    {
        Vector2 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }

    protected virtual void MakeAttack()
    {
        //Debug.Log("Враг атакует");
    }

    public void SawPlayer(Vector2 playerPos)
    {
        currentPlayerPosition = playerPos;
        currentAgroTime = startAgroTime;
        sawPlayer = true;
        //Debug.Log("SawPlayer:" + sawPlayer);
    }

    protected void AnimPlay()
    {
        switch(currentState)
        {  
            case EnemyState.idle:
                anim.Play("Idle" + enemyName);
                break;
            case EnemyState.walk:
                anim.Play("Walk" + enemyName);
                break;
            case EnemyState.run:
                anim.Play("Run" + enemyName);
                break;
            case EnemyState.attack:
                anim.Play(curAttack + enemyName);
                //anim.SetTrigger(curAttack);
                break;
        }
    }

}

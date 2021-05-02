using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*public enum EnemyState
{
    idle,
    walk,
    run, 
    attack
}*/

public class Enemy : MonoBehaviour
{
    DungeonGenerator DG;
    GameObject gameManager;
    EnemyAnimator anim;

    public Image healthBar;
    public Vector2 currentPlayerPosition;
    public List<GameObject> attackPoses;
    public List<float> attackRanges;
    public List<int> attackDamages;
    //public EnemyState currentState;


    public float defaultSpeed;
    public int maxHealth;
    public bool sawPlayer;
    public float startAgroTime;
    public int maxStamina;
    public float attackRadius;
    public bool isDied;

    protected Rigidbody2D rB;
    //protected Animator anim;
    protected Vector2 moveVelocity;
    protected Vector2 startPosition;
    protected Vector2 target;
    protected List<Attack> enemyAttacks;


    protected int xCord;
    protected int yCord;
    protected string faceTo;
    protected float mapk;
    //protected string enemyName;
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
        anim = GetComponent<EnemyAnimator>();
        DG = gameManager.GetComponent<DungeonGenerator>();
        //hitBox = gameObject.GetComponent<HitBox>();

        mapk = DG.mapk;
        //enemyName = gameObject.name.Split('(')[0];
        startPosition = transform.position;
        speed = defaultSpeed;
        faceTo = "Right";
        //currentState = EnemyState.idle;
        stamina = maxStamina;
        health = maxHealth;
        isDied = false;

        enemyAttacks = new List<Attack>();
        CreateAttacksList();
    }

    // Update is called once per frame
    protected void Update()
    {
        if (isDied)
            return;

        if (!anim.isAttacking)
        {
            DefaultBehavior();
            Move(target, speed);
        }
        //attackTime -= Time.deltaTime;
        //AnimPlay();
        RefreshStamina();
    }

    private void CreateAttacksList()
    {
        int attacksCount = attackPoses.Count;

        for (int i = 0; i < attacksCount; i++)
        {
            string name = attackPoses[i].name.Substring(0, attackPoses[i].name.Length - 3);
            //Vector2 pos = attackPoses[i].position;
            float range = attackRanges[i];
            int damage = attackDamages[i];

            Attack attack = new Attack(name, range, damage);
            enemyAttacks.Add(attack);
            attackPoses[i].GetComponent<HitBox>().damage = damage;
        }

    }

    protected void Move(Vector2 target, float moveSpeed)
    {
        if (Vector2.Distance(target, transform.position) > attackRadius)
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
            //currentState = EnemyState.run;
            anim.curState = "Run";
            speed = defaultSpeed * 2;
            currentAgroTime -= Time.deltaTime;
        }
        else
        {
            target = startPosition;
            //currentState = EnemyState.walk;
            anim.curState = "Walk";
            speed = defaultSpeed;

            if (Vector2.Distance(transform.position, target) < 0.001)
                anim.curState = "Idle";
            //currentState = EnemyState.idle;
        }

        //anim.Play(currentAnimation + enemyName);

        if (currentAgroTime <= 0)
        {
            sawPlayer = false;
        }
    }



    protected void RefreshStamina()
    {
        if (stamina < maxStamina)
        {
            if (anim.curState == "Idle")
                stamina += Time.deltaTime * 20;
            else if (anim.curState == "Run")
                stamina += Time.deltaTime * 10;
        }
    }

    protected IEnumerator Attacking()
    {
        anim.curAttack = curAttack;
        anim.isAttacking = true;
        yield return new WaitForSeconds(0.5f);
        anim.isAttacking = false;
    }

    protected IEnumerator Hurting()
    {
        anim.curState = "Hurt";
        anim.isHurting = true;
        yield return new WaitForSeconds(0.5f);
        anim.isHurting = false;
    }

    void Flip()
    {
        Vector2 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }

    protected virtual void MakeAttack()
    {
        //Debug.Log("���� �������");
    }

    public void SawPlayer(Vector2 playerPos)
    {
        currentPlayerPosition = playerPos;
        currentAgroTime = startAgroTime;
        sawPlayer = true;
        //Debug.Log("SawPlayer:" + sawPlayer);
    }

    /*protected void AnimPlay()
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
    }*/

    public void GetDamage(int damage)
    {
        if (health > 0)
        {
            StartCoroutine(Hurting());
            health -= damage;
            currentAgroTime = startAgroTime;
        }
        else if (health <= 0)
        {
            anim.curState = "Dying";
            isDied = true;
        }
           
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    DungeonGenerator DG;
    GameObject gameManager;

    public Image healthBar;
    public float speed = 3f;
    public int health = 20;
    public int maxHealth = 20;
    public int moveRange;

    private Rigidbody2D rB;
    private Animator anim;
    private SpriteRenderer spriteRend;
    private Vector2 moveVelocity;
    private List<int> cordIncr;
    private Vector2 movement;
    private Vector2 guardPosition;
    private int xCord;
    private int yCord;
    private float mapk;
    private bool goBackX = false;
    private bool goBackY = false;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager");
        rB = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>();
        guardPosition = transform.position;
        cordIncr = new List<int> { 0, 1, -1 };

        DG = gameManager.GetComponent<DungeonGenerator>();
        mapk = DG.mapk;
    }

    // Update is called once per frame
    void Update()
    {
        //Chill();
    }
    private void FixedUpdate()
    {  
            Chill(); 
    }

    void Chill()
    {
        Vector2 scaler = transform.localScale;

        if (transform.position.x > guardPosition.x + moveRange)
        {
            goBackX = true;
            scaler.x *= -1;
            transform.localScale = scaler;
        }
        else if (transform.position.x < guardPosition.x - moveRange)
        {
            goBackX = false;
            scaler.x *= -1;
            transform.localScale = scaler;
        }

        if (goBackX)
            transform.position = new Vector2(transform.position.x - speed * Time.deltaTime, transform.position.y);
        else
            transform.position = new Vector2(transform.position.x + speed * Time.deltaTime, transform.position.y);

        anim.Play("Troll_1Walk");

        /*movement = new Vector2(cordIncr[Random.Range(0, 2)], cordIncr[Random.Range(0, 2)]);
        moveVelocity = movement * speed;

        xCord = Mathf.FloorToInt(rB.position.x / mapk + 0.5f);
        yCord = Mathf.FloorToInt(rB.position.y / mapk + 0.5f);
        MapManager.map[xCord, yCord].baseObject = null;
        rB.MovePosition(rB.position + moveVelocity * Time.deltaTime);
        xCord = Mathf.FloorToInt(rB.position.x / mapk + 0.5f);
        yCord = Mathf.FloorToInt(rB.position.y / mapk + 0.5f);
        MapManager.map[xCord, yCord].baseObject = this.gameObject;*/
    }
}

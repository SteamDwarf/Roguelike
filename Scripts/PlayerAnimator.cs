using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    Animator curAnimator;
    Player player;
    public GameObject playerFront;
    public GameObject playerBack;
    public GameObject playerLeft;
    public GameObject playerRight;

    Animator playerFrontAnim;
    Animator playerBackAnim;
    Animator playerLeftAnim;
    Animator playerRightAnim;

    public string curState;
    public string faceTo;
    public bool isAttacking;
    public bool isHurting;
    public string curAttack;

    void Start()
    {
        player = GetComponent<Player>();
        /*playerFront = GameObject.Find("Player_Front");
        playerBack = GameObject.Find("Player_Back");
        playerLeft = GameObject.Find("Player_Left");
        playerRight = GameObject.Find("Player_Right");*/

        Debug.Log(playerFront);
        Debug.Log(playerFront.GetComponent<Animator>());

        playerFrontAnim = playerFront.GetComponent<Animator>();
        playerBackAnim = playerBack.GetComponent<Animator>();
        playerLeftAnim = playerLeft.GetComponent<Animator>();
        playerRightAnim = playerRight.GetComponent<Animator>();

        curState = "Idle";
        faceTo = "Front";
        curAnimator = playerFrontAnim;
    }

    private void FixedUpdate()
    {
        ChangeFaceTo();
    }

    private void ChangeFaceTo()
    {   
        switch(faceTo)
        {
            case "Front":
                playerBack.SetActive(false);
                playerLeft.SetActive(false);
                playerRight.SetActive(false);
                playerFront.SetActive(true);
                curAnimator = playerFrontAnim;
                break;
            case "Back":
                playerBack.SetActive(true);
                playerLeft.SetActive(false);
                playerRight.SetActive(false);
                playerFront.SetActive(false);
                curAnimator = playerBackAnim;
                break;
            case "Left":
                playerBack.SetActive(false);
                playerLeft.SetActive(true);
                playerRight.SetActive(false);
                playerFront.SetActive(false);
                curAnimator = playerLeftAnim;
                break;
            case "Right":
                playerBack.SetActive(false);
                playerLeft.SetActive(false);
                playerRight.SetActive(true);
                playerFront.SetActive(false);
                curAnimator = playerRightAnim;
                break;
        }

        AnimationPlay();
        //Debug.Log("player" + curState + faceTo);
    }

    private void AnimationPlay()
    {
        if (isAttacking)
            curAnimator.Play("player" + curAttack + faceTo);
        else if (isHurting)
            curAnimator.Play("player" + "Hurt" + faceTo);
        else
            curAnimator.Play("player" + curState + faceTo);

    }

}

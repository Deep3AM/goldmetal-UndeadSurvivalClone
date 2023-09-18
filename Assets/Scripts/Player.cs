using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public float speed;
    private Vector2 inputVec;
    public Vector2 InputVec {  get { return inputVec; } }
    public Scanner scanner;
    public Hand[] hands;
    public RuntimeAnimatorController[] animCon;
    private Rigidbody2D rigid;
    private SpriteRenderer spriter;
    private Animator anim;
    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        scanner = GetComponent<Scanner>();
        hands = GetComponentsInChildren<Hand>(true);
    }
    private void OnEnable()
    {
        speed *= Character.Speed;
        anim.runtimeAnimatorController = animCon[GameManager.instance.playerId];
    }
    private void FixedUpdate()
    {
        if (!GameManager.instance.isLive)
            return;
        Vector2 nextVec = inputVec.normalized * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);
    }
    private void LateUpdate()
    {
        if (!GameManager.instance.isLive)
            return;
        anim.SetFloat("Speed", inputVec.magnitude);
        if(inputVec.x!=0)
        {
            spriter.flipX=inputVec.x<0?true:false;
        }
    }
    private void OnMove(InputValue value)
    {
        if (!GameManager.instance.isLive)
            return;
        inputVec = value.Get<Vector2>();
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if(!GameManager.instance.isLive)
            return;
        GameManager.instance.health -= Time.deltaTime * 10f;
        if(GameManager.instance.health < 0)
        {
            for(int i =2;i<transform.childCount;i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
            anim.SetTrigger("Dead");
            GameManager.instance.GameOver();
        }
    }
}

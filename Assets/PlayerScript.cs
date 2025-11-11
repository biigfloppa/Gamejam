using Unity.VisualScripting.InputSystem;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;

public class PlayerScript : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] Rigidbody2D rigid = null;
    [SerializeField] PlayerInput input = null;
    [SerializeField] float speed = 10;
    [SerializeField] float jumpForce = 100;
    [SerializeField] LayerMask layerMask = 0;

    [Header("Attack")]
    [SerializeField] GameObject attack = null;
    [SerializeField] Transform bulletOffset = null;
    [SerializeField] BulletScript bullet = null;
    [SerializeField] int attackDamage = 5;
    [SerializeField] float duration = 1;
    [SerializeField] float delay = 1;
    [SerializeField] float bulletSpeed = 1;

    Transform myTransform = null;
    Vector2 moveDir;
    bool isGrounded = true;
    bool isJumping = false;


    bool isAttackable = true;
    void Start()
    {
        myTransform = transform;

        attack.SetActive(false);

        input.SwitchCurrentActionMap("Player");
        input.actions["Move"].started += OnMove;
        input.actions["Move"].performed += OnMove;
        input.actions["Move"].canceled += OnMove;
        input.actions["Attack"].started += OnAttack;
        input.actions["SubAttack"].started += OnSubAttack;
        input.actions["Jump"].started += OnJump;
        //input.actions["Interact"].started += OnInteract;
    }

    void OnMove(InputAction.CallbackContext inp)
    {
        Vector2 v = inp.ReadValue<Vector2>();

        moveDir = Vector2.right * v.x;
    }

    void OnJump(InputAction.CallbackContext inp)
    {
        if (isGrounded)
        {
            rigid.linearVelocity = Vector2.zero;
            rigid.AddForce(Vector2.up * jumpForce);
            StartCoroutine(JumpAble());
        }
    }
    
    void OnAttack(InputAction.CallbackContext inp)
    {
        Transform transform = attack.transform;
        if (isAttackable)
        {
            Vector3 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 lookDir = point - transform.position;
            float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;

            isAttackable = false;
            attack.SetActive(true);
            attack.transform.rotation = Quaternion.Euler(0, 0, angle);
            StartCoroutine(AttackAble());
        }
    }

    void OnSubAttack(InputAction.CallbackContext inp)
    {
        Vector3 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 lookDir = point - transform.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        BulletScript b = Instantiate(bullet, transform.position, Quaternion.Euler(0, 0, angle));
        Rigidbody2D rigid = b.GetComponent<Rigidbody2D>();

        rigid.linearVelocity = bulletSpeed * transform.up;
    }


    void FixedUpdate()
    {
        Vector2 movevelocity = Vector2.zero;
        // 중력 작용
        movevelocity.y = Physics.gravity.y * 2.5f * Time.deltaTime + rigid.linearVelocity.y;
        // 움직임
        rigid.linearVelocity = moveDir * speed + movevelocity;
    }

    private void Update()
    {
        // 아래 방향 레이캐스트(점프 가능 여부 체크)
        RaycastHit2D hit = Physics2D.Raycast(myTransform.position, Vector3.down, 1000, layerMask);
        if (hit)
        {
            if (!isJumping) isGrounded = (hit.distance <= 1.3f);

            /*if (hit.collider.gameObject.CompareTag("MovingPlatform") & hit.distance <= 1.3) transform.SetParent(hit.transform);
            else transform.SetParent(GameManagerScript.Instance.transform);*/
        }
    }

    IEnumerator JumpAble()
    {
        isJumping = true;
        isGrounded = false;
        yield return new WaitForSeconds(0.1f);
        isJumping = false;
    }

    IEnumerator AttackAble()
    {
        if (duration > delay) delay = duration;
        yield return new WaitForSeconds(duration);
        attack.SetActive(false);
        yield return new WaitForSeconds(delay - duration);
        isAttackable = true;
    }

    public int GetDamage()
    {
        return attackDamage;
    }
}

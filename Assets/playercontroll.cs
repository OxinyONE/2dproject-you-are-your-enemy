using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using static System.Collections.Generic.IEnumerable<playercontroll>;
using UnityEngine.UI;
using TMPro;

public class playercontroll : MonoBehaviour
{
    #region Fields
    [Header("Speed & JumpForce")]
    [Range(0,100)]
    public int speed;

    [Range(0,30)]
    public int jumpForce;
    private float horizontal;
    private bool isFacingRight = true;
    private bool isJumping;
    private float coyoteTime = 0.2f;
    private float coyoteTimeCounter;
    private float jumpBufferTime = 0.2f;
    private float jumpBufferCounter;
    [Space(25)]

    public TMPro.TextMeshProUGUI Right;
    public TMPro.TextMeshProUGUI Left;
    [SerializeField] float overlapRadius;
    [SerializeField] Transform groundOverlap;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] Rigidbody2D rb2d;
    
    public List<KeyCode> stringList = new List<KeyCode>();

    #endregion
 
    void Shuffle<T>(List<T> inputList)
    {
        for (int i = 0; i < inputList.Count - 1; i++)
        {
            T temp = inputList[i];
            int rand = Random.Range(i, inputList.Count);
            inputList[i] = inputList[rand];
            inputList[rand] = temp;
        }
    }

    private void Flip() {
        if(isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f) {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    private bool IsGrounded() {
        return Physics2D.OverlapCircle(groundOverlap.position, overlapRadius, groundLayer);
    }

    private void Awake() {
        rb2d = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {

        if (IsGrounded())
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }
        if (Input.GetButtonDown("Jump"))
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        if (coyoteTimeCounter > 0f && jumpBufferCounter > 0f && !isJumping)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, jumpForce);

            jumpBufferCounter = 0f;
        }

        if (Input.GetButtonUp("Jump") && rb2d.velocity.y > 0f)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, rb2d.velocity.y * 0.5f);

            coyoteTimeCounter = 0f;
        }
        Left.text = "left - " + stringList[0];
        Right.text = "Right - " + stringList[1];

        if(Input.GetKeyUp(stringList[0])) {
            Shuffle(stringList);
        }

        if(Input.GetKeyUp(stringList[1])) {
            Shuffle(stringList);
        }

        horizontal = Input.GetAxisRaw("Horizontal");
        if(Input.GetKeyDown(KeyCode.Space) && IsGrounded()) {
            rb2d.velocity = new Vector2(rb2d.velocity.x, jumpForce);
        }

        Flip();

        if(Input.GetKeyDown(KeyCode.R))
        {
            // string stringToRetrieve = GetRandomItem(stringList);
        }           
    }

    private void FixedUpdate() {
        if(Input.GetKey(stringList[0])) {
            rb2d.velocity = new Vector2(-1 * speed, rb2d.velocity.y);
        } else if(Input.GetKey(stringList[1])) {
            rb2d.velocity = new Vector2(1 * speed, rb2d.velocity.y);
        }
    }
}
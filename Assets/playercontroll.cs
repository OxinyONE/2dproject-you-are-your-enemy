using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using static System.Collections.Generic.IEnumerable<playercontroll>;

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
    bool holdingDown;
    [Space(25)]

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

    // public string GetRandomItem(List<string> listToRandomize)
    // {
    //     int randomNum = Random.Range(0, listToRandomize.Count);
    //     print(randomNum);
    //     string printRandom = listToRandomize[randomNum];
    //     print(printRandom);
    //     return printRandom;
    // }

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
        if (Input.anyKey) {
             Debug.Log("A key is being pressed");
             holdingDown = true;
         }
 
         if (!Input.anyKey && holdingDown) {
             Shuffle(stringList);
             holdingDown = false;
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
        if(Input.GetKeyDown(stringList[0])) {
            rb2d.velocity = new Vector2(1 * speed, rb2d.velocity.y);
        } else if(Input.GetKeyDown(stringList[1])) {
            rb2d.velocity = new Vector2(-1 * speed, rb2d.velocity.y);
        }
    }
}
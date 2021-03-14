using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    private Rigidbody2D rd2d;
    public float speed;
    public Text countText;
    public Text winText;
    public Text livesText;

    private int count; 
    private int lives;

    //Audio Stuff
    public AudioClip musicClipOne;
    public AudioClip musicClipTwo;
    public AudioSource musicSource;

    Animator anim;

    private bool facingRight = true;
    private bool isOnGround;
    public Transform groundCheck;
    public float checkRadius;
    public float jumpForce;
    public LayerMask allGround;

    public Text hozText;
    public Text jumpText;

    // Start is called before the first frame update
    void Start()
    {
        rd2d = GetComponent<Rigidbody2D>();
        count = 0;
        lives = 3;
        winText.text = "";
        SetCountAndLivesText();
        musicSource.clip = musicClipOne;
        musicSource.Play();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float hozMovement = Input.GetAxis("Horizontal");
        float vertMovement = Input.GetAxis("Vertical");
        rd2d.AddForce(new Vector2(hozMovement * speed, vertMovement * speed));

        isOnGround = Physics2D.OverlapCircle(groundCheck.position, checkRadius, allGround);

        
        //Facing The Correct Direction
        if (facingRight == false && hozMovement > 0)
        {
            
            Flip();
        }

        else if (facingRight == true && hozMovement < 0)
        {
            
            Flip();
        }

        if(hozMovement > 0 && facingRight == true)
        {
           
            hozText.text = "Facing Right";
            
        }

        if(hozMovement < 0 && facingRight == false)
        {
            
            hozText.text = "Facing Left";
            
        }

        if(vertMovement > 0 && isOnGround == false)
        {
            
            jumpText.text = "Jumping";
           
        }

        else if (vertMovement == 0 && isOnGround == true)
        {
            
            jumpText.text = "Not Jumping";
            
        }

       
    }
    void Update()
    {
        if (isOnGround == true)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                anim.SetInteger("State", 2);
            }
            if (Input.GetKeyUp(KeyCode.W))
            {
                anim.SetInteger("State", 0);
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                anim.SetInteger("State", 1);
            }
            if (Input.GetKeyUp(KeyCode.D))
            {
                anim.SetInteger("State", 0);
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                anim.SetInteger("State", 1);
            }

        }
        if (isOnGround == false)
        {
            anim.SetInteger("State", 2);

        }
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Coin")
        {
            count += 1;
            SetCountAndLivesText();
            Destroy(collision.collider.gameObject);

            if (count == 4)
            {
                transform.position = new Vector2(9f, 20.6f);
                lives = 3;
            }
        }

        if (collision.collider.tag == "Enemy")
        {
            lives--;
            SetCountAndLivesText();
            Destroy(collision.collider.gameObject);

        }

    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground" && isOnGround)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                rd2d.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            }
        }
    }


    void SetCountAndLivesText()
    {
        countText.text = "Score: " + count.ToString();
        livesText.text = "Lives: " + lives.ToString();
        
        if (count == 8)
        {
          winText.text = "You Win! Congrats! \n Game created by Patrice Clarke.";
            musicSource.Stop();
          musicSource.clip = musicClipTwo;
            musicSource.Play();
        }

        
        if (lives == 0)
        {
            Destroy(this);
            winText.text = "You lost!";
        }

    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector2 Scaler = transform.localScale;
        Scaler.x = Scaler.x * -1;
        transform.localScale = Scaler;
    }
}

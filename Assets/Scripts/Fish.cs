using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour
{
    // Start is called before the first frame update

    private Rigidbody2D _rb;
    public float speed;
    int angle;
    int maxAngle = 20;
    int minAngle = -60;
    public Score score;
    bool touchedGround;
    public GameManager gameManager;
    public ObstacleSpawner obstacleSpawner;
    public Sprite fishDied;
    SpriteRenderer sp;
    Animator anim;
    [SerializeField] private AudioSource hit, point, swim;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.gravityScale = 0;
        sp = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        FishSwim();

    }
    private void FixedUpdate()
    {
        FishRotation();
    }

    void FishSwim()
    {
        if (Input.GetMouseButtonDown(0) && (GameManager.gameOver == false))
        {
            swim.Play();

            if (GameManager.gameStarted == false )
            {
                _rb.gravityScale = 5f;
                _rb.velocity = Vector2.zero;
                _rb.velocity = new Vector2(_rb.velocity.x, speed);
                obstacleSpawner.InstantiateObstacle();
                gameManager.GameHasStarted();
            }
            else
            {
                _rb.velocity = Vector2.zero;
                _rb.velocity = new Vector2(_rb.velocity.x, speed);
            }
        }
    }

    void FishRotation()
    {
        if (_rb.velocity.y > 0)
        {

            if (angle <= maxAngle)
            {
                angle = angle + 4;
            }

        }
        else if (_rb.velocity.y < -2.5f)
        {
            if (angle > minAngle)
            {
                angle = angle - 2;
            }

        }

        if (touchedGround == false)
        {
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("obstacle"))
        {
            //Debug.Log("Scored!..");
            score.Scored();
            point.Play();
        }
        else if (collision.CompareTag("coloumn") && GameManager.gameOver == false)
        {
            gameManager.GameOver();
            FishDieEffect();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("ground"))
        {

            if (GameManager.gameOver == false)
            {
                gameManager.GameOver();
                GameOver();
                FishDieEffect();
            }
        }
    }
    void FishDieEffect()
    {
        hit.Play();
    }

    void GameOver() 
    {
        sp.sprite = fishDied;
        anim.enabled = false;
        touchedGround = true;
        transform.rotation = Quaternion.Euler(0, 0, -90);
    }

}

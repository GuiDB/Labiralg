﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float jumpForce;
    public float jumpDelay;
    public float cronometer;
    public Text scoreText;
    public Text timeText;
    public Text dashText;
    public GameObject endGamePanel;
    public GameObject pausePanel;
    public Maze maze;
    public Zoom zoom;
    public Joystick joystick;
    public Joybutton joybutton;
    public UIEndGameMenu uiEndGameMenu;
    public bool isPlayable;
    
    private Vector3 movement;
    private Vector3 jump;
    private Rigidbody rb;
    private float initialMass;
    private float initialDrag;
    private float initialSpeed;
    private float moveHorizontal;
    private float moveVertical;
    private int dashQuantity;
    private float dashForce;
    private long score;
    private long levelScore;
    private long pickupScore;
    private long portalScore;
    private float timeSurvived;
    private int pickUpsCollected;

    // Inicia os valores das propriedades
    private void Init ()
    {
        jump = new Vector3(0.0f, 1.0f, 0.0f);
        rb = GetComponent<Rigidbody>();
        initialMass = rb.mass;
        initialDrag = rb.drag;
        initialSpeed = speed;
        dashQuantity = 0;
        dashForce = 20f;
        score = 0;
        levelScore = maze.xSize * maze.zSize;
        pickupScore = 50;
        portalScore = 100;
        timeSurvived = 0;
        pickUpsCollected = 0;
        isPlayable = true;
    }

    // Método chamado no primeiro frame que o script é ativo
    private void Start ()
    {
        Init();
    }

    // Método chamado antes de renderizar um frame
    private void Update ()
    {
        // TODO: Debug only, must be removed before release;
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if (isPlayable)
        {
            if (cronometer > 0.0f)
            {
                Countdown();
                UpdateHUD();
            }
            else
            {
                TimesUp();
            }
        }
    }

    // Chamado antes de realizar cálculos de física
    private void FixedUpdate ()
    {
        if (isPlayable)
        {
            Move();
            Dash();
            //Jump();
        }
    }

    public void ButtonPausePressed()
    {
        isPlayable = false;
        pausePanel.SetActive(true);
    }

    public void ButtonReturnPressed()
    {
        isPlayable = true;
        pausePanel.SetActive(false);
    }

    // Chamado quando ocorre uma colisão no objeto que esse script é atribuído
    private void OnTriggerEnter (Collider other)
    {
        if (other.gameObject.CompareTag(Tags.PickUp))
        {
            other.gameObject.SetActive(false);
            dashQuantity += 10;
            pickUpsCollected ++;
            score += pickupScore;
        }
        if (other.gameObject.CompareTag(Tags.Portal))
        {
            cronometer += ((maze.xSize + maze.zSize) / 4);
            score += ((long) (portalScore)) + (maze.xSize * maze.zSize);
            isPlayable = false;
            zoom.ChangeZoom();
        }
        //Destroy(other.gameObject);
    }

    // Movimenta o Jogador
    private void Move ()
    {
        moveHorizontal = Input.GetAxis("Horizontal") + joystick.Horizontal;
        moveVertical = Input.GetAxis("Vertical") + joystick.Vertical;

        //Debug.Log(moveHorizontal);
        //Debug.Log(moveVertical);

        movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        rb.AddForce(movement * speed);
    }

    // Realiza um 'Dash';
    private void Dash ()
    {
        // Realiza um "dash" na direção pressionada,
        // enquanto aumenta a massa e diminui a velocidade,
        // deixando o Player mais lento com o passar do tempo
        if (joybutton.Pressed)
        {
            if (dashQuantity > 0)
            {
                rb.AddForce(movement * speed * dashForce);
                rb.mass = initialMass * dashForce;
                speed = speed / 1.5f;
                dashQuantity--;
            }
        }
        else
        {
            rb.mass = initialMass;
            speed = initialSpeed;
        }
    }

    // Faz o Jogador pular
    private void Jump ()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (transform.position.y < 0.75 && jumpDelay <= 0)
            {
                rb.AddForce(jump * jumpForce);
                //rb.drag = 1;
                jumpDelay = 1;
            }
            /*
            else
            {
                rb.drag = initialDrag;
            }
            */
        }
        jumpDelay -= Time.deltaTime;
    }

    private void UpdateHUD ()
    {
        scoreText.text = "Pontuação: " + score;
        timeText.text = "Tempo: " + Math.Round(cronometer, 2);
        dashText.text = dashQuantity.ToString();
        timeSurvived += Time.deltaTime;
    }

    private void Countdown()
    {
        cronometer -= Time.deltaTime;
    }

    private void TimesUp()
    {
        cronometer = 0.0f;
        isPlayable = false;
        endGamePanel.SetActive(true);
        uiEndGameMenu.textTime.text = "Você sobreviveu por " + Math.Round(timeSurvived, 2) + "s";
        uiEndGameMenu.textItems.text = "Itens coletados: " + pickUpsCollected;
        uiEndGameMenu.textMazes.text = "Labirintos concluídos: " + (maze.xSize - 5);
        uiEndGameMenu.textTotalScore.text = "Pontuação total: " + score;

        GPGS.PostToLeaderboard(score);
    }
}
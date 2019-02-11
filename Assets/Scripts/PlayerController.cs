﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public Text scoreText;
    public Text winText;

    private Rigidbody rb;
    private int score;

    // Chamado no primeiro frame que o script é ativo
    void Start ()
    {
        rb = GetComponent<Rigidbody>();
        score = 0;
        SetScoreText();
        winText.text = "";
    }

    // Chamado antes de renderizar um frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.R)) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    // Chamado antes de realizar cálculos de física
    void FixedUpdate ()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

	    Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);

        rb.AddForce (movement * speed);

        if (Input.GetKey(KeyCode.LeftShift)) {
            rb.AddForce(movement * speed * 10f);
            rb.mass = 50f;
        } else {
            rb.mass = 1f;
        }
    }

    // Chamado quando ocorre uma colisão no objeto que esse script é atribuído
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Pick Up"))
        {
            other.gameObject.SetActive (false);
            score++;
            SetScoreText();
        }
        //Destroy(other.gameObject);
    }

    void SetScoreText()
    {
        scoreText.text = "Score: " + score.ToString();
        if (score >= 8)
        {
            winText.text = "You Win!";
        }
    }
}
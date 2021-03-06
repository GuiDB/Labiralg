﻿using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;

    private Vector3 offset;

    // Start is called before the first frame update
    private void Start ()
    {
        offset = transform.position - player.transform.position;
    }

    // Chamado antes de renderizar um frame, mas após processamento
    private void LateUpdate ()
    {
        transform.position = player.transform.position + offset;
    }
}

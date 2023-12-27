using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class GameManagerB : MonoBehaviour
{
    public static GameManagerB instance;
    public int vidas = 2;
    public int tijolosRestantes;

    public GameObject playerPrefab;
    public GameObject ballPrefab;

    public Transform playerSpawnPoint;
    public Transform ballSpawnPoint;

    public PlayerB PlayerAtual;
    public BallB BallAtual;

    public TextMeshProUGUI contador;
    public TextMeshProUGUI msgFinal;

    public bool segurando;
    private Vector3 offset;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        SpawnarNovoJogador();
        AtualizarContador();
        tijolosRestantes = GameObject.FindGameObjectsWithTag("Tijolo").Length;
    }

    public void AtualizarContador()
    {
        contador.text = $"Vidas: {vidas}";
    }

    public void SpawnarNovoJogador()
    {
        GameObject PlayerObj = Instantiate(playerPrefab, playerSpawnPoint.position, Quaternion.identity);
        GameObject ballObj = Instantiate(ballPrefab, ballSpawnPoint.position, Quaternion.identity);

        PlayerAtual = PlayerObj.GetComponent<PlayerB>();
        BallAtual = ballObj.GetComponent<BallB>();

        segurando = true;
        offset = PlayerAtual.transform.position - BallAtual.transform.position;

    }
    public void SubtrairTijolo()
    {
        tijolosRestantes--;
        if (tijolosRestantes <= 0)
        {
            Vitoria();
        }
    }

    public void subtrairVida()
    {
        vidas--;
        AtualizarContador();
        Destroy(PlayerAtual.gameObject);
        Destroy(BallAtual.gameObject);
        if (vidas <= 0) 
        {
            GameOver();
        }
       else
        { 
            Invoke(nameof(SpawnarNovoJogador), time: 2);
        }
    }
    public void Vitoria()
    {
        msgFinal.text = "Parabéns!";
        Destroy(BallAtual.gameObject);
        Invoke(nameof(ReiniciarCena), time:2);
    }
    public void GameOver()
    {
        msgFinal.text = "Game Over";

        Invoke(nameof(ReiniciarCena), time: 2);
    }
    public void ReiniciarCena()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void Update()
    {
        if (segurando)
        {
            BallAtual.transform.position = PlayerAtual.transform.position - offset;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                BallAtual.DispararBolinha(PlayerAtual.inputX);
                segurando = false;
            }
        }
    }
}

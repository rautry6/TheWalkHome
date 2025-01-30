using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    public EvilGuySpawning Spawner;
    public PlayerController PlayerController;
    public GameObject GameWinUI;

    public GameObject EatenUI;
    public Image BlackScreen;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EndGame()
    {
        PlayerController.StopMovement();
        Spawner.StopSpawning();
        
    }

    public void StartGame()
    {
        PlayerController.EnableMovement();
        Spawner.EnableSpawning();
    }

    public void Eaten()
    {
        EndGame();

        BlackScreen.DOFade(1, 1).OnComplete(() =>
        {
            EatenUI.gameObject.SetActive(true);
        });
    }

    public void ResetGame()
    {
        SceneManager.LoadScene(0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            EndGame();

            GameWinUI.SetActive(true);
        }
    }
}

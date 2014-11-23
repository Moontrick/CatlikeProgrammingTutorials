using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour 
{


    [SerializeField]
    private Maze mazePrefab;

    [SerializeField]
    Player playerPrefab;

    private Maze mazeInstance;
    private Player playerInstance;



    // ----------------------------------------------------
    // Mono Functions

    private void Start()
    {
        StartCoroutine(BeginGame());
    }


    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
            RestartGame();
    }



    // ----------------------------------------------------
    // Private Functions

    private IEnumerator BeginGame()
    {
        Camera.main.clearFlags = CameraClearFlags.Skybox;
        Camera.main.rect = new Rect(0f, 0f, 1f, 1f);

        mazeInstance = Instantiate(mazePrefab) as Maze;
        yield return StartCoroutine(mazeInstance.Generate());

        playerInstance = Instantiate(playerPrefab) as Player;
        playerInstance.SetLocation(mazeInstance.GetCell(mazeInstance.RandomCoordinates));

        Camera.main.clearFlags = CameraClearFlags.Depth;
        Camera.main.rect = new Rect(0f, 0f, 0.5f, 0.5f);
    }


    private void RestartGame()
    {
        StopCoroutine(mazeInstance.Generate());
        Destroy(mazeInstance.gameObject);
        if (playerInstance != null)
            Destroy(playerInstance.gameObject);

        StartCoroutine(BeginGame());
    }


}

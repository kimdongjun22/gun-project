// ExitGameManager.cs
using UnityEngine;

public class ExitGameManager : MonoBehaviour
{
    public KeyCode exitKey = KeyCode.Escape;

    void Update()
    {
        if (Input.GetKeyDown(exitKey))
        {
            QuitGame();
        }
    }

    public void QuitGame()
    {
        Debug.Log("게임 종료");
        Application.Quit();
    }
}


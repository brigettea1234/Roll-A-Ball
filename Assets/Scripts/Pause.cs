using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    public GameObject pausePanel;
    public bool isPaused = false;

    // Start is called before the first frame update
    private void Start()
    {
       pausePanel.SetActive(false);
       isPaused = false;
       Cursor.visible = false;
    }

    // Update is called once per frame
   private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        if (!isPaused)
        {
            isPaused = true;
            Debug.Log("trying to pause");
            pausePanel.SetActive(true);
            Time.timeScale = 0;
            Cursor.visible = true;
        }
        else
        {
            isPaused = false;
            Debug.Log("trying to unpause");
            pausePanel.SetActive(false);
            Time.timeScale = 1;
            Cursor.visible = false;
        }
    }
}

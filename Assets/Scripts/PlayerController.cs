using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5.0f;
    [HideInInspector]
    public float baseSpeed;
    private Rigidbody rb;
    private int pickupCount;
    private Timer timer;
    private bool gameOver = false;
    CameraController CameraController;
    Vector3 movement;
    bool grounded = true;

    [Header("Respawn")]
    public GameObject resetPoint;
    public GameObject checkPoint1;
    public GameObject checkPoint2;
    public GameObject checkPoint3;
    public GameObject checkPoint4;
    public bool resetting = false;
    public Color originalColour;
    
    [Header("UI")]
    public GameObject inGamePanel;
    public GameObject winPanel;
    public TMP_Text scoreText;
    public TMP_Text timerText;
    public TMP_Text winTimeText;

    // Start is called before the first frame update
    void Start()
    {
        //power up speed
        baseSpeed = speed;
        Time.timeScale = 1;
        rb = GetComponent<Rigidbody>();
        //Get the number of pickups in our scene
        pickupCount = GameObject.FindGameObjectsWithTag("Pick Up").Length;
        //Run the check pickups function
        CheckPickups();
        //Get the timer object and start the timer
        timer = FindObjectOfType<Timer>();
        timer.StartTimer();
        //Turn on our in game panel
        inGamePanel.SetActive(true);
        //Turn off our win panel
        winPanel.SetActive(false);
        originalColour = GetComponent<Renderer>().material.color;

        CameraController = FindObjectOfType<CameraController>();
    }

    void Update()
    {
        timerText.text = "Time: " + timer.GetTime().ToString("F2");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (gameOver == true || resetting)
        {
            return;
        }

        //when grounded is true
        if(grounded)
        {

            //player moves
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");
            movement = new Vector3(moveHorizontal, 0, moveVertical);

        }
        

        
        


        if (CameraController.cameraStyle == CameraStyle.Free)
        {
            //Rotates the player to the direction of the camera
            transform.eulerAngles = Camera.main.transform.eulerAngles;
            //Translates the input vectors into coordinates
            movement = transform.TransformDirection(movement);
        }
        rb.AddForce(movement * speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Pick Up")
        {
            Destroy(other.gameObject);
            //Decrement the pickup count
            pickupCount -= 1;
            //Run the check pickups function
            CheckPickups();
        }
        if (other.tag == "Respawn")
        {
            StartCoroutine(ResetPlayer());
        }

        if (other.gameObject.CompareTag("Powerup"))
        {
            other.GetComponent<Powerup>().UsePowerup();
            other.gameObject.transform.position = Vector3.down * 1000;
        }

        //If player rolls through checkpoint1, they will be transported to checkpoint1 when they die
        if(other.tag == "Checkpoint1")
        {
            other.GetComponent<Particles>().CreateParticles();
            resetPoint = checkPoint1;
           
        }

        if (other.tag == "Checkpoint2")
        {
            other.GetComponent<Particles>().CreateParticles();
            resetPoint = checkPoint2;
            checkPoint1.SetActive(false);       //Checkpoint1 will become inactive when player rolls through next  checkpoint
           
        }

        if (other.tag == "Checkpoint3")
        {
            other.GetComponent<Particles>().CreateParticles();
            resetPoint = checkPoint3;
            checkPoint1.SetActive(false);
            checkPoint2.SetActive(false);
            
        }

        if (other.tag == "Checkpoint4")
        {
            other.GetComponent<Particles>().CreateParticles();
            resetPoint = checkPoint4;
            checkPoint1.SetActive(false);
            checkPoint2.SetActive(false);
            checkPoint3.SetActive(false);
            
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag == "Float")
        {
            transform.parent = collision.transform;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.tag == "Float")
        {
            transform.parent = null;
        }
    }

    public IEnumerator ResetPlayer()
    {
        resetting = true;
        GetComponent<Renderer>().material.color = Color.black;
        rb.velocity = Vector3.zero;
        Vector3 startPos = transform.position;
        float resetSpeed = 2f;
        var i = 0.0f;
        var rate = 1.0f / resetSpeed;
        while (i < 1.0f)
        {
            i += Time.deltaTime * rate;
            transform.position = Vector3.Lerp(startPos, resetPoint.transform.position, i);
            yield return null;
        }
        GetComponent<Renderer>().material.color = originalColour;
        resetting = false;
    }

    void CheckPickups()
    {
        //Display the amount of pickups left in our scene
        scoreText.text = "Pickups Left: " + pickupCount;

        if (pickupCount == 0)
        {
            WinGame();
        }
    }

    void WinGame()
    {
        //Set the game over to true
        gameOver = true;
        //Stop the timer
        timer.StopTimer();
        //Turn on our win panel
        winPanel.SetActive(true);
        //Turn off our in game panel
        inGamePanel.SetActive(false);
        //Display the timer on the win time text
        winTimeText.text = "Your time was: " + timer.GetTime().ToString("F2");

        //Set the velocity of the rigidbody to zero
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    //Temporary - Remove when doing modules in A2
    public void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

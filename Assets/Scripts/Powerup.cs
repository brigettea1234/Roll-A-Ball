using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    //An enum is a dataype that we can sepcify its values and use
    public enum PowerupType {SpeedUp, SpeedDown, Grow, Shrink}

    public PowerupType myPowerup;           //This objects powerup type
    public float powerupDuration = 7f;      //The duration of the powerup
    public float powerupSpeed = 5;          //The speed of the powerup
    PlayerController playerController;      //A reference to our player controller

    void Start()
    {
        //Find and assign the player controller object to this local reference
        playerController = FindObjectOfType<PlayerController>();
    }

    public void UsePowerup()
    {
        //If this powerup is the grow powerup, increase the player controller size by double
        //We also need to move it up on the y axis otherwise it will go through the ground collider
        if (myPowerup == PowerupType.Grow)
        {
            Vector3 temp = playerController.gameObject.transform.position;
            temp.y = 1;
            playerController.gameObject.transform.position = temp;
            playerController.gameObject.transform.localScale = Vector3.one * 2;
        }

        //If this powerup is the shrink powerup, decrease the player size by half
        if (myPowerup == PowerupType.Shrink)
            playerController.gameObject.transform.localScale = Vector3.one / 2;

        //Start a coroutine to reset the powerups effects
        StartCoroutine(ResetPowerup());


        //If this powerup is the speedDown powerup, increase the player controller speed by double
        if (myPowerup == PowerupType.SpeedUp)
            playerController.speed = playerController.baseSpeed * powerupSpeed;

        //If this powerup is the speedDown powerup, decrease the player controller speed times 3
        if (myPowerup == PowerupType.SpeedDown)
            playerController.speed = playerController.baseSpeed / 3;

        //Start a coroutine to reset the powerups effects
        StartCoroutine(ResetPowerup());

        
    }


    IEnumerator ResetPowerup()
    {
        yield return new WaitForSeconds(powerupDuration);


        //If this powerup relates to size, reset our player controller size to 1
        if (myPowerup == PowerupType.Grow || myPowerup == PowerupType.Shrink)
        {
            playerController.gameObject.transform.localScale = Vector3.one;
        }

        //If this powerup relates to speed, reset our player controller speed to its base speed
        if (myPowerup == PowerupType.SpeedUp || myPowerup == PowerupType.SpeedDown)
            playerController.speed = playerController.baseSpeed;


        
    }

}

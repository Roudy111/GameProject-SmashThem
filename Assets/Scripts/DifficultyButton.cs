using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class DifficultyButton : MonoBehaviour
{
    private Button button;

    private GameManager gameManager;     // Importing Game Manger Scripts

    public int difficulty; // the value assigned each difficulty stat that will be passed to startgame Method for controlling game difficulty
 


    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(setDifficulty);

        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void setDifficulty()
    {

        gameManager.startGame(difficulty); // play the game when any Difficulty button has been clicked
    
    }


}


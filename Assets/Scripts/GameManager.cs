using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

/// <summary>
/// The GameManager script manages the core game states, including starting, playing, and ending the game. 
/// It controls the spawning of target objects, updates the player's score, and handles the game over sequence.
/// Manages UI elements
/// Also triggers post-processing effects
/// 
/// 
/// Coroutines:
/// - spawnTarget(): Spawns targets at a regular interval during gameplay.
/// - FadeInVingnette(): Gradually increases the vignette intensity to create a fade-in effect during the game over sequence.
/// 
/// 
/// </summary>

public class GameManager : MonoBehaviour
{
    [Header("Targets To Spawn")]
    public List<GameObject> targets;

    [Space(10)]

    [HideInInspector]
    public bool gameIsOn; // main condition for running game loops within the game -- it should be assigned as condition to all methods within the game play loops

    // UI Elements
    [Header("UI Elements")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI gameOverText;
    public Button restartElement;
    [SerializeField] GameObject startUi;  //The parent object of the Ui elements in startgame menu
    [SerializeField] TextMeshProUGUI healthLives; // the text ui for showing the health status 
    [Space(30)]

    [HideInInspector]
    public int lives;   
    private int score;
    private float spawnRate = 1.0f;

    // Post Processing Effect controller
    [Header("Post Processing")]
    public GameObject gameOverEffects;
    private Vignette vignette;
    //Post Processing Inputs
    private bool hasFadedIn = false;     // Flag for Coroutine Check and also Vig
    [Tooltip("set a fade duration")][SerializeField] float fadeDuration = 1f; // duration of the FadeIn
    [Space(30)]

    // Sensor is an object to turn it off after game over -- prevent the repeating of the game over processs
    [HideInInspector]
    public GameObject sensor;





    [Header("SFXs")]
    //SFXs Inputs to reference when is needed
    public AudioClip loosLiveSfx;
    public AudioSource playerAudio;
    public AudioClip clickSfx;
    public AudioClip scoreLoosSfx;




    //Stats of the Game 

    public void startGame(int difficulty)
    {
        gameIsOn = true;
        updateLives(3);
        spawnRate /= difficulty + 0.2f;
        startUi.SetActive(false); // To hide all start UI elements

        StartCoroutine(spawnTarget());
        score = 0;
        if (gameOverEffects != null)
        {
            Volume volume = gameOverEffects.GetComponent<Volume>();
            if (volume.profile.TryGet(out vignette))
            {
                vignette.intensity.value = 0.0f;

            }
            else
            {
                Debug.Log("Vignette is not found ");

            }

        }
        else
        {
            Debug.LogError("GameOverEffects reference is missing.");
        }
    }
    public void gameOver()
    {
        // To stop the game 
        gameIsOn = false;

        // UI elements Section in GameOver 
        restartElement.gameObject.SetActive(true);
        gameOverText.gameObject.SetActive(true);




        sensor.gameObject.SetActive(false); // to ensure that remaining objects do not colide and repeat the gameover method
                                            // to active Postprocessing

        gameOverEffects.gameObject.SetActive(true);

        if (!hasFadedIn)
        {
            StartCoroutine(FadeInVingnette());
        }



    }
    public void restartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }



    private IEnumerator FadeInVingnette()
    {
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float intensity = Mathf.Lerp(0, 1, elapsed / fadeDuration);
            
            vignette.intensity.value = intensity;
            yield return null;
        }
    }
    

    IEnumerator spawnTarget()
    {
        while (gameIsOn)
        {
            yield return new WaitForSeconds(spawnRate);
            int index = Random.Range(0, targets.Count);
            Instantiate(targets[index]);
        }
        


    }



    /// <summary>
    /// Two times LiveToChange should be assinged --> 
    /// 1.in start for initialization of the method which the total number of the lives player has assignes 
    /// 2.in situations that player loose health - here we need assing a negative and value that player loose 
    /// </summary>
    /// <param name="livesToChange"></param>
    public void updateLives(int livesToChange)
    {
        lives += livesToChange;
        healthLives.text = "lives:" + lives;
        if (lives <= 0)
        {
            gameOver();
        }

    }

    // Functions for score & Live calculation
    public void updateScore(int scoreToAdd)
    {
        score += scoreToAdd;
        scoreText.text = "Score: " + score;


    }
    public void negetiveScore(int scoreTominus)
    {
        score -= scoreTominus;
        scoreText.text = "Score: " + score;
    }

}

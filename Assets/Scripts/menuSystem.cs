using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class menuSystem : MonoBehaviour
{
    public GameObject Canvas;

    public AudioSource touchSound;

    public Color siyah;

    public Button exit, retry, undo, show_squares;
    public Slider percent;

    public GameObject GameoverPanel,Squares;

    public GameObject menuPanel, gamePanel, infoPanel;

    public Text Title;

    public Text generalTitle, generalContent, gameplayTitle, gameplayContent, okContent;

    public Text GameOverTxt, GoUndoTxt, GoRetryTxt, exitTxt, retryTxt, undoTxt, hintTxt, sliderPercentTxt, timerTxt;

    public Button soundOnOff;
    public Sprite soundOn, soundOff;

    public static int sound;
    int counter,bestSkor;

    public static int HintObjects;

    bool condition, gameover, retry_touch, pressBack, pressBackGame;

    public static string undoLastTouch;
    public static int undoLastTouch_sayac;

    float time;

    public static bool Game_over, GameStarted, UndoEdildi;
    
    AsyncOperation scene;

    void Start()
    {
        pressBack = false;
        pressBackGame = false;

        if (PlayerPrefs.HasKey("sound"))
        {
            sound = PlayerPrefs.GetInt("sound");
        }
        else
        {
            sound = 1;
            PlayerPrefs.SetInt("sound", sound);
        }

        if (PlayerPrefs.HasKey("skor"))
        {
            bestSkor = PlayerPrefs.GetInt("skor");
        }
        else
        {
            bestSkor = 1;
            PlayerPrefs.SetInt("skor", bestSkor);
        }

        counter = 0;
        retry_touch = false;
        menuPanel.SetActive(true);
        gamePanel.SetActive(false);
        infoPanel.SetActive(false);
        GameoverPanel.SetActive(false);

        gameover = false;
        GameStarted = false;
        UndoEdildi = false;

        Game_over = false;
        condition = false;
        HintObjects = 0;
        undoLastTouch_sayac = 0;
        time = 0.0f;

        StartCoroutine("SahneyiYukle");
    }

    IEnumerator SahneyiYukle()
    {
        scene = SceneManager.LoadSceneAsync("main");
        scene.allowSceneActivation = false;
        yield return scene;
    }

    void extEsc()
    {
        counter = 0;
        pressBack = false;
    }
    void extEscGame()
    {
        pressBackGame = false;
    }

    void Update ()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        if (Input.GetKeyDown(KeyCode.Escape) && menuPanel.activeInHierarchy && !gamePanel.activeInHierarchy && !infoPanel.activeInHierarchy)
        {
            pressBack = true;
            counter++;
            if (counter == 2)
            {
                Application.Quit();
            }
            Invoke("extEsc", 1.0f);
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && !menuPanel.activeInHierarchy && gamePanel.activeInHierarchy && !pressBackGame)
        {
            // gameObject.GetComponent<ads>().GecisReklami();
            Canvas.GetComponent<Animator>().Play("gameClosing_menuOpening", -1, 0f);
            Squares.GetComponent<Animator>().Play("GameSquaresParent_Close", -1, 0f);
            Squares.GetComponent<gridSystem>().sSpeed(0.15f);
            pressBackGame = true;
            Invoke("extEscGame", 1.0f);
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && menuPanel.activeInHierarchy && !gamePanel.activeInHierarchy && infoPanel.activeInHierarchy && !pressBackGame)
        {
            Canvas.GetComponent<Animator>().Play("menuInfoClosing", -1, 0f);
            pressBackGame = true;
            Invoke("extEscGame", 0.55f);
        }

        if (Squares.transform.position.y == 2.395f && gridSystem.square == 100 && !retry_touch)
        {
            GameStarted = true;     
        }
        else
        {
            GameStarted = false;
        }

        if (sound == 1)
        {
            soundOnOff.image.sprite = soundOn;
        }
        else
        {
            soundOnOff.image.sprite = soundOff;
        }

        if (!pressBack) {
            if (bestSkor == 1 && gridSystem.First_Use == 0)
            {
                Title.fontSize = Screen.width / 4;
                Title.text = "100%";
            }
            else if (gridSystem.First_Use == 1)
            {
                Title.fontSize = Screen.width / 10;
                Title.text = "CURRENT SCORE:\n" + (gridSystem.NumberSayac - 1) + "%";
            }
            else
            {
                Title.fontSize = Screen.width / 9;
                Title.text = "BEST SCORE:\n" + bestSkor + "%";
            }
        }
        else
        {
            Title.fontSize = Screen.width / 12;
            Title.text = "Press Back One More Time To Exit";
        }

        generalTitle.fontSize = Screen.width / 14;
        gameplayTitle.fontSize = Screen.width / 15;

        generalContent.fontSize = Screen.width / 23;
        gameplayContent.fontSize = Screen.width / 25;
        okContent.fontSize = Screen.width / 16;


        // game
        exitTxt.fontSize = Screen.width / 16;
        retryTxt.fontSize = Screen.width / 16;
        undoTxt.fontSize = Screen.width / 16;
        hintTxt.fontSize = Screen.width / 16;

        sliderPercentTxt.fontSize = Screen.width / 15;
        sliderPercentTxt.text = (percent.value).ToString() + "%";

        timerTxt.fontSize = Screen.width / 12;

        GameOverTxt.fontSize = Screen.width / 9;

        GoUndoTxt.fontSize = Screen.width / 15;
        GoRetryTxt.fontSize = Screen.width / 15;
        percent.value = gridSystem.NumberSayac - 1;


        if (!Game_over && GameStarted)
        {
            time += Time.deltaTime;
            timerTxt.text = string.Format("{0:0}:{1:00}", Mathf.Floor(time / 60), time % 60);
            timerTxt.color = siyah;
        }
        else
        {
            timerTxt.color = Color.white;
        }

        if (Game_over && !gameover)
        {
            GameoverPanel.SetActive(true);
            GameoverPanel.GetComponent<Animator>().Play("Gameover_Opening", -1, -0.1f);

            if (gridSystem.NumberSayac - 1 > bestSkor)
            {
                bestSkor = gridSystem.NumberSayac - 1;
                PlayerPrefs.SetInt("skor", bestSkor);
            }

            gameover = true;
        }

        if (GameoverPanel.activeInHierarchy)
        {
            exit.interactable = false;
            retry.interactable = false;
            undo.interactable = false;
            show_squares.interactable = false;
        }
        else
        {
            exit.interactable = true;
            retry.interactable = true;
            undo.interactable = true;
            show_squares.interactable = true;
        }
    }

    public void Gameover_Close()
    {
        GameoverPanel.GetComponent<Animator>().Play("Gameover_Closing", -1, -0.1f);
        Invoke("GoKapat", 0.51f);
    }
    void GoKapat()
    {
        GameoverPanel.SetActive(false);
    }

    public void muteButton()
    {
        if (sound == 1)
        {
            sound = 0;
            PlayerPrefs.SetInt("sound", sound);
        }
        else
        {
            sound = 1;
            PlayerPrefs.SetInt("sound", sound);
        }

    }

    public void retry_button()
    {
        retry_touch = true;

        for (int i = 0; i < Squares.transform.childCount; i++)
        {
            Squares.transform.GetChild(i).GetComponent<Rigidbody>().isKinematic = false;
            Squares.transform.GetChild(i).GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            Squares.transform.GetChild(i).GetComponent<Rigidbody>().AddExplosionForce(25.0f, new Vector3(0, 0, 0), 5.0f);

            Invoke("reStart", 1.5f);
        }
    }
    void reStart()
    {
        scene.allowSceneActivation = true;
    }

    public void undo_button()
    {
        if (gridSystem.First_Use == 1 && undoLastTouch_sayac == 1)
        {
            if (GameObject.Find("Squares/" + undoLastTouch).transform.childCount != 0)
            {
                Destroy(GameObject.Find("Squares/" + undoLastTouch).transform.GetChild(0).gameObject, 0.1f);

                gridSystem.NumberSayac--;
                UndoEdildi = true;

                for (int i = 0; i < GameObject.Find("Squares").transform.childCount; i++)
                {
                    if (GameObject.Find("Squares").transform.GetChild(i).childCount != 0)
                    {
                        if (GameObject.Find("Squares").transform.GetChild(i).GetChild(0).GetComponent<TextMesh>().text == (gridSystem.NumberSayac - 1).ToString())
                        {
                            GameObject.Find("Squares/" + GameObject.Find("Squares").transform.GetChild(i).name).GetComponent<touchSquare>().UndoTarget();
                            Game_over = false;
                        }
                    }
                }

                if (gridSystem.NumberSayac == 1)
                {
                    gridSystem.First_Use = 0;
                }

                undoLastTouch_sayac = 0;
            }
        }
    }
    public void gameover_undo()
    {
        gameover = false;
    }

    public void showSquares_down()
    {
        if (gridSystem.First_Use == 1)
        {
            HintObjects = 1;
            condition = true;
        }
    }

    public void showSquares_up()
    {
        if (gridSystem.First_Use == 1 && condition)
        {
            HintObjects = 2;
            Invoke("Sifirla", 0.35f);
            condition = false;
        }
    }

    void Sifirla()
    {
        HintObjects = 0;
    }

}

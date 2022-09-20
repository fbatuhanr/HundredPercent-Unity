using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class gridSystem : MonoBehaviour {

    public AudioClip touchSound;

    public AnimationClip hintWhite, hintGrey, DestroyWhite, DestroyGrey, gameoverWhite, gameoverGrey;

    public Color gri;

    public static int NumberSayac, First_Use, square;

    public GameObject square_bw;

    float xPosition, yPosition;

    int SquareName, whiteStart;

    float spawnSpeed;

    float positionX, positionY;

    int x,y;
        
    void Start()
    {
        square = 0;
        SquareName = 1;

        spawnSpeed = 0.15f;

        First_Use = 0;

        NumberSayac = 1;

        whiteStart = 1;

        positionX = 0.566f;
        positionY = 0;

        GameObject spawn_square = Instantiate(square_bw, new Vector2(square_bw.transform.position.x, square_bw.transform.position.y), Quaternion.identity);
        spawn_square.transform.parent = GameObject.Find("Squares").transform;
        spawn_square.name = "square_" + SquareName;
        SquareName++;
        square++;

        StartCoroutine(SquareSpawner());
    }

    public void sSpeed(float i)
    {
        spawnSpeed = i;
    }

   IEnumerator SquareSpawner()
    {
        while (square < 100)
        {
            if (square % 10 == 0 && square != 0)
            {
                positionX = 0;
                positionY -= 0.566f;
                x -= 8;
            }
            else
            {
                positionY = 0;
                positionX = 0.566f;
                x = transform.childCount;
            }

            GameObject spawn_square = Instantiate(square_bw, new Vector2(transform.GetChild(x - 1).position.x + positionX, transform.GetChild(x - 1).position.y + positionY), Quaternion.identity);
            spawn_square.transform.parent = GameObject.Find("Squares").transform;
            spawn_square.name = "square_" + SquareName;
           

            if (square % 20 == 0)
            {
                whiteStart = 1;                
            }
            else if (square % 10 == 0)
            {
                whiteStart = 0;
            }

            if (whiteStart == 1)
            {
                if (square % 2 == 0)
                {
                    spawn_square.GetComponent<SpriteRenderer>().color = Color.white;
                    spawn_square.GetComponent<Animation>().AddClip(hintWhite, "hintWhite");
                    spawn_square.GetComponent<Animation>().AddClip(DestroyWhite, "DestroyWhite");
                    spawn_square.GetComponent<Animation>().AddClip(gameoverWhite, "gameoverWhite");
                }
                else
                {
                    spawn_square.GetComponent<SpriteRenderer>().color = gri;
                    spawn_square.GetComponent<Animation>().AddClip(hintGrey, "hintGrey");
                    spawn_square.GetComponent<Animation>().AddClip(DestroyGrey, "DestroyGrey");
                    spawn_square.GetComponent<Animation>().AddClip(gameoverGrey, "gameoverGrey");
                }
            }
            else
            {
                if (square % 2 == 0)
                {
                    spawn_square.GetComponent<SpriteRenderer>().color = gri;
                    spawn_square.GetComponent<Animation>().AddClip(hintGrey, "hintGrey");
                    spawn_square.GetComponent<Animation>().AddClip(DestroyGrey, "DestroyGrey");
                    spawn_square.GetComponent<Animation>().AddClip(gameoverGrey, "gameoverGrey");
                }
                else
                {
                    spawn_square.GetComponent<SpriteRenderer>().color = Color.white;
                    spawn_square.GetComponent<Animation>().AddClip(hintWhite, "hintWhite");
                    spawn_square.GetComponent<Animation>().AddClip(DestroyWhite, "DestroyWhite");
                    spawn_square.GetComponent<Animation>().AddClip(gameoverWhite, "gameoverWhite");
                }
            }

            SquareName += 1;

            yield return new WaitForSeconds(spawnSpeed);

            square++;
        }
    }
}

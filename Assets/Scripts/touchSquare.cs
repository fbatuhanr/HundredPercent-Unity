using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class touchSquare : MonoBehaviour
{
    public Color yellow;
    public GameObject Mesh_NumberTxt, targetObj;

    public Animator SquareHint;

    GameObject[] x;

    public int MeshDeger;

    static string name_num_txt;
    public static int nameNum;

    public bool tangent;

    float z;

    int GameOver_counter;

    bool anims,goAnims;

    void Start()
    {
        anims = false;
        goAnims = false;

        z = 0.5f;
        tangent = false;
        GameOver_counter = 0;
    }

    void Update()
    {
        if (transform.childCount != 0 && menuSystem.GameStarted)
        {
            MeshDeger = int.Parse(transform.GetChild(0).GetComponent<TextMesh>().text);

            if (MeshDeger + 1 == gridSystem.NumberSayac)
            {
                foreach (BoxCollider colls in transform.GetChild(0).GetComponents<Collider>())
                {
                    if (z > 0.5f)
                    {
                        z -= 0.25f;
                    }
                    colls.center = new Vector3(colls.center.x, colls.center.y, z);
                }
            }
            else if (MeshDeger + 3 == gridSystem.NumberSayac)
            {
                foreach (BoxCollider colls in transform.GetChild(0).GetComponents<Collider>())
                {
                    Destroy(colls);
                }

                if (transform.childCount == 2)
                {
                    Destroy(transform.GetChild(1).gameObject);
                }
            }
            else
            {   
                foreach (BoxCollider colls in transform.GetChild(0).GetComponents<Collider>())
                {
                    if (z <= 1)
                    {
                        z += 0.25f;
                    }
                    colls.center = new Vector3(colls.center.x, colls.center.y, z);
                }
                if (transform.childCount == 2)
                {
                    Destroy(transform.GetChild(1).gameObject);
                }
            }      
        }
    }

    void OnTriggerStay(Collider contact)
    {
        if (contact.gameObject.tag == "squareCol" && menuSystem.GameStarted)
        {
            contact.gameObject.GetComponent<touchSquare>().tangent = true;

            if (menuSystem.HintObjects == 1)
            {
                if (contact.transform.childCount == 0)
                {
                    if (contact.gameObject.GetComponent<Animation>().GetClip("hintWhite") != null && !anims)
                    {
                        contact.gameObject.GetComponent<Animation>().Play("hintWhite");            
                    }
                    if (contact.gameObject.GetComponent<Animation>().GetClip("hintGrey") != null && !anims)
                    {
                        contact.gameObject.GetComponent<Animation>().Play("hintGrey");
                    }
                    Invoke("animsTrue", 0.25f);
                }
            }
            else if (menuSystem.HintObjects == 2)
            {
                if (contact.transform.childCount == 0)
                {
                    if (contact.gameObject.GetComponent<Animation>().GetClip("DestroyWhite") != null && anims)
                    {
                        contact.gameObject.GetComponent<Animation>().Play("DestroyWhite");
                    }
                    if (contact.gameObject.GetComponent<Animation>().GetClip("DestroyGrey") != null && anims)
                    {
                        contact.gameObject.GetComponent<Animation>().Play("DestroyGrey");
                    }
                    Invoke("animsFalse", 0.35f);
                }
            }

            if (!goAnims && menuSystem.Game_over && contact.transform.childCount != 0)
            {
                if (contact.gameObject.GetComponent<Animation>().GetClip("gameoverWhite") != null)
                {
                    contact.gameObject.GetComponent<Animation>().Play("gameoverWhite");
                }
                if (contact.gameObject.GetComponent<Animation>().GetClip("gameoverGrey") != null)
                {
                    contact.gameObject.GetComponent<Animation>().Play("gameoverGrey");
                }
                Invoke("goAnimsT", 1.2f);
            }
        }
    }
    void OnTriggerEnter(Collider contact)
    {
        if ((contact.gameObject.tag == "squareCol" || contact.gameObject.tag == "border") && !menuSystem.UndoEdildi && menuSystem.GameStarted)
        {
            foreach (BoxCollider col in contact.GetComponents<Collider>())
            {
                if (contact.transform.childCount != 0)
                {
                    GameOver_counter++;

                    if (GameOver_counter == 8)
                    {
                        menuSystem.Game_over = true;                     
                    }
                }
            }
        }
    }


    void OnTriggerExit(Collider contact)
    {
        if (contact.gameObject.tag == "squareCol" && menuSystem.GameStarted)
        {
            contact.gameObject.GetComponent<touchSquare>().tangent = false;
        }
    }

    void goAnimsT()
    {
        goAnims = true;
    }

    void animsTrue()
    {
        anims = true;
    }
    void animsFalse()
    {
        anims = false;
    }

    void OnMouseDown()
    {
        if ((gridSystem.First_Use == 0 || tangent) && transform.childCount == 0 && menuSystem.GameStarted)
        {
            goAnims = false;

            GameOver_counter = 0;
            menuSystem.UndoEdildi = false;

            if (menuSystem.sound == 1) { 
            transform.parent.GetComponent<AudioSource>().PlayOneShot(transform.parent.GetComponent<gridSystem>().touchSound, 1);
            }

            GameObject number_spawn = Instantiate(Mesh_NumberTxt, new Vector3(0, 0, 0), Quaternion.identity);

            number_spawn.GetComponent<TextMesh>().text = gridSystem.NumberSayac.ToString();

            number_spawn.transform.parent = GameObject.Find("Squares/" + transform.name).transform;

            number_spawn.transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);

            number_spawn.transform.localPosition = new Vector3(0, 0, -0.3f);


            GameObject target = Instantiate(targetObj, new Vector3(0, 0, 0), Quaternion.identity);

            target.transform.parent = GameObject.Find("Squares/" + transform.name).transform;

            target.transform.localScale = new Vector3(1, 1, 1);

            target.transform.localPosition = new Vector3(0, 0, -0.1f);

            gridSystem.NumberSayac++;
            gridSystem.First_Use = 1;

            menuSystem.undoLastTouch = transform.name;
            menuSystem.undoLastTouch_sayac = 1;
        }
    }

   public void UndoTarget()
    {
        GameOver_counter = 0;

        GameObject target = Instantiate(targetObj, new Vector3(0, 0, 0), Quaternion.identity);

        target.transform.parent = GameObject.Find("Squares/" + transform.name).transform;

        target.transform.localScale = new Vector3(1, 1, 1);

        target.transform.localPosition = new Vector3(0, 0, -0.1f);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    //Types of spawnables
    [SerializeField]
    GameObject Cube;
    [SerializeField]
    GameObject Sphere;
    [SerializeField]
    GameObject Capsule;

    //Important level information
    public float gridmax;
    public float level;
    public float instantiatedcollectables;
    public float nextlevelscore;

    //Result Values
    public float numofpushedobjects;
    public float score;
    public float time;

    //Important gameplay information
    public string lastpushedobject;
    private bool countingtime;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this); //Gamemanager to be used on every scene

        //Initialize game variables
        level = 1;
        nextlevelscore = 10;
        gridmax = 10;
        instantiatedcollectables = 0;
        numofpushedobjects = 0;
        score = 0;
        time = 0;
        countingtime = true;

        //Initialize game mechanics
        SpawnCollectable();
        InvokeRepeating("SpawnCube", 0, 10f);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }


    // Update is called once per frame
    void Update()
    {
        if(countingtime)
            time += Time.deltaTime;
    }

    //Scene change initializers depending on level
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(level == 2)
        {
            InvokeRepeating("SpawnCube", 0, 5f);
            SpawnCollectable();
        }
        else if(level == 3)
        {
            InvokeRepeating("SpawnCube", 0, 2f);
            SpawnCollectable();
        }
    }

    //Score calculations are done here and pushed object information is updated here, this method is called from the Collectable class
    public void AddPoints(string name, float points)
    {
        //Score-----------------
        if (name.Equals(lastpushedobject))
            score -= (2 * points);
        else
            score += points;
        //Pushed Object------------
        lastpushedobject = name;
        numofpushedobjects++;
        instantiatedcollectables--;
        SpawnCollectable();

        if(score >= nextlevelscore)
        {
            DealWithChangeLevel();
        }
    }

    //Update variables on level change
    void DealWithChangeLevel()
    {
        if(level == 1)
        {
            level++;
            nextlevelscore = 200;
            gridmax = 8;
            instantiatedcollectables = 0;
            lastpushedobject = "";
            SceneManager.LoadScene(1);
        }
        else if(level == 2)
        {
            level++;
            nextlevelscore = 400;
            gridmax = 5;
            instantiatedcollectables = 0;
            lastpushedobject = "";
            SceneManager.LoadScene(2);
        }

        else if(level == 3)
        {
            EndGame();
        }
    }

    //Method to find a position where an object can be spawnable,
    //this means a position without an object already there or a position inside the square
    Vector3 FindValidPosition()
    {
        //bool foundspot = false;
        Vector3 pos = Vector3.zero;
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Spawnable");
        float cont = 0;
        while (pos.Equals(Vector3.zero) && cont<100)
        {
            Vector3 temppos = new Vector3((int)Random.Range(-gridmax - 1, gridmax + 1), 0f, (int)Random.Range(-gridmax - 1, gridmax + 1));
            pos = temppos;
            for (int i = 0; i < objects.Length; i++)
            {
                if (Vector3.Distance(objects[i].transform.position, temppos) < 0.6f)
                {
                    pos = Vector3.zero;
                    cont++;
                    break;
                }
            }
        }
        if (cont == 100) { Debug.Log("O TABULEIRO TA CHEIO");
            CancelInvoke();
        }
        return pos;
    }

    //Spawn of a cube, this is called every X seconds depending on the Invoke Repeating method
    void SpawnCube()
    {
        Vector3 pos = FindValidPosition();
        Instantiate(Cube, pos, Quaternion.identity);
    }

    //Spawn of collectables, we only spawn collectables if there are less than 3 collectables on the board
    // to avoid board filling with collectables. We also always spawn one of each, so the player doesn't get stuck only being able
    // to catch same types of objects.
    public void SpawnCollectable()
    {
        if (instantiatedcollectables < 3)
        {
            Vector3 spherepos = FindValidPosition() + new Vector3(0,0.5f,0);
            Vector3 capsulepos = FindValidPosition() + new Vector3(0, 0.5f, 0);
            Instantiate(Sphere, spherepos, Quaternion.identity);
            Instantiate(Capsule, capsulepos, Quaternion.identity);
            instantiatedcollectables += 2;
        }
    }

    //Endgame function to be called on each end states
    public void EndGame()
    {
        CancelInvoke();
        countingtime = false;
        level = 4;
        SceneManager.LoadScene(3);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    float point;
    GameManager manager;

    // Start is called before the first frame update
    void Start()
    {
        manager = FindObjectOfType<GameManager>();
        if (gameObject.name.Contains("Sphere"))
        {
            if (manager.level == 1) point = 1;
            if (manager.level == 2) point = 10;
            if (manager.level == 3) point = 20;
        }
        else if (gameObject.name.Contains("Capsule"))
        {
            if (manager.level == 1) point = 2;
            if (manager.level == 2) point = 12;
            if (manager.level == 3) point = 22;
        }
    }

    //Push checker
    public void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.name.Equals("Cylinder")) DealWithPush();
    }

    //Method to deal with what happens when collectable is pushed.
    public void DealWithPush()
    {
        manager.AddPoints(this.gameObject.name, point);
        Destroy(this.gameObject);
    }
}

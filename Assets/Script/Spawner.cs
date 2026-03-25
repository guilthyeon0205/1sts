using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject stone;
    public float cooldown;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        cooldown += Time.deltaTime;
        SpawnStone();
    }

    public void SpawnStone()
    {
        float X = 65f;
        float Z = Random.Range(-50f, 50f);
        Vector3 wichi = new Vector3(X, 0, Z);
        if (cooldown >= 2f)
        {
            Instantiate(stone, wichi, Quaternion.identity);
            cooldown = 0f;


        }
        

    }
}

﻿using UnityEngine;
using System.Collections.Generic;

public class EntityManager : MonoBehaviour
{
    void Awake()
    {
        m_currentWave = 0;  // Sets the first wave to 0
        SpawnNextWave();    // Spawns the first wave of entities
    }

    void Update()
    {
        
        //if (Entities.Count == 1)// && Entities[0].GetComponent<LgEnemy>())// != null)
        //{
        //    if(Entities[0].GetType() == typeof(LgEnemy))
        //        if (Entities[0].GetComponent<LgEnemy>().SmEnemy.Count == 0)
        //            Entities[0].GetComponent<LgEnemy>().CallForHelp();
        //}


        if (Entities.Count > 0 && Reset == false)                                                 // Makes sure that there are still entities alive
        {
            foreach (GameObject e in Entities)                                      // For each entity that we are managing
            {
                if (!e)                 // Sees if the entity has been destroyed
                {
                    Entities.Remove(e);     // Remove them from the list
                    for (int i = 0; i < Entities.Count; i++)
                    {
                        if (Entities[i].GetComponent<LgEnemy>())
                        {
                            if (i == Entities.Count - 1)
                            {
                                Entities[i].GetComponent<LgEnemy>().CallForHelp();
                            }
                            else
                            {
                                continue;
                            }
                        }
                        else
                        {
                            return;                 // and restart the check
                        }
                    }

                }
                else if (e.transform.position.x < ScreenBorders.m_bottomLeft.x - 10 && !e.GetComponent<SmEnemy>())// == null)    // Check to see if they are still on the screen
                {                                                                       // if they're not...
                    Destroy(e);             // Destroy them
                    //Entities.Remove(e);     // Remove them from the list
                    return;                 // and restart the check
                }
            }
        }
        else if(Reset == true)
        {
            foreach(GameObject e in Entities)
            {
                Destroy(e);
            }
            Reset = false;
            SpawnNextWave();
        }
        else if(++m_currentWave < EntityWaves.Count)    // If all the entites are dead
        {                                                   // AND there is a NEXT wave
            SpawnNextWave();                            // spawn the next wave
        }
        else
        {
            GameStates.ChangeState("GameOver");
        }
    }

    void SpawnNextWave()
    {
        GameObject wave = // The current wave we are spawning
            Instantiate(EntityWaves[m_currentWave], transform.position, transform.localRotation) as GameObject;

        Entities = new List<GameObject>();                  // Makes sure the old list of entities are clear
        for(int i = 0; i < wave.transform.childCount; ++i)      // and for every new entity the the new wave
        {
            Entities.Add(wave.transform.GetChild(i).gameObject);    // Add them to the new list of entites
        }

        wave.transform.DetachChildren();    // Unparent them from the wave
        Destroy(wave);                      // and destroy the now empty wave
    }

    public static bool Reset = false;

    public static void ResetWave()
    {
        Reset = true;
        foreach(GameObject g in Entities)
        {
            if(g.GetComponent<LgEnemy>())
            {
                foreach (GameObject go in g.GetComponent<LgEnemy>().SmEnemy)
                    Destroy(go);
            }
        }
    }

    private int m_currentWave;

    public List<GameObject> EntityWaves = new List<GameObject>();
    public static List<GameObject> Entities = new List<GameObject>();
}

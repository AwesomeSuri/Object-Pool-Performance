using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TechniqueObject", menuName = "ScriptableObjects/TechniqueObject")]
public class TechniqueObject : ScriptableObject
{
    public GameObject manager;

    public int duration;
    public int seed;
    public int instances;
    public long date;
    public List<Vector2> performance = new();

    public void ResetPerformance(int newDuration, int newSeed, int newInstances)
    {
        duration = newDuration;
        seed = newSeed;
        instances = newInstances;
        date = DateTime.Now.Ticks;
        performance.Clear();
    }

    public void AddPerformance(float time, float value)
    {
        performance.Add(new Vector2(time, value));
    }
}
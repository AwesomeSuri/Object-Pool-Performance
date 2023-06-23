using UnityEngine;

[CreateAssetMenu(fileName = "TestSettings", menuName = "ScriptableObjects/TestSettings")]
public class TestSettings : ScriptableObject
{
    public int duration = 10;
    public int seed;
    public int instances = 10;
    public TechniqueObject technique;
}

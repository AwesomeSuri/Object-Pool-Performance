using UnityEngine;

[CreateAssetMenu(fileName = "MovementSettings", menuName = "ScriptableObjects/MovementSettings")]
public class MovementSettings : ScriptableObject
{
    public Vector3 bounds = Vector3.one * 10;
    public float acceleration = 10;
    public float threshold = 1;
    public Gradient gradient;
    public float colorSpeed = 1;
}

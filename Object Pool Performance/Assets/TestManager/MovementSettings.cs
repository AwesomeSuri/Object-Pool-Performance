using UnityEngine;

[CreateAssetMenu(fileName = "MovementSettings", menuName = "ScriptableObjects/MovementSettings")]
public class MovementSettings : ScriptableObject
{
    public Vector3 bounds = Vector3.one * 10;
    public float acceleration = 1;
    public float threshold = 1;
    public Gradient gradient;
    public float colorSpeed = 1;
    public int trailCorners = 10;
    public int trailCount = 20;
    public float trailOffset = 1;
    public float trailRotationCount = 2;
    public float trailRotationSpeed = 30;
}

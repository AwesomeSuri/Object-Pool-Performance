using UnityEngine;

public class InstanceMovement : MonoBehaviour
{
    public MovementSettings settings;
    
    private Transform _transform;
    private Renderer _renderer;
    private MaterialPropertyBlock _material;

    private int id;
    private static readonly int Color1 = Shader.PropertyToID("_BaseColor");

    // current state
    private Vector2 velocity;
    private Vector2 target;
    private Vector2 dir;
    private float currentSpeed;

    private void Awake()
    {
        _transform = transform;
        _renderer = GetComponent<Renderer>();
        _material = new MaterialPropertyBlock();
        
        SetTarget();
        _transform.position = target;
        SetTarget();
    }

    public void Setup(int newId)
    {
        id = newId;
    }

    private void Update()
    {
        // movement
        velocity += ((target - (Vector2)_transform.position) * settings.acceleration - velocity)
                    * Time.deltaTime;
        currentSpeed = velocity.magnitude;
        _transform.position += (Vector3)(velocity * Time.deltaTime);
        _transform.up = velocity.normalized;
        
        // size
        _transform.localScale = currentSpeed > 1 ? Vector3.one / currentSpeed : Vector3.one;
        
        // color
        _material.SetColor(Color1, settings.gradient.Evaluate(((id + Time.time) * settings.colorSpeed) % 1f));
        _renderer.SetPropertyBlock(_material);

        if (Vector3.Distance(_transform.position, target) <= settings.threshold)
        {
            SetTarget();
        }
    }

    private void SetTarget()
    {
        target.x = Random.Range(-settings.bounds.x, settings.bounds.x);
        target.y = Random.Range(-settings.bounds.y, settings.bounds.y);
    }
}
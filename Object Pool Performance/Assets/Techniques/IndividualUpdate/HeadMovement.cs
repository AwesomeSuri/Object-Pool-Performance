using UnityEngine;

public class HeadMovement : MonoBehaviour
{
    public MovementSettings settings;
    public GameObject trailPart;

    private Transform _transform;
    private Material _material;

    private int id;

    // current state
    private float maxScale;
    private Vector2 velocity;
    private Vector2 target;
    private Vector2 dir;
    private float currentSpeed;

    public AnimationCurve trackX;
    public AnimationCurve trackY;
    public AnimationCurve trackScale;
    public AnimationCurve trackRot;
    private float nextTrack;


    public void Setup(int newId)
    {
        id = newId;

        // set up variables
        _transform = transform;
        _material = GetComponent<Renderer>().material;
        trackX = new AnimationCurve();
        trackY = new AnimationCurve();
        trackScale = new AnimationCurve();
        trackRot = new AnimationCurve();
        maxScale = _transform.localScale.x;

        // spawn at random pos
        SetTarget();
        _transform.position = target;
        SetTarget();

        // setup tracking
        if (settings.trailCorners < 1) settings.trailCorners = 2;
        if (settings.trailCount < 1) settings.trailCount = 2;
        for (int i = 0; i < settings.trailCorners; i++)
        {
            var time = i / (settings.trailCount - 1f);
            trackX.AddKey(new Keyframe(time, _transform.position.x));
            trackY.AddKey(new Keyframe(time, _transform.position.y));
            trackScale.AddKey(new Keyframe(time, maxScale));
            trackRot.AddKey(new Keyframe(time, 0));
        }

        // setup trail
        for (int i = 0; i < settings.trailCount; i++)
        {
            // create part
            var trail = Instantiate(trailPart).transform;
            trail.name = $"{name} TrailPart {i}";
            trail.position = _transform.position;
            trail.localEulerAngles = Vector3.forward * 360 * (settings.trailRotationCount / (settings.trailCount - 1));
            trail.localScale = _transform.localScale;

            // setup behaviour
            var follower = trail.GetComponent<TrailFollow>();
            follower.Setup(this, (float)i / settings.trailCount);
        }
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
        //_transform.localScale = currentSpeed > 1 ? Vector3.one / currentSpeed : Vector3.one;

        // color
        _material.color = settings.gradient.Evaluate(((id + Time.time) * settings.colorSpeed) % 1f);

        // track attributes
        var pos = _transform.position;
        var scale = _transform.localScale.x;
        var xKeys = trackX.keys;
        var yKeys = trackY.keys;
        var scaleKeys = trackScale.keys;
        var rotKeys = trackRot.keys;
        xKeys[0].value = pos.x;
        yKeys[0].value = pos.y;
        scaleKeys[0].value = scale;
        rotKeys[0].value = _transform.localEulerAngles.z;
        var shift = Time.deltaTime / (settings.trailOffset * (settings.trailCorners - 1));
        for (int i = 1; i < xKeys.Length; i++)
        {
            if (i < xKeys.Length - 1 || Time.time < nextTrack)
            {
                xKeys[i].time += shift;
                yKeys[i].time += shift;
                scaleKeys[i].time += shift;
                rotKeys[i].time += shift;
            }
            else
            {
                nextTrack = Time.time + settings.trailOffset;
                xKeys[i].time = shift;
                yKeys[i].time = shift;
                scaleKeys[i].time = shift;
                rotKeys[i].time = shift;
                xKeys[i].value = pos.x;
                yKeys[i].value = pos.y;
                scaleKeys[i].value = scale;
                rotKeys[i].value = _transform.localEulerAngles.z;
            }
        }

        for (int i = 0; i < xKeys.Length; i++)
        {
            trackX.MoveKey(i, xKeys[i]);
            trackY.MoveKey(i, yKeys[i]);
            trackScale.MoveKey(i, scaleKeys[i]);
            trackRot.MoveKey(i, rotKeys[i]);
        }

        SetCurveLinear(trackX);
        SetCurveLinear(trackY);
        SetCurveLinear(trackScale);
        SetCurveLinear(trackRot);

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


    // https://forum.unity.com/threads/how-to-set-an-animation-curve-to-linear-through-scripting.151683/
    public static void SetCurveLinear(AnimationCurve curve)
    {
        for (int i = 0; i < curve.keys.Length; ++i)
        {
            float intangent = 0;
            float outtangent = 0;
            bool intangent_set = false;
            bool outtangent_set = false;
            Vector2 point1;
            Vector2 point2;
            Vector2 deltapoint;
            Keyframe key = curve[i];

            if (i == 0)
            {
                intangent = 0;
                intangent_set = true;
            }

            if (i == curve.keys.Length - 1)
            {
                outtangent = 0;
                outtangent_set = true;
            }

            if (!intangent_set)
            {
                point1.x = curve.keys[i - 1].time;
                point1.y = curve.keys[i - 1].value;
                point2.x = curve.keys[i].time;
                point2.y = curve.keys[i].value;

                deltapoint = point2 - point1;

                intangent = deltapoint.y / deltapoint.x;
            }

            if (!outtangent_set)
            {
                point1.x = curve.keys[i].time;
                point1.y = curve.keys[i].value;
                point2.x = curve.keys[i + 1].time;
                point2.y = curve.keys[i + 1].value;

                deltapoint = point2 - point1;

                outtangent = deltapoint.y / deltapoint.x;
            }

            key.inTangent = intangent;
            key.outTangent = outtangent;
            curve.MoveKey(i, key);
        }
    }
}
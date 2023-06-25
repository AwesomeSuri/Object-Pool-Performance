using UnityEngine;

public class TrailFollow : MonoBehaviour
{
    private Transform _transform;
    private HeadMovement head;
    private float linearDistance;


    public void Setup(HeadMovement newHead, float dist)
    {
        head = newHead;
        linearDistance = dist;
        _transform = transform;

        // change over dist
        var change = 1 - dist;
        _transform.localScale = newHead.transform.localScale * change;
        GetComponent<Renderer>().material.color = new Color(change, change, change);
    }

    private void Update()
    {
        // pos
        var toPos = _transform.position;
        toPos.x = head.trackX.Evaluate(linearDistance);
        toPos.y = head.trackY.Evaluate(linearDistance);
        _transform.position = toPos;

        // rot
        _transform.localEulerAngles = Vector3.forward * head.trackRot.Evaluate(linearDistance);
    }
}
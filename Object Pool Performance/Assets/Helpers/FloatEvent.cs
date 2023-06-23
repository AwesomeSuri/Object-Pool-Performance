using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "FloatEvent", menuName = "ScriptableObjects/FloatEvent")]
public class FloatEvent : ScriptableObject
{
    private List<UnityAction<float>> subscribers = new();

    public void Subscribe(UnityAction<float> callback)
    {
        subscribers.Add(callback);
    }

    public void Unsubscribe(UnityAction<float> callback)
    {
        subscribers.Remove(callback);
    }

    public void Invoke(float value)
    {
        foreach (var subscriber in subscribers)
        {
            subscriber?.Invoke(value);
        }
    }
}

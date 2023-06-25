using UnityEngine;
using UnityEngine.UIElements;

public class ProgressUI : MonoBehaviour
{
    public FloatEvent progressEvent;
    
    private ProgressBar progressBar;


    private void OnEnable()
    {
        progressBar ??= GetComponent<UIDocument>().rootVisualElement.Q<ProgressBar>();
        progressEvent.Subscribe(SetProgress);
    }

    private void OnDisable()
    {
        progressEvent.Unsubscribe(SetProgress);
    }

    private void SetProgress(float progress)
    {
        progressBar.value = progress;
    }
}

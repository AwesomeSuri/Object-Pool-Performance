using UnityEngine;
using UnityEngine.UIElements;

public class ProgressUI : MonoBehaviour
{
    public FloatEvent progressEvent;
    
    private VisualElement root;
    private VisualElement progressBar;


    private void OnEnable()
    {
        progressBar ??= GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("ProgressBar");
        progressEvent.Subscribe(SetProgress);
    }

    private void OnDisable()
    {
        progressEvent.Unsubscribe(SetProgress);
    }

    private void SetProgress(float progress)
    {
        progressBar.style.width = Length.Percent(progress);
    }
}

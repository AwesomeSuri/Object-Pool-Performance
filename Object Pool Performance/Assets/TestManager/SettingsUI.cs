using UnityEngine;
using UnityEngine.UIElements;

public class SettingsUI : MonoBehaviour
{
    public TestSettings settings;
    
    private IntegerField instances;
    private IntegerField duration;
    private IntegerField seed;

    private bool init;


    private void Init()
    {
        init = true;
        
        var settingsElem = GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("Settings");
        instances = settingsElem.Q<IntegerField>("Instances");
        duration = settingsElem.Q<IntegerField>("Duration");
        seed = settingsElem.Q<IntegerField>("Seed");

        instances.value = settings.instances;
        duration.value = settings.duration;
        seed.value = settings.seed;
    }

    private void OnEnable()
    {
        if (!init) Init();
        
        instances.RegisterValueChangedCallback(SetInstances);
        duration.RegisterValueChangedCallback(SetDuration);
        seed.RegisterValueChangedCallback(SetSeed);
    }

    private void OnDisable()
    {
        instances.UnregisterValueChangedCallback(SetInstances);
    }

    public void SetInstances(ChangeEvent<int> value)
    {
        settings.instances = value.newValue;
    }

    public void SetDuration(ChangeEvent<int> value)
    {
        settings.duration = value.newValue;
    }

    public void SetSeed(ChangeEvent<int> value)
    {
        settings.seed = value.newValue;
    }
}

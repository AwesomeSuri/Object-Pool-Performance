using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestManager : MonoBehaviour
{
    private const int startOffset = 1;
    private const string mainMenu = "MainMenu";

    public FloatEvent progressEvent;
    public TestSettings settings;


    private void Start()
    {
        Invoke(nameof(StartTechnique), startOffset);
    }

    private void StartTechnique()
    {
        Instantiate(settings.technique.manager);
        StartCoroutine(TrackingPerformance());
    }

    private IEnumerator TrackingPerformance()
    {
        var techObj = settings.technique;
        techObj.ResetPerformance(settings.duration, settings.seed, settings.instances);

        var startTime = Time.time;
        var endTime = Time.time + settings.duration;

        do
        {
            yield return null;
            
            var currTime = Time.time - startTime;
            var fps = 1 / Time.deltaTime;
            techObj.AddPerformance(currTime, fps);

            var progress = currTime / settings.duration;
            progressEvent.Invoke(progress);

        } while (Time.time < endTime);

        SceneManager.LoadScene(mainMenu);
    }
}
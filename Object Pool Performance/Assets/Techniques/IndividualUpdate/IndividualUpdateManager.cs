using UnityEngine;
using Random = UnityEngine.Random;

public class IndividualUpdateManager : MonoBehaviour
{
    public GameObject instancePrefab;
    public TestSettings settings;

    private void Start()
    {
        Random.InitState(settings.seed);
        for (int i = 0; i < settings.instances; i++)
        {
            Instantiate(instancePrefab).GetComponent<InstanceMovement>().Setup(i);
        }
    }
}

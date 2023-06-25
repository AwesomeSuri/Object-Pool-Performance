using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenuUI : MonoBehaviour
{
    private class ButtonActionPair
    {
        public Button button;
        public Action action;
    }

    public TechniqueObject[] techniques;
    public TestSettings settings;
    public VisualTreeAsset techniqueUI;

    private ButtonActionPair[] startTestActions;
    private bool initiated;


    private void OnEnable()
    {
        if (!initiated)
        {
            initiated = true;
            startTestActions = new ButtonActionPair[techniques.Length];

            // setup technique UIs ------------------------------------------------------------------------------------/
            var list = GetComponent<UIDocument>().rootVisualElement.Q<ListView>()
                .Q<VisualElement>("unity-content-container");
            for (var i = 0; i < techniques.Length; i++)
            {
                // create new technique UI
                var technique = techniques[i];
                var elem = techniqueUI.Instantiate();
                list.Add(elem);

                // set label
                var main = elem.Q<VisualElement>("Main");
                main.Q<Label>().text = technique.name;

                // setup button
                var action = new Action(() => StartTest(technique));
                var button = main.Q<Button>();
                var pair = new ButtonActionPair()
                {
                    button = button,
                    action = action
                };
                startTestActions[i] = pair;

                // setup information
                var info = elem.Q<VisualElement>("Info");
                info.Q<TextField>("Date").value = new DateTime(technique.date).ToString("yyyy/MM/dd - hh:mm:ss");
                info.Q<TextField>("Instances").value = technique.instances.ToString();
                info.Q<TextField>("Seed").value = technique.seed.ToString();

                // setup diagram ------------------------------------------------------------------
                var perfCount = technique.performance.Count;
                if (perfCount > 0)
                {
                    var diagram = elem.Q<VisualElement>("Diagram");
                    var startTime = technique.performance[0].x;
                    var endTime = technique.performance[perfCount - 1].x;
                    var duration = endTime - startTime;

                    // get important info
                    float minMMS = float.PositiveInfinity, maxMMS = 0;
                    foreach (var mms in technique.performance.Select(perf => perf.y))
                    {
                        minMMS = Mathf.Min(minMMS, mms);
                        maxMMS = Mathf.Max(maxMMS, mms);
                    }

                    var rangeMMS = maxMMS - minMMS;

                    // create points for each performance track ---------------
                    var points = diagram.Q<VisualElement>("Points");
                    var connections = diagram.Q<VisualElement>("Connections");
                    VisualElement lastPoint = null;
                    for (int j = 0; j < perfCount; j++)
                    {
                        var perf = technique.performance[j];

                        // create new point
                        var point = new VisualElement();
                        point.AddToClassList("point");
                        points.Add(point);

                        // set position
                        Vector2 pos;
                        pos.x = 100 * (perf.x - startTime) / duration;
                        point.style.left = Length.Percent(pos.x);
                        pos.y = 100 * (perf.y - minMMS) / rangeMMS;
                        point.style.bottom = Length.Percent(pos.y);

                        if (lastPoint != null)
                        {
                            var pointA = lastPoint;
                            var pointB = point;
                            // wait first until the new point is created
                            point.RegisterCallback<GeometryChangedEvent>(_ =>
                            {
                                var posA = new Vector2(
                                    pointA.resolvedStyle.left,
                                    pointA.resolvedStyle.bottom
                                );
                                var posB = new Vector2(
                                    pointB.resolvedStyle.left,
                                    pointB.resolvedStyle.bottom
                                );

                                // create new connection
                                var connection = new VisualElement();
                                connection.AddToClassList("connection");
                                connections.Add(connection);

                                // position
                                connection.style.left = posA.x;
                                connection.style.bottom = -posA.y;
                                // scale
                                var dir = posB - posA;
                                var distance = dir.magnitude;
                                connection.style.width = distance;
                                // rotation
                                var angle = Vector2.SignedAngle(Vector2.right, dir.normalized);
                                connection.style.rotate =
                                    new StyleRotate(new Rotate(new Angle(angle, AngleUnit.Degree)));
                            });
                        }

                        lastPoint = point;
                    }

                    // setup axes ----------------------------------------------
                    // x axis
                    var axis = diagram.Q<VisualElement>("X-Axis");
                    var children = axis.Children().ToArray();
                    for (int j = 0; j < children.Length; j++)
                    {
                        var time = technique.duration / (children.Length - 1f) * j;
                        children[j].Q<Label>().text = $"{time:F1}s";
                    }

                    // x axis
                    axis = diagram.Q<VisualElement>("Y-Axis");
                    children = axis.Children().ToArray();
                    for (int j = 0; j < children.Length; j++)
                    {
                        var perf = minMMS + rangeMMS / (children.Length - 1f) * j;
                        children[j].Q<Label>().text = perf / 1000 < 1 ? $"{perf:F1}mms" : $"{(perf / 1000):F1}s";
                    }
                }
            }
        }

        foreach (var pair in startTestActions)
        {
            pair.button.clicked += pair.action;
        }
    }

    private void OnDisable()
    {
        foreach (var pair in startTestActions)
        {
            pair.button.clicked -= pair.action;
        }
    }

    private void StartTest(TechniqueObject technique)
    {
        settings.technique = technique;
        SceneManager.LoadScene("TestScene");
    }
}
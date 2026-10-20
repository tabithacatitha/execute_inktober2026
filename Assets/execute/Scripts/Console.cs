using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.UIElements.Experimental;

public class Console : MonoBehaviour
{
    private Text text;
    private string textValue = "";
    private int animatedTextIndex;
    private string[] values = null;
    private float lastUpdateTime = 0;

    public void Initialize()
    {
        Canvas canvas = gameObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        CanvasScaler scaler = gameObject.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        GameObject textChild = new GameObject("Text (Legacy)");
        textChild.transform.parent = this.transform;
        text = textChild.AddComponent<Text>();
        textChild.transform.localPosition = Vector3.zero;
        text.alignment = TextAnchor.MiddleCenter;
        text.color = Color.black;
        text.verticalOverflow = VerticalWrapMode.Overflow;
        text.horizontalOverflow = HorizontalWrapMode.Overflow;
        text.fontSize = 30;
        text.font = Resources.Load<Font>("font"); // OpenSans - Regular 
    }

    public void SetText(string value)
    {
        text.text = value;
        textValue = value;
        animatedTextIndex = -1;
        values = null;
    }

    public void AnimatedText(params string[] values)
    {
        this.values = values;
        animatedTextIndex = 0;
        lastUpdateTime = Time.time;
    }

    public void Clear()
    {
        SetText("");
    }

    void Update()
    {
        if (Time.time > lastUpdateTime + Track.BeatsToSeconds(1))
        {
            if (values != null)
            {
                text.text = values[animatedTextIndex];

                animatedTextIndex++;
                if (animatedTextIndex >= values.Length)
                {
                    animatedTextIndex = 0;
                }
            }

            lastUpdateTime += (float) Track.BeatsToSeconds(1);
        }
    }
}
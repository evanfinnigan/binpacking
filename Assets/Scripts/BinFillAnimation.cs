using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BinFillAnimation : MonoBehaviour {

    public GameObject fillPrefab;
    public Transform fillArea;

    private List<Image> images;

    public Color highlightedColor;
    public Color unHighlightedColor;

    Image bg;

    public void Awake()
    {
        bg = GetComponentInChildren<Image>();
        images = new List<Image>();
    }

    public void AddFill(Color c, float yPos, float height)
    {
        GameObject fill = Instantiate(fillPrefab, fillArea);

        RectTransform t = (RectTransform)fill.transform;
        t.position += new Vector3(0f, yPos, 0f);
        t.sizeDelta = new Vector2(t.sizeDelta.x, height);

        Image img = fill.GetComponent<Image>();
        images.Add(img);

        // update colors for bin
        foreach (var im in images)
        {
            if (im != null)
                im.color = c;
        }
    }

    public void Highlight()
    {
        bg.color = highlightedColor;
    }

    public void UnHighlight()
    {
        bg.color = unHighlightedColor;
    }
}

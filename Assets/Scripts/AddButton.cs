using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddButton : MonoBehaviour {

    public InputField input;
    public BinPackingAnimation binPackingAnimation;
    public Text displayText;

    public void Add()
    {
        float newValue = float.Parse(input.text);
        if (newValue > 0f && newValue <= 1f)
        {
            binPackingAnimation.testValues.Add(newValue);
            binPackingAnimation.UpdateList();
        }
        else
        {
            displayText.text = "Please enter a value in the range 0 < value <= 1";
        }
    }

}

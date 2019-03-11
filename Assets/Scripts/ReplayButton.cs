using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReplayButton : MonoBehaviour {

    public Dropdown dropDown;
    public BinPackingAnimation binPackingAnimation;

    public void OnClick()
    {
        binPackingAnimation.TestAlgorithm(dropDown.value);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeightHandler : MonoBehaviour
{
  public Text heightHolder;

  public void onHeightSliderChange(Slider heightSlider) {
    heightHolder.text = heightSlider.value.ToString();

    SquareHandler.height = (int) heightSlider.value;
  }
}

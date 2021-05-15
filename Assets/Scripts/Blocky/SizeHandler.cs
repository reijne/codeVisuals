using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SizeHandler : MonoBehaviour
{
  public Text SizeHolder;
  public Slider sizeSlider;
  public Slider heightSlider;

  private void Awake() {
    Blocky_s.SIZE = (int) sizeSlider.minValue;
  }

  public void onSizeSliderChange(Slider sizeSlider) {
    if (sizeSlider.value % 2 == 0) sizeSlider.value += 1; 
    SizeHolder.text = sizeSlider.value.ToString();
    
    int radius = (int) (sizeSlider.value - 1) / 2;
    heightSlider.minValue = -radius;
    heightSlider.maxValue = radius;
    heightSlider.value = 0;

    // SquareHandler.size = (int) sizeSlider.value;
    Blocky_s.SIZE = (int) sizeSlider.value;
  }
}

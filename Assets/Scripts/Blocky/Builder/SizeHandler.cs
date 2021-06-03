// UI element to handle the size of the desired blocky.
// Author: Youri Reijne

using UnityEngine;
using UnityEngine.UI;

public class SizeHandler : MonoBehaviour
{
  public Text SizeHolder;
  public Slider sizeSlider;
  public Slider heightSlider;

  /// <summary> Set the size of the blocky to be the minimal value of the slider, such that is is never null. </summary>
  private void Awake() {
    Blocky_s.SIZE = (int) sizeSlider.minValue;
  }

  /// <summary> Update the size text, and the height slider accordingly. </summary>
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

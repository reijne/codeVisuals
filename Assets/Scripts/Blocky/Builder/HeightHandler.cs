// UI element to handle the change of inspected height.
// Author: Youri Reijne

using UnityEngine;
using UnityEngine.UI;

public class HeightHandler : MonoBehaviour
{
  public Text heightHolder;

  /// <summary> Set the text to represent the new value and set the height for the squares. </summary>
  public void onHeightSliderChange(Slider heightSlider) {
    heightHolder.text = heightSlider.value.ToString();

    SquareHandler.height = (int) heightSlider.value;
  }
}

// UI element to set the current selected colour of spawned tiles.
// Author: Youri Reijne

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorButton_s : MonoBehaviour
{
  public List<Button> colorButtons;

  /// <summary> Set the colour of tiles to spawn. </summary>
  public void setColor(string col) {
    SelectSquare_s.currentColor = Colours.StringToColor[col];
    removeOutlines();
  }

  /// <summary> Remove outlines from all the colourbuttons in the interface to only highlight the selected one. </summary>
  private void removeOutlines() {
    foreach (Button cb in colorButtons) {
      cb.GetComponent<Outline>().enabled = false;
    }
  }
}


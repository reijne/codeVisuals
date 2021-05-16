using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorButton_s : MonoBehaviour
{
  public List<Button> colorButtons;
  public void setColor(string col) {
    SelectSquare_s.currentColor = Colours.StringToColor[col];
    removeOutlines();
  }

  private void removeOutlines() {
    foreach (Button cb in colorButtons) {
      cb.GetComponent<Outline>().enabled = false;
    }
  }
}


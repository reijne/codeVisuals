// UI element to select which tiles are spawned in the Blocky definition.
// Author: Youri Reijne

using UnityEngine;
using UnityEngine.UI;

public class SelectSquare_s : MonoBehaviour
{
  public static Color currentColor = Color.white;
  public Vector3Int gridpos;
  public Color desiredColor = Color.gray;
  private SquareHandler squareHandler;

  /// <summary> Initialise the current square to the default color and set the handler. </summary>
  public void init() {
    GetComponent<Image>().color = Color.gray;
    squareHandler = transform.parent.gameObject.GetComponent<SquareHandler>();
  }
  
  /// <summary> Toggle between highlighted colour and off, and handle the corresponding tile in the Blocky.  </summary>
  public void clickSelectSquare() {
    if (!squareHandler) init();
    Color squareColor = GetComponent<Image>().color;

    if (squareColor != Color.gray)  {
      GetComponent<Image>().color = Color.gray;
      desiredColor = Color.gray;
      squareHandler.removeGridPos(gridpos);
    } else {
      if (desiredColor != Color.gray) {
        GetComponent<Image>().color = desiredColor;
      } else {
        GetComponent<Image>().color = currentColor;
      }
      squareHandler.addGridPos(gridpos, GetComponent<Image>().color);
    }
  }
}

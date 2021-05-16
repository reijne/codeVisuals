using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectSquare_s : MonoBehaviour
{
  public static Color currentColor = Color.white;
  public Vector3Int gridpos;
  public Color desiredColor = Color.gray;
  private SquareHandler squareHandler;

  public void init() {
    GetComponent<Image>().color = Color.gray;
    squareHandler = transform.parent.gameObject.GetComponent<SquareHandler>();
  }
  
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectSquare_s : MonoBehaviour
{
  public static Color currentColor = Color.white;
  [SerializeField] private SquareHandler squareHandler;
  public Vector3Int gridpos;
  public Color desiredColor = Color.gray;

  public void init() {
    GetComponent<Image>().color = Color.gray;
    squareHandler = transform.parent.gameObject.GetComponent<SquareHandler>();
  }
  
  public void clickSelectSquare() {
    if (!squareHandler) init();
    Color squareColor = GetComponent<Image>().color;

    if (squareColor != Color.gray)  {
      GetComponent<Image>().color = Color.gray;
      squareHandler.removeGridPos(gridpos);
    } else {
      Debug.Log("clicked a gray square ");
      if (desiredColor != Color.gray) {
        Debug.Log("The squarecolor is not gray ");
        GetComponent<Image>().color = desiredColor;
      } else {
        Debug.Log("The squarecolor is FUCKING gray ");
        GetComponent<Image>().color = currentColor;
      }
      squareHandler.addGridPos(gridpos, GetComponent<Image>().color);
    }
  }
}

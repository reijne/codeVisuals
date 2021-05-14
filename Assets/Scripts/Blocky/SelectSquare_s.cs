using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectSquare_s : MonoBehaviour
{
  public SquareHandler squareHandler;
  public Vector3Int gridpos;

  public void init() {
    squareHandler = transform.parent.gameObject.GetComponent<SquareHandler>();
  }
  
  public void clickSelectSquare() {
    if (!squareHandler) init();
    Color squareColor = GetComponent<Image>().color;
    if (squareColor == Color.white)  {
      GetComponent<Image>().color = Color.gray;
      squareHandler.removeGridPos(gridpos);
    } else {
      GetComponent<Image>().color = Color.white;
      Debug.Log(squareHandler);
      squareHandler.addGridPos(gridpos);
    }
  }

}

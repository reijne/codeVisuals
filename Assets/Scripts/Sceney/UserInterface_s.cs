using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserInterface_s : MonoBehaviour
{
  [SerializeField] Sprite heart;
  [SerializeField] Sprite collectable;
  [SerializeField] Image icon;
  [SerializeField] Text topText;
  [SerializeField] Text midText;
  public void setIcon(string iconName) {
    switch (iconName) {
      case "heart" : icon.sprite = heart; break;
      case "collectable" : icon.sprite = collectable; break;
    }
  }

  public void setCount(int count) {
    topText.text = count.ToString();
    if (count <= 0) midText.text = "The Errors have fixed YOU!";
  }
}

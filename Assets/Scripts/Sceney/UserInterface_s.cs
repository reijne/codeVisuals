using System;
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
  [NonSerialized] public int collected = 0;
  public static int maxCollected = 100;

  public void setIcon(string iconName) {
    switch (iconName) {
      case "heart" : icon.sprite = heart; break;
      case "collectable" : icon.sprite = collectable; break;
    }
  }

  public void setCount(int count) {
    topText.text = count.ToString();
    if (count <= 0 && icon.sprite == heart) midText.text = "The Errors have fixed YOU!";
  }

  public void increaseCollected() {
    collected++;
    setCount(collected);
    if (collected == maxCollected) midText.text = "Winner!\nYou have collected all shiny orbs!";
  }
}

using System;
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
  [NonSerialized] public int collected = 0;
  public static int maxCollected = 100;
  private float removeDelay;

  /// <summary> Set the icon according to the name. </summary>
  public void setIcon(string iconName) {
    switch (iconName) {
      case "heart" : icon.sprite = heart; break;
      case "collectable" : icon.sprite = collectable; break;
    }
    topText.gameObject.SetActive(true);
    icon.gameObject.SetActive(true);
  }

  /// <summary> Set the counter at the top of the screen. Or display a lost message. </summary>
  public void setCount(int count) {
    topText.text = count.ToString();
    if (count <= 0 && icon.sprite == heart) midText.text = "The Errors have fixed YOU!";
  }

  /// <summary> Increase the amount of collectables. </summary>
  public void increaseCollected() {
    collected++;
    setCount(collected);
    if (collected == maxCollected) midText.text = "Winner!\nYou have collected all shiny orbs!";
  }

  /// <summary> Display a message for a specified duration. </summary>
  public void displayMessage(string messagePlusDuration) {
    string[] split = messagePlusDuration.Split('+');
    string msg = split[0];
    float dur = 2;
    if (split.Length > 1) dur = float.Parse(split[1]);
    displayMessage(msg, dur);
  }

  /// <summary> Display a message for a specified duration. </summary>
  public void displayMessage(string msg, float duration) {
    midText.text += msg;
    removeDelay = duration;
    StartCoroutine("removeMessage");
  }

  /// <summary> Remove the displayed message after the duration has passed. </summary>
  IEnumerator removeMessage() {
    yield return new WaitForSeconds(removeDelay);;
    midText.text = "";
  }
}

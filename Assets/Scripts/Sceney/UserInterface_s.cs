using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserInterface_s : MonoBehaviour
{
  [SerializeField] GameObject collectableElement;
  [SerializeField] GameObject heartElement;
  [SerializeField] Text heartText;
  [SerializeField] Text collectableText;
  [SerializeField] Text midText;
  [NonSerialized] public int collected = 0;
  public static int maxCollected = 100;
  private float removeDelay;

  /// <summary> Set the counter at the top of the screen. </summary>
  public void setCount(int count) {
    collectableText.text = count.ToString();
  }

  /// <summary> Set the health at the top of the screen, next to the hearth icon or display loss. </summary>
  public void setHealth(int count) {
    heartText.text = count.ToString();
    if (count <= 0) midText.text = "The Errors have fixed YOU!";
  }

  /// <summary> Enable the heart element, to show health icon and text. </summary>
  public void enableHeartElement() {
    heartElement.SetActive(true);
  }

  /// <summary> Enable the collectable element, to show icon and text. </summary>
  public void enableCollectElement() {
    collectableElement.SetActive(true);
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

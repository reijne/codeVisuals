using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colours
{
  public static Dictionary<Color, string> ColorToString = new Dictionary<Color, string>() {
    {Color.red, "red"},
    {Color.blue, "blue"},
    {Color.cyan, "cyan"},
    {Color.white, "white"},
    {Color.green, "green"},
    {Color.yellow, "yellow"},
    {Color.magenta, "magenta"}
  };
  public static Dictionary<string, Color> StringToColor = new Dictionary<string, Color>() {
    {"red", Color.red},
    {"blue", Color.blue},
    {"cyan", Color.cyan},
    {"white", Color.white},
    {"green", Color.green},
    {"yellow", Color.yellow},
    {"magenta", Color.magenta}
  }; 
}

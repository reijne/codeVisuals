// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class Tile_s : MonoBehaviour 
// {
//   public Vector3Int gridpos {get; set;}
//   public Color color {get; set;}
//   public void makeTile(Vector3Int gridpos, Color color) {
//     this.gridpos = gridpos;
//     this.color = color;
//   }

//   public void makeTileFromString(string posAndColor) {
//     string[] split = posAndColor.Split('|');
//     Debug.Log(split[0]);
//     Debug.Log(split[1]);
//     // (x, y, z | "c")
//   }

//   public string toString() {
//     return "(" + gridpos.x.ToString() + "," + gridpos.y.ToString() + "," + gridpos.z.ToString() 
//           + "|" + Colours.ColorToString[color] + ")";
//   }  
// }

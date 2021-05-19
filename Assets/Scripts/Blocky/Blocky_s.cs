using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// using tile = (Vector3Int position, string color); 


public class Blocky_s : MonoBehaviour
{
  public static int SIZE; // For ease of use all the blockys are the same size i.e. static var
  [SerializeField] GameObject tile_prefab;
  private List<GameObject> tiles = new List<GameObject>();
  public List<(Vector3Int, Color)> tilePosCols = new List<(Vector3Int, Color)>();
  
  public void setTilePositions(List<(Vector3Int, Color)> tilePosCols) {
    this.tilePosCols = tilePosCols;
  }

  public void spawnTiles(bool lookAtMe=false) {
    if (tiles.Count > 0) removeTiles();

    foreach ((Vector3Int tilepos, Color col) in tilePosCols) {
      if (tileOutsideBlock(tilepos)) {
        Debug.LogError("Tile position: " + tilepos + " outside of block with size" + SIZE);
        continue;
      }

      GameObject tile = GameObject.Instantiate(tile_prefab);
      Debug.Log(tile);
      // tile.GetComponent<Tile_s>().gridpos = tilepos;
      // tile.GetComponent<Tile_s>().color = col;
      tile.transform.SetParent(this.transform);
      tile.transform.position = this.transform.position + tilepos;
      tile.GetComponent<Renderer>().material.color = col;
      tiles.Add(tile);

      // if (lookAtMe) {
      //   GameObject cam = GameObject.FindWithTag("MainCamera");
      //   cam.GetComponent<UserMovement>().moveCam(new Vector3(0,5,-10), transform.position);
      // }
    }
  }

  public void removeTiles() {
    foreach (GameObject tile in tiles) {
      GameObject.Destroy(tile);
    }
  }

  public void removeTilePositions() {
    tilePosCols = new List<(Vector3Int, Color)>();
  }

  public void removeTilesOutsideBlock() {
    List<(Vector3Int, Color)> inBounds = new List<(Vector3Int, Color)>();
    foreach ((Vector3Int tilepos, Color col) in tilePosCols) {
      if (tileOutsideBlock(tilepos)) continue;
      inBounds.Add((tilepos, col));
    }
    tilePosCols = inBounds;
    spawnTiles();
  }

  public bool tileOutsideBlock(Vector3Int tilepos) {
    int limit = Mathf.FloorToInt(SIZE / 2);
    return (Mathf.Abs(tilepos.x) > limit || 
            Mathf.Abs(tilepos.y) > limit ||
            Mathf.Abs(tilepos.z) > limit);
  }

  public string toString() {
    string textrep = "[";
    foreach ((Vector3Int tilePos, Color col) in tilePosCols) {
      textrep += "\n\t(" + tilePos.x.ToString() + ", " 
                         + tilePos.y.ToString() + ", " 
                         + tilePos.z.ToString() +  " | " 
                         + Colours.ColorToString[col] + ")"; 
    }
    // Option using Tile_s class and prefab
    // foreach (GameObject tile in tiles) {
    //   textrep += "\n\t" + tile.GetComponent<Tile_s>().toString();
    // }
    return textrep + "\n]";
  }

  public void addTile(Vector3Int tilepos, Color col) {
    if (containsTilepos(tilepos)) return;
    tilePosCols.Add((tilepos, col));
    spawnTiles();
  }

  public void removeTile(Vector3Int gridpos) {
    foreach ((Vector3Int tilepos, Color col) in tilePosCols) {
      if (tilepos == gridpos) {
        tilePosCols.Remove((tilepos, col)); break;
      }
    }
    spawnTiles();
  }

  public bool containsTilepos(Vector3Int gridpos) {
    foreach ((Vector3Int tilepos, _) in tilePosCols) {
      if (tilepos == gridpos) return true;
    }
    return false;
  }

  public Color getColor(Vector3Int gridpos) {
    foreach ((Vector3Int tilepos, Color col) in tilePosCols) {
      if (tilepos == gridpos) return col;
    }
    return Color.gray;
  }
}
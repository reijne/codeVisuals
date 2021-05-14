using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blocky_s : MonoBehaviour
{
  private List<GameObject> tiles = new List<GameObject>();
  private List<Vector3Int> tilePositions;
  
  public void setTilePositions(List<Vector3Int> tilePositions) {
    this.tilePositions = tilePositions;
    spawnTiles();
  }

  void spawnTiles() {
    if (tiles.Count > 0) removeTiles();

    foreach (Vector3Int tilepos in tilePositions) {
      if (tileOutsideBlock(tilepos)) {
        Debug.LogError("Tile position: " + tilepos + " outside of block with size" + Blocky_Spawner_s.BLOCKSIZE);
        continue;
      }
      GameObject tile = GameObject.CreatePrimitive(PrimitiveType.Cube);
      tile.transform.SetParent(this.transform);
      tile.transform.position = this.transform.position + tilepos;
      tiles.Add(tile);
    }
  }

  void removeTiles() {
    foreach (GameObject tile in tiles) {
      GameObject.Destroy(tile);
    }
  }

  bool tileOutsideBlock(Vector3Int tilepos) {
    int limit = Mathf.FloorToInt(Blocky_Spawner_s.BLOCKSIZE / 2);
    return (Mathf.Abs(tilepos.x) > limit || 
            Mathf.Abs(tilepos.y) > limit ||
            Mathf.Abs(tilepos.z) > limit);
  }

}

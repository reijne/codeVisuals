using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blocky_Spawner_s : MonoBehaviour
{
  public static int BLOCKSIZE = 5;
  public GameObject blocky_prefab;
  // Start is called before the first frame update
  void Start() {
    List<Vector3Int> tilepos = new List<Vector3Int>();
    tilepos.Add(new Vector3Int(-2, 0, 0));
    tilepos.Add(new Vector3Int(-1, 0, 0));
    tilepos.Add(new Vector3Int(0, 0, 0));
    tilepos.Add(new Vector3Int(1, 0, 0));
    tilepos.Add(new Vector3Int(2, 0, 0));

    GameObject centre = GameObject.Instantiate(blocky_prefab, new Vector3(0,0,0), Quaternion.identity);
    Blocky_s b = centre.GetComponent<Blocky_s>();
    b.setTilePositions(tilepos);

    List<Vector3Int> tilepos2 = new List<Vector3Int>();
    tilepos2.Add(new Vector3Int(-2, 0, 0));
    tilepos2.Add(new Vector3Int(-1, 0, 0));
    tilepos2.Add(new Vector3Int(0, 0, 0));
    tilepos2.Add(new Vector3Int(0, 0, 1));
    tilepos2.Add(new Vector3Int(1, 1, 1));
    tilepos2.Add(new Vector3Int(2, 1, 1));
    tilepos2.Add(new Vector3Int(0, 0, -1));
    tilepos2.Add(new Vector3Int(1, -1, -1));
    tilepos2.Add(new Vector3Int(2, -1, -1));

    GameObject right = GameObject.Instantiate(blocky_prefab, new Vector3(BLOCKSIZE,0,0), Quaternion.identity);
    Blocky_s b2 = right.GetComponent<Blocky_s>();
    b2.setTilePositions(tilepos2);
  }

  // Update is called once per frame
  void Update() {
    
  }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blocky_Spawner_s : MonoBehaviour
{
  public GameObject blocky_prefab;
  private static int blockyCount = 0;
  // Start is called before the first frame update
  void Start()
  {
    spawnBlocky("number threshold = 4");
    spawnBlocky("boolean isBig = false");
    spawnBlocky("string name = ``Bob``");
    Debug.Log("done");
  }

  private void spawnBlocky(string name) {
    GameObject blocky_obj = Object.Instantiate(blocky_prefab, new Vector3(0, 0, blockyCount*blocky_prefab.transform.localScale.x), Quaternion.identity);
    blocky_obj.GetComponent<Blocky_s>().setName(name);
    blockyCount += 1;
  }

  // Update is called once per frame
  void Update()
  {
    
  }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Formats;
public class Text_Blocky_Spawner_s : MonoBehaviour
{
  public static List<GameObject> blocky_objects = new List<GameObject>();
  public GameObject blocky_prefab;

  void Start()
  {
    // spawnBlocky("number varName = 5");
    // // spawnBlocky("boolean isBig = false");
    // spawnBlocky("string name = ``lol``");
    // spawnBlocky("if (varName == 5):\n\tsomeName=``hahano``");
    Debug.Log("done");
  }

  private void spawnBlocky(string name, string variablesSnapshot) {
    GameObject blocky_obj = Object.Instantiate(blocky_prefab, new Vector3(0, 0, blocky_objects.Count*blocky_prefab.transform.localScale.z), Quaternion.identity);
    blocky_obj.GetComponent<Text_Blocky_s>().setName(name);
    blocky_objects.Add(blocky_obj);
  }

  private void spawnFromStones(MileStones stones) {
    for (int i = 0; i < stones.milestoneList.Count; i++) {
      Stone s = stones.milestoneList[i];
      string variablesSnapshot = "";

      foreach (KeyValuePair<string,string> variableNameState in s.vmap){
        variablesSnapshot += variableNameState.Key + " = " + variableNameState.Value + "\n";
      }

      spawnBlocky(s.snippet, variablesSnapshot);
    }
  }
}

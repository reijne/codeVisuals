using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable] public struct ShoweyVars {
  public string sign;
  public string genDir;
  public string camMode;
  public string camDir;
  public int blockySize;
  public ShoweyVars(string sign, string genDir, string camMode, string camDir, int blockySize) {
    this.sign = sign;
    this.genDir = genDir;
    this.camMode = camMode;
    this.camDir = camDir;
    this.blockySize = blockySize;
  }
}

/// <summary> An entire Showey definition used within unity to map nodes and children to the 3D world. </summary>
public class ShoweyDefinition {
  public ShoweyVars vars;
  public Dictionary<string, List<(Vector3Int, Color)>> blockyMap;
  public Dictionary<string, Category_D> categoryNodeMap = new Dictionary<string, Category_D>();

  
  /// <summary> From a JSON create the showey definition workable within unity. </summary>
  public static ShoweyDefinition fromSerialise(string json) {
    SeriShoweyDefinition serShow = JsonUtility.FromJson<SeriShoweyDefinition>(json);
    ShoweyDefinition showdef = new ShoweyDefinition();
    showdef.vars.sign = serShow.vars.sign;
    showdef.vars.genDir = serShow.vars.genDir;
    showdef.vars.camMode = serShow.vars.camMode;
    showdef.vars.camDir = serShow.vars.camDir;
    showdef.vars.blockySize = serShow.vars.blockySize;

    showdef.blockyMap = makeBlockyMap(serShow.blockyDefinitions);
    showdef.categoryNodeMap = makeCategoryMap(serShow.categoryList);
    return showdef;
  }
  
  /// <summary> Create a mapping from the blocky name to their coloured tiles they 
  private static Dictionary<string, List<(Vector3Int, Color)>> makeBlockyMap(List<BlockyDefinition> blockyDefinitions) {
    Dictionary<string, List<(Vector3Int, Color)>> blockyMap = new Dictionary<string, List<(Vector3Int, Color)>>();
    foreach (BlockyDefinition blockydef in blockyDefinitions) {
      List<(Vector3Int, Color)> tiles = new List<(Vector3Int, Color)>();
      for (int i = 0; i < blockydef.tilePositions.Count; i++) {
        tiles.Add((blockydef.tilePositions[i], blockydef.tileColors[i]));
      }
      blockyMap[blockydef.name] = tiles;
    } 
    return blockyMap;
  }

  /// <summary> Make the dictionary from category names to category object. </summary>
  private static Dictionary<string, Category_D> makeCategoryMap(List<Category> categoryList) {
    Dictionary<string, Category_D> categoryMap = new Dictionary<string, Category_D>();
    foreach (Category cat in categoryList) {
      Category_D newCat = new Category_D();
      newCat.nodes = makeNodeMap(cat.nodes);
      categoryMap[cat.categoryName] = newCat;
    }
    return categoryMap;
  }

  /// <summary> Make the dictionary from node names to node object. </summary>
  private static Dictionary<string, Node_D> makeNodeMap(List<Node> nodes) {
    Dictionary<string, Node_D> nodeMap = new Dictionary<string, Node_D>();
    foreach (Node node in nodes) {
      Node_D newNode = new Node_D();
      newNode.blockyName = node.blockyName;
      newNode.children = makeChildMap(node.children);
      nodeMap[node.nodeName] = newNode;
    }
    return nodeMap;
  }

  /// <summary> Make the dictionary from child name to the child object. </summary>
  private static Dictionary<string, Child> makeChildMap(List<Child> children) {
    Dictionary<string, Child> childMap = new Dictionary<string, Child>();
    foreach (Child child in children) {
      childMap[child.childName] = child;
    }
    return childMap;
  }
}

#region Dictionary definitions used in-Unity for quick lookup O(1)
public class Category_D {
  public Dictionary<string, Node_D> nodes;
}
public class Node_D {
  public string blockyName;
  public Dictionary<string, Child> children;
}
#endregion

#region Serializable definition of every component of Showey
/// <summary> An entire Showey definition contained in a serializable class. </summary>
[Serializable] public class SeriShoweyDefinition {
  public ShoweyVars vars;
  public List<BlockyDefinition> blockyDefinitions;
  public List<Category> categoryList;

  public string serialize() {
    return JsonUtility.ToJson(this);
  }
}

/// <summary> A serializable Blocky definition containing the name, and tile positions with colours. </summary>
[Serializable] public struct BlockyDefinition {
  public string name;
  public List<Vector3Int> tilePositions;
  public List<Color> tileColors;
}

/// <summary> A serializable Category definition, which is the heighest level of the mapping. </summary>
[Serializable] public struct Category {
  public string categoryName;
  public List<Node> nodes;
} 

/// <summary> A serializable Node definition containing name, Blocky to which its supposed to map and its children. </summary>
[Serializable] public struct Node {
  public string nodeName;
  public string blockyName;
  public List<Child> children;
} 

/// <summary> A serializable Child definition with its name, relative direction and offset. </summary>
[Serializable] public struct Child {
  public string childName;
  public string relativeDirection;
  public Vector3Int offset;
} 
#endregion
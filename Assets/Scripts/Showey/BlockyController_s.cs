// Controller for the logic of a Blocky, along with the interface to create one.
// Author: Youri Reijne

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockyController_s : MonoBehaviour
{
  [SerializeField] private GameObject showeyInterface;
  [SerializeField] private GameObject blockyInterface;
  [SerializeField] private Blocky_s showBlocky;
  [SerializeField] private Blocky_s createBlocky;
  [SerializeField] private SquareHandler squareHandler;
  [SerializeField] private InputField createBlockyNameField;
  [SerializeField] private Button sizeButton;
  [SerializeField] private Slider sizeSlider;
  [SerializeField] private Text sizeText;
  [SerializeField] private Button addBlockyButton;
  [SerializeField] private Button removeSelectedButton;
  [SerializeField] public Dropdown blockySelector;
  [SerializeField] private Text blockySelectorLabel;
  [SerializeField] private HeightHandler heightHandler;
  private Vector3 showBlockyStandardPosition;

  public Dictionary<string, List<(Vector3Int, Color)>> blockyMap = new Dictionary<string, List<(Vector3Int, Color)>>();

  /// <summary> Put the visual blocky on the standard position. </summary>
  private void Start() {
    showBlockyStandardPosition = showBlocky.transform.position;
  }
  
  /// <summary> Load in a Map of Blockys using name to list of coloured tiles. </summary>
  public void loadBlockyMap(Dictionary<string, List<(Vector3Int, Color)>> blockyMap) {
    this.blockyMap = blockyMap;
    addOptions();
    selectLastBlocky();
    sizeText.text = Blocky_s.SIZE.ToString();
  }

  #region Button Logic
  /// <summary> Cycle through the possible Blocky sizes.</summary>
  public void cycleBlockySize() {
    sizeText.text = Maps.blockySizeMap[sizeText.text];
    Blocky_s.SIZE = int.Parse(sizeText.text);
  }

  public void changeSize() {
    if (sizeSlider.value % 2 == 0) {
      sizeSlider.value += 1;
    }
    sizeText.text = sizeSlider.value.ToString();
    Blocky_s.SIZE = (int) sizeSlider.value;
  }
  
  /// <summary> Switch to the integrated Blocky builder interface. </summary>
  public void addBlocky() {
    changeInterfaces();
  }

  /// <summary> Remove the currently selected Blocky. </summary>
  public void removeSelected() {
    if (blockySelectorLabel.text == "") return;
    blockyMap.Remove(blockySelector.options[blockySelector.value].text);
    blockySelector.options.RemoveAt(blockySelector.value);
    showBlocky.removeTiles();
    if (blockyMap.Count == 0) {
      sizeSlider.interactable = true;
      blockySelectorLabel.text = "";
    } else {
      blockySelector.value = blockySelector.options.Count - 1; 
      onBlockySelection(blockySelector);
    }
  }

  /// <summary> Save the creation of Blocky Builder with the given name.isValid => return to the Showey Interface. </summary>
  public void saveBlockyReturn() {
    if (createBlockyNameField.text == "") { 
      // No name given for the created blocky
      createBlockyNameField.placeholder.GetComponent<Text>().text = "Name Here... REQUIRED!!!";
    } else if (blockyMap.ContainsKey(createBlockyNameField.text)) {
      // Name is already taken
      createBlockyNameField.text = "";
      createBlockyNameField.placeholder.GetComponent<Text>().text = "Name Already Used!";
    } else if (createBlockyNameField.text.ToLower() == Node_s.skipKeyword) {
      // Name cannot be the skipKeyword
      createBlockyNameField.text = "";
      createBlockyNameField.placeholder.GetComponent<Text>().text = "Name Cannot be "+ Node_s.skipKeyword +" keyword!";
    } else {
      saveNewBlocky(createBlockyNameField.text);
      sizeSlider.interactable = false;
    }
  }
  #endregion

  /// <summary> Select a blocky definition and show it in 3D space. </summary>
  public void onBlockySelection(Dropdown selector) {
    showBlocky.setTilePositions(blockyMap[selector.options[selector.value].text]);
    showBlocky.spawnTiles();
    Vector3 oldpos = showBlocky.transform.position;
    showBlocky.transform.position = showBlockyStandardPosition + new Vector3(0, Blocky_s.SIZE, Blocky_s.SIZE + (Blocky_s.SIZE * 2)); 
  }

  #region Adding a new Blocky
  /// <summary> Add a blocky to the dictionary, clear the BlockyBuilder, return to ShoweyBuilder with the creation selected. </summary>
  private void saveNewBlocky(string blockyName) {
    blockyMap[createBlockyNameField.text] = createBlocky.tilePosCols;
    clearCreateBlocky();
    addOptions();
    changeInterfaces();
    selectLastBlocky();
  }

  /// <summary> Clear the Blocky in the BlockyBuilder interface</summary>
  private void clearCreateBlocky() {
    squareHandler.clearTiles();
  }

  /// <summary> Set the options to include all the created Blockys. </summary>
  private void addOptions() {
    blockySelector.ClearOptions();
    List<string> newOptions = new List<string>();
    foreach (string key in blockyMap.Keys) {
      newOptions.Add(key);
    }
    blockySelector.AddOptions(newOptions);
    if (newOptions.Count > 0) sizeSlider.interactable = false;
  }

  /// <summary> Select the lastly created Blocky. </summary>
  private void selectLastBlocky() {
    blockySelector.value = blockySelector.options.Count - 1;
    onBlockySelection(blockySelector);
  }
  #endregion

  /// <summary> Switch between the interfaces: ShoweyBuilder and BlockyBuilder. </summary>
  private void changeInterfaces() {
    showeyInterface.SetActive(!showeyInterface.activeSelf);
    blockyInterface.SetActive(!blockyInterface.activeSelf);
    squareHandler.spawnSelectSquares();
    heightHandler.updateHeightSlider();
    createBlockyNameField.text = "";
    createBlockyNameField.placeholder.GetComponent<Text>().text = "Blocky Name Here...";
  }

  /// <summary> Contain all the valuable information of the Blockycontroller in a string. </summary> 
  public string toString() {
    string blockyInfo = "blockySize=\""+ Blocky_s.SIZE +"\";\n";
    foreach (string name in blockyMap.Keys) {
      blockyInfo += name + "=[";
      for (int i = 0; i < blockyMap[name].Count; i++) {
        blockyInfo += tileToString(blockyMap[name][i]);
        if (i < blockyMap[name].Count-1) blockyInfo += "+";
      }
      blockyInfo += "]\n";
    }
    return blockyInfo;
  }

  /// <summary> Contain the blocky definition in a string </summary>
  private string tileToString((Vector3Int, Color) tile) {
    Vector3Int pos = tile.Item1;
    Color col = tile.Item2;
    return "(" + pos.x + "," + pos.y + "," + pos.z + "|"  + Colours.ColorToString[col] + ")";
  }
}
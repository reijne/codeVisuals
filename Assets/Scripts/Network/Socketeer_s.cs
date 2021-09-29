using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Net.Sockets;

struct RemoteCall {
    public string method;   // Which method to call
    public string param;    // Parameter to pass to method
    public bool close;      // Whether or not to close the connection after handling call
}

public class Socketeer_s : MonoBehaviour
{
  [SerializeField] CompositeController_s compositeController;
  [SerializeField] SceneController_s sceneController;
  [SerializeField] int port;
  public static bool doSocket = true;
  private static string request = "";
  private static Thread creator_thread;
  private static Thread reader_thread;
  private static Mutex mut = new Mutex();
  IPAddress address = IPAddress.Parse("127.0.0.1");
  TcpListener listener;
  static Stream s;
  static StreamReader sr;
  Socket soc;
  bool isSocketCreated = false;
  bool isReaderAlive = false;
  
  /// <summary> Make a thread to create a socket. </summary>
  void Start() {
    if (doSocket) {
      creator_thread = new Thread(new ThreadStart(createSocket));
      creator_thread.Start();
    }
  }

  /// <summary> Create a tcp listener to accept a connection and create a stream. </summary>
  void createSocket() {
    listener = new TcpListener(address, port);
    listener.Start();
    soc = listener.AcceptSocket(); // blocks
    s = new NetworkStream(soc);
    sr = new StreamReader(s);
    isSocketCreated = true;
  }

  /// <summary> Read from the socket and store the request for handling. </summary>
  private void readSocket() {
    while (doSocket) {
      if (s.CanRead && request == "") {
        string requestLine = sr.ReadLine();
        if (requestLine == null || requestLine == "") continue;
        mut.WaitOne();
        request = requestLine;
        mut.ReleaseMutex();
        Debug.Log("Set the request to " + requestLine);
      }
    }
    s.Close();
    soc.Close();
  }

  /// <summary> Spawn the reader thread that fetches requests from the socket. </summary>
  private void spawnReader() {
    isReaderAlive = true;
    reader_thread = new Thread(new ThreadStart(readSocket));
    reader_thread.Start();
    creator_thread.Abort();
  }

  /// <summary> Once the socket is created spawn the reader thread, and handle the requests filed by it. </summary>
  void Update()
  {
    if (isSocketCreated && !isReaderAlive) {
      spawnReader();
    }

    Debug.Log(String.Format("Current request::\n{0}", request));
    if (request != "") {
      RemoteCall request = fetchRequest();
      handleRequest(request);
    }
    
  }

  /// <summary> Fetch a request from the reader thread and empty the current request buffer. </summary>
  private RemoteCall fetchRequest() {
    RemoteCall cu = JsonUtility.FromJson<RemoteCall>(request);
    mut.WaitOne();
    request = "";
    mut.ReleaseMutex();
    return cu;
  }
  
  /// <summary> Handle a remote process call request received over socket. </summary>
  void handleRequest(RemoteCall cu) {
    Debug.Log(cu.method);
    switch (cu.method) {
      case "createShowey":      compositeController.createShowey(cu.param);          break;
      case "updateShowey":      compositeController.loadShoweyDefinition(cu.param);  break;
      case "createSceney":      sceneController.createSceney(cu.param);              break;
      case "updateSceney":      sceneController.updateSceney(cu.param);              break;
      case "updateErrors":      sceneController.updateErrors(cu.param);              break;
      case "updateBranches":    sceneController.updateBranches(cu.param);            break;
      case "updateMessage":     sceneController.updateMessage(cu.param);             break;
      case "updateEnemy":       sceneController.updateEnemy(cu.param);               break;
      case "updateFalling":     sceneController.updateFalling(cu.param);             break;
      case "updateMovement":    sceneController.updateMovement(cu.param);            break;
      case "updateCollectable": sceneController.updateCollectable(cu.param);         break;
      case "updateInteraction": sceneController.updateInteraction(cu.param);         break;
      case "updateHighlight":   sceneController.updateHighlight(cu.param);           break;
      case "updateSequence":    sceneController.updateSequence(cu.param);            break;
      default: Debug.LogError("Unknown request received by socket."); break;
    }
    if (cu.close) closeSocket();
  }

  /// <summary> Close the opened socket, threads and streamreader. </summary>
  private void closeSocket() {
    if (creator_thread != null) creator_thread.Abort();
    if (reader_thread != null) reader_thread.Abort();
    if (s != null) s.Close();
    if (soc != null) soc.Close();
  }

  /// <summary> Close the socket upon exiting app. </summary>
  private void OnApplicationQuit() {
    closeSocket();
  }
}
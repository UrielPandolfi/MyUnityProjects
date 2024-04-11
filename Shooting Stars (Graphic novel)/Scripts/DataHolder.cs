using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct DataHolder
{
    public GameScene scene;
    public int sentenceIndex;
    public AudioClip music;
    public List<Speaker> charactersOnScene; // No la inicializamos aqui ya que no se puede, se inicializa en el gamemanager
    public Vector2 camPosition;
}

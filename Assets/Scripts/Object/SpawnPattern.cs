using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SpawnObjectData
{
    public ObjectData objectData;
    public Vector3 cameraOffset; // Posição relativa à câmera no momento do spawn
}


[CreateAssetMenu(fileName = "SpawnPattern", menuName = "Scriptable Objects/SpawnPattern")]
public class SpawnPattern : ScriptableObject
{
    public List<SpawnObjectData> objectsToSpawn; //ucne object data e local onde nasce
    public List<ObjectData> answer; //os objetos, em ordem, que é resposta
    public List<String> logicList; //O que aparece "antes" de cada espaço em branco na lista lógica
    // o Espaço em branco usará o nome que está no "ObjectTrigger" ou equivalente
}

using System;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

[CreateAssetMenu(fileName = "SpawnPattern", menuName = "Scriptable Objects/SpawnPattern")]
public class SpawnPattern : ScriptableObject
{
    public List<GameObject> objectsToSpawn; //os objetos a serem criados, depois trocados por ObjectTrigger (ou ObjectData?)
    public List<Transform> startPosition; //quais as posições, em relação à camera
    public List<GameObject> answer; //os objetos, em ordem, que é resposta
    public List<String> logicList; //O que aparece "antes" de cada espaço em branco na lista lógica
    // o Espaço em branco usará o nome que está no "ObjectTrigger" ou equivalente
}

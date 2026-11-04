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
    public List<SpawnObjectData> objectsToSpawn; //une object data e local onde nasce
    //public CorrectObjectList answer; //usa referencia a outro SO, para que assim fique mais organizado a conexão pesquisa - físico
    //no momento da captura obtem a pesquisa alvo comparando o CorrectObjectList desse SO e as pesquisas do físico, 
    //se a lista lógica atual = answer, na captura, ativa a pesquisa e encerra o encontro
    public int maxEntries = 3; //número máximo de entradas na lista lógica, garante que o número de entradas
    // da logicList = máximo de entradas de currentList, feito dessa forma e exigindo checagem ao inserir pois 
    // no fim facilita a captura
    public List<String> logicList; //O que aparece "antes" de cada espaço em branco na lista lógica
    // o Espaço em branco usará o nome que está no "ObjectTrigger" ou equivalente
    public PhysicistTimelineEraPesquisa targetResearch; // a pesquisa a ser desbloqueada
}

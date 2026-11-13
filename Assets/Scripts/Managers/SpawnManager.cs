using System;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private SpawnPattern currentSpawnPattern; // isso atualmente vem do Image tracker quando os objetos spawnam
    private PhysicistTrigger triggerFisicoAtual;
    [SerializeField]
    private EncounterManager encounterManager;
    [SerializeField]
    private ObjpediaManager objpediaManager;

    // adiciona objetos apenas se não estiverem na objespedia
    public void AdicionarObjetosObjespedia(SpawnPattern lista)
    {
        if (!lista.targetResearch)
        {
            Debug.LogError("Não há pesquisa definida no SpawnPattern!");
            return;
        }
        var novosObjetos = lista.targetResearch.objetosPesquisa.correctObjects;

        foreach (var obj in novosObjetos)
        {
            // Só adiciona se o objeto ainda não estiver na lista
            if (!encounterManager.objectDatabase.allObjects.Contains(obj))
            {
                encounterManager.objectDatabase.allObjects.Add(obj);
            }
        }

        objpediaManager.RestartObjespedia();
    }
    // atualiza lista de Spawn Patter atual e atualiza objespedia
    public void AtualizarObjetosSpawnados(SpawnPattern listaAtualizada)
    {
        currentSpawnPattern = listaAtualizada;
        AdicionarObjetosObjespedia(currentSpawnPattern);
        printarStringListaLogicaAtual();
    }

    public void AtualizarFisicoAtual(PhysicistTrigger trigger)
    {
        triggerFisicoAtual = trigger;
    }

    // Desbloqueia pesquisa e objetos capturados conforme spawn pattern atual
    public void GanhouMinigameAtual()
    {
        if (!currentSpawnPattern)
        {
            Debug.LogError("Não há Spawn Pattern para desbloquear a pesquisa, isso COM CERTEZA É UM BUG!");
            return;
        }
        PhysicistTimelineEraPesquisa.DesbloquearPesquisa(currentSpawnPattern.targetResearch);
        PhysicistTimelineEraPesquisa.DesbloquearObjetosPesquisa(currentSpawnPattern.targetResearch);

    }

    public void printarStringListaLogicaAtual()
    {
        string nomeFisico = "NULL";
        if (triggerFisicoAtual)
        {
            nomeFisico = triggerFisicoAtual.data.name;
        }
        string texto = "";
        for (int i = 0; i < currentSpawnPattern.logicList.Count; i++)
        {
            string textoFormatado = currentSpawnPattern.logicList[i].Replace("{nome}", nomeFisico);
            texto += textoFormatado + " _______ ";
        }
        Debug.Log(texto);
    }
}

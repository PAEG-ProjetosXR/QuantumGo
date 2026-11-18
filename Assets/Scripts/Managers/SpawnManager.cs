using System;
using Unity.VisualScripting;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private SpawnPattern currentSpawnPattern; // isso atualmente vem do Image tracker quando os objetos spawnam
    private List<string> currentLogicList = new List<string>(); // lista atual, deve ser apagada com morte de físico e seu tamanho é =
    private PhysicistTrigger triggerFisicoAtual;
    [SerializeField]
    private EncounterManager encounterManager;
    [SerializeField]
    private ObjpediaManager objpediaManager;

    private const string BLANK_TEXT = "_____";

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
        encounterManager.RegisterPhysicistEncounter(triggerFisicoAtual.data, 1);
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

    public SpawnPattern GetCurrentSpawnPattern()
    {
        return this.currentSpawnPattern;
    }

    public void SetCurrentSpawnPattern(SpawnPattern newPattern)
    {
        this.currentSpawnPattern = newPattern;
        return;
    }

    public List<string> GetCurrentLogicList()
    {
        return this.currentLogicList;
    }

    public void initializeCurrentLogicList()
    {
        
        this.currentLogicList.Clear(); // limpa a lista para iniciar novo
        foreach(string entrada in currentSpawnPattern.logicList)
        {
          this.currentLogicList.Add(BLANK_TEXT); // um espaço em branco
        }
        
        return;
    }

    public void insertInCurrentLogicList(string newObject)
    {
        for (int i = 0; i < currentLogicList.Count; i++)
        {
            if (currentLogicList[i] == BLANK_TEXT)
            {
                currentLogicList[i] = newObject;
                return;
            }
        }
        Debug.LogWarning("Não há mais espaços disponíveis na currentLogicList!");
    }

    
}

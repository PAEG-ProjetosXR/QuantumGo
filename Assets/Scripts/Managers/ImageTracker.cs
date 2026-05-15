using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ImageTracker : MonoBehaviour
{
    private ARTrackedImageManager trackedImages;
    public GameObject[] ArPrefabs;

    List<GameObject> ARObjects = new List<GameObject>();

    /*void Update()
    {
        outputTracking();
    }

    void outputTracking()
    {
        int i = 0;
        foreach (var trackedImage in trackedImages.trackables)
        {
            // --- ADICIONADO CHECK DE SEGURANÇA ---
            // Antes de fazer qualquer coisa, verifique se o objeto correspondente
            // na nossa lista ainda existe. Se foi destruído (capturado), pule para o próximo.
            if (ARObjects.Count <= i || ARObjects[i] == null)
            {
                i++;
                continue; // Pula para a próxima iteração do loop
            }
            // ------------------------------------

            if (trackedImage.trackingState == TrackingState.Limited)
            {
                ARObjects[i].SetActive(false);
            }
            if (trackedImage.trackingState == TrackingState.Tracking)
            {
                ARObjects[i].SetActive(true);
            }

            i++;
        }
    }
    */
    void Awake()
    {
        trackedImages = GetComponent<ARTrackedImageManager>();
    }

    /*
    void OnEnable()
    {
        trackedImages.trackablesChanged += OnTrackablesChanged;
    }
    */

    void OnEnable()
    {
    trackedImages.trackablesChanged.AddListener(OnTrackedImagesChanged);
    }

    void OnDisable()
    {
    trackedImages.trackablesChanged.RemoveListener(OnTrackedImagesChanged);
    }
    /*
    void OnEnable()
    {
        trackedImages.trackablesChanged += OnTrackedImagesChanged;
    }

    void OnDisable()
    {
        
        trackedImages.trackablesChanged -= OnTrackedImagesChanged;
    }
    */
/*
    void OnTrackablesChanged(ARTrackablesChangedEventArgs<ARTrackedImage> eventArgs)
{
    // Imagens detectadas pela primeira vez
    foreach (var trackedImage in eventArgs.added)
    {
        Debug.Log($"Imagem nova detectada: {trackedImage.referenceImage.name}");
    }

    // Imagens que se moveram ou mudaram de estado
    foreach (var trackedImage in eventArgs.updated)
    {
        // Exemplo: verificar se a imagem ainda está sendo rastreada
        if (trackedImage.trackingState == UnityEngine.XR.ARSubsystems.TrackingState.Tracking)
        {
            // Atualizar posição de um objeto 3D, por exemplo
        }
    }

    // Imagens que saíram do rastreio ou foram removidas
    foreach (var kvp in eventArgs.removed)
    {
        var trackedImage = kvp.Value;
        Debug.Log($"Imagem removida: {trackedImage.referenceImage.name}");
    }
}
*/
private void OnTrackedImagesChanged(ARTrackablesChangedEventArgs<ARTrackedImage> eventArgs)
{
    //Create object based on image tracked
    foreach (var trackedImage in eventArgs.added)
    {
        foreach (var arPrefab in ArPrefabs)
        {
            if(trackedImage.referenceImage.name == arPrefab.name)
            {
                var obj = Instantiate(arPrefab);
                    obj.transform.position = trackedImage.transform.position;
                    obj.transform.rotation = trackedImage.transform.rotation;
                    ARObjects.Add(obj);
            }
        }
    }
    
    //Update tracking position
    foreach (var trackedImage in eventArgs.updated)
    {
        for (int i = ARObjects.Count - 1; i >= 0; i--)
        {
            var arObject = ARObjects[i];

            if (arObject == null)
                continue;
        }
    }

    //Opcional: lidar com removidos
    foreach (var trackedImage in eventArgs.removed)
    {
        // lógica se quiser destruir ou esconder objetos
    }
}
}
/*
    // Event Handler
    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        //Create object based on image tracked
        foreach (var trackedImage in eventArgs.added)
        {
            foreach (var arPrefab in ArPrefabs)
            {
                if(trackedImage.referenceImage.name == arPrefab.name)
                {
                    var newPrefab = Instantiate(arPrefab, trackedImage.transform);
                    ARObjects.Add(newPrefab);
                }
            }
        }
        
        //Update tracking position
        foreach (var trackedImage in eventArgs.updated)
        {
            // Acessamos a lista pelo índice para poder remover itens se necessário
            for (int i = ARObjects.Count - 1; i >= 0; i--)
            {
                var arObject = ARObjects[i];

                // --- ADICIONADO CHECK DE SEGURANÇA ---
                // Se o objeto foi destruído (capturado), não fazemos nada com ele.
                if (arObject == null)
                {
                    continue; // Pula para o próximo objeto da lista
                }
                // ------------------------------------

                // Comparamos o nome da imagem com o nome do prefab instanciado,
                // removendo "(Clone)" que a Unity adiciona.
                if (arObject.name.Replace("(Clone)", "") == trackedImage.referenceImage.name)
                {
                    arObject.transform.position = trackedImage.transform.position;
                    arObject.transform.rotation = trackedImage.transform.rotation;
                    arObject.SetActive(trackedImage.trackingState == TrackingState.Tracking);
                }
            }
        }
    }
}
*/
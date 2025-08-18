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


    void Update()
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
    
    void Awake()
    {
        trackedImages = GetComponent<ARTrackedImageManager>();
    }

    void OnEnable()
    {
        trackedImages.trackedImagesChanged += OnTrackedImagesChanged;
    }

    void OnDisable()
    {
        trackedImages.trackedImagesChanged -= OnTrackedImagesChanged;
    }


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

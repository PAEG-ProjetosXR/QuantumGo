using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ImageTracker : MonoBehaviour
{
    private ARTrackedImageManager trackedImages;
    public GameObject obj_escolhido;
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
        TouchTest.Chosen += AoSelecionarObjeto;
    }

    void OnDisable()
    {
        trackedImages.trackablesChanged.RemoveListener(OnTrackedImagesChanged);
        TouchTest.Chosen -= AoSelecionarObjeto;
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
    public CaptureInfo HasTrackedImage(PhysicistData identidade, ARTrackedImage tracked)
    {
    if (identidade == null ||
        identidade.physicistCaptureInfo == null ||
        tracked == null)
    {
        return null;
    }

    foreach (CaptureInfo info in identidade.physicistCaptureInfo)
    {
        if (info.trackedImage == tracked)
        {
            return info;
        }
    }

    return null;
    }

    public bool CheckAndUpdateCapture(PhysicistData identidade, ARTrackedImage tracked)
    {
    if (identidade == null ||
        identidade.physicistCaptureInfo == null ||
        tracked == null)
    {
        return false;
    }

    foreach (CaptureInfo info in identidade.physicistCaptureInfo)
    {
        if (info.trackedImage == tracked)
        {
            info.recaptureTime = DateTime.Now;
            return true;
        }
    }

    return false;
    }

    private void OnTrackedImagesChanged(ARTrackablesChangedEventArgs<ARTrackedImage> eventArgs)
    {
        //Create object based on image tracked
        foreach (var trackedImage in eventArgs.added)
        {
            if (obj_escolhido != null)
            {
                break;
            }

            foreach (var arPrefab in ArPrefabs)
            {
                if (trackedImage.referenceImage.name == arPrefab.name)
                {
                    // 1. Instancia o prefab do cientista na posição da imagem
                    var obj = Instantiate(arPrefab);
                    obj.transform.position = trackedImage.transform.position;
                    obj.transform.rotation = trackedImage.transform.rotation;
                    ARObjects.Add(obj);

                    // 2. Procura pelo componente no objeto que acabou de nascer
                    PhysicistData identidade = obj.GetComponent<PhysicistTrigger>().data;
                    if(identidade.physicistCaptureInfo is null)
                    {
                        identidade.physicistCaptureInfo = new List<CaptureInfo>();
                    }
                    CaptureInfo isOnList = HasTrackedImage(identidade, trackedImage);
                    obj.GetComponent<PhysicistTrigger>().info = isOnList; // se tem CaptureInfo dado aquela imagem, anexa ao físico gerado
                    if (isOnList != null)
                    {
                        // 3. Batiza o cientista passando a imagem detectada pelo AR Foundation
                        //identidade.physicistCaptureInfo.Find(trackedImage) = trackedImage;
                        Debug.Log($"[ImageTracker] Cientista já vinculado à imagem: {trackedImage.referenceImage.name}");
                    }
                    else
                    {
                        identidade.physicistCaptureInfo.Add(new CaptureInfo(trackedImage, obj, null, DateTime.Now));
                        Debug.LogWarning($"[ImageTracker] Aviso: O prefab '{arPrefab.name}' não possui Capture Info, criando...");
                    }
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
    private void AoSelecionarObjeto(GameObject objTouched)
    {
        Debug.Log("O ImageTracker recebeu o evento! O objeto tocado foi: " + objTouched.name);
        obj_escolhido = objTouched;
        PhysicistTrigger trig = objTouched.GetComponent<PhysicistTrigger>();
        
        if(trig != null)
        {
            trig.foiEscolhido = true;
        }

        // Rodamos de trás para frente (Count - 1) porque vamos deletar itens da lista
        for (int i = ARObjects.Count - 1; i >= 0; i--)
        {
            GameObject atual = ARObjects[i];

            // Se o objeto da lista NÃO for o que o jogador escolheu
            if (atual != obj_escolhido)
            {
                // Remove da lista interna para não pesar
                ARObjects.RemoveAt(i);

                // Destrói o objeto do mundo 3D
                Destroy(atual);
            }
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
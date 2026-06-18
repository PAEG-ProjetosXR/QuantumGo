using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ImageTracker : MonoBehaviour
{
    private ARTrackedImageManager trackedImages;
    public GameObject obj_escolhido;
    public GameObject[] ArPrefabs;
    public UIHandler uiHandler;
    [SerializeField] private Camera arCamera;
    private Dictionary<ARTrackedImage, TMP_Text> cooldownLabels = new(); //dicionario de imagem e seu cooldown

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

    private void Start()
    {
        uiHandler = FindAnyObjectByType<UIHandler>();
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
        PhysicistTrigger.OnPhysicistDestroyed += HandleDestroyed;
        TouchTest.Chosen += AoSelecionarObjeto;
    }

    void OnDisable()
    {
        trackedImages.trackablesChanged.RemoveListener(OnTrackedImagesChanged);
        PhysicistTrigger.OnPhysicistDestroyed -= HandleDestroyed;
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
    private void UpdateLabelPosition(ARTrackedImage trackedImage, TMP_Text label)
    {
    Vector3 screenPos = arCamera.WorldToScreenPoint(trackedImage.transform.position);
    label.rectTransform.position = screenPos;
    }


    private IEnumerator UpdateCooldownLabel(ARTrackedImage trackedImage, DateTime recaptureTime, TMP_Text label)
    {
    while (trackedImage != null)
    {
        TimeSpan remaining = recaptureTime - DateTime.Now;

        if (remaining.TotalSeconds <= 0)
        {
            Destroy(label.gameObject);
            cooldownLabels.Remove(trackedImage);
            yield break;
        }
        label.text = $"{Mathf.CeilToInt((float)remaining.TotalSeconds)}s";
        //label.fontSize = trackedImage.size.x * 100f;
        UpdateLabelPosition(trackedImage, label);
        yield return null;
    }
    }

    private void CreateCooldownLabel(
    ARTrackedImage trackedImage,
    DateTime recaptureTime)
{
    if (cooldownLabels.ContainsKey(trackedImage))
        return;

    UIHandler ui = FindFirstObjectByType<UIHandler>();

    TMP_Text label = Instantiate(ui.cooldownLabelPrefab, ui.cooldownContainer);

    cooldownLabels.Add(trackedImage, label);

    StartCoroutine(UpdateCooldownLabel(trackedImage,recaptureTime,label));
}

    public bool CanSpawn(PhysicistData identidade, ARTrackedImage tracked, out TimeSpan remaining)
    {
    remaining = TimeSpan.Zero;

    CaptureInfo info =
        HasTrackedImage(identidade, tracked);

    if (info == null)
        return true;

    if (!info.recaptureTime.HasValue)
        return true;

    if (DateTime.Now >= info.recaptureTime.Value)
        return true;

    remaining =
        info.recaptureTime.Value - DateTime.Now;

    return false;
    }

    private void TrySpawnFromImage(ARTrackedImage trackedImage)
    {
    if (trackedImage == null) //se imagem invalida
        return;

    if (obj_escolhido != null) //se já tem um outro objeto escolhido
        return;

    foreach (var arPrefab in ArPrefabs)
    {
        if (trackedImage.referenceImage.name != arPrefab.name)
            continue;

        // Evita duplicatas
        foreach (GameObject obje in ARObjects)
        {
            if (obje == null)
                continue;

            PhysicistTrigger trig = obje.GetComponent<PhysicistTrigger>();

            if (trig != null &&
                trig.info != null &&
                trig.info.trackedImage == trackedImage)
            {
                return;
            }
        }

        PhysicistData identidade =
            arPrefab.GetComponent<PhysicistTrigger>().data;

        if (identidade == null)
            return;

        if (identidade.physicistCaptureInfo == null)
        {
            identidade.physicistCaptureInfo =
                new List<CaptureInfo>();
        }

        CaptureInfo info = HasTrackedImage(identidade, trackedImage);

        if(info != null)
        {
            Debug.Log($"Info encontrada. Recapture={info.recaptureTime}");
        }

        // Verifica cooldown
        if (info != null && info.recaptureTime.HasValue && DateTime.Now < info.recaptureTime.Value)
        {
            TimeSpan remaining =
                info.recaptureTime.Value - DateTime.Now;

            // gerar o timer na tela
            Debug.Log("Tentando Tempo... nhie");
           CreateCooldownLabel(trackedImage, info.recaptureTime.Value);

            return;
        }

        // Instancia o objeto
        GameObject obj = Instantiate(arPrefab);

        obj.transform.position =
            trackedImage.transform.position;

        obj.transform.rotation =
            trackedImage.transform.rotation;

        ARObjects.Add(obj);

        PhysicistTrigger trigger =
            obj.GetComponent<PhysicistTrigger>();

        if (info != null)
        {
            trigger.info = info;

            Debug.Log(
                $"[ImageTracker] Reutilizando CaptureInfo."
            );
        }
        else
        {
            info = new CaptureInfo(
                trackedImage,
                obj,
                DateTime.Now,
                null
            );

            identidade.physicistCaptureInfo.Add(info);

            trigger.info = info;

            Debug.Log(
                $"[ImageTracker] Criando novo CaptureInfo."
            );
        }

        break;
    }
}

    private void ForceRescan()
    {
    foreach(var trackedImage in trackedImages.trackables)
    {
        if(trackedImage.trackingState != TrackingState.Tracking)
            continue;

        TrySpawnFromImage(trackedImage);
    }
    }




    public CaptureInfo HasTrackedImage(PhysicistData identidade, ARTrackedImage tracked)
    {
    if (identidade == null || identidade.physicistCaptureInfo == null ||tracked == null)
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

            foreach (var arPrefab in ArPrefabs)
            {
                TrySpawnFromImage(trackedImage);
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

        uiHandler.enableCapture();
    }
    private IEnumerator DelayedRescan()
    {
    yield return new WaitForEndOfFrame();

    Debug.Log("Executando ForceRescan");

    ForceRescan();
    }

    private void HandleDestroyed(PhysicistTrigger instance)
    {
    Debug.Log("Objeto destruído!");
    if (instance.gameObject != obj_escolhido)
        return;

    obj_escolhido = null;
    StartCoroutine(DelayedRescan());
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
// ArtifactData.cs
using UnityEngine;

[CreateAssetMenu(fileName = "New Artifact", menuName = "QuantumGo/Artifact Data")]
public class ArtifactData : ScriptableObject
{
    public string artifactName = "New Artifact";
    public Sprite icon; // Ícone para o mini-menu
    public GameObject artifactModelPrefab; // Modelo 3D para quando for "lançar", para o futuro
    // public int artifactID; // Se precisar de um ID único
}
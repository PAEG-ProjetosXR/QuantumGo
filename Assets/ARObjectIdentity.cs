using UnityEngine;
using UnityEngine.XR.ARFoundation; // Necessário para o sistema de AR
using System.Text.RegularExpressions; // Necessário para isolar o número do nome

public class ARObjectIdentity : MonoBehaviour
{
    // Guarda o nome da imagem que gerou este objeto específico (ex: planck)
    public string nomeDaImagemOrigem;

    // Guarda o ID numérico extraído do nome (ex: planck2 -> 2)
    public int indexDaImagem;

    public void ConfigurarIdentidade(ARTrackedImage imagemPaiRecebida)
    {
        if (imagemPaiRecebida != null && imagemPaiRecebida.referenceImage != null)
        {
            // 1. Pega o nome exato da imagem cadastrada no AR usando o dado vindo do Tracker
            nomeDaImagemOrigem = imagemPaiRecebida.referenceImage.name;

            // 2. Descobre o index extraindo apenas os números do nome da imagem
            indexDaImagem = ExtrairNumeroDoNome(nomeDaImagemOrigem);

            Debug.Log($"[AR] Objeto identificado! Nome: {nomeDaImagemOrigem} | Index: {indexDaImagem}");
        }
    }


    // Função auxiliar que limpa o texto e pega só o número do final do nome
    private int ExtrairNumeroDoNome(string nome)
    {
        // Procura por números no texto
        string apenasNumeros = Regex.Match(nome, @"\d+").Value;

        if (!string.IsNullOrEmpty(apenasNumeros))
        {
            return int.Parse(apenasNumeros); // Retorna o número encontrado (ex: 2 ou 3)
        }

        return 1; // Se for imagem sem numero (como so "planck"), define como index 1
    }
}


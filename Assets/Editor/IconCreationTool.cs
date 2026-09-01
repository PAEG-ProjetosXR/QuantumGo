using UnityEngine;
using UnityEditor;
using System.IO;

public class IconCreationTool
{
    [MenuItem("Assets/Create/Icon From Image", priority = 20)]  //ao clicar com o botao direito
    public static void CreateIconFromImage()
    {
        Texture2D selectedImage = Selection.activeObject as Texture2D;  //pega a imagem selecionada
        if (selectedImage == null)
        {
            Debug.LogWarning("Por favor, selecione uma imagem primeiro!");
            return;
        }

        string imagePath = AssetDatabase.GetAssetPath(selectedImage);
        TextureImporter textureImporter = AssetImporter.GetAtPath(imagePath) as TextureImporter;
        if (textureImporter.textureType != TextureImporterType.Sprite)
        {
            textureImporter.textureType = TextureImporterType.Sprite; //muda o tipo da textura
            AssetDatabase.ImportAsset(imagePath, ImportAssetOptions.ForceUpdate); //aplica a mudança
        }
        // -----------------------------------------------------------

        ObjectData newIconData = ScriptableObject.CreateInstance<ObjectData>(); // cria uma "ficha" de ObjectData vazia

        //preenche a "ficha" com os dados
        newIconData.name = selectedImage.name;      //usa o campo "name" da ficha
        newIconData.icon = AssetDatabase.LoadAssetAtPath<Sprite>(imagePath); //usa o campo "icon" e o caminho que ja pegamos

        //define onde salvar o novo arquivo do icone
        string newPath = Path.GetDirectoryName(imagePath) + "/" + selectedImage.name + "_Icon.asset";

        AssetDatabase.CreateAsset(newIconData, newPath);        //cria o arquivo do icone na mesma pasta da imagem
        AssetDatabase.SaveAssets();

        Debug.Log("Ícone criado com sucesso em: " + newPath);
    }
}
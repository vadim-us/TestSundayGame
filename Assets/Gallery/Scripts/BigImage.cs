using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// большая картинка
/// </summary>
public class BigImage : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<RawImage>().texture = Galery.selectedImage.Texture;
    }

    public void Change(Image image)
    {
        GetComponent<RawImage>().texture = image.Texture;
    }


}

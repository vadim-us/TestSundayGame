using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// маленькая картинка в галлереи
/// </summary>
public class SmallImage : MonoBehaviour
{
    private Image image { get; set; }

    public void Setup(Image image)
    {
        this.image = image;

        GetComponent<RawImage>().texture = image.Texture;
        GetComponent<Button>().onClick.AddListener(() => FullImage());
    }

    private void FullImage()
    {
        Galery.selectedImage = image;
        Galery.SwichScene("FullImage");
    }
}

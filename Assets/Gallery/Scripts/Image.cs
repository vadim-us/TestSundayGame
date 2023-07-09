using UnityEngine;

public class Image
{
    public int Id { set; get; }
    public Texture Texture { set; get; }

    public Image(int id, Texture texture)
    {
        Id = id;
        Texture = texture;
    }
}

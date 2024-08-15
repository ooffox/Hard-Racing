using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelData
{
    public int Number;
    public string SceneName;
    public Texture2D ThumbnailTexture;

    [TextArea(5, 10)]
    public string Description;
}

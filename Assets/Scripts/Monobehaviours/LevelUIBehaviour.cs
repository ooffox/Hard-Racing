using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
public class LevelUIBehaviour : MonoBehaviour
{
    public LevelData Data;

    void Start()
    {
        Transform imageObj = transform.GetChild(transform.childCount - 2); // the image object will always be second to last in the hierarchy so it gets rendered last, before the click trigger
        Sprite sprite = UIManager.TextureToSprite(Data.ThumbnailTexture);
        imageObj.GetComponent<Image>().sprite = sprite;
    }
}

using UnityEngine;

[CreateAssetMenu(fileName = "SongList", menuName = "ScriptableObjects/SongList")]
public class SongList : ScriptableObject
{
    public AudioClip[] Songs;
}
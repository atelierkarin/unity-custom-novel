using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NovelContent", menuName = "ScriptableObject/NovelContent")]
public partial class NovelContent : ScriptableObject
{
    [SerializeField]
    [Tooltip("データの説明")]
    public string comment;

    [SerializeField]
    [Tooltip("データ")]
    public List<ContentData> contentData = new List<ContentData>();

    [System.Serializable]
    public class ContentData
    {
        [SerializeField] public int id;
        [SerializeField] public NovelCommandType command;
        [SerializeField] public string content;
    }
}

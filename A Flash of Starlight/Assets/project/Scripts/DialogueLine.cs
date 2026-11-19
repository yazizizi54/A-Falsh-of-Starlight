using System;
using UnityEngine;

[Serializable]
public class DialogueLine
{
    [Tooltip("说话者名字，留空则不显示名字框")]
    public string speakerName;

    [TextArea(3, 8)]
    [Tooltip("对话内容")]
    public string text;

    [Tooltip("这一句的背景图，留空则沿用上一张")]
    public Sprite background;

    [Tooltip("这一句的立绘，留空则隐藏立绘")]
    public Sprite character;
}
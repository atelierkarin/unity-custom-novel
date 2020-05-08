using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class NovelView : MonoBehaviour
{
    [SerializeField] Transform canvas;
    public Transform Canvas { get { return canvas; } }

    [SerializeField] Image backgroundImage;
    public Image BackgroundImage { get { return backgroundImage; } }

    [SerializeField] GameObject panelDialog;
    public GameObject PanelDialog { get { return panelDialog; } }

    [SerializeField] GameObject panelDialogName;
    public GameObject PanelDialogName { get { return panelDialogName; } }

    [SerializeField] TMPro.TextMeshProUGUI textName;
    public TMPro.TextMeshProUGUI TextName { get { return textName; } }

    [SerializeField] GameObject panelMessage;
    public GameObject PanelMessage { get { return panelMessage; } }

    [SerializeField] TMPro.TextMeshProUGUI textMessage;
    public TMPro.TextMeshProUGUI TextMessage { get { return textMessage; } }

}

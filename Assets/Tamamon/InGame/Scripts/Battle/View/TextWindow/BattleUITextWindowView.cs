using System;
using UnityEngine;
using Cysharp.Threading.Tasks;
using TMPro;

public class BattleUITextWindowView : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI m_messageText = default;

    private string m_encountMessage = "–ì¶‚Ì {0} ‚ªŒ»‚ê‚½!";

    private string m_bringOutMessage = "s‚¯ ! {0} !!";

    private TypeWriteEffect m_typeWriteEffect = default;
    public TypeWriteEffect TypeWriteEffect => m_typeWriteEffect;


}

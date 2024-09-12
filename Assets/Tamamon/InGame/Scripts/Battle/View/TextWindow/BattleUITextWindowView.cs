using System;
using UnityEngine;
using Cysharp.Threading.Tasks;
using TMPro;

public class BattleUITextWindowView : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI m_messageText = default;

    private string m_encountMessage = "野生の {0} が現れた!";

    private string m_bringOutMessage = "行け ! {0} !!";

    private TypeWriteEffect m_typeWriteEffect = default;
    public TypeWriteEffect TypeWriteEffect => m_typeWriteEffect;


}

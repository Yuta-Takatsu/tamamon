using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class TitleController : MonoBehaviour
{

    [SerializeField]
    private TitleView m_titleView = default;

    // Start is called before the first frame update
    void Start()
    {
        m_titleView.OnInitialize().Forget();
    }

}

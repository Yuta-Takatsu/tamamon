using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CheckPos : MonoBehaviour
{
    public Button buttonComponent = null;
    GameObject P;
    GameObject P2;
    GameObject P3;
    GameObject Scroll;

    ScrollRect ScrollRect;

    public static int co;
    static int u;
    static int d;

    void Start()
    {
        P = transform.parent.gameObject;
        P2 = P.transform.parent.gameObject;
        P3 = P2.transform.parent.gameObject;
        Scroll = GameObject.FindWithTag("Player");
        ScrollRect = Scroll.GetComponent<ScrollRect>();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Debug.Log(EventSystem.current.currentSelectedGameObject.GetComponent<RectTransform>().position);
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            if (this.gameObject.name == "0")
            {
                buttonComponent.Select();
            }
        }

        if (P3.activeSelf == true && co == 0)
        {
            if (this.gameObject.name == "0")
            {
                buttonComponent.Select();
            }
            co++;   //co�͈�x�����I�������킹�邽�߂Ɏg�p���Ă���A���ꂪ�Ȃ���Update�Ȃ̂ŁA��ɑI�����ꑱ����B
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;

namespace Framework
{
    public class SceneButtonContent : MonoBehaviour
    {
        [SerializeField]
        private Button m_button = default;

        public void Start()
        {
            CreateChangeSceneButton();
        }

        /// <summary>
        /// �w��V�[���ɑJ�ڂ���{�^���𐶐�
        /// </summary>
        public void CreateChangeSceneButton()
        {
            // NOTE: EditorBuildSettings������ƃr���h���ʂ�Ȃ��̂ŁA��ֈĂ��o��܂ŕ���
            /*foreach(var scene in EditorBuildSettings.scenes)
            {
                // �p�X������V�[�������擾
                string scenePath = scene.path;
                string sceneName = System.IO.Path.GetFileNameWithoutExtension(scenePath);

                // Boot�V�[���Ȃ��΂�
                if(sceneName == "Boot")
                {
                    continue;
                }

                // �R�[���o�b�N�o�^
                Button button = Instantiate(m_button, m_button.transform.parent);
                button.GetComponentInChildren<TextMeshProUGUI>().text = sceneName;

                button.onClick.AddListener(async () => await SceneManager.Instance.LoadSceneAsync($"{sceneName}"));
            }

            m_button.gameObject.SetActive(false);*/
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class PopulateModelChoice : MonoBehaviour
{
    [SerializeField] private Transform m_ContentContainer;
    [SerializeField] private GameObject m_ItemPrefab;
    [SerializeField] private Transform m_modelCheckboxManager;
    private string directoryPath = "NN Models";

    void Start()
    {
        var sth = Resources.LoadAll<TextAsset>(directoryPath);
        for (int i = 0; i < sth.Length; i++)
        {
            string text = sth[i].text;
            string[] separatedText = separateByNewLine(text);

            var item_go = Instantiate(m_ItemPrefab);
            var title = item_go.transform.Find("Title");
            title.gameObject.GetComponent<Text>().text = separatedText[0];

            var desc = item_go.transform.Find("Description");
            desc.gameObject.GetComponent<Text>().text = separatedText[1];
            item_go.transform.SetParent(m_ContentContainer);
            item_go.transform.localScale = Vector2.one;

            var toggle = item_go.transform.Find("Toggle");
            var toggleComponent = toggle.gameObject.GetComponent<Toggle>();
            if (i == 0) toggleComponent.isOn = true;
            else toggleComponent.isOn = false;
        }
        ToggleManager script = m_modelCheckboxManager.GetComponent<ToggleManager>();
        script.startToggleManager();
    }
    private string[] separateByNewLine(string s)
    {
        var result = s.Split(new string[] { "\r\n", "\n", "\r" }, StringSplitOptions.None);
        return result;
    }
}

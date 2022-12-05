using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopulateMapChoice : MonoBehaviour
{
    [SerializeField] private Transform m_ContentContainer;
    [SerializeField] private GameObject m_ItemPrefab;
    [SerializeField] private int m_ItemsToGenerate;
    [SerializeField] private string[] m_Images;
    [SerializeField] private string[] m_Titles;
    [SerializeField] private string[] m_Desc;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < m_ItemsToGenerate; i++)
        {
            var item_go = Instantiate(m_ItemPrefab);
            // do something with the instantiated item -- for instance
            var sth = item_go.GetComponentsInChildren<Transform>();
            var cos = sth[0];
            var texts = item_go.GetComponentsInChildren<Text>();
            texts[0].text = m_Titles[i];
            texts[1].text = m_Desc[i];
            item_go.GetComponent<Image>().color = i % 2 == 0 ? Color.yellow : Color.cyan;
            //parent the item to the content container
            item_go.transform.SetParent(m_ContentContainer);
            //reset the item's scale -- this can get munged with UI prefabs
            item_go.transform.localScale = Vector2.one;
        }
    }
}

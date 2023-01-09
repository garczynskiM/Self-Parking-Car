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
    private string directoryPath = "Assets/NN Models";

    void Start()
    {
        var NNModelDirectoryInfo = new DirectoryInfo(directoryPath);
        List<string> fileNames = getTextFiles(NNModelDirectoryInfo);
        for (int i = 0; i < fileNames.Count; i++)
        {
            string[] lines = File.ReadAllLines(string.Format("{0}/{1}", directoryPath, fileNames[i]));


            var item_go = Instantiate(m_ItemPrefab);
            // do something with the instantiated item -- for instance
            var title = item_go.transform.Find("Title");
            title.gameObject.GetComponent<Text>().text = lines[0];

            var desc = item_go.transform.Find("Description");
            desc.gameObject.GetComponent<Text>().text = lines[1];
            //parent the item to the content container
            item_go.transform.SetParent(m_ContentContainer);
            //reset the item's scale -- this can get munged with UI prefabs
            item_go.transform.localScale = Vector2.one;

            var toggle = item_go.transform.Find("Toggle");
            var toggleComponent = toggle.gameObject.GetComponent<Toggle>();
            if (i == 0) toggleComponent.isOn = true;
            else toggleComponent.isOn = false;
        }
        CheckboxManager script = m_modelCheckboxManager.GetComponent<CheckboxManager>();
        script.startToggleManager();
    }
    private List<string> getTextFiles(DirectoryInfo d)
    {
        List<string> result = new List<string>();
        // Add file sizes.
        FileInfo[] fis = d.GetFiles();
        foreach (FileInfo fi in fis)
        {
            if (fi.Extension.Contains("txt")) result.Add(fi.Name);
        }
        return result;
    }
}

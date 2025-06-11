using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DialogManager : Singleton<DialogManager>
{
    public Dictionary<int, List<string>> dialogDatas = new Dictionary<int, List<string>>();

    [Header("Resources 폴더 하위부터 csv파일 경로 작성")]
    public string csvName;

    //string filePath = "Assets/Resources/Json/DialogData.json";

    private void Awake()
    {
        if(Instance == this)
            DontDestroyOnLoad(this);

        DialogRead();
    }

    public void DialogRead()
    {
        if (csvName == string.Empty)
            return;

        TextAsset csvData = Resources.Load<TextAsset>(csvName);

        string[] data = csvData.text.Split('\n');

        for(int i = 1; i < data.Length;)
        {
            string[] row = data[i].Split(',');

            int index = int.Parse(row[0]);
            List<string> slist  = new List<string>();

            do
            {
                row[1] = row[1].Replace("/enter", "\n");
                slist.Add(row[1]);

                if (++i < data.Length)
                    row = data[i].Split(',');
                else
                    break;

            } while (row[0].ToString() == string.Empty);

            dialogDatas.Add(index, slist);
        }
    }

    public List<string> GetDialog(int index)
    {
        if (dialogDatas.ContainsKey(index))
            return dialogDatas[index];

        return null;
    }
}
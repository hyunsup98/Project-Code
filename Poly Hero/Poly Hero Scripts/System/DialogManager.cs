using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogData
{
    public int dialogType;
    public List<string> prevDialog;     //npc�� ó�� ���� ���� ���
    public List<string> nextDialog;     //npc�� �� �� ��ȭ �Ŀ� �ٲ� ���
    public int prevEventIndex;          //ù ���� ��� �� �� ��° ������ ������ �̺�Ʈ�� �߻��� ����
    public int nextEventIndex;          //�� �� ��ȭ �Ŀ� �̺�Ʈ �ε���
}

public class DialogManager : Singleton<DialogManager>
{
    
    public Dictionary<int, DialogData> dialogDatas = new Dictionary<int, DialogData>();

    [Header("Resources ���� �������� csv���� ��� �ۼ�")]
    public string csvName;

    private void Awake()
    {
        DialogRead();
    }

    void DialogRead()
    {
        if (csvName == null)
            return;

        //TextAsset ������ �ε��ϴµ� ���Ǵ� Ŭ����, csvData�� Resources���� ����� csvName ������ �־���
        TextAsset csvData = Resources.Load<TextAsset>(csvName);

        string[] data = csvData.text.Split('\n');

        //i�� 1�� ����:csv ������ 1��° ���� �����Ͱ� �ƴ� �� �÷��� Ÿ���� ����α� ����
        for(int i = 1; i < data.Length;)
        {
            DialogData dialogData = new DialogData();

            bool isAlready = false;
            bool isDone = false;

            int previndex = 1;
            int nextindex = 1;

            string[] row = data[i].Split(',');
            int indexId = int.Parse(row[0]);
            int indexType = int.Parse(row[1]);
            List<string> prevDialogList = new List<string>();
            List<string> nextDialogList = new List<string>();

            do
            {
                row[2] = row[2].Replace("/enter", "\n");

                if(!isAlready)
                    prevDialogList.Add(row[2]);
                else
                    nextDialogList.Add(row[2]);

                if (!isDone)
                {
                    if (string.IsNullOrEmpty(row[4].Trim()))
                    {
                        if (!isAlready)
                            previndex++;
                        else
                            nextindex++;
                    }
                    else
                    {
                        if (!isAlready)
                            dialogData.prevEventIndex = previndex;
                        else
                            dialogData.nextEventIndex = nextindex;

                        isDone = true;
                    }
                }

                if (!string.IsNullOrEmpty(row[3].Trim()))
                {
                    isAlready = true;
                    isDone = false;
                }

                if (++i < data.Length)
                {
                    row = data[i].Split(',');
                }
                else 
                    break;
            } while (row[0].ToString() == string.Empty);

            
            dialogData.dialogType = indexType;
            dialogData.prevDialog = prevDialogList;
            dialogData.nextDialog = nextDialogList;

            dialogDatas.Add(indexId, dialogData);
        }
    }

    public DialogData GetDialog(int index)
    {
        if(dialogDatas.ContainsKey(index))
            return dialogDatas[index];

        return null;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogData
{
    public int dialogType;
    public List<string> prevDialog;     //npc를 처음 봤을 때의 대사
    public List<string> nextDialog;     //npc와 한 번 대화 후에 바뀔 대사
    public int prevEventIndex;          //첫 만남 대사 중 몇 번째 라인이 끝나고 이벤트를 발생할 건지
    public int nextEventIndex;          //한 번 대화 후에 이벤트 인덱스
}

public class DialogManager : Singleton<DialogManager>
{
    
    public Dictionary<int, DialogData> dialogDatas = new Dictionary<int, DialogData>();

    [Header("Resources 폴더 하위부터 csv파일 경로 작성")]
    public string csvName;

    private void Awake()
    {
        DialogRead();
    }

    void DialogRead()
    {
        if (csvName == null)
            return;

        //TextAsset 파일을 로드하는데 사용되는 클래스, csvData에 Resources파일 경로의 csvName 파일을 넣어줌
        TextAsset csvData = Resources.Load<TextAsset>(csvName);

        string[] data = csvData.text.Split('\n');

        //i가 1인 이유:csv 파일의 1번째 줄은 데이터가 아닌 각 컬럼의 타입을 적어두기 때문
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

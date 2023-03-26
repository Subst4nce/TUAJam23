using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioTask : ScriptableObject, PlayerTask
{
    public string[] initialText;


    public void BeginTask()
    {
        DialogueWriter.instance.ReadSentences(initialText);

    }

   

}

public class ArgumentTask : MonoBehaviour, PlayerTask
{
    public void BeginTask()
    {
        throw new System.NotImplementedException();
    }


}


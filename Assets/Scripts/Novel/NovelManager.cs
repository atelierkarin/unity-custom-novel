using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NovelManager : MonoBehaviour
{
    [SerializeField] NovelView view;

    [SerializeField] NovelContent data;

    private NovelExecuter executer;

    private void Start()
    {
        StartNovel();
    }

    public void StartNovel(NovelCommand.CommonData commonData = null, NovelCommand.CommonVariable commonVariable = null)
    {
        this.executer = new NovelExecuter(this.data);

        if (commonVariable == null)
        {
            commonVariable = new NovelCommand.CommonVariable()
            {
                currentIndex = 0
            };
        }

        if (commonData == null)
        {
            // Create Initial Common Data
            commonData = new NovelCommand.CommonData()
            {
                manager = this,
                view = this.view,
                data = this.data
            };
        }

        StartCoroutine(RunCoroutine(commonData, commonVariable));
    }


    private IEnumerator RunCoroutine(NovelCommand.CommonData commonData, NovelCommand.CommonVariable commonVariable)
    {
        yield return executer.SetupCoroutine(commonData);
        yield return executer.RunCoroutine(commonVariable);
        yield break;
    }

    public void OnTouch()
    {
        ExecuteEventCoroutine(new NovelCommand.EventData() { intParameter = 0 });
    }

    public void ExecuteEventCoroutine(NovelCommand.EventData eData)
    {
        if (executer != null && eData != null)
        {
            Debug.Log("Mouse Clicked!");
            StartCoroutine(executer.EventCoroutine(eData));
        }
    }
}

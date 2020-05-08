using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NovelCommandTypeGroup
{
    _Text = 10000,
    _TextName = _Text + 1000,
    _TextMessage = _Text + 2000,

    _System = 900000000,

}

public enum NovelCommandType
{    
    NameSet = NovelCommandTypeGroup._TextName + 1,
    NameClear = NovelCommandTypeGroup._TextName + 2,
    NameShow = NovelCommandTypeGroup._TextName + 3,
    NameHide = NovelCommandTypeGroup._TextName + 4,    
    MessageSet = NovelCommandTypeGroup._TextMessage + 1,
    MessageClear = NovelCommandTypeGroup._TextMessage + 2,
    MessageShow = NovelCommandTypeGroup._TextMessage + 3,
    MessageHide = NovelCommandTypeGroup._TextMessage + 4,

    WaitTime = NovelCommandTypeGroup._System + 1001,
    WaitEvent = NovelCommandTypeGroup._System + 1002,
    Pause = NovelCommandTypeGroup._System + 1003,
}

// For NovelExecuter to search inner classes by indexes
public class NovelCommandAttribute : Attribute
{
    public NovelCommandType t;
    public NovelCommandAttribute(NovelCommandType type)
    {
        this.t = type;
    }
}

public class NovelCommand
{
    public class CommonData
    {
        public NovelManager manager;
        public NovelView view;
        public NovelContent data;
        public NovelContent.ContentData contentData;
    }

    public class CommonVariable
    {
        public int currentIndex;
    }

    [System.Serializable]
    public class EventData
    {
        public int intParameter = -1;
    }

    public interface NovelCommandInterface
    {
        IEnumerator Do(CommonData commonData, CommonVariable variable);
        IEnumerator Undo(CommonData commonData, CommonVariable variable);
        IEnumerator Event(CommonData commonData, CommonVariable variable, EventData eData);
    }

    #region Name
    [NovelCommandAttribute(NovelCommandType.NameSet)]
    public class NameSet : NovelCommandInterface
    {
        public IEnumerator Do(CommonData commonData, CommonVariable variable)
        {
            commonData.view.TextName.text += commonData.contentData.content;
            yield break;
        }

        public IEnumerator Undo(CommonData commonData, CommonVariable variable) { yield break; }
        public IEnumerator Event(CommonData commonData, CommonVariable variable, EventData eData) { yield break; }
    }

    [NovelCommandAttribute(NovelCommandType.NameClear)]
    public class NameClear : NovelCommandInterface
    {
        public IEnumerator Do(CommonData commonData, CommonVariable variable)
        {
            commonData.view.TextName.text = "";
            yield break;
        }

        public IEnumerator Undo(CommonData commonData, CommonVariable variable) { yield break; }
        public IEnumerator Event(CommonData commonData, CommonVariable variable, EventData eData) { yield break; }
    }
    #endregion

    #region Message
    [NovelCommandAttribute(NovelCommandType.MessageSet)]
    public class MessageSet : NovelCommandInterface
    {
        private bool isNext = false;
        public IEnumerator Do(CommonData commonData, CommonVariable variable)
        {
            CustomTextAnimatior animator = commonData.view.TextMessage.GetComponent<CustomTextAnimatior>();
            if (animator == null) { animator = commonData.view.TextMessage.gameObject.AddComponent<CustomTextAnimatior>(); }

            commonData.view.TextMessage.text = commonData.contentData.content;

            animator.Play();
            yield return new WaitWhile(() => { return animator.IsAnimating; });

            // Pause after text shown
            yield return new WaitUntil(() => { return isNext || Input.GetKeyDown(KeyCode.Z); });
            yield break;
        }
        public IEnumerator Undo(CommonData commonData, CommonVariable variable) { yield break; }
        public IEnumerator Event(CommonData commonData, CommonVariable variable, EventData eData)
        {
            // Touched
            if (eData.intParameter == 0)
            {
                CustomTextAnimatior animator = commonData.view.TextMessage.GetComponent<CustomTextAnimatior>();

                if (animator.IsAnimating)
                {
                    animator.Finish();
                }
                else
                {
                    isNext = true;
                }
            }
            yield break;
        }
    }

    [NovelCommandAttribute(NovelCommandType.MessageClear)]
    public class MessageClear : NovelCommandInterface
    {
        public IEnumerator Do(CommonData commonData, CommonVariable variable)
        {
            commonData.view.TextMessage.text = "";
            yield break;
        }
        public IEnumerator Undo(CommonData commonData, CommonVariable variable) { yield break; }
        public IEnumerator Event(CommonData commonData, CommonVariable variable, EventData eData) { yield break; }
    }
    #endregion

    #region System
    [NovelCommandAttribute(NovelCommandType.WaitTime)]
    public class WaitTime : NovelCommandInterface
    {
        public IEnumerator Do(CommonData commonData, CommonVariable variable)
        {
            float time = float.Parse(commonData.contentData.content);
            if (time <= 0.0f) { yield break; }
            yield return new WaitForSecondsRealtime(time);
            yield break;
        }
        public IEnumerator Undo(CommonData commonData, CommonVariable variable) { yield break; }
        public IEnumerator Event(CommonData commonData, CommonVariable variable, EventData eData) { yield break; }
    }


    [NovelCommandAttribute(NovelCommandType.Pause)]
    public class Pause : NovelCommandInterface
    {
        private bool isNext = false;
        public IEnumerator Do(CommonData commonData, CommonVariable variable)
        {
            yield return new WaitUntil(() => { return isNext || Input.GetKeyDown(KeyCode.Z); });
            yield break;
        }
        public IEnumerator Undo(CommonData commonData, CommonVariable variable) { yield break; }
        public IEnumerator Event(CommonData commonData, CommonVariable variable, EventData eData)
        {
            // Touched
            if (eData.intParameter == 0)
            {
                isNext = true;
            }
            yield break;
        }
    }
    #endregion
}

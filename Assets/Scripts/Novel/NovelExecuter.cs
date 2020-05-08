using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NovelExecuter
{
    public NovelCommand.CommonData commonData { get; private set; }
    public NovelCommand.CommonVariable commonVariable { get; private set; }

    private NovelContent novelContentData;

    private Dictionary<NovelCommandType, Type> dicNovelCommandType = new Dictionary<NovelCommandType, Type>();

    private NovelCommand.NovelCommandInterface command;

    public NovelExecuter(NovelContent data)
    {
        this.novelContentData = data;

        // Get all inner classes of NoveCommand
        var nestedType = typeof(NovelCommand).GetNestedTypes(System.Reflection.BindingFlags.Public);

        dicNovelCommandType = nestedType
            .Where(type => 
                {
                    return 0 < type.GetCustomAttributes(typeof(NovelCommandAttribute), false).Length;
                }
            )
            .Select(type => type)
            .ToDictionary(type => ((NovelCommandAttribute)type.GetCustomAttributes(typeof(NovelCommandAttribute), false).First()).t);
    }

    public IEnumerator EventCoroutine(NovelCommand.EventData eData)
    {
        if (command == null) { yield break; }
        if (commonData == null) { yield break; }
        if (commonVariable == null) { yield break; }
        if (eData == null) { yield break; }

        yield return command.Event(commonData, commonVariable, eData);
    }

    public IEnumerator SetupCoroutine(NovelCommand.CommonData commonData)
    {
        if (commonData == null) { yield break; }

        this.commonData = commonData;

        yield break;
    }

    public IEnumerator RunCoroutine(NovelCommand.CommonVariable variable)
    {
        if (variable == null) { yield break; }
        if (this.commonData == null) { yield break; }

        this.commonVariable = variable;

        while (commonVariable.currentIndex < novelContentData.contentData.Count)
        {
            this.commonData.contentData = novelContentData.contentData[commonVariable.currentIndex];

            // Try to create specific novel command actions
            Type commandType;
            if (dicNovelCommandType.TryGetValue(commonData.contentData.command, out commandType))
            {
                command = Activator.CreateInstance(commandType) as NovelCommand.NovelCommandInterface;

                var coroutine = command.Do(commonData, commonVariable);
                yield return coroutine;
            }

            commonVariable.currentIndex++;
        }

        yield break;
    }
}

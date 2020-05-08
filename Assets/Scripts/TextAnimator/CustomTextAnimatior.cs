using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CustomTextAnimatior : MonoBehaviour
{
    [SerializeField]
    private float speedPerCharacter = 0.1f;

    [SerializeField]
    private bool animateWhenStart = false;
        
    private TMPro.TextMeshProUGUI text;

    private IEnumerator coroutine;

    // アニメーション中かどうか
    private bool isAnimating = false;
    public bool IsAnimating { get { return isAnimating; } }

    private void Awake()
    {
        text = GetComponent<TMPro.TextMeshProUGUI>();
    }

    void Start()
    {
        if (animateWhenStart)
        {
            Play();
        }
    }

    private void OnEnable()
    {
        if (animateWhenStart)
        {
            Play();
        }
    }

    public void Play()
    {
        isAnimating = true;
        if (coroutine != null) { StopCoroutine(coroutine); }
        StartCoroutine(coroutine = AnimationCoroutine());
    }

    public void Finish()
    {
        isAnimating = false;
        if (coroutine != null) { StopCoroutine(coroutine); }
        var textInfo = text.textInfo;
        var visibleCharacters = textInfo.characterCount;
        text.maxVisibleCharacters = visibleCharacters;
    }

    private IEnumerator AnimationCoroutine()
    {
        isAnimating = true;
        text.ForceMeshUpdate();

        var textInfo = text.textInfo;
        var visibleCharacters = textInfo.characterCount;

        float time = 0.0f;
        float maxTime = visibleCharacters * speedPerCharacter;
        while (time < maxTime)
        {
            text.maxVisibleCharacters = Mathf.FloorToInt(time / speedPerCharacter);
            yield return null;
            time += Time.deltaTime;
        }
        text.maxVisibleCharacters = visibleCharacters;
        isAnimating = false;
        yield break;
    }
}

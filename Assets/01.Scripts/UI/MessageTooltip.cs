using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MessageTooltip : MonoBehaviour
{
    private TextMeshProUGUI _msgText;
    private Sequence _seq = null;

    private int _openCount = 0;

    private void Awake()
    {
        _msgText = transform.Find("MessageText").GetComponent<TextMeshProUGUI>();
    }

    public void ShowText(string msg, float time = 0)
    {
        _openCount++;
        _msgText.SetText(msg);
        if (_openCount > 1)
        {
            StopAllCoroutines();
            if(time > 0)
            {
                _openCount--;
                StartCoroutine(CloseCoroutine(time));
            }
            return;
        }

        DOTween.Kill(transform); //해당 트랜스폼에 있는 모든 트윈을 종료시켜버린다.
        if (_seq != null) _seq.Kill(); //시퀀스가 null이 아니면 킬

        transform.localScale = Vector3.zero;
        _seq = DOTween.Sequence();
        _seq.Append(transform.DOScale(Vector3.one * 1.2f, 0.3f));
        _seq.Append(transform.DOScale(Vector3.one * 0.9f, 0.1f));
        _seq.Append(transform.DOScale(Vector3.one, 0.1f));

        if(time > 0) //종료시간이 설정된 툴팁의 경우는 종료시간이후 툴팁이 자동으로 닫히게
        {
            StartCoroutine(CloseCoroutine(time + 0.5f));
        }
    }

    IEnumerator CloseCoroutine(float time)
    {
        yield return new WaitForSeconds(time);
        CloseText();
    }

    public void CloseText()
    {
        _openCount--;
        if (_openCount > 0) return;

        if (_seq != null) _seq.Kill();
        _seq = DOTween.Sequence();
        _seq.Append(transform.DOScale(Vector3.one * 1.2f, 0.1f));
        _seq.Append(transform.DOScale(Vector3.zero, 0.3f));
    }
    public void CloseImmediatly()
    {
        if (_seq != null) _seq.Kill();
        transform.localScale = Vector3.zero;
    }

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.T))
    //    {
    //        ShowText("테스트 툴팁", 1f);
    //    }

    //    if (Input.GetKeyDown(KeyCode.Y))
    //    {
    //        ShowText("이건 무한히 열립니다.");
    //    }
    //    if (Input.GetKeyDown(KeyCode.U))
    //    {
    //        CloseText();
    //    }
    //    if (Input.GetKeyDown(KeyCode.I))
    //    {
    //        CloseImmediatly();
    //    }
    //}
}

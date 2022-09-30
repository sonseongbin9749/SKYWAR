using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FeedBack : MonoBehaviour
{
    public abstract void CreateFeedBack();  //피드백을 재생하는거고
    public abstract void CompletePrevFeedBack();  //이전피드백을 끄는 기능

    private void OnDestroy()
    {
        CompletePrevFeedBack();
    }

    private void OnDisable()
    {
        CompletePrevFeedBack();
    }
}

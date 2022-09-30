using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Experimental.Rendering.Universal;

public class TorchAnimation : MonoBehaviour
{
    [SerializeField]
    private bool _changeRadius; //��鸱�� �ݰ���� ��鸱����?

    [SerializeField] private float _intensityRandomness;
    [SerializeField] private float _radiusRandomness;
    [SerializeField] private float _timeRandomness;

    private float _baseIntensity;
    private float _baseTime = 1f;
    private float _baseRadius;

    private Light2D _light;

    private Sequence seq = null;

    private void Awake()
    {
        _light = GetComponentInChildren<Light2D>();
        _baseIntensity = _light.intensity;
        _baseRadius = _light.pointLightOuterRadius;
    }

    private void OnDestroy()
    {
        seq?.Kill();
    }

    private void OnEnable()
    {
        ShakeLight();
    }

    private void ShakeLight()
    {        
        //��Ʈ�� �������� �̿��ؼ� ���⼭ �ѹ� Ƚ���� ��鲨��
        float targetIntensity = _baseIntensity + Random.Range(-_intensityRandomness, _intensityRandomness);
        float targetTime = _baseTime + Random.Range(-_timeRandomness, _timeRandomness);

        if (!gameObject.activeSelf)
            return;

        seq = DOTween.Sequence();
        seq.Append(DOTween.To(
            () => _light.intensity,
            value => _light.intensity = value,
            targetIntensity,
            targetTime
        ));

        //�������� ���� �ʹٸ� ���⵵ �ۼ��غ���.
        if (_changeRadius)
        {
            float targetRadius = _baseRadius + Random.Range(-_radiusRandomness, _radiusRandomness);
            seq.Join(
                DOTween.To(
                    () => _light.pointLightOuterRadius, 
                    v => _light.pointLightOuterRadius = v, 
                    targetRadius, 
                    targetTime));
        }

        seq.AppendCallback(() => ShakeLight());

        //�������� ���������� (�ѹ� ��鸰 ��������) �ٽ��ѹ� ShakeLight

    }
}

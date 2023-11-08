using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualHandler : MonoBehaviour
{
    [SerializeField]
    ParticleSystem _moveVFX;

    [SerializeField]
    SpriteRenderer _renderer;
    [SerializeField]
    float _flashTime = 0.2f;
    public void PlayMoveVFX(bool isPlay)
    {
        if (isPlay) _moveVFX?.Play(true);
        else _moveVFX?.Stop(true, ParticleSystemStopBehavior.StopEmitting);
    }

    public void Flashing(float time)
    {
        StartCoroutine(Flash(time));
    }

    IEnumerator Flash(float time)
    {
        float total = 0;
        Color orgColor = _renderer.color;
        Color transColor = orgColor;
        transColor.a = 0;
        while(total < time)
        {
            _renderer.color = transColor;
            yield return new WaitForSeconds(_flashTime);
            _renderer.color = orgColor;
            yield return new WaitForSeconds(_flashTime);
            total += _flashTime * 2;
        }
    }
}

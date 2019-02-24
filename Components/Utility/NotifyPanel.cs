using TMPro;
using UniRx.Async;
using UnityEngine;

namespace VRUtils.Components
{
[RequireComponent(typeof(Animation))]
[RequireComponent(typeof(TextMeshPro))]
public class NotifyPanel : MonoBehaviour
{
    private Animation anim;
    private TextMeshPro text;

    private void Awake()
    {
        anim = GetComponent<Animation>();
        text = GetComponent<TextMeshPro>();
        gameObject.SetActive(false);
    }

    public async void ShowNotifyPanel(string content)
    {
        if (gameObject.activeSelf == false)
        {
            gameObject.SetActive(true);
            anim.Play();
        }

        text.text = content;
        await UniTask.DelayFrame(240);
        gameObject.SetActive(false);
    }
}
}
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcessManager : MonoBehaviour
{
    private VolumeProfile profile;
    private DepthOfField depthOfField;

    private void Start()
    {
        Volume volume = GetComponent<Volume>();

        if (volume == null)
        {
            Debug.LogError("Volume��������Ȃ�");
            return;
        }

        profile = volume.profile;
        if (profile == null)
        {
            Debug.LogError("Profile���ݒ肳��Ă��Ȃ�");
            return;
        }

        if (!profile.TryGet<DepthOfField>(out depthOfField))
        {
            Debug.LogError("DepthOfField��Profile�ɑ��݂��Ȃ�");
        }
        else
        {
            // ������Ԃ��I�t�ɐݒ�
            depthOfField.active = false;
        }
    }

    public void ActiveDepthOfField(bool active)
    {
        if (depthOfField != null)
        {
            depthOfField.active = active;
        }
    }
}

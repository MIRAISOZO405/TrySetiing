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
            Debug.LogError("Volumeが見つからない");
            return;
        }

        profile = volume.profile;
        if (profile == null)
        {
            Debug.LogError("Profileが設定されていない");
            return;
        }

        if (!profile.TryGet<DepthOfField>(out depthOfField))
        {
            Debug.LogError("DepthOfFieldがProfileに存在しない");
        }
        else
        {
            // 初期状態をオフに設定
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

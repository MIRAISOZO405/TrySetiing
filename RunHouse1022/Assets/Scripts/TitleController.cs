using UnityEngine;
using UnityEngine.UI;

public class TitleController : MonoBehaviour
{
    private void Start()
    {
        this.GetComponent<InputManager>().SwitchToUI(); // ActionMap�؂�ւ�
    }

    public void OnStart()
    {
        FadeManager.Instance.LoadScene("GameScene");
    }
}

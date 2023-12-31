using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControllerManager : SingletonMonoBehaviour<ControllerManager>
{
    private Gamepad gamepad;    // ゲームパッド

    public void ShakeController(float time)
    {
        gamepad = Gamepad.current;

        if (gamepad != null)
        {
            gamepad.SetMotorSpeeds(0.5f, 0.5f);

            // 0.5秒後に振動をストップ
            Invoke("ShakeStop", time);

        }
    }

    private void ShakeStop()
    {
        gamepad = Gamepad.current;

        if (gamepad != null)
        {
            gamepad.SetMotorSpeeds(0.0f, 0.0f);
        }
    }

    // ゲーム終了時に振動を終了
    private void OnApplicationQuit()
    {
        ShakeStop();
    }
}

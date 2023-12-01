using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    public Canvas missionCanvas;
    public Canvas scoreCanvas;
    public Canvas shopCanvas;

    private void Start()
    {
        CheckAndSetCanvas(missionCanvas, true);
        CheckAndSetCanvas(scoreCanvas, true);
        CheckAndSetCanvas(shopCanvas, false);
    }

    private void CheckAndSetCanvas(Canvas canvas, bool state)
    {
        if (canvas)
            canvas.enabled = state;
        else
            Debug.Log($"{canvas.name}がアタッチされてない");
    }

    public void DisableAllCanvases()
    {
        SetAllCanvasesState(false);
    }

    public void EnableAllCanvases()
    {
        SetAllCanvasesState(true);
    }

    private void SetAllCanvasesState(bool state)
    {
        Canvas[] allCanvases = FindObjectsOfType<Canvas>();
        foreach (Canvas canvas in allCanvases)
        {
            canvas.enabled = state;
        }
    }

    public void EnableOnlyThisCanvas(Canvas targetCanvas)
    {
        SetOnlyThisCanvasState(targetCanvas, true);
    }

    public void DisableOnlyThisCanvas(Canvas targetCanvas)
    {
        SetOnlyThisCanvasState(targetCanvas, false);
    }

    private void SetOnlyThisCanvasState(Canvas targetCanvas, bool state)
    {
        Canvas[] allCanvases = FindObjectsOfType<Canvas>();
        foreach (Canvas canvas in allCanvases)
        {
            canvas.enabled = state ? (canvas == targetCanvas) : (canvas != targetCanvas);
        }
    }

    public void SetCanvas(Canvas canvas, bool active)
    {
        canvas.enabled = active;
    }
}

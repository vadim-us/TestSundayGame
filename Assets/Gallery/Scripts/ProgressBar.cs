using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Шкала прогресса 
/// </summary>
[ExecuteAlways]
public class ProgressBar : MonoBehaviour
{
    /// <summary>
    /// текущей процент
    /// </summary>
    [SerializeField, Range(0, 1)]
    private float percent;

    /// <summary>
    /// предыдушее значение процента
    /// </summary>
    private float tempPercent;

    /// <summary>
    /// число одновременных загрузок
    /// </summary>
    private int parts;

    /// <summary>
    /// процен каждой части
    /// </summary>
    private List<float> partsPercent;


    [SerializeField]
    private TextMeshProUGUI text;

    [SerializeField]
    private RectTransform progress;

    private void Update()
    {
        if (percent != tempPercent)
        {
            tempPercent = percent;
            UpdateProgress();
        }
    }

    /// <summary>
    /// Сброс
    /// </summary>
    public void ResetProgress(int parts)
    {
        partsPercent = new();
        this.parts = parts;
    }

    /// <summary>
    /// Показать или скрыть
    /// </summary>
    public void Show(bool b)
    {
        gameObject.SetActive(b);
    }

    private void OnEnable()
    {
        percent = 0;
        UpdateProgress();
    }

    /// <summary>
    /// изменение значения
    /// </summary>
    public IEnumerator Progress(UnityWebRequestAsyncOperation operation, int i)
    {
        //Debug.Log(i);
        partsPercent.Add(0);
        while (true)
        {
            if (operation.progress > 0)
            {
                //Debug.Log(i + " - " + operation.progress);

                partsPercent[i] = 1f / parts * operation.progress;

                percent = 0;
                partsPercent.ForEach(part =>
                {
                    percent += part;
                });

                if (operation.isDone) break;
            }
            yield return new WaitForSeconds(0.01f);
        }
    }

    /// <summary>
    /// обновление шкалы
    /// </summary>
    private void UpdateProgress()
    {
        text.text = $"{Mathf.FloorToInt(percent * 100)} %";
        progress.anchorMax = new Vector2(percent, 1);
    }
}

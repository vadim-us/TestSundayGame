using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// ����� ��������� 
/// </summary>
[ExecuteAlways]
public class ProgressBar : MonoBehaviour
{
    /// <summary>
    /// ������� �������
    /// </summary>
    [SerializeField, Range(0, 1)]
    private float percent;

    /// <summary>
    /// ���������� �������� ��������
    /// </summary>
    private float tempPercent;

    /// <summary>
    /// ����� ������������� ��������
    /// </summary>
    private int parts;

    /// <summary>
    /// ������ ������ �����
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
    /// �����
    /// </summary>
    public void ResetProgress(int parts)
    {
        partsPercent = new();
        this.parts = parts;
    }

    /// <summary>
    /// �������� ��� ������
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
    /// ��������� ��������
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
    /// ���������� �����
    /// </summary>
    private void UpdateProgress()
    {
        text.text = $"{Mathf.FloorToInt(percent * 100)} %";
        progress.anchorMax = new Vector2(percent, 1);
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Gear : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    private float speed = 1;

    [SerializeField]
    private Slider slider;

    [SerializeField]
    private TextMeshProUGUI speedText;

    void Awake()
    {
        slider.onValueChanged.AddListener((Single) => ChangeSpeed(Single));
    }

    void Update()
    {
        if ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) || Input.GetMouseButtonDown(0))
        {
            Vector2 position = Input.touchCount > 0 ? Input.GetTouch(0).position : Input.mousePosition;
            Ray ray = Camera.main.ScreenPointToRay(position);

            if (Physics.Raycast(ray, out RaycastHit hit) && hit.collider == gameObject.GetComponentInChildren<MeshCollider>())
            {
                Color color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
                gameObject.GetComponentInChildren<MeshRenderer>().material.color = color;
            }
        }
    }

    void ChangeSpeed(float speed)
    {
        this.speed = speed;
        speedText.text = speed.ToString("F1");
        animator.SetFloat("speed", speed);
    }
}

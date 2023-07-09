using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class Galery : MonoBehaviour
{
    [SerializeField]
    private GridLayoutGroup imagesGrid;

    [SerializeField]
    private ProgressBar progressBar;

    [SerializeField]
    [Header("������ ��������")]
    private GameObject imagePrefab;

    [SerializeField]
    private RectTransform safeArea;

    /// <summary>
    /// ���������� ������ � ������� �����
    /// </summary>
    [SerializeField]
    [Header("���������� ������ � ������� �����")]
    private ScreenOrientation orientation;

    [SerializeField]
    private ScrollRect scrollView;

    /// <summary>
    /// ������ ���������� ��������
    /// </summary>
    private static List<Image> images;

    /// <summary>
    /// ������������ ����� ��������
    /// </summary>
    private readonly int maxImages = 66;

    /// <summary>
    /// ������� ���������
    /// </summary>
    private int loadImages;

    /// <summary>
    /// ������� ����������
    /// </summary>
    private int loadingImages;

    /// <summary>
    /// ���� �� ��������� ��������
    /// </summary>
    private bool downloadComplete = true;

    /// <summary>
    /// �������� �������
    /// </summary>
    public static Image selectedImage;

    /// <summary>
    /// ������� ���������
    /// </summary>
    private static Vector2 scrollPosition;

    /// <summary>
    /// ��� ������� �����  
    /// </summary>
    private string curentSceneName;

    [Header("�������� ������")]
    /// <summary>
    /// ������� ���� ������
    /// </summary>
    [SerializeField]
    private float deadZone = 80;

    private Vector2 tapPosition;
    private Vector2 swipeDelta;
    private bool isSwiping;

    [SerializeField]
    private BigImage bigImage;


    /// <summary>
    /// ������������ �������� ������� ��� ������
    /// </summary>
    void Swipe()
    {
        if ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) || Input.GetMouseButtonDown(0))
        {
            isSwiping = true;
            tapPosition = Input.touchCount > 0 ? Input.GetTouch(0).position : Input.mousePosition;
        }
        else
        {
            if ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended) || Input.GetMouseButtonUp(0))
            {
                ResetSwipe();
            }
        }

        CheckSwipe();

        void CheckSwipe()
        {
            swipeDelta = Vector2.zero;

            if (isSwiping == true)
            {
                Vector2 pos = Input.touchCount > 0 ? Input.GetTouch(0).position : Input.mousePosition;
                swipeDelta = pos - tapPosition;
            }

            if (swipeDelta.magnitude > deadZone)
            {
                if (Mathf.Abs(swipeDelta.x) > Mathf.Abs(swipeDelta.y))
                {
                    int swipe = swipeDelta.x > 0 ? selectedImage.Id - 1 : selectedImage.Id + 1;
                    if (swipe >= 0 && swipe < images.Count)
                    {
                        bigImage.Change(images[swipe]);
                        selectedImage = images[swipe];
                    }
                }
                ResetSwipe();
            }
        }

        void ResetSwipe()
        {
            isSwiping = false;
            tapPosition = Vector2.zero;
            swipeDelta = Vector2.zero;
        }
    }


    private void Update()
    {
        if (curentSceneName == "FullImage")
        {
            // ������� ������ �����
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                SwichScene("Galery");
            }

            Swipe();
        }
    }

    /// <summary>
    /// ����� �����
    /// </summary>
    /// <param name="sceneName"></param>
    public static void SwichScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    /// <summary>
    /// ������ ������ ��������
    /// </summary>
    /// <param name="button">������ �� ������</param>
    public void StartDownload(Button button)
    {
        button.gameObject.SetActive(false);

        images = new();
        DownloadImages();
    }

    /// <summary>
    /// �������� ��������
    /// </summary>
    private void DownloadImages()
    {
        downloadComplete = false;

        progressBar.ResetProgress(loadImages);
        progressBar.Show(true);
        loadingImages = 0;

        for (int i = 0; i < loadImages; i++)
        {
            var index = images.Count + i;
            string uri = "https://data.ikppbb.com/test-task-unity-data/pics/" + (index + 1) + ".jpg";
            StartCoroutine(DownloadImage(uri, index, i));
        }
    }

    /// <summary>
    /// �������� �������� � ���������� �� � ������ �����������
    /// </summary>
    private IEnumerator DownloadImage(string uri, int index, int i)
    {
        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(uri))
        {
            var operation = uwr.SendWebRequest();
            yield return progressBar.Progress(operation, i);

            if (uwr.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(uwr.error + "\n" + uri);

            }
            else
            {
                // Get downloaded asset bundle
                images.Add(new Image(index, DownloadHandlerTexture.GetContent(uwr)));
                loadingImages++;

                DownloadComplete();
            }
        }
    }

    /// <summary>
    /// ���������� ��������
    /// </summary>
    private void DownloadComplete()
    {
        if (loadingImages == loadImages)
        {
            // ���������� ��� ����������� �� ������� � �� ��� ����������
            images.Sort((a, b) => a.Id.CompareTo(b.Id));

            if (SceneManager.GetActiveScene().name == "Menu")
                SwichScene("Galery");

            if (SceneManager.GetActiveScene().name == "Galery")
            {
                SpawnImage(images.GetRange(images.Count - 2, 2));

                // ���������� ������� ������� ���������
                var height = imagesGrid.GetComponent<RectTransform>().sizeDelta.y;
                height += imagesGrid.cellSize.y;
                imagesGrid.GetComponent<RectTransform>().sizeDelta = new Vector2(0, height);
            }

            progressBar.Show(false);
            downloadComplete = true;
        }
    }

    /// <summary>
    /// ���������
    /// </summary>
    private void Scroll(Vector2 scroll)
    {
        // ���������� �������
        scrollPosition = imagesGrid.GetComponent<RectTransform>().anchoredPosition;
        //Debug.Log(scrollPosition);

        // ������ �������� �������������� �������� ����� ��������� ����� ����� �� ����� ���� ��� �� ��� ���������
        // � ��� �������� �� ������������� �������� ����� ������ �� ������ ���� �� ���
        if (scroll.y < 0.1f && downloadComplete == true && images.Count < maxImages)
        {
            loadImages = 2;
            DownloadImages();
        }
    }

    private void Awake()
    {
        Application.targetFrameRate = 60;
        Screen.orientation = orientation;

        curentSceneName = SceneManager.GetActiveScene().name;

        if (scrollView != null)
            scrollView.onValueChanged.AddListener((Vector2) => Scroll(Vector2));

        switch (curentSceneName)
        {
            case "Menu":
                {
                    // ����������� ����� �������� ��� �������� �� ������ ������� ���������� ���� ������
                    float width = safeArea.rect.width / 2;
                    loadImages = Mathf.CeilToInt(safeArea.rect.height / width) * 2;
                    //Debug.Log(loadImages);
                }
                break;
            case "Galery":
                {
                    SmallImagesSize();

                    // ������������� ������� ���������
                    imagesGrid.GetComponent<RectTransform>().anchoredPosition = scrollPosition;
                }

                SpawnImage();
                break;
        }
    }

    /// <summary>
    /// ���������� �������� � �����
    /// </summary>
    /// <param name="images">������ ��������</param>
    private void SpawnImage(List<Image> images = null)
    {
        // ���� ������ �������� �� ����� ������������ ��� �����������
        images ??= Galery.images;

        foreach (var img in images)
        {
            SmallImage imageObject = Instantiate(imagePrefab, imagesGrid.transform).GetComponent<SmallImage>();
            imageObject.Setup(img);
        }
    }

    /// <summary>
    /// ����������� ������� �������� ��������
    /// </summary>
    private void SmallImagesSize()
    {
        // ����� ���� �������� �� ����� ������������ ���������� (������ ���� ��������)
        // ������� ����� �������� ������ ���� ����������� ������� ������ �� ������ ������������ �� �� ������ � �� ������ ������
        if (Screen.orientation == ScreenOrientation.LandscapeLeft || Screen.orientation == ScreenOrientation.LandscapeRight)
        {
            Size(imagesGrid.transform.parent.GetComponent<RectTransform>().rect.height / 2);
        }
        else
        {
            Size(imagesGrid.GetComponent<RectTransform>().rect.width / 2);
        }

        void Size(float w)
        {
            float width = w;
            float height = Mathf.Ceil(images.Count / 2) * width;

            imagesGrid.GetComponent<RectTransform>().sizeDelta = new Vector2(0, height);
            imagesGrid.cellSize = new Vector2(width, width);
        }
    }
}
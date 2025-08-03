using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class CompletionChecker : MonoBehaviour
{
    public ProgressInfo progressInfo;

    [Header("Cutscene Settings")]
    public VideoPlayer videoPlayer;
    public GameObject cutsceneCanvas; // Assign a Canvas with a RawImage

    private string videoFileName;

    public string nextScene = "SubmarineScene";

    private bool cutsceneStarted = false;

    public RawImage videoRawImage;

    void Start()
    {
        if (videoRawImage != null)
            videoRawImage.enabled = false;
    }

    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        #if UNITY_EDITOR_LINUX
            videoFileName = "ending_vp8.webm";
        #else
            videoFileName = "ending.mp4";
        #endif

        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached += OnCutsceneFinished;
        }
    }

    void Update()
    {
        if (!cutsceneStarted && progressInfo != null && progressInfo.IsFullyCompleted())
        {
            cutsceneStarted = true;
            progressInfo.endingTriggered = true; // Mark to avoid retrigger
            PlayCutscene();
        }
    }

    void PlayCutscene()
    {
        Debug.Log("🎬 Playing cutscene...");

        if (cutsceneCanvas != null)
            cutsceneCanvas.SetActive(true);

        if (videoRawImage != null)
            videoRawImage.enabled = true;

        string path = System.IO.Path.Combine(Application.streamingAssetsPath, videoFileName);

        if (videoPlayer != null)
        {
            videoPlayer.source = VideoSource.Url;
            videoPlayer.url = path;
            videoPlayer.Play();
        }
        else
        {
            Debug.LogWarning("⚠️ VideoPlayer not assigned, skipping to next scene.");
            LoadNextScene();
        }
    }

    void OnCutsceneFinished(VideoPlayer vp)
    {
        Debug.Log("✅ Cutscene done. Loading next scene...");

        if (videoRawImage != null)
            videoRawImage.enabled = false;

        if (cutsceneCanvas != null)
            cutsceneCanvas.SetActive(false);

        SceneManager.LoadScene(nextScene);
    }


    void LoadNextScene()
    {
        Debug.Log("✅ Cutscene done. Loading next scene...");
        SceneManager.LoadScene(nextScene);
    }
}

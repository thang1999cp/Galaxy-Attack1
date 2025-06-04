using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
    public Slider progressBar;
    public float loadingDuration = 3f;

    private float timer = 0f;

    void Start()
    {
        progressBar.value = 0f;
        StartCoroutine(nameof(LoadingBar));
    }

    private IEnumerator LoadingBar()
    {
        float elapsed = 0f;

        while (elapsed < loadingDuration)
        {
            elapsed += Time.deltaTime;
            float progress = Mathf.Clamp01(elapsed / loadingDuration);
            progressBar.value = progress;
            yield return null;
        }
        progressBar.value = 1f;
        UIManager.instance.homePanel.SetActive(true);
        gameObject.SetActive(false);
    }
}

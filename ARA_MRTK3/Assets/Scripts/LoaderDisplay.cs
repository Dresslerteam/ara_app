using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

public class LoaderDisplay : MonoBehaviour
    {
        [SerializeField] private GameObject loader;
        public static LoaderDisplay Instance;

        private void Awake()
        {
            if (loader == null)
            {
                loader = GameObject.Find("Loader");
            }
            // Singleton
            if (Instance != null && Instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                Instance = this;
            }
        }

        public void ShowLoader(bool show)
        {
            loader.SetActive(show);
        }
        public Task ShowLoader(float seconds)
        {
            var tcs = new TaskCompletionSource<object>();

            StartCoroutine(ShowLoaderForSeconds(seconds, () => tcs.SetResult(null)));

            return tcs.Task;
        }

        private IEnumerator ShowLoaderForSeconds(float seconds, Action onCompleted)
        {
            ShowLoader(true);
            yield return new WaitForSeconds(seconds);
            ShowLoader(false);

            onCompleted?.Invoke();
        }
    }
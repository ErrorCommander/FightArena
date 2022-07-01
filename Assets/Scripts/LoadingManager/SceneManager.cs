using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LoadingScreen
{
    public class SceneManager : MonoBehaviour
    {
        public Action<float> OnChangeLoadingProgress;

        private const int IndexSceneLoadingScreen = 0;
        private const int IndexSceneLoadingDefault = 1;
        private static IdentifierScene _loadingScene;

        /// <summary>
        /// Load Scene with load screen.
        /// </summary>
        /// <param name="sceneBildIndex"> Index loading scene in bild settings.</param>
        public static void LoadScene(int sceneBildIndex)
        {
            _loadingScene = new IdentifierScene(sceneBildIndex);
            LoadLoadingScreenScene();
        }

        /// <summary>
        /// Load Scene with load screen.
        /// </summary>
        /// <param name="sceneName"> Name loading scene in bild settings.</param>
        public static void LoadScene(string sceneName)
        {
            _loadingScene = new IdentifierScene(sceneName);
            LoadLoadingScreenScene();
        }

        private static void LoadLoadingScreenScene() => UnityEngine.SceneManagement.SceneManager.LoadScene(IndexSceneLoadingScreen);

        private void Start()
        {
            AsyncOperation asyncLoadScene;

            if (_loadingScene == null)
                asyncLoadScene = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(IndexSceneLoadingDefault);
            else if (_loadingScene.LoadByName)
                asyncLoadScene = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(_loadingScene.Name);
            else
                asyncLoadScene = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(_loadingScene.Index);

            StartCoroutine(LoadingScene(asyncLoadScene));
        }

        private IEnumerator LoadingScene(AsyncOperation asyncLoadScene)
        {
            while (!asyncLoadScene.isDone)
            {
                float progress = asyncLoadScene.progress / 0.9f;
                OnChangeLoadingProgress?.Invoke(progress);
                Debug.Log("Loading progress " + progress);
                yield return null;
            }

            _loadingScene = null;
        }

        private class IdentifierScene
        {
            public string Name { get; private set; }
            public int Index { get; private set; }
            public bool LoadByName { get; private set; }

            public IdentifierScene(int index)
            {
                Index = index;
                LoadByName = false;
            }

            public IdentifierScene(string name)
            {
                Name = name;
                LoadByName = true;
            }
        }
    }
}

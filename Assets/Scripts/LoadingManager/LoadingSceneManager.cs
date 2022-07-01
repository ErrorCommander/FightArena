using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace LoadingScreen
{
    public class LoadingSceneManager : MonoBehaviour
    {
        public void LoadScene(int indexScene) => SceneManager.LoadScene(indexScene);
        public void LoadScene(string nameScene) => SceneManager.LoadScene(nameScene);
        public void QuitGame() => Application.Quit();
    }
}

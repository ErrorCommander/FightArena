using UnityEngine;
using UnityEngine.UI;

namespace LoadingScreen
{
    public class LoadingScreen : MonoBehaviour
    {
        [SerializeField] private Text _loadingText;
        [SerializeField] private Image _image;
        [SerializeField] private SceneManager _loadingScene;

        private void Awake()
        {
            _image.fillAmount = 0;
            _loadingText.text = string.Format("Loading\t 0%");
            _loadingScene.OnChangeLoadingProgress += Refresh;
        }

        private void Refresh(float progress)
        {
            _loadingText.text = string.Format("Loading\t{0:0}%", progress * 100);
            _image.fillAmount = progress;
        }
    }
}

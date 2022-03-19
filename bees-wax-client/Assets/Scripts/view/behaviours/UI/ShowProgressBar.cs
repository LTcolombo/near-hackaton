using model;
using model.data;
using utils.injection;
using UnityEngine;

namespace view.behaviours.UI
{
    public class ShowProgressBar : InjectableBehaviour
    {
        public GameObject Prefab;

        GameObject _canvas;

        [Inject]
        ActionProgressModel _progress;

        GameObject _progressBar;

        void Start()
        {
            _canvas = GameObject.Find("ProgressBarLayer"); //todo OMG
        }

        // Update is called once per frame
        void Update()
        {
            var progress = _progress.Get(gameObject.GetInstanceID());

            if (progress != null && progress.Faction == Faction.Player && progress.Progress < progress.MaxProgress &&
                _progressBar == null)
            {
                CreateProgressBar();
            }
            else if (_progressBar != null && (progress == null || progress.Progress >= progress.MaxProgress))
            {
                DestroyProgressBar();
            }
            else if (progress != null && _progressBar != null)
            {
                UpdateProgressBar(progress);
            }
        }

        private void CreateProgressBar()
        {
            _progressBar = Instantiate(Prefab, _canvas.transform);
            UpdateProgressBar();
        }

        private void UpdateProgressBar(ActionProgress value = null)
        {
            var cameraPlane = new Plane(Camera.main.transform.position, Camera.main.transform.position.normalized * 5);
            _progressBar.SetActive(cameraPlane.GetSide(transform.position));
            if (!_progressBar.activeSelf) return;

            _progressBar.transform.position = Camera.main.WorldToScreenPoint(transform.position);

            if (value == null)
                return;

            var fill = _progressBar.transform.GetChild(1);
            fill.localScale = new Vector3(value.Progress / value.MaxProgress, 1, 1);
        }

        private void DestroyProgressBar()
        {
            Destroy(_progressBar);
        }

        void OnDestroy()
        {
            if (_progressBar != null)
            {
                DestroyProgressBar();
            }
        }
    }
}
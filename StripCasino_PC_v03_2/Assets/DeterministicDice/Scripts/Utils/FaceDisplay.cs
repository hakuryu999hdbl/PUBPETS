using System.Collections.Generic;
using UnityEngine;

namespace MADD
{
    public class FaceDisplay : MonoBehaviour
    {
        #region member variables

        public GameObject _displayCanvas;
        public float _offset = 5f;

        private Dice _dice;
        private List<CanvasGroup> _uiElements = new List<CanvasGroup>();

        #endregion

        void Start()
        {
            _dice = GetComponentInChildren<Dice>();
            // generate faces
            Transform[] faces = _dice._fakeFacePositions;
            for (int i = 0; i < faces.Length; i++)
            {
                // setup
                GameObject go = Instantiate(_displayCanvas, faces[i]);
                CanvasGroup cg = go.GetComponentInChildren<CanvasGroup>();
                _uiElements.Add(cg);
                go.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = (i + 1).ToString();
                // offset display
                Vector3 direction = (go.transform.position - transform.position).normalized;
                go.transform.position += direction * _offset;
            }
        }

        void Update()
        {
            // fade and rotate
            foreach (CanvasGroup face in _uiElements)
            {
                // get topmost face
                Transform faceIndex = _dice.FindTopFace();

                // face camera
                face.transform.LookAt(Camera.main.transform);
                face.transform.Rotate(0, 180, 0);

                // fade out
                float y = face.transform.position.y;
                float yMin = transform.position.y;
                float yMax = faceIndex.position.y + _offset;

                if (yMax > yMin)
                {
                    float percent = (y - yMin) / (yMax - yMin);
                    face.alpha = Mathf.Clamp01(Mathf.Sin(percent));
                    face.transform.localScale = _displayCanvas.transform.localScale * percent;
                }
            }
        }
    }
}

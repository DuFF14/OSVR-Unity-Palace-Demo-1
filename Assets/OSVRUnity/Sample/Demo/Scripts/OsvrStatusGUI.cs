using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace OSVR
{
    namespace Unity
    {
        public class OsvrStatusGUI : MonoBehaviour
        {
            public Canvas canvas;
            public Text osvrStatusText;

            void Awake()
            {
                if(canvas == null)
                {
                    canvas = GetComponentInChildren<Canvas>();
                }
                if(osvrStatusText == null)
                {
                    osvrStatusText = GetComponentInChildren<Text>();
                }
            }
            // Use this for initialization
            void Start()
            {

            }

            // Update is called once per frame
            void OnGUI()
            {
                if(ClientKit.instance == null)
                {
                    osvrStatusText.text = "There is no ClientKit in the scene. OSVR requires a ClientKit object.";
                    osvrStatusText.text = "There is no ClientKit in the scene. OSVR requires a ClientKit object.";
                }
            }
        }
    }
}

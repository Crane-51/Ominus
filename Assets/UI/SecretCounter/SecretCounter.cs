using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Secret
{
    public class SecretCounter : MonoBehaviour
    {
        private const string SecretText = "{0}/{1}";

        [SerializeField] private GameObject secretUI;
        //public float ScaleWidth;
        //public float ScaleHeight;

        private static List<string> secretsId;
        private static GameObject SpawnedSecretUI { get; set; }
        private static int TotalCountOfSecret { get; set; }

        // Use this for initialization
        void Awake()
        {
            secretsId = new List<string>();
            TotalCountOfSecret = GameObject.FindGameObjectsWithTag("Secret").Length;
			//var width = Screen.width / ScaleWidth;
			//var height = Screen.height / ScaleHeight;

			//SecretUI.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
			//SecretUI.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);

			//SpawnedSecretUI = Instantiate(secretUI, transform);
			SpawnedSecretUI = secretUI;

            UpdateText();
        }

        public static void SecretFound(string secretFound)
        {
            if (!secretsId.Contains(secretFound))
            {
                secretsId.Add(secretFound);
                UpdateText();
            }
        }

        private static void UpdateText()
        {
            SpawnedSecretUI.GetComponent<Text>().text = string.Format(SecretText , secretsId.Count, TotalCountOfSecret);
        }
    }
}

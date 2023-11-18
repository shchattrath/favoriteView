using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace CesiumForUnity {
    public class TextHandler : MonoBehaviour {

        public CesiumGeoreference geoRef;
        public TMP_InputField longitudeIn;
        public TMP_InputField latitudeIn;
        public TMP_InputField heightIn;
        public Button button;
        public Button saveRotation;
        public Button moveToSaved;
        public GameObject dynCamera;
        public string savedRotation;
        public string savedUnParsedText;

        /**
        * FORMAT FOR STORING VIEW INFORMATION


        * SAVING LONGITUDE,LATITUDE,HEIGHT IN STRING 
        *~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        * savedUnParsedText = "(double) LONGITUDE, (double) LATITUDE, (double) HEIGHT)";
        *~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

        * SAVING ROTATION
        *~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        * savedRotation = "(double) x, (double) y, (double) z, (double) w"
        * ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        *
        *
        */
        void Start() {
            button.onClick.AddListener(Teleport);
            saveRotation.onClick.AddListener(SaveView);
            moveToSaved.onClick.AddListener(viewSavedView);
        }

        void viewSavedView() {
            string[] viewArray = savedUnParsedText.Split(',');
            if (viewArray.Length >= 3 &&
                double.TryParse(viewArray[0], out double latitude) &&
                double.TryParse(viewArray[1], out double longitude) &&
                double.TryParse(viewArray[2], out double height)) {
                setView(latitude, longitude, height, dynCamera.transform, savedRotation);
            } else {
                Debug.LogError("Failed to parse saved view parameters.");
            }
        }

        void SaveView() {
            savedRotation = QuaternionToString(dynCamera.transform.rotation);
            locationSaver(geoRef.latitude, geoRef.longitude, geoRef.height);
        }

        void locationSaver(double latitude, double longitude, double height) {
            savedUnParsedText = $"{latitude},{longitude},{height}";
        }

        // Update is called once per frame
        void Teleport() {
            if (TryParseLocationInput(longitudeIn.text, out float longitude) &&
                TryParseLocationInput(latitudeIn.text, out float latitude) &&
                float.TryParse(heightIn.text, out float height)) {
                // Parsing successful
                setView((double)longitude, (double)latitude, (double)height, dynCamera.transform, savedRotation);
            } else {
                Debug.LogError("Invalid input for longitude, latitude, or height.");
            }
        }

        bool TryParseLocationInput(string input, out float value) {
            value = 0;
            if (string.IsNullOrEmpty(input)) return false;

            string number = input.Substring(0, input.Length - 1);
            if (float.TryParse(number, out value)) {
                if (input.EndsWith("W") || input.EndsWith("w") || input.EndsWith("S") || input.EndsWith("s")) {
                    value = -value;
                }
                return true;
            }
            return false;
        }

        public static string QuaternionToString(Quaternion quaternion) {
            return $"{quaternion.x},{quaternion.y},{quaternion.z},{quaternion.w}";
        }

        public static Quaternion StringToQuaternion(string quaternionString) {
            string[] values = quaternionString.Split(',');
            if (values.Length != 4) {
                throw new System.FormatException("String format is not correct for Quaternion parsing.");
            }

            return new Quaternion(
                float.Parse(values[0]),
                float.Parse(values[1]),
                float.Parse(values[2]),
                float.Parse(values[3])
            );
        }

        public void setView(double latitude, double longitude, double height, Transform transform, string quaternionInfo) {
            geoRef.SetOriginLongitudeLatitudeHeight(longitude, latitude, height);
            Quaternion newRotation = StringToQuaternion(quaternionInfo);
            setCameraRotate(newRotation);
        }

        public void setCameraRotate(Quaternion rotation) {
            Transform camTransform = dynCamera.transform;
            camTransform.rotation = rotation;
        }
    }
}

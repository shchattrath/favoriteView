using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
namespace CesiumForUnity {
public class TextHandler : MonoBehaviour
{

    public CesiumGeoreference geoRef;   
    public TMP_InputField longitudeIn;
    public TMP_InputField latitudeIn;
    public TMP_InputField heightIn;
    public Button button;
    public GameObject camera;
    // Start is called before the first frame update
    void Start()
    {
        button.onClick.AddListener(Teleport);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void Teleport()
    {
    
        string longitudeText  = longitudeIn.text;
        string latitudeText  = latitudeIn.text;
        string heightText  = heightIn.text;
        string longitudeParsed = " ";
        string latitudeParsed = " ";

        if (!string.IsNullOrEmpty(longitudeText) && !string.IsNullOrEmpty(latitudeText)) {
            longitudeParsed = longitudeText.Substring(0, longitudeText.Length - 1);
            latitudeParsed = latitudeText.Substring(0, latitudeText.Length - 1);
            latitudeText = latitudeText.Substring(latitudeText.Length-1, latitudeText.Length);
            longitudeText = longitudeText.Substring(longitudeText.Length-1, longitudeText.Length);
        }

        float longitude = 0, latitude = 0, height = 0;


        if (float.TryParse(longitudeText, out longitude) && float.TryParse(latitudeText, out latitude) && float.TryParse(heightText, out height )) {
            // Parsing successful, longitude, height, latitude contain the parsed float values
            Debug.Log("Parsed Longitude: " + longitude);
            Debug.Log("Parsed Latitude: " + latitude);
            Debug.Log("Parsed Height: "  + height);
        } else {
            // Parsing failed, inputText is not a valid float
            Debug.LogError("Invalid input for longitude, height, or latitude.");
        }
     

        // Check if longitude is west (W) and make it negative
        if (longitudeText.EndsWith("W") || longitudeText.EndsWith("w")) {
            longitude = -longitude;
        }

        // Check if longitude is east (E) and make it positive
        else if (longitudeText.EndsWith("E") || longitudeText.EndsWith("e")) {
            longitude = Mathf.Abs(longitude);
        }

        // Check if latitude is south (S) and make it negative
        if (latitudeText.EndsWith("S") || latitudeText.EndsWith("s")) {
            latitude = -latitude;
        }

        // Check if latitude is north (N) and make it positive
        else if (latitudeText.EndsWith("N") || latitudeText.EndsWith("n")) {
            latitude = Mathf.Abs(latitude);
        }


        geoRef.SetOriginLongitudeLatitudeHeight((double)longitude, (double)latitude, (double)height);




    }
}
}

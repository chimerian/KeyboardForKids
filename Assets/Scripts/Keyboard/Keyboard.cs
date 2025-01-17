using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Keyboard : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI nextKeyText;

    private Score Score { get; set; }
    private SoundManager SoundManager { get; set; }

    private string currentKey;
    private bool wasKeyPress;

    private readonly string[] keys = new string[] {
        "q", "w", "e", "r", "t", "y", "u", "i", "o", "p",
        "a", "s", "d", "f", "g", "h", "j", "k", "l",
        "z", "x", "c", "v", "b", "n", "m"
    };

    private Dictionary<string, Key> keysObjects;

    private void Start() {
        ConfigureObjects();
        FindKeysObjects();
        GenerateNextKey();
    }

    private void Update() {
        CheckKeyboard();

        if (Input.GetKeyDown(KeyCode.Escape)) {
            Application.Quit();
        }
    }

    private void ConfigureObjects() {
        Score = FindObjectOfType<Score>();
        SoundManager = FindObjectOfType<SoundManager>();
    }

    private void FindKeysObjects() {
        keysObjects = new Dictionary<string, Key>();
        GameObject[] keysGameObjects = GameObject.FindGameObjectsWithTag("Key");

        foreach (GameObject keyGameObject in keysGameObjects) {
            AddKeyObject(keyGameObject);
        }
    }

    private void AddKeyObject(GameObject keyGameObject) {
        string name = keyGameObject.name;
        Key keyObject = keyGameObject.GetComponent<Key>();
        if (name != string.Empty && !keysObjects.ContainsKey(name)) {
            keysObjects.Add(name.ToLower(), keyObject);
        }
    }

    private void GenerateNextKey() {
        if (!string.IsNullOrEmpty(currentKey)) {
            Image PreviousImageComponent = keysObjects[currentKey].ImageComponent;
            PreviousImageComponent.color = Color.white;
        }

        int randomNumber = Random.Range(0, keys.Length);
        currentKey = keys[randomNumber];
        nextKeyText.text = currentKey.ToUpper();
        Image currentImageComponent = keysObjects[currentKey].ImageComponent;
        currentImageComponent.color = Color.yellow;
        SoundManager.PlayVoice(currentKey);
    }

    private void CheckKeyboard() {
        bool isCurrentKeyPress = IsCurrentKeyPress();
        if (!isCurrentKeyPress && wasKeyPress) {
            wasKeyPress = false;
            GenerateNextKey();
            return;
        }

        if (!isCurrentKeyPress || wasKeyPress) {
            return;
        }

        wasKeyPress = true;
        bool isCorrectKey = Input.GetKeyDown(currentKey);
        if (isCorrectKey) {
            Score.GoodAnswer();
        } else {
            Score.BadAnswer();
        }
    }

    private static bool IsCurrentKeyPress() {
        return Input.anyKeyDown && !(Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2));
    }
}
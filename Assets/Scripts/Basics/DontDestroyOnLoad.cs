using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour {
    public GameObject[] objects;

    // Start is called before the first frame update
    private void Awake() {
        foreach (var element in objects) {
            DontDestroyOnLoad(element);
        }
    }
}
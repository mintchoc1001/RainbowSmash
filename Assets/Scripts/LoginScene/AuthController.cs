using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class AuthController : MonoBehaviour
{
    [Header("Login")]
    public InputField idInputField;
    public InputField passwordInputField;

    [Header("Info")]
    public Text infoText;

    // Start is called before the first frame update
    void Start()
    {
        AuthManager.Instance.InitializeFirebase();
    }

    public void CreateUser()
    {
        string id = idInputField.text;
        string password = passwordInputField.text;

        string email = id + "@naver.com";

        AuthManager.Instance.CreateUser(email, password);
    }

    public void LogIn()
    {
        string id = idInputField.text;
        string password = passwordInputField.text;
        
        string email = id + "@naver.com";

        AuthManager.Instance.LogIn(email, password);
    }

    // Update is called once per frame
    void Update()
    {
        infoText.text = AuthManager.Instance.info;
    }

    public void OnQuit()
    {
        Application.Quit();
    }
}

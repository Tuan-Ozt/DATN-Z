using Assets.HeroEditor.FantasyInventory.Scripts.Interface.Elements;
using System.Collections;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using static MenuManager;

public class AuthManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject registerPanel;
    public GameObject loginPanel;

    [Header("Register UI")]
    public TMP_InputField registerUsername;
    public TMP_InputField registerEmail;
    public TMP_InputField registerPassword;

    [Header("Login UI")]
    public TMP_InputField loginEmail;
    public TMP_InputField loginPassword;

    private string apiUrl = "https://localhost:7124/api/Account";
    private Coroutine tokenCheckCoroutine;  // token

    private void Start()
    {
        // Nếu đã login và có token, bắt đầu kiểm tra token định kỳ
        if (PlayerPrefs.HasKey("token"))
        {
            tokenCheckCoroutine = StartCoroutine(TokenChecker());
        }
    }
    public static AuthManager Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    IEnumerator TokenChecker()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f); // kiểm tra mỗi 10 giây
            yield return StartCoroutine(GetUserProfile()); // gọi API kiểm tra token
        }
    }

    public void OnRegisterClick()
    {
        StartCoroutine(Register());
    }

    public void OnLoginClick()
    {
        StartCoroutine(Login());
    }
    //Đăng Ký

    IEnumerator Register()
    {
        RegisterDto registerDto = new RegisterDto
        {
            Name = registerUsername.text,
            Email = registerEmail.text,
            Password = registerPassword.text
        };

        string json = JsonUtility.ToJson(registerDto);

        UnityWebRequest request = new UnityWebRequest(apiUrl + "/register", "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Đăng ký thành công: " + request.downloadHandler.text);

            registerPanel.SetActive(false);
            loginPanel.SetActive(true);
        }
        else
        {
            Debug.LogError("Lỗi đăng ký: " + request.downloadHandler.text);
        }
    }
    //Đăng Nhập
    IEnumerator Login()
    {
        LoginDto loginDto = new LoginDto
        {
            Email = loginEmail.text,
            Password = loginPassword.text
        };

        string json = JsonUtility.ToJson(loginDto);

        UnityWebRequest request = new UnityWebRequest(apiUrl + "/login", "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string responseJson = request.downloadHandler.text;
            Debug.Log("Login thành công: " + responseJson);

            LoginResponse loginResponse = JsonUtility.FromJson<LoginResponse>(responseJson);

            PlayerPrefs.SetInt("accountId", loginResponse.accountId);
            PlayerPrefs.SetString("token", loginResponse.token);
            PlayerPrefs.Save();
            //thêm token
            PlayerDataHolder1.AccountId = loginResponse.accountId;
            PlayerDataHolder1.Token = loginResponse.token;

            StartCoroutine(TokenChecker());  // bắt đầu kiểm tra token định kỳ

            SceneManager.LoadScene("MenuGame");
        }
        else
        {
            Debug.LogError("Lỗi đăng nhập: " + request.downloadHandler.text);
        }
    }
    //Send Token , để Check
    public async Task<UnityWebRequest> SendAuthRequest(string url)
    {
        UnityWebRequest req = UnityWebRequest.Get(url);
        req.SetRequestHeader("Authorization", "Bearer " + PlayerDataHolder1.Token);
        await req.SendWebRequest();

        if (req.responseCode == 401)
        {
            // Token không hợp lệ - bị login trùng
            Debug.Log("Bị kick về login do đăng nhập trùng!");
            SceneManager.LoadScene("LoginScene");
        }

        return req;
    }

    //  gọi API có xác thực token , nếu trùng Token  , đẩy client đầu về Scene Login
    public IEnumerator GetUserProfile()
    {
        string token = PlayerPrefs.GetString("token");
        if (string.IsNullOrEmpty(token))
        {
            Debug.LogError("Token không tồn tại, vui lòng đăng nhập lại.");
            yield break;
        }

        UnityWebRequest request = UnityWebRequest.Get(apiUrl + "/profile");
        request.SetRequestHeader("Authorization", "Bearer " + token);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            //Debug.Log("Dữ liệu user: " + request.downloadHandler.text);
        }
        else
        {
            Debug.LogError($"Lỗi lấy dữ liệu user. Response code: {request.responseCode}, Error: {request.error}, Body: {request.downloadHandler.text}");

            if (request.responseCode == 401)
            {
                Debug.LogWarning("Token không hợp lệ hoặc đã đăng nhập ở nơi khác.");

                PlayerPrefs.DeleteKey("token");
                PlayerPrefs.DeleteKey("accountId");
                PlayerPrefs.Save();

                if (tokenCheckCoroutine != null)
                {
                    StopCoroutine(tokenCheckCoroutine);
                }

                SceneManager.LoadScene("Login");
            }
        }

    }
    //Load Dữ liệu CharacterData từ database qua API
    public IEnumerator LoadCharacterData(int accountId)
    {
        string url = apiUrl + "/get-character/" + accountId;
        UnityWebRequest request = UnityWebRequest.Get(url);
        request.SetRequestHeader("Authorization", "Bearer " + PlayerPrefs.GetString("token"));
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            var responseJson = request.downloadHandler.text;
            Debug.Log("Lấy dữ liệu nhân vật thành công: " + responseJson);
            CharacterResponse characterResponse = JsonUtility.FromJson<CharacterResponse>(responseJson);
            Debug.Log("Character JSON: " + characterResponse.characterJson);
        }
        else
        {
            Debug.LogError("Lỗi khi lấy dữ liệu nhân vật: " + request.error);
        }

    }

    // Gọi API , lấy dữ liệu CharacterData từ database xuống
    public IEnumerator SaveCharacterToServer(string characterJson)
    {
        int accountId = PlayerPrefs.GetInt("accountId");
        if (accountId == 0)
        {
            Debug.LogError("AccountId chưa được lưu, không thể lưu nhân vật.");
            yield break;
        }

        SaveCharacterDto dto = new SaveCharacterDto
        {
            AccountId = accountId,
            CharacterJson = characterJson
        };

        string json = JsonUtility.ToJson(dto);

        UnityWebRequest request = new UnityWebRequest(apiUrl + "/save-character", "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", "Bearer " + PlayerPrefs.GetString("token")); // nếu API cần token

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Lưu nhân vật lên server thành công.");
        }
        else
        {
            Debug.LogError("Lỗi khi lưu nhân vật: " + request.downloadHandler.text);
        }
    }
}

[System.Serializable]
public class CharacterSimpleResponse
{
    public string name;
    public string characterJson;
}

[System.Serializable]
    public class SaveCharacterDto
    {
        public int AccountId;
        public string CharacterJson;
    }


[System.Serializable]
public class LoginResponse
{
    public string message;
    public int accountId;
    public string token;
}

[System.Serializable]
public class RegisterDto
{
    public string Name;
    public string Email;
    public string Password;
}

[System.Serializable]
public class LoginDto
{
    public string Email;
    public string Password;
}
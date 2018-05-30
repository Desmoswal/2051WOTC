using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour {


    [SerializeField]
    RectTransform fuelFill;

    [SerializeField]
    RectTransform healthbarFill;

    [SerializeField]
    Text ammoText;

    [SerializeField]
    GameObject pauseMenu;

    [SerializeField]
    GameObject scoreboard;

    [SerializeField]
    GameObject minimapCamera;

    private Player player;
    private PlayerController controller;
    private WeaponManager weaponManager;

    public void SetPlayer(Player _player)
    {
        player = _player;
        controller = player.GetComponent<PlayerController>();
        weaponManager = player.GetComponent<WeaponManager>();
    }

    private void Start()
    {
        PauseMenu.isOn = false;
        minimapCamera = (GameObject)Instantiate(minimapCamera);
        scoreboard = GameManager.instance.scoreboard;
    }

    private void Update()
    {
        if (player.isLocalPlayer)
        {
            Vector3 newPosition = player.transform.position;
            newPosition.y = player.transform.position.y+10;
            minimapCamera.transform.position = newPosition;
        }

        SetFuelAmount(controller.GetThrusterFuelAmount());
        SetHealthAmount(player.GetHealthPct());
        SetAmmoAmount(weaponManager.GetCurrentWeapon().bullets);

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseMenu();
        }

        if(Input.GetKeyDown(KeyCode.Tab))
        {
            scoreboard.SetActive(true);
        }
        else if(Input.GetKeyUp(KeyCode.Tab))
        {
            scoreboard.SetActive(false);
        }
    }

    public void TogglePauseMenu()
    {
        pauseMenu.SetActive(!pauseMenu.activeSelf);
        PauseMenu.isOn = pauseMenu.activeSelf;
    }

    void SetFuelAmount(float _amount)
    {
        fuelFill.localScale = new Vector3(_amount, 1f, 1f);
    }

    void SetHealthAmount(float _amount)
    {
        healthbarFill.localScale = new Vector3(_amount, 1f, 1f);

        if(player.GetHealthPct() >= 1)
        {
            healthbarFill.GetComponent<Image>().color = Color.green;
        }
        else if(player.GetHealthPct() <= 0.5f && player.GetHealthPct() > 0.25f)
        {
            healthbarFill.GetComponent<Image>().color = Color.yellow;
        }
        else if(player.GetHealthPct() <= 0.25f)
        {
            healthbarFill.GetComponent<Image>().color = Color.red;
        }
    }

    void SetAmmoAmount(int _amount)
    {
        ammoText.text = _amount.ToString();
    }
}

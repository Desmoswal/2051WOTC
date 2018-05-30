using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour {

    [SerializeField]
    private float speed = 5f;

    [SerializeField]
    private float lookSensitivity = 3f;

    [SerializeField]
    private float thrusterForce = 1000f;

    [SerializeField]
    private float thrusterFuelBurnSpeed = 1f;

    [SerializeField]
    private float thrusterFuelRegenSpeed = 0.3f;
    private float thrusterFuelAmount = 1f;

    [SerializeField]
    AudioSource audioSource;

    [SerializeField]
    AudioClip clip;

    public float GetThrusterFuelAmount()
    {
        return thrusterFuelAmount;
    }

    private PlayerMotor motor;

    void Start()
    {
        motor = GetComponent<PlayerMotor>();
        //audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (PauseMenu.isOn)
        {
            if (Cursor.lockState != CursorLockMode.None)
                Cursor.lockState = CursorLockMode.None;
            if (Cursor.visible == false)
                Cursor.visible = true;
            
            motor.Move(Vector3.zero);
            motor.Rotate(Vector3.zero);
            motor.RotateCamera(0f);
            motor.ApplyThruster(Vector3.zero);

            return;
        }
            

        if(Cursor.lockState != CursorLockMode.Locked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        //Calculate movement
        float _xMov = Input.GetAxis("Horizontal");
        float _zMov = Input.GetAxis("Vertical");

        Vector3 _movHorizontal = transform.right * _xMov;
        Vector3 _movVertical = transform.forward * _zMov;

        Vector3 _velocity = (_movHorizontal + _movVertical) * speed;

        motor.Move(_velocity);

        //Calculate rotation
        float _yRot = Input.GetAxisRaw("Mouse X");

        Vector3 _rotation = new Vector3(0f, _yRot, 0f) * lookSensitivity;

        motor.Rotate(_rotation);

        //Calculate camera rotation
        float _xRot = Input.GetAxisRaw("Mouse Y");

        float _cameraRotationX = _xRot * lookSensitivity;

        motor.RotateCamera(_cameraRotationX);

        //Apply thruster force
        Vector3 _thrusterForce = Vector3.zero;

        if(Input.GetButton("Jump") && thrusterFuelAmount > 0)
        {
            thrusterFuelAmount -= thrusterFuelBurnSpeed * Time.deltaTime;
            
            if(thrusterFuelAmount >= 0.01f)
            {
                _thrusterForce = Vector3.up * thrusterForce;
            }
        }
        else
        {
            thrusterFuelAmount += thrusterFuelRegenSpeed * Time.deltaTime;
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            audioSource.Play();
        }
        else if(Input.GetKeyUp(KeyCode.Space))
        {
            audioSource.Stop();
        }

        thrusterFuelAmount = Mathf.Clamp(thrusterFuelAmount, 0f, 1f);

        motor.ApplyThruster(_thrusterForce);
    }
}

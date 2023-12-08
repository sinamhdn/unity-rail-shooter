using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Controller")]
    [SerializeField][Tooltip("In m/s")] float horizontalSpeed = 20f;
    [SerializeField][Tooltip("In m/s")] float verticalSpeed = 20f;
    [SerializeField][Tooltip("In m/s")] float horizontalRange = 5f;
    [SerializeField][Tooltip("In m/s")] float verticalRange = 3f;
    [SerializeField] float pitchFactorPosition = -5f;
    [SerializeField] float pitchFactorControl = -20f;
    [SerializeField] float yawFactorPosition = 5f;
    [SerializeField] float rollFactorControl = -20f;
    float horizontalThrow, verticalThrow;
    bool isControlEnabled = true;

    void Update()
    {
        if (isControlEnabled)
        {
            XMovement();
            YMovement();
            Rotation();
            Fire();
        }
    }

    void XMovement()
    {
        horizontalThrow = Input.GetAxis("Horizontal");
        float horizontalOffsetPerFrame = horizontalThrow * horizontalSpeed * Time.deltaTime;
        float rawNewXPos = transform.localPosition.x + horizontalOffsetPerFrame;
        float clampedXPos = Mathf.Clamp(rawNewXPos, -horizontalRange, horizontalRange);
        //float clampedXPos = Mathf.Clamp01(rawNewXPos);
        //local position is the position acording to parent
        transform.localPosition = new Vector3(clampedXPos, transform.localPosition.y, transform.localPosition.z);
        //print(horizontalThrow);
    }

    void YMovement()
    {
        verticalThrow = Input.GetAxis("Vertical");
        float verticalOffsetPerFrame = verticalThrow * verticalSpeed * Time.deltaTime;
        float rawNewYPos = transform.localPosition.y + verticalOffsetPerFrame;
        float clampedYPos = Mathf.Clamp(rawNewYPos, -verticalRange, verticalRange);
        //float clampedYPos = Mathf.Clamp01(rawNewYPos);
        //local position is the position acording to parent
        transform.localPosition = new Vector3(transform.localPosition.x, clampedYPos, transform.localPosition.z);
        //print(horizontalThrow);
    }

    void Rotation()
    {
        //local rotation is a Quatornion so you cant manipulate it directly and you need to create a new rotation value and assign it
        //transform.localRotation.x;
        //how much rotate around x axis based on position
        float pitchPosition = transform.localPosition.y * pitchFactorPosition;
        //we want to nodge nose a littl up and down when moving (beware that throw goes to 1 and returns to 0)
        float pitchControl = verticalThrow * pitchFactorControl;
        float pitch = pitchPosition + pitchControl;
        float yaw = transform.localPosition.x * yawFactorPosition;
        float roll = horizontalThrow * rollFactorControl;
        transform.localRotation = Quaternion.Euler(pitch, yaw, roll);
    }

    void Fire()
    {
        if (Input.GetButton("Fire1"))
        {
            if (transform.GetChild(0).gameObject.GetComponent<ParticleSystem>().isPaused)
            {
                transform.GetChild(0).gameObject.GetComponent<ParticleSystem>().Play();
                transform.GetChild(1).gameObject.GetComponent<ParticleSystem>().Play();
            }
        }
        else
        {
            if (transform.GetChild(0).gameObject.GetComponent<ParticleSystem>().isPlaying)
            {
                // or we can just disable the emission module of the particle system
                transform.GetChild(0).gameObject.GetComponent<ParticleSystem>().Pause();
                transform.GetChild(1).gameObject.GetComponent<ParticleSystem>().Pause();
            }
        }
    }

    //dont rename (string reference)
    void OnDeath()
    {
        isControlEnabled = false;
    }
}

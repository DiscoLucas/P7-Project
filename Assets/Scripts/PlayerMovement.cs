using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{

    public Camera playerCamera;
    public float walkSpeed = 4f;
    public float runSpeed = 6f;
    public float jumpPower = 7f;
    public float gravity = 20f;
    //new stuff
    public float maxStamina = 100f;
    public float currentStamina;
    public float staminaRegen = 10f;
    public float staminaCunsumption_Jump = 40f;

    public float lookSpeed = 2f;
    public float lookXLimit = 45f;

    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;

    public bool canMove = true;

    [SerializeField] private float m_StepInterval;
    [SerializeField] private AudioClip[] m_FootstepSounds;    // an array of footstep sounds that will be randomly selected from.
    [SerializeField] private AudioClip m_JumpSound;           // the sound played when character leaves the ground.
    [SerializeField] private AudioClip m_LandSound;           // the sound played when character touches back on ground.


    CharacterController characterController;
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        currentStamina = maxStamina;
    }

    void Update()
    {

        //Debug.Log(Input.GetAxis("Mouse Y"));

        #region Handles Movment
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        // Press Left Shift to run
        bool isRunning = Input.GetKey(KeyCode.LeftShift);

        float curSpeedX = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxisRaw("Vertical") : 0;
        float curSpeedY = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxisRaw("Horizontal") : 0;
        float movementDirectionY = moveDirection.y;
        moveDirection = Vector3.Normalize((forward * curSpeedX) + (right * curSpeedY))* (isRunning ? runSpeed : walkSpeed);


        #endregion

        #region Handles Jumping
        if (Input.GetButton("Jump") && canMove && characterController.isGrounded && currentStamina > staminaCunsumption_Jump)
        {
            moveDirection.y = jumpPower;
            ReduceStamina(staminaCunsumption_Jump);
        }
        else
        {
            moveDirection.y = movementDirectionY;
        }

        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        #endregion

        #region Handles Rotation
        characterController.Move(moveDirection * Time.deltaTime);

        if (canMove)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }

        #endregion

        #region Stamina regeneration (Stamina system is scattered in the code)
        if (currentStamina < maxStamina)
        {
            currentStamina += staminaRegen * Time.deltaTime;
        }

        #endregion

        /*#region Sound
                private void ProgressStepCycle(float speed)
        {
            if (m_CharacterController.velocity.sqrMagnitude > 0 && (m_Input.x != 0 || m_Input.y != 0))
            {
                m_StepCycle += (m_CharacterController.velocity.magnitude + (speed * (m_IsWalking ? 1f : m_RunstepLenghten))) *
                             Time.deltaTime;
            }

            if (!(m_StepCycle > m_NextStep))
            {
                return;
            }

            m_NextStep = m_StepCycle + m_StepInterval;

            PlayFootStepAudio();
        }

        private void PlayFootStepAudio()
        {
            if (!characterController.isGrounded)
            {
                return;
            }

            if (m_FootstepSounds != null && m_FootstepSounds.Length > 0)
            {
                // Pick & Play a random footstep sound from the array,
                // excluding sound at index 0
                int n = UnityEngine.Random.Range(1, m_FootstepSounds.Length);
                m_AudioSource.clip = m_FootstepSounds[n];
                m_AudioSource.PlayOneShot(m_AudioSource.clip);
                // move picked sound to index 0 so it's not picked next time
                m_FootstepSounds[n] = m_FootstepSounds[0];
                m_FootstepSounds[0] = m_AudioSource.clip;
            }
        }


        #endregion*/
    }
    private void ReduceStamina(float reductionAmount)
    {
        currentStamina -= reductionAmount;
    }
}
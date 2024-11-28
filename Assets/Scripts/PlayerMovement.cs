using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JSAM;
using Unity.VisualScripting;
using JetBrains.Annotations;
using System.Drawing.Text;
using System;

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
    bool lastGroundCheck = false;
    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;

    public bool canMove = true;
    public bool pause = false;
    [SerializeField] private float m_StepInterval;
    [SerializeField] private AudioClip[] m_FootstepSounds;    // an array of footstep sounds that will be randomly selected from.
    [SerializeField] private AudioClip m_JumpSound;           // the sound played when character leaves the ground.
    [SerializeField] private AudioClip m_LandSound;           // the sound played when character touches back on ground.


    CharacterController characterController;

    [SerializeField] private Transform playerFeet;
    [SerializeField] private float radius = 2;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float yOffset = 1f;
    private RaycastHit groundInfo;
    [SerializeField] private SoundFileObject WaterWalk;
    [SerializeField] private SoundFileObject MetalWalk;
    [SerializeField] private SoundFileObject RockWalk;
    [SerializeField] private SoundFileObject TileWalk;
    [SerializeField] private SoundFileObject currentSoundObj;
    [SerializeField] private SoundFileObject jumpSoundWater;
    [SerializeField] private SoundFileObject jumpSoundMetal;
    [SerializeField] private SoundFileObject jumpSoundRock;
    [SerializeField] private SoundFileObject jumpSoundTile;
    [SerializeField] private SoundFileObject landSoundWater;
    [SerializeField] private SoundFileObject landSoundMetal;
    [SerializeField] private SoundFileObject landSoundRock;
    [SerializeField] private SoundFileObject landSoundTile;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        currentStamina = maxStamina;
        Debug.Log("pos: " +transform.position);

        GameObject spawnpoint = GameObject.FindWithTag("Spawnpoint");
        if (spawnpoint != null )
        {
            Debug.Log("Moving player to spawnpoint FROM: " + transform.position + " to: " + spawnpoint.transform.position);
            transform.position = spawnpoint.transform.position;
            Debug.Log("moved to: " + transform.position);
            //  var newobj = Instantiate(playerObject);
            // playerObject.SetActive(false);
            // playerObject = newobj;
        }
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
            playJumpSounds();
            moveDirection.y = jumpPower;
            //ReduceStamina(staminaCunsumption_Jump);
        }
        else
        {
            moveDirection.y = movementDirectionY;
        }

        if (!lastGroundCheck && characterController.isGrounded) {
            playerLanding();
        }

        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }
        #endregion

        #region Handles Rotation
        characterController.Move(moveDirection * Time.deltaTime);

        if (canMove && !pause)
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

        #region handle pause menu
        if (Input.GetButtonUp("Cancel"))
        {
            pause = !pause;
            if (pause)
            {
                GameManager.Instance.ChangeState(GameState.PauseMenu);
            }
            else {
                GameManager.Instance.ChangeState(GameState.Game);
            }
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

        lastGroundCheck = characterController.isGrounded;
    }
    private void ReduceStamina(float reductionAmount)
    {
        currentStamina -= reductionAmount;
    }

    /// <summary>
    /// play grunt sound
    /// </summary>
    public void playJumpSounds() {

        CheckGroundLayer(jumpSoundWater, jumpSoundMetal, jumpSoundRock, jumpSoundTile, playerFeet);

    }

    /// <summary>
    /// Play the landing sound
    /// </summary>
    public void playerLanding() {

        
        CheckGroundLayer(landSoundWater, landSoundMetal, landSoundRock, landSoundTile, playerFeet);

    }

    private void FixedUpdate()
    {
        Physics.SphereCast(transform.position + new Vector3(0, yOffset, 0f), radius, Vector3.down, out groundInfo, yOffset + .1f, groundLayer.value);
        //Debug.Log(groundInfo.collider.gameObject.layer.ToString());
        if(groundInfo.collider != null)
            if (characterController.velocity.magnitude > 0 && characterController.isGrounded && !AudioManager.IsSoundPlaying(currentSoundObj))
            {
                CheckGroundLayer(WaterWalk, MetalWalk, RockWalk, TileWalk, playerFeet);
            }
    }

    private void CheckGroundLayer(SoundFileObject Water, SoundFileObject Metal, SoundFileObject Rock, SoundFileObject Tile, Transform playerFeet)
    {
        try {
            if (groundInfo.collider.gameObject.layer == 4)
            {
                AudioManager.PlaySound(Water, playerFeet.position);
                currentSoundObj = Water;
            }
            else if (groundInfo.collider.gameObject.layer == 10)
            {
                AudioManager.PlaySound(Metal, playerFeet.position);
                currentSoundObj = Metal;
            }
            else if (groundInfo.collider.gameObject.layer == 11)
            {
                AudioManager.PlaySound(RockWalk, playerFeet.position);
                currentSoundObj = RockWalk;
            }
            else if (groundInfo.collider.gameObject.layer == 12)
            {
                AudioManager.PlaySound(TileWalk, playerFeet.position);
                currentSoundObj = TileWalk;
            }
        }
        catch (Exception e) { 
            Debug.LogWarning(e.ToString());
        }
        
    }

}
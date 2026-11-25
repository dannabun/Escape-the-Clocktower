using UnityEngine;
using UnityEngine.InputSystem;

// first person movement. project uses the new input system so we read
// the keyboard/mouse directly instead of Input.GetAxis
[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public Transform cam;
    public Light flashlight;

    [Header("Movement")]
    public float walkSpeed = 3.2f;
    public float sprintSpeed = 5.2f;
    public float acceleration = 10f;   // higher = snappier starts and stops
    public float gravity = -20f;
    public float jumpHeight = 0.8f;

    [Header("Look")]
    public float lookSensitivity = 0.12f;

    [Header("Feel")]
    public float bobAmount = 0.035f;
    public float bobSpeed = 9f;
    public float normalFov = 70f;
    public float sprintFov = 76f;
    public float footstepVolume = 0.35f;

    [Header("Ladders")]
    public float climbSpeed = 2.5f;

    [HideInInspector] public bool canMove = true;
    [HideInInspector] public bool onLadder = false;

    static PlayerController instance;
    static float shakeAmount;
    static float shakeTime;
    static float shakeTotal;

    // moving platform we're standing on (rotating gears, walkways)
    Transform ground;
    Vector3 groundLocalPoint;
    Quaternion groundLastRot;

    CharacterController cc;
    Camera camComp;
    AudioSource stepSource;
    AudioClip[] woodSteps;
    AudioClip[] stoneSteps;

    Vector3 horizVel;
    Vector3 camStartPos;
    float pitch;
    float yVel;
    float bobTimer;
    float stepTimer;

    void Awake()
    {
        instance = this;
        cc = GetComponent<CharacterController>();
        camComp = cam.GetComponent<Camera>();
        camStartPos = cam.localPosition;

        stepSource = gameObject.AddComponent<AudioSource>();
        stepSource.playOnAwake = false;
        stepSource.spatialBlend = 0f;
        woodSteps = Resources.LoadAll<AudioClip>("Footsteps/Wood");
        stoneSteps = Resources.LoadAll<AudioClip>("Footsteps/Stone");
    }

    void Update()
    {
        if (!canMove || Time.timeScale == 0f)
        {
            horizVel = Vector3.zero;
            return;
        }

        var kb = Keyboard.current;
        var mouse = Mouse.current;
        if (kb == null || mouse == null) return;

        RideGround();

        // look (ignore the huge jump you get the frame the cursor locks)
        Vector2 delta = mouse.delta.ReadValue();
        if (delta.sqrMagnitude < 250f * 250f)
        {
            Vector2 look = delta * lookSensitivity;
            transform.Rotate(0f, look.x, 0f);
            pitch = Mathf.Clamp(pitch - look.y, -85f, 85f);
        }

        // move
        Vector2 input = Vector2.zero;
        if (kb.wKey.isPressed) input.y += 1;
        if (kb.sKey.isPressed) input.y -= 1;
        if (kb.dKey.isPressed) input.x += 1;
        if (kb.aKey.isPressed) input.x -= 1;
        input = Vector2.ClampMagnitude(input, 1f);

        bool sprinting = kb.leftShiftKey.isPressed && input.y > 0;
        float speed = sprinting ? sprintSpeed : walkSpeed;
        Vector3 target = (transform.right * input.x + transform.forward * input.y) * speed;

        // ease into the target speed instead of snapping to it
        horizVel = Vector3.Lerp(horizVel, target, 1f - Mathf.Exp(-acceleration * Time.deltaTime));

        Vector3 move = horizVel;
        if (onLadder)
        {
            // W/S climbs, gravity off
            yVel = 0f;
            move = transform.right * input.x * walkSpeed * 0.5f;
            move.y = input.y * climbSpeed;
            if (input.y > 0) move += transform.forward * 0.5f;   // lean into the ladder so you step off at the top
        }
        else
        {
            if (cc.isGrounded && yVel < 0) yVel = -2f;
            if (cc.isGrounded && kb.spaceKey.wasPressedThisFrame)
                yVel = Mathf.Sqrt(jumpHeight * -2f * gravity);
            yVel += gravity * Time.deltaTime;
            move.y = yVel;
        }
        cc.Move(move * Time.deltaTime);

        RememberGround();

        HeadBobAndSteps(sprinting);

        // little fov push when sprinting
        if (camComp != null)
            camComp.fieldOfView = Mathf.Lerp(camComp.fieldOfView, sprinting ? sprintFov : normalFov, Time.deltaTime * 6f);

        cam.localRotation = Quaternion.Euler(pitch, 0f, 0f);

        // camera shake (machines starting, getting hit, the bell)
        if (shakeTime > 0f)
        {
            shakeTime -= Time.deltaTime;
            float k = shakeAmount * Mathf.Clamp01(shakeTime / shakeTotal);
            cam.localPosition += Random.insideUnitSphere * k;
        }

        if (flashlight != null && kb.fKey.wasPressedThisFrame)
        {
            flashlight.enabled = !flashlight.enabled;
            SFX.Play("tick", transform.position, 0.4f);
        }
    }

    void HeadBobAndSteps(bool sprinting)
    {
        float spd = new Vector2(horizVel.x, horizVel.z).magnitude;

        if (cc.isGrounded && spd > 0.4f)
        {
            bobTimer += Time.deltaTime * bobSpeed * (spd / walkSpeed);
            cam.localPosition = camStartPos + new Vector3(Mathf.Cos(bobTimer * 0.5f) * bobAmount * 0.5f,
                                                          Mathf.Sin(bobTimer) * bobAmount, 0f);

            stepTimer -= Time.deltaTime;
            if (stepTimer <= 0f)
            {
                PlayStep();
                stepTimer = sprinting ? 0.32f : 0.5f;
            }
        }
        else
        {
            cam.localPosition = Vector3.Lerp(cam.localPosition, camStartPos, Time.deltaTime * 8f);
            stepTimer = 0.1f;
        }
    }

    void PlayStep()
    {
        AudioClip[] set = stoneSteps;
        if (Physics.Raycast(transform.position + Vector3.up * 0.3f, Vector3.down, out RaycastHit hit, 0.8f))
        {
            var r = hit.collider.GetComponent<Renderer>();
            if (r != null && r.sharedMaterial != null && r.sharedMaterial.name.Contains("Wood"))
                set = woodSteps;
        }
        if (set == null || set.Length == 0) return;
        stepSource.pitch = Random.Range(0.9f, 1.1f);
        stepSource.PlayOneShot(set[Random.Range(0, set.Length)], footstepVolume);
    }

    public static void Shake(float amount, float time)
    {
        shakeAmount = amount;
        shakeTime = time;
        shakeTotal = time;
    }

    // move with whatever we were standing on last frame
    void RideGround()
    {
        if (ground == null) return;
        Vector3 now = ground.TransformPoint(groundLocalPoint);
        Vector3 delta = now - transform.position;
        if (delta.sqrMagnitude < 1f) cc.Move(delta);

        float yaw = (ground.rotation * Quaternion.Inverse(groundLastRot)).eulerAngles.y;
        if (yaw > 180f) yaw -= 360f;
        transform.Rotate(0f, yaw, 0f);
    }

    void RememberGround()
    {
        ground = null;
        if (!cc.isGrounded) return;
        if (Physics.Raycast(transform.position + Vector3.up * 0.3f, Vector3.down, out RaycastHit hit, 0.6f, ~0, QueryTriggerInteraction.Ignore))
        {
            bool moving = hit.collider.GetComponentInParent<Rotator>() != null || hit.collider.GetComponentInParent<PowerMover>() != null;
            if (moving)
            {
                ground = hit.collider.transform;
                groundLocalPoint = ground.InverseTransformPoint(transform.position);
                groundLastRot = ground.rotation;
            }
        }
    }

    // used when the player gets hit and sent back to a checkpoint
    public void Teleport(Vector3 pos, float yaw)
    {
        cc.enabled = false;
        transform.position = pos;
        transform.rotation = Quaternion.Euler(0f, yaw, 0f);
        cc.enabled = true;
        yVel = 0;
        horizVel = Vector3.zero;
        ground = null;
    }
}

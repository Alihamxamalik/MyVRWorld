using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Haptics;

public class PistoalScript : MonoBehaviour
{
    [Header("Trigger")]
    [SerializeField] Transform m_TriggerTransform;
    [SerializeField] InputActionProperty m_ActivateValue;
    [SerializeField] InputActionProperty m_ActivateButton;

    [Header("Haptics")]
    [SerializeField] HapticImpulsePlayer m_HapticImpulsePlayer;
    [SerializeField] float m_HapticAmplitudePrimary = 0.95f;
    [SerializeField] float m_HapticDurationPrimary = 0.045f;
    [SerializeField] float m_HapticAmplitudeSecondary = 0.35f;
    [SerializeField] float m_HapticDurationSecondary = 0.05f;

    [Header("Recoil")]
    [SerializeField] Transform m_ControllerTransform; // Assign RecoilOffset
    [SerializeField] float m_RecoilKickBack = 0.03f;
    [SerializeField] float m_RecoilUpAngle = 6f;
    [SerializeField] float m_RecoilReturnSpeed = 8f;

    [Header("GunSound")]
    public AudioSource m_GunAudioSource;

    [Header("MuzzleFlash")]
    public ParticleSystem m_MuzzleFlashParticles;

    [Header("Bulit")]
    public GameObject m_BulitPrefab;
    public Transform m_BulitTransform;

    [Header("Animator")]
    [SerializeField] Animator m_Animator;


    float m_OriginalZ;

    Vector3 m_BaseLocalPosition;
    Quaternion m_BaseLocalRotation;

    Vector3 m_RecoilPosOffset;
    Vector3 m_RecoilRotOffset;

    #region Unity Lifecycle

    void Awake()
    {
        if (m_TriggerTransform != null)
            m_OriginalZ = m_TriggerTransform.localPosition.z;

        if (m_ControllerTransform != null)
        {
            m_BaseLocalPosition = m_ControllerTransform.localPosition;
            m_BaseLocalRotation = m_ControllerTransform.localRotation;
        }
    }

    void OnEnable()
    {
        if (m_ActivateButton.action != null)
        {
            m_ActivateButton.action.Enable();
            m_ActivateButton.action.performed += OnFirePerformed;
        }

        if (m_ActivateValue.action != null)
            m_ActivateValue.action.Enable();
    }

    void OnDisable()
    {
        if (m_ActivateButton.action != null)
        {
            m_ActivateButton.action.performed -= OnFirePerformed;
            m_ActivateButton.action.Disable();
        }

        if (m_ActivateValue.action != null)
            m_ActivateValue.action.Disable();
    }

    void Update()
    {
        AnimateTrigger();
        UpdateRecoil();
    }

    #endregion

    #region Trigger Animation

    void AnimateTrigger()
    {
        if (m_TriggerTransform == null || m_ActivateValue.action == null)
            return;

        float triggerVal = m_ActivateValue.action.ReadValue<float>();

        Vector3 pos = m_TriggerTransform.localPosition;
        pos.z = Mathf.Lerp(m_OriginalZ, 0f, triggerVal);
        m_TriggerTransform.localPosition = pos;
    }

    #endregion

    #region Fire Logic

    void OnFirePerformed(InputAction.CallbackContext context)
    {
        Fire();
    }

    void Fire()
    {
        Debug.Log("Gun Fired");
        m_GunAudioSource.Play();
        m_MuzzleFlashParticles.Play();
        StartCoroutine(SendHaptics());
        ApplyRecoil();

        if (m_Animator != null)
        {
            m_Animator.Play("PistolRecoil");
        }

        BulitScript bulit = Instantiate(m_BulitPrefab).GetComponent<BulitScript>();
        bulit.transform.position = m_BulitTransform.position;
        bulit.StartMoving(m_BulitTransform.forward);
    }

    #endregion

    #region Haptics

    System.Collections.IEnumerator SendHaptics()
    {
        if (m_HapticImpulsePlayer == null)
            yield break;

        m_HapticImpulsePlayer.SendHapticImpulse(
            m_HapticAmplitudePrimary,
            m_HapticDurationPrimary
        );

        yield return new WaitForSeconds(m_HapticDurationPrimary);

        m_HapticImpulsePlayer.SendHapticImpulse(
            m_HapticAmplitudeSecondary,
            m_HapticDurationSecondary
        );
    }

    #endregion

    #region Recoil

    void ApplyRecoil()
    {
        // Backward kick
        m_RecoilPosOffset += new Vector3(0f, 0f, -m_RecoilKickBack);

        // Upward rotation + slight horizontal randomness
        m_RecoilRotOffset += new Vector3(
            -m_RecoilUpAngle,
            Random.Range(-2f, 2f),
            0f
        );
    }

    void UpdateRecoil()
    {
        if (m_ControllerTransform == null)
            return;

        // Smoothly return to neutral
        m_RecoilPosOffset = Vector3.Lerp(
            m_RecoilPosOffset,
            Vector3.zero,
            m_RecoilReturnSpeed * Time.deltaTime
        );

        m_RecoilRotOffset = Vector3.Lerp(
            m_RecoilRotOffset,
            Vector3.zero,
            m_RecoilReturnSpeed * Time.deltaTime
        );

        // Apply as absolute offset
        m_ControllerTransform.localPosition =
            m_BaseLocalPosition + m_RecoilPosOffset;

        m_ControllerTransform.localRotation =
            m_BaseLocalRotation * Quaternion.Euler(m_RecoilRotOffset);
    }

    #endregion
}

using UnityEngine;

public class Door : MonoBehaviour
{
    public string requiredKeyName = "Key1"; // Name of the required key (e.g., "Key1")
    public float interactDistance = 3f; // Distance to interact with door
    public float openAngle = 90f; // Angle to rotate door when opening (degrees)
    public float openDuration = 1f; // Time to complete door opening (seconds)
    public bool isSlidingDoor = false; // Set to true for sliding doors
    public Vector3 slideOffset = Vector3.zero; // Offset for sliding door movement
    private AudioManager audioManager;
    private AudioSource doorAudioSource; // For 3D door open sound
    private bool isOpen = false;
    private Quaternion closedRotation;
    private Vector3 closedPosition;

    void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio")?.GetComponent<AudioManager>();

        // Add and configure AudioSource for 3D sound
        doorAudioSource = gameObject.AddComponent<AudioSource>();
        doorAudioSource.spatialBlend = 1f; // 3D audio
        doorAudioSource.minDistance = 1f;
        doorAudioSource.maxDistance = 10f;
        doorAudioSource.rolloffMode = AudioRolloffMode.Logarithmic;
        doorAudioSource.playOnAwake = false;

        // Store initial rotation and position
        closedRotation = transform.rotation;
        closedPosition = transform.position;
    }

    void Start()
    {
        // Ensure door has a non-trigger collider
        var collider = GetComponent<Collider>();
        if (collider != null)
        {
            collider.isTrigger = false; // Non-trigger to block player
        }
        else
        {
            Debug.LogError("Door requires a Collider component!");
        }
    }

    void Update()
    {
        // Sync audio with PauseMenuManager or MenuManager
        var pauseMenuManager = FindObjectOfType<PauseMenuManager>();
        var menuManager = FindObjectOfType<MenuManager>();
        AudioSource sfxSource = pauseMenuManager != null ? pauseMenuManager.sfxSource : menuManager?.sfxSource;

        if (sfxSource != null)
        {
            doorAudioSource.volume = sfxSource.volume; // Sync volume
            doorAudioSource.mute = sfxSource.mute; // Sync mute state
        }

        // Check for player interaction
        if (Input.GetKeyDown(KeyCode.E) && !isOpen)
        {
            // Check if player is close enough
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null && Vector3.Distance(player.transform.position, transform.position) <= interactDistance)
            {
                // Check if correct key is selected
                if (InventoryManager.Instance != null && InventoryManager.Instance.currentHeldItem != null)
                {
                    PickableObject heldItem = InventoryManager.Instance.currentHeldItem.GetComponent<PickableObject>();
                    if (heldItem != null && heldItem.itemType == ItemType.Key && heldItem.itemName == requiredKeyName)
                    {
                        // Open door
                        OpenDoor();
                    }
                    else
                    {
                        Debug.Log($"Wrong key or no key selected! Held item: {(heldItem != null ? heldItem.itemName : "null")}, Required key: {requiredKeyName}");
                    }
                }
                else
                {
                    Debug.Log("No item selected in Inventory!");
                }
            }
        }
    }

    void OpenDoor()
    {
        if (!isOpen)
        {
            isOpen = true;
            // Disable collider to allow player to pass through
            var collider = GetComponent<Collider>();
            if (collider != null)
            {
                collider.enabled = false;
            }
            // Play door open sound
            if (audioManager != null && audioManager.doorOpenClip != null)
            {
                audioManager.PlaySFX(audioManager.doorOpenClip, doorAudioSource);
            }
            // Start opening door (rotation or sliding)
            if (isSlidingDoor)
            {
                StartCoroutine(SlideDoor());
            }
            else
            {
                StartCoroutine(RotateDoor());
            }
        }
    }

    private System.Collections.IEnumerator RotateDoor()
    {
        Quaternion startRotation = transform.rotation;
        Quaternion targetRotation = closedRotation * Quaternion.Euler(0f, openAngle, 0f);
        float elapsedTime = 0f;

        while (elapsedTime < openDuration)
        {
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, elapsedTime / openDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.rotation = targetRotation;
    }

    private System.Collections.IEnumerator SlideDoor()
    {
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = closedPosition + slideOffset;
        float elapsedTime = 0f;

        while (elapsedTime < openDuration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / openDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPosition;
    }
}

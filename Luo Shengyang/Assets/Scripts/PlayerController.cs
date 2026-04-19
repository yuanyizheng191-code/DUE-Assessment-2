using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    // Movement settings
    public float moveSpeed = 5f;
    public float turnSpeed = 20f;

    private int count;

    public TextMeshProUGUI countText;
    public GameObject winTextObject;

    public AudioClip collectSound;
    private AudioSource audioSource;

    void Start()
    {
        count = 0;
        SetCountText();
        if (winTextObject != null) winTextObject.SetActive(false);
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
        
        audioSource.playOnAwake = false;
        audioSource.loop = false;
    }

    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector3 moveDir = new Vector3(h, 0, v).normalized;

        if (moveDir.magnitude > 0.1f)
        {
            transform.Translate(moveDir * moveSpeed * Time.deltaTime, Space.World);

            Quaternion targetRot = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, turnSpeed * Time.deltaTime);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Gold coin +5 points
        if (other.gameObject.CompareTag("AdvanceItem"))
        {
            PlayCollectSound();
            other.gameObject.SetActive(false);
            count += 5;
            GameManager.Instance.CheckWinCondition(count);
            SetCountText();
        }
        // Silver coin +1 point
        else if (other.gameObject.CompareTag("BasicItem"))
        {
            PlayCollectSound();
            other.gameObject.SetActive(false);
            count += 1;
            GameManager.Instance.CheckWinCondition(count);
            SetCountText();
        }
    }

    void SetCountText()
    {
        if (countText != null)
        {
            countText.text = "Score: " + count + " / " + GameManager.Instance.targetScore;
        }
    }

    
    void PlayCollectSound()
    {
        if (collectSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(collectSound);
        }
    }
    
}
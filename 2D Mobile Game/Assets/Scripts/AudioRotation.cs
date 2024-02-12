using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioRotation : MonoBehaviour
{
    [Header("Audio Clips")]
    [SerializeField] private AudioClip[] audioClips;
    [Min(1) ,SerializeField] private float audioSpeed = 1;

    //Internal Variables
    private int song = 0;
    private float songLength = 0;
    private Player player;
    private AudioSource audioSource;

    private void Awake()
    {
        player = GetComponent<Player>();
        audioSource = Camera.main.GetComponent<AudioSource>();
    }

    private void Update()
    {
        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        bool isMoving = Mathf.Abs(rb.velocity.x) > Mathf.Epsilon || Mathf.Abs(rb.velocity.y) > Mathf.Epsilon;
        if (isMoving)
        {
            PlayNextAudio(audioClips);
        }
    }

    public void PlayNextAudio(AudioClip[] audios)
    {
        songLength += Time.deltaTime;

        if (songLength < audioClips[song].length * (1 / audioSpeed) || song == audioClips.Length)
        {
            return;
        }

        if (song < audioClips.Length - 1)
        {
            song++;
        }
        else
        {
            song = 0;
        }

        audioSource.PlayOneShot(audios[song]);
        songLength = 0;
    }
}

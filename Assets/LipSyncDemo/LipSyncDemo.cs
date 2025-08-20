using UnityEngine;
using uLipSync;
using System.Collections.Generic;

[RequireComponent(typeof(Renderer))]
public class LipSyncDemo : MonoBehaviour
{
    [Header("LipSync Settings")]
    public uLipSync.Profile profile;
    public AudioClip audioClip;

    [Header("Phoneme Textures")]
    public Texture A;
    public Texture I;
    public Texture U;
    public Texture E;
    public Texture O;
    public Texture N;
    public Texture Default;

    private Dictionary<string, Texture> _phonemeTextures = new Dictionary<string, Texture>();
    private Renderer _renderer;
    private uLipSyncTexture _lipSyncTexture;

    void Start()
    {
        _renderer = GetComponent<Renderer>();

        // Add and configure AudioSource
        var audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = audioClip;
        audioSource.loop = true;
        audioSource.playOnAwake = true;

        // Add and configure uLipSync
        var lipSync = gameObject.AddComponent<uLipSync.uLipSync>();
        lipSync.profile = profile;

        // Add and configure uLipSyncTexture
        _lipSyncTexture = gameObject.AddComponent<uLipSyncTexture>();
        _lipSyncTexture.renderer = _renderer;

        // Populate the phoneme texture dictionary
        _phonemeTextures.Add("A", A);
        _phonemeTextures.Add("I", I);
        _phonemeTextures.Add("U", U);
        _phonemeTextures.Add("E", E);
        _phonemeTextures.Add("O", O);
        _phonemeTextures.Add("N", N);

        // Set the initial texture
        _renderer.material.mainTexture = Default;

        // Subscribe to the lip sync update event
        lipSync.onLipSyncUpdate.AddListener(OnLipSyncUpdate);

        // Play the audio
        audioSource.Play();
    }

    public void OnLipSyncUpdate(LipSyncInfo info)
    {
        if (_phonemeTextures.TryGetValue(info.phoneme, out Texture texture))
        {
            if (_renderer.material.mainTexture != texture)
            {
                _renderer.material.mainTexture = texture;
            }
        }
        else
        {
            if (_renderer.material.mainTexture != Default)
            {
                _renderer.material.mainTexture = Default;
            }
        }
    }
}

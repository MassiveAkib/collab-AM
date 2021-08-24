using UnityEngine;
using UnityEngine.SceneManagement;

public class BgMusicManager : MonoBehaviour
{
    public static BgMusicManager instance;

    // Drag in the .mp3 files here, in the editor
    public AudioClip[] MusicClips;

    public AudioSource Audio;

    // public AudioClip winAudio;
    // public AudioClip faildAudio;


    // Singelton to keep instance alive through all scenes
    void Awake()
    {
        if (instance == null) { instance = this; }
        else { Destroy(gameObject); }

        DontDestroyOnLoad(gameObject);

        // Hooks up the 'OnSceneLoaded' method to the sceneLoaded event
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // Called whenever a scene is loaded
    void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode)
    {
        // Replacement variable (doesn't change the original audio source)
        AudioSource source = gameObject.GetComponent<AudioSource>();
        

        /*if (GameObject.Find("Canvas").transform.GetChild(8).gameObject.activeSelf)
        {
            source.Stop();
            return;
        }*/

        // Plays different music in different scenes
        /*switch (scene.name)
        {
            case "Start":
                source.clip = MusicClips[0];
                break;
            case "Levels":
                source.clip = MusicClips[0];
                break;
            case "Scene 1":
                source.clip = MusicClips[5];
                break;
            case "Scene 2":
                source.clip = MusicClips[1];
                break;
            case "Scene 3":
                source.clip = MusicClips[2];
                break;
            case "Scene 4":
                source.clip = MusicClips[3];
                break;
            case "Scene 5":
                source.clip = MusicClips[4];
                break;
            case "Scene 6":
                source.clip = MusicClips[5];
                break;
            case "Scene 7":
                source.clip = MusicClips[6];
                break;
            case "Scene 8":
                source.clip = MusicClips[7];
                break;
            case "Scene 9":
                source.clip = MusicClips[6];
                break;
            case "Scene 10":
                source.clip = MusicClips[4];
                break;
            case "Scene 11":
                source.clip = MusicClips[5];
                break;
            case "Scene 12":
                source.clip = MusicClips[1];
                break;
            case "Scene 13":
                source.clip = MusicClips[2];
                break;
            case "Scene 14":
                source.clip = MusicClips[6];
                break;
            case "Scene 15":
                source.clip = MusicClips[3];
                break;
            case "Scene 16":
                source.clip = MusicClips[4];
                break;
            case "Scene 17":
                source.clip = MusicClips[5];
                break;
            case "Scene 18":
                source.clip = MusicClips[6];
                break;
            case "Scene 19":
                source.clip = MusicClips[3];
                break;

            default:
                source.clip = MusicClips[0];
                break;
        }*/
        if (scene.name == "EmergencyLanding" || scene.name == "PlaneFlyingScene" || scene.name == "PlaneCrash")
        {
            source.Stop();
        }
        else
        {
            source.clip = MusicClips[0];
            source.loop = true;
            source.Play();
        }
        
        // Only switch the music if it changed
       /* if (source.clip != Audio.clip)
        {
            Audio.enabled = false;
            Audio.clip = source.clip;
            Audio.enabled = true;
        }*/
    }

    /*public void winAudios()
    {
        AudioSource source = gameObject.GetComponent<AudioSource>();
        source.Stop();
        source.clip = winAudio;
        source.loop = false;
        source.Play();
    }

    public void faildAudios()
    {
        AudioSource source = gameObject.GetComponent<AudioSource>();
        source.Stop();
        source.clip = faildAudio;
        source.loop = false;
        source.Play();
    }*/
}

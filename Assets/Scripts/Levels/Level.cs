using Hazards;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Levels
{
    public class Level : MonoBehaviour
    {
        [SerializeField] private Transform playerSpawnPoint;

        [SerializeField] private AudioClip musicClip;

        private PickUp[] gems;
        private Spikes[] spikesArray;
        private Door door;

        // private AudioSource audioSource;

        private int gemsCount;

        private LevelsManager levelsManager;
        public Transform PlayerSpawnPoint => playerSpawnPoint;

        private void Awake()
        {
            gems = GetComponentsInChildren<PickUp>();
            spikesArray = GetComponentsInChildren<Spikes>();
            door = GetComponentInChildren<Door>();
            // audioSource = GetComponent<AudioSource>();
        }

        private void Start()
        {
            SetupLevelComponents();

            SoundManager.Instance.PlayMusic(musicClip);
            // PLayMusic();

            if (gems.Length > 0)
                gems[0].transform.parent.GetComponent<TilemapRenderer>().enabled = false;
        }

        /*private void PLayMusic()
        {
            audioSource.loop = true;
            audioSource.clip = musicClip;
            audioSource.Play();
        }*/

        private void SetupLevelComponents()
        {
            foreach (var gem in gems) gem.Setup(this);
            door.Setup(this);
        }

        public void PickUpGem()
        {
            SoundManager.Instance.PlayPickUp();
            gemsCount++;
            if (gemsCount >= gems.Length) door.TurnOn();
        }
    }
}
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
        private Door door;

        private int gemsCount;

        private LevelsManager levelsManager;
        public Transform PlayerSpawnPoint => playerSpawnPoint;

        private void Awake()
        {
            gems = GetComponentsInChildren<PickUp>();
            door = GetComponentInChildren<Door>();
        }

        private void Start()
        {
            SetupLevelComponents();

            if (SoundManager.Instance != null)
                SoundManager.Instance.PlayMusic(musicClip);

            //if (gems.Length > 0) gems[0].transform.parent.GetComponent<TilemapRenderer>().enabled = false;
        }


        private void SetupLevelComponents()
        {
            foreach (var gem in gems) gem.Setup(this);
        }

        public void PickUpGem()
        {
            gemsCount++;
            SoundManager.Instance.PlayPickUp();

            if (gemsCount < gems.Length) return;

            door.TurnOn();
            SoundManager.Instance.PlayOpenDoor();
        }
    }
}
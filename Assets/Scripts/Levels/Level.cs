using Enemies;
using Interfaces;
using PlayerComponents;
using UnityEngine;

namespace Levels
{
    public class Level : MonoBehaviour
    {
        [SerializeField] private Transform playerSpawnPoint;
        [SerializeField] private AudioClip musicClip;

        private PickUp[] gems;
        private IResettable[] resettable;
        private Door door;
        private Player player;

        private int gemsCount;

        private LevelsManager levelsManager;

        private void Awake()
        {
            levelsManager = GetComponentInParent<LevelsManager>();
            resettable = GetComponentsInChildren<IResettable>();
            gems = GetComponentsInChildren<PickUp>();
            door = GetComponentInChildren<Door>();
        }

        private void Start()
        {
            SetupLevelComponents();
            SpawnPlayer();
            if (SoundManager.Instance != null)
                SoundManager.Instance.PlayMusic(musicClip);
        }

        private void SpawnPlayer()
        {
            player = levelsManager.PlayerPrefab.Get<Player>(playerSpawnPoint.position, Quaternion.identity);
            player.AssignLevel(this);
        }

        private void SetupLevelComponents()
        {
            door.AssignManager(levelsManager);
            DoResets();
            foreach (var gem in gems) gem.Setup(this);
        }

        private void DoResets()
        {
            if (!GameManager.AssistMode) gemsCount = 0;

            if (gemsCount < gems.Length && door.IsOn) door.TurnOff();

            foreach (var reset in resettable) reset.Reset();
        }

        public void PickUpGem()
        {
            gemsCount++;
            SoundManager.Instance.PlayPickUp();

            if (gemsCount < gems.Length) return;

            door.TurnOn();
            SoundManager.Instance.PlayOpenDoor();
        }

        public void PlayerDeath()
        {
            GameManager.Instance.PlayerDeath();
            player.DeSpawn();
            SpawnPlayer();
            DoResets();
        }
    }
}
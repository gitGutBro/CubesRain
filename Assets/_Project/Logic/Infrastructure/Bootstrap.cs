using UnityEngine;
using Spawn;
using Spawn.Spawners;

namespace Infrastructure
{
    internal sealed class Bootstrap : MonoBehaviour
    {
        [SerializeField] private CubeSpawner _cubeSpawner;
        [SerializeField] private BombSpawner _bombSpawner;

        [SerializeField] private SpawnStatsView _cubeSpawnStatsView;
        [SerializeField] private SpawnStatsView _bombSpawnStatsView;

        private readonly SpawnStatsModel _cubeStatsModel = new();
        private readonly SpawnStatsModel _bombStatsModel = new();

        private void Awake()
        {
            _cubeSpawner.InitBombSpawn(_bombSpawner.Spawn);

            InitModelView(_cubeSpawner, _cubeStatsModel, _cubeSpawnStatsView);
            InitModelView(_bombSpawner, _bombStatsModel, _bombSpawnStatsView);
        }

        private void OnDestroy()
        {
            Unsubscribe(_cubeStatsModel, _cubeSpawnStatsView);
            Unsubscribe(_bombStatsModel, _bombSpawnStatsView);
        }

        private static void InitModelView<TSpawnable>(Spawner<TSpawnable> spawner, SpawnStatsModel statsModel,
            SpawnStatsView statsView) where TSpawnable : MonoBehaviour
        {
            statsModel.SpawnedAdded += statsView.OnUpdateTotalSpawned;
            statsModel.CreatedAdded += statsView.OnUpdateCreated;
            statsModel.ActiveChanged += statsView.OnUpdateActive;

            spawner.InitStatsModel(statsModel);
        }

        private static void Unsubscribe(SpawnStatsModel statsModel, SpawnStatsView statsView)
        {
            statsModel.SpawnedAdded -= statsView.OnUpdateTotalSpawned;
            statsModel.CreatedAdded -= statsView.OnUpdateCreated;
            statsModel.ActiveChanged -= statsView.OnUpdateActive;
        }
    }
}

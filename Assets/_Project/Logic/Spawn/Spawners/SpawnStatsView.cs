using UnityEngine;
using TMPro;

namespace Spawn.Spawners
{
    internal class SpawnStatsView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _totalSpawnedText;
        [SerializeField] private TextMeshProUGUI _createdText;
        [SerializeField] private TextMeshProUGUI _activeText;

        private void Start()
        {
            OnUpdateTotalSpawned(0);
            OnUpdateCreated(0);
            OnUpdateActive(0);
        }

        public void OnUpdateTotalSpawned(int totalSpawned) =>
            _totalSpawnedText.text = $"Total spawned: {totalSpawned.ToString()}";

        public void OnUpdateCreated(int created) =>
            _createdText.text = $"Created: {created.ToString()}";

        public void OnUpdateActive(int active) =>
            _activeText.text = $"Active: {active.ToString()}";
    }
}

using UnityEngine;

namespace Utilities
{
    internal static class RandomGenerator
    {
        public static float Value01 => Random.value;
        public static Color ColorHSV => Random.ColorHSV();
        public static Vector2 InsideUnitCircle => Random.insideUnitCircle;
        public static Vector3 OnUnitSphere => Random.onUnitSphere;
        public static Vector3 InsideUnitSphere => Random.insideUnitSphere;

        public static float Range(float minInclusive, float maxInclusive) =>
            Random.Range(minInclusive, maxInclusive);

        public static int Range(int minInclusive, int maxExclusive) =>
            Random.Range(minInclusive, maxExclusive);

        public static bool GetChance01Value(float probability)
        {
            if (probability is < 0f or > 1f)
            {
                throw new System.ArgumentOutOfRangeException(
                    nameof(probability),
                    probability,
                    "Probability must be in range [0, 1].");
            }

            return Random.value <= probability;
        }

        public static Vector3 GetRandomPoint(Bounds bounds)
        {
            return new Vector3
            (
                Random.Range(bounds.min.x, bounds.max.x),
                Random.Range(bounds.min.y, bounds.max.y),
                Random.Range(bounds.min.z, bounds.max.z)
            );
        }
    }
}

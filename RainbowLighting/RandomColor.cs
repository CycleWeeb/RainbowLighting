using UnityEngine;

namespace RainbowLighting
{
    internal class RandomColor : SimpleColorSO
    {
        public override Color color
        {
            get
            {
                return new Color(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), Random.Range(.65f, .82f));
            }
        }
    }
}
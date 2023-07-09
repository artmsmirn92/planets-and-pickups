using mazing.common.Runtime;
using UnityEngine;

namespace MiniPlanetDefense
{
    /// <summary>
    /// A data object that contains information that
    /// a) are needed by multiple scripts and
    /// b) should be configurable in the editor, but don't change at runtime.
    /// </summary>
    [CreateAssetMenu]
    public class MainData : ScriptableObject
    {
        public float playfieldRadius;
        public bool  inGame;
        public int   currentLevelScore;
    }
}
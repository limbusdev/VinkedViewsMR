using UnityEngine;

namespace ETV
{
    /// <summary>
    /// Dummy Implementation for the client (pETV).
    /// </summary>
    public class NullETVManager : AETVManager
    {
        public override bool AutoPlaceETV(GameObject ETV)
        {
            return false;
        }

        public override void Init(int ring) { }

        public override void Reset() { }
    }
}
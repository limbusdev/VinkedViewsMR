using UnityEngine;

namespace ETV
{
    public abstract class AETVManager : MonoBehaviour
    {
        /// <summary>
        /// Takes an ET visualization object and puts it at an empty
        /// circularily ordered slot.
        /// </summary>
        /// <param name="ETV"></param>
        /// <returns>Whether there was an empty slot available</returns>
        public abstract bool AutoPlaceETV(GameObject ETV);

        /// <summary>
        /// Generates place holders for newly created ETVs.
        /// </summary>
        /// <param name="ring"></param>
        public abstract void Init(int ring);

        /// <summary>
        /// Resets the available place holders.
        /// </summary>
        public abstract void Reset();
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Gameplay
{
    public interface IDamageType
    {
        ///<summary>
        /// Apply damage type logic to the damaged object. E.G. if an object got hit by a flame attack
        /// this function it's going to apply the burning effect to that object
        ///</summary>
        void ApplyDamageType(GameObject DamagedObject);

        ///<summary>
        /// Apply damage type logic to the damaged object. E.G. if an object got hit by a flame attack
        /// this function it's going to apply the burning effect to that object
        ///</summary>
        void ApplyDamageType(MonoBehaviour DamagedComponent);
    }
}


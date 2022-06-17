using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Gameplay
{
    ///<summary>
    /// Interface used to mark an object as Damageable.
    /// Damageable objects will receive damage events when damaged by
    /// other entities.
    ///</summary>
    public interface IDamageable
    {

        /// <summary>
        /// Called each time this component receives damage
        /// </summary>
        public void OnReceiveAnyDamage(DamageProperties damageProperties);

        /// <summary>
        /// Check if the component is currently immune to damage
        /// </summary>
        ///<returns> True if the component is immune to damage, false otherwise.</returns>
        public bool IsCurrentlyDamageable();
    }

    public struct DamageProperties
    {
        float damage;
        GameObject damageCauser;
        IDamageType damageType;

        public DamageProperties(float damage, GameObject damageCauser, IDamageType damageType)
        {
            this.damage = damage;
            this.damageCauser = damageCauser;
            this.damageType = damageType;
        }
    }
}


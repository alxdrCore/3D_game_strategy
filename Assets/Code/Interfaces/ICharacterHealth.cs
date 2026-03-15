using UnityEngine;

namespace EngineRoom.Examples.Interfaces
{
    public interface ICharacterHealth
    {
        int currentHealth {get;}
        void TakeDamage(int damage);
    }
}

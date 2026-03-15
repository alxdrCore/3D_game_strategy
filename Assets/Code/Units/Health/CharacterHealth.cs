using System;
using EngineRoom.Examples.Interfaces;
namespace EngineRoom.Examples.Health
{
    public class CharacterHealth : ICharacterHealth
    {
        public int currentHealth { get; private set; }

        public void TakeDamage(int damage)
        {
            currentHealth -= damage;
            Console.WriteLine($"Character took {damage} damage." + $"Current health = {currentHealth}");
        }
    }

}
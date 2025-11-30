

// TriviaGame.Domain/Entities/Player.cs
using System;

namespace TriviaGame.Domain.Entities
{
    public class Player
    {
        public Guid PlayerId { get; set; } = Guid.NewGuid();
        public User User { get; set; }        
        public int Score { get; set; } = 0;   
    }
}

using System;

namespace Environment
{
    class Program
    {
        static void Main(string[] args)
        {
            Bot bot = new Bot();
            Player player = new Player(60);

            bot.SpotPlayer(player);
        }
    }

    class Weapon
    {
        private int _damage;
        private int _bulletsQuantity;

        public Weapon(int damage, int bulletsQuantity)
        {
            _damage = damage;
            _bulletsQuantity = bulletsQuantity;
        }

        public void Fire(Player player)
        {
            if (_bulletsQuantity <= 0)
                throw new ArgumentOutOfRangeException(nameof(_bulletsQuantity));

            player.TakeDamage(_damage);
            _bulletsQuantity -= 1;
        }
    }

    class Player
    {
        public int _health { get; private set; }

        public Player(int health)
        {
            _health = health;
        }

        public void TakeDamage(int damage)
        {
            if (_health <= 0)
                throw new ArgumentOutOfRangeException();

            _health -= damage;
        }
    }

    class Bot
    {
        private Weapon _weapon = new Weapon(20, 2);

        public void SpotPlayer(Player player)
        {
            _weapon.Fire(player);
        }
    }
}
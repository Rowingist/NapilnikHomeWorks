using System;

namespace Environment
{
    class Program
    {
        static void Main(string[] args)
        {
            Bot bot = new Bot();
            Player player = new Player(100);

            bot.OnSeePlayer(player);

            Console.WriteLine(player.Health);
        }
    }

    class Weapon
    {
        private int _damage;
        private int _bullets;

        public Weapon(int damage, int bullets)
        {
            _damage = damage;
            _bullets = bullets;
        }

        public void Fire(Player player)
        {
            player.TakeDamage(_damage);
            _bullets -= 1;
        }
    }

    class Player
    {
        public int Health { get; private set; }

        public Player(int value)
        {
            Health = value;
        }

        public void TakeDamage(int damage)
        {
            Health -= damage;

            if (Health <= 0)
                Health = 0;
        }
    }

    class Bot
    {
        private Weapon _weapon = new Weapon(40, 60);

        public void OnSeePlayer(Player player)
        {
            _weapon.Fire(player);
        }
    }
}

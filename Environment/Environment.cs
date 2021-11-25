using System;

namespace Environment
{
    public interface IDamageable
    {
        void TakeDamage(int damage);
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            Bot bot = new Bot();
            Player player = new Player(50);

            bot.SpotPlayer(player);
        }
    }

    public class Weapon
    {
        private int _damage;
        private int _bullets;

        public Weapon(int damage, int bullets)
        {
            if (damage < 0)
            {
                throw new ArgumentException(nameof(damage));
            }

            if (bullets < 0)
            {
                throw new ArgumentException(nameof(bullets));
            }

            _damage = damage;
            _bullets = bullets;
        }

        public void Fire(Player player)
        {
            if (_bullets <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(_bullets));
            }

            player.TakeDamage(_damage);
            _bullets -= 1;
        }
    }

    public class Player : IDamageable
    {
        private int _health;

        public Player(int health)
        {
            if (health <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(health));
            }

            _health = health;
        }

        public void TakeDamage(int damage)
        {
            _health -= damage;
            if (_health <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(_health));
            }
        }
    }

    public class Bot
    {
        private Weapon _weapon = new Weapon(60, 5);

        public void SpotPlayer(Player player)
        {
            _weapon.Fire(player);
        }
    }
}
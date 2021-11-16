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
            Player player = new Player(new Health(50));

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
        private Health _health;

        public Player(Health health)
        {
            if (health == null)
            {
                throw new NullReferenceException(nameof(health));
            }

            _health = health;
        }

        public void TakeDamage(int damage)
        {
            _health.TakeDamage(damage);
        }
    }

    public class Health : IDamageable
    {
        private int _value;

        public Health(int value)
        {
            if (value <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(value));
            }

            _value = value;
        }

        public void TakeDamage(int damage)
        {
            _value -= damage;
            if (_value <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(_value));
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
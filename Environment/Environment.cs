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
            IDamageable player = new Player(50);

            bot.OnSeePlayer(player);
        }
    }

    public class Weapon
    {
        private int _damage;
        private int _bullets;

        public Weapon(int damage, int bullets)
        {
            if (damage < 0)
                throw new ArgumentException(nameof(damage));

            if (bullets < 0)
                throw new ArgumentException(nameof(bullets));

            _damage = damage;
            _bullets = bullets;
        }

        public void Shoot(IDamageable target)
        {
            if (_bullets <= 0)
                throw new ArgumentOutOfRangeException(nameof(_bullets));

            target.TakeDamage(_damage);
            _bullets -= 1;
        }
    }

    public class Player : IDamageable
    {
        private int _health;

        public Player(int health)
        {
            if (health <= 0)
                throw new ArgumentOutOfRangeException(nameof(health));

            _health = health;
        }

        public void TakeDamage(int damage)
        {
            if (damage > _health)
                throw new ArgumentException(nameof(damage));

            _health -= damage;

            if (_health <= 0)
                throw new ArgumentOutOfRangeException(nameof(_health));
        }
    }

    public class Bot
    {
        private Weapon _weapon = new Weapon(1, 60);

        public void OnSeePlayer(IDamageable player)
        {
            _weapon.Shoot(player);
        }
    }
}
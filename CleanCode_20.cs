using System;

class Example_Task_20_HW
{
    class Player
    {
        private Weapon _weapon;
        private Movement _movement;

        public Player(Weapon weapon, Movement movement)
        {
            _weapon = weapon;
            _movement = movement;
        }

        public string Name { get; private set; }

        public int Age { get; private set; }

        public bool TryAttack()
        {
            return _weapon.IsReloading();
        }

        public void Attack()
        {
            if (!TryAttack())
            {
                //attack
            }
        }

        public void Move()
        {
            _movement.Move();
        }
    }

    class Weapon
    {
        public float Cooldown { get; private set; }

        public int Damage { get; private set; }

        public bool IsReloading()
        {
            throw new NotImplementedException();
        }
    }

    class Movement
    {
        public float DirectionX { get; private set; }

        public float DirectionY { get; private set; }

        public float Speed { get; private set; }

        public void Move()
        {
            //Do move
        }
    }
}
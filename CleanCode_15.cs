using System;

class Example_Task_15_HW
{
    class Player { }

    class Gun { }

    class Follower { }

    class Unit
    {
        public IReadOnlyCollection<Unit> Units { get; private set; }
    }
}
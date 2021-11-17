using System;
using System.Collections.Generic;

namespace Napilnik_homeworks
{
    public interface IReadOnlyCell
    {
        public Good Good { get; }

        public int Count { get; }
    }

    public interface IShowable
    {
        void Show();
    }

    public class Shop_HW
    {
        public static void Main(string[] args)
        {
            Good iPhone12 = new Good("IPhone 12");
            Good iPhone11 = new Good("IPhone 11");

            Warehouse warehouse = new Warehouse();

            Shop shop = new Shop(warehouse);

            warehouse.Delive(iPhone12, 10);
            warehouse.Delive(iPhone11, 1);

            Console.WriteLine("In warehouse: ");
            warehouse.Show();

            Cart cart = shop.Cart();
            cart.Add(iPhone12, 4);
            cart.Add(iPhone11, 3);

            Console.WriteLine("In Cart: ");
            cart.Show();

            Console.WriteLine(cart.Order().Paylink);
        }
    }

    public class Shop
    {
        private readonly Warehouse _warehouse;

        public Shop(Warehouse warehouse)
        {
            _warehouse = warehouse;
        }

        public Cart Cart()
        {
            return new Cart(_warehouse);
        }
    }

    public class Cart : IShowable
    {
        private readonly Warehouse _shopWarehouse;
        private readonly Order _order = new Order();

        public Cart(Warehouse shopWarehouse)
        {
            _shopWarehouse = shopWarehouse;
        }

        public void Add(Good good, int count)
        {
            _shopWarehouse.Derive(good, count);
            _order.AddGoods(good, count);
        }

        public void Show()
        {
            _order.Show();
        }

        public Order Order()
        {
            return _order;
        }
    }

    public class Order : IShowable
    {
        private readonly Warehouse _cartWarehouse = new Warehouse();

        public Order()
        {
            Paylink = "Cart order by #123456";
        }

        public string Paylink { get; private set; }

        public void AddGoods(Good good, int count)
        {
            _cartWarehouse.Delive(good, count);
        }

        public void Show()
        {
            _cartWarehouse.Show();
        }
    }

    public class Warehouse : IShowable
    {
        private readonly List<Cell> _cells;

        public Warehouse()
        {
            _cells = new List<Cell>();
        }

        public IReadOnlyList<IReadOnlyCell> Cells => _cells;

        public void Delive(Good good, int count)
        {
            var newCell = new Cell(good, count);

            int cellIndex = _cells.FindIndex(cell => cell.Good == newCell.Good);

            if (cellIndex == -1)
            {
                _cells.Add(newCell);
            }
            else
            {
                _cells[cellIndex] = _cells[cellIndex].Merge(newCell);
            }
        }

        public void Derive(Good good, int count)
        {
            var necessaryCell = new Cell(good, count);

            int cellIndex = _cells.FindIndex(cell => cell.Good == necessaryCell.Good);

            if (cellIndex == -1)
            {
                throw new InvalidOperationException();
            }
            else
            {
                _cells[cellIndex] = _cells[cellIndex].Derive(necessaryCell);
            }
        }

        public void Show()
        {
            foreach (Cell cell in _cells)
            {
                cell.View.Show();
            }
        }
    }

    public class Cell : IReadOnlyCell, IShowable
    {
        public Cell(Good good, int count)
        {
            if (count < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            Good = good;
            Count = count;
            View = new View(good.Name, count);
        }

        public Good Good { get; private set; }

        public int Count { get; private set; }

        public View View { get; private set; }

        public Cell Merge(Cell newCell)
        {
            if (newCell.Good != Good)
            {
                throw new InvalidOperationException();
            }

            int cellExtention = newCell.Count + Count;

            return new Cell(Good, cellExtention);
        }

        public Cell Derive(Cell targetCell)
        {
            if (targetCell.Good != Good)
            {
                throw new InvalidOperationException();
            }

            if (targetCell.Count > Count)
            {
                throw new IndexOutOfRangeException(nameof(Count));
            }

            int cellReduction = Count - targetCell.Count;

            return new Cell(Good, cellReduction);
        }

        public void Show()
        {
            View.Show();
        }
    }

    public class Good
    {
        public string Name { get; private set; }

        public Good(string name)
        {
            Name = name;
        }
    }

    public class View : IShowable
    {
        private readonly string _name;
        private readonly int _count;

        public View(string name, int count)
        {
            _name = name;
            _count = count;
        }

        public void Show()
        {
            Console.WriteLine($"Good: {_name}, amount avaliable: {_count}");
        }
    }
}
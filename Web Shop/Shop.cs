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

            Shop shop = new Shop(new Warehouse());

            IShowable warehouseView = new WarehouseView(shop.Warehouse);

            shop.Warehouse.Ship(iPhone12, 10);
            shop.Warehouse.Ship(iPhone11, 1);

            Console.WriteLine("In warehouse: ");
            warehouseView.Show();

            Cart cart = shop.Cart();
            IShowable cartView = new WarehouseView(cart.Order().CartWarehouse);

            cart.Add(iPhone12, 4);
            cart.Add(iPhone11, 1);

            Console.WriteLine("In Cart: ");
            cartView.Show();

            Console.WriteLine(cart.Order().Paylink);
        }
    }

    public class Shop
    {
        private readonly Warehouse _warehouse;

        public Warehouse Warehouse => _warehouse;

        public Shop(Warehouse warehouse)
        {
            _warehouse = warehouse;
        }

        public Cart Cart()
        {
            return new Cart(_warehouse);
        }
    }

    public class Cart
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

        public Order Order()
        {
            return _order;
        }
    }

    public class Order
    {
        private readonly Warehouse _cartWarehouse = new Warehouse();

        public Order()
        {
            Paylink = "Cart order by #123456";
        }

        public string Paylink { get; private set; }

        public Warehouse CartWarehouse => _cartWarehouse;

        public void AddGoods(Good good, int count)
        {
            _cartWarehouse.Ship(good, count);
        }
    }

    public class Warehouse
    {
        private readonly List<Cell> _cells;

        public Warehouse()
        {
            _cells = new List<Cell>();
        }

        public IReadOnlyList<IReadOnlyCell> Cells => _cells;

        public void Ship(Good good, int count)
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
    }

    public class Cell : IReadOnlyCell
    {
        public Cell(Good good, int count)
        {
            if (count < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            Good = good;
            Count = count;
        }

        public Good Good { get; private set; }

        public int Count { get; private set; }

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
    }

    public class Good
    {
        public string Name { get; private set; }

        public Good(string name)
        {
            Name = name;
        }
    }

    public class CellView : IShowable
    {
        private readonly Cell _cell;

        public CellView(Cell cell)
        {
            _cell = cell;
        }

        public void Show()
        {
            Console.WriteLine($"Good: {_cell.Good.Name}, amount avaliable: {_cell.Count}");
        }
    }

    public class WarehouseView : IShowable
    {
        private readonly Warehouse _warehouse;
        private readonly IReadOnlyList<IReadOnlyCell> _cells;
        private readonly List<CellView> _cellsView = new List<CellView>();

        public WarehouseView(Warehouse warehouse)
        {
            _warehouse = warehouse;
            _cells = _warehouse.Cells;
        }

        public void Show()
        {
            if (_cellsView == null)
            {
                throw new InvalidOperationException();
            }

            Refresh();

            for (int i = 0; i < _cellsView.Count; i++)
            {
                _cellsView[i].Show();
            }
        }

        private void Refresh()
        {
            _cellsView.Clear();
            foreach (Cell cell in _warehouse.Cells)
            {
                _cellsView.Add(new CellView(cell));
            }
        }
    }
}
using System;
using System.Collections.Generic;

namespace Napilnik_homeworks
{
    class Program
    {
        static void Main(string[] args)
        {
            Good iPhone12 = new Good("IPhone 12");
            Good iPhone11 = new Good("IPhone 11");

            Warehouse warehouse = new Warehouse();

            Shop shop = new Shop(warehouse);

            warehouse.Delive(iPhone12, 10);
            warehouse.Delive(iPhone11, 1);

            //Вывод всех товаров на складе с их остатком
            Console.WriteLine("In warehouse: ");
            warehouse.ShowAwaliableGoods();

            Cart cart = shop.Cart();
            cart.Add(iPhone12, 4);
            cart.Add(iPhone11, 3); //при такой ситуации возникает ошибка так, как нет нужного количества товара на складе

            //Вывод всех товаров в корзине
            Console.WriteLine("In Cart: ");
            cart.ShowGoodsInOrder();

            Console.WriteLine(cart.Order().Paylink);
        }
    }

    public class Shop
    {
        private Warehouse _warehouse;

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
        private Warehouse _shopWarehouse;
        private Order _order = new Order();

        public Cart(Warehouse shopWarehouse)
        {
            _shopWarehouse = shopWarehouse;
        }

        public void Add(Good good, int count)
        {
            _shopWarehouse.DeriveGoodsForOrder(good, count);
            _order.AddGoods(good, count);
        }

        public void ShowGoodsInOrder()
        {
            _order.ShowGoodsInCart();
        }

        public Order Order()
        {
            return _order;
        }
    }

    public class Order
    {
        private Warehouse _cartWarehouse = new Warehouse();

        public string Paylink { get; private set; }

        public Order()
        {
            Paylink = "Cart order by #123456";
        }

        public void AddGoods(Good good, int count)
        {
            _cartWarehouse.Delive(good, count);
        }

        public void ShowGoodsInCart()
        {
            _cartWarehouse.ShowAwaliableGoods();
        }
    }

    public class Warehouse
    {
        private readonly List<Cell> _cells;
        public IReadOnlyList<IReadOnlyCell> Cells => _cells;

        public Warehouse()
        {
            _cells = new List<Cell>();
        }

        public void Delive(Good good, int count)
        {
            var newCell = new Cell(good, count);

            int cellIndex = _cells.FindIndex(cell => cell.Good == good);

            if (cellIndex == -1)
                _cells.Add(newCell);
            else
                _cells[cellIndex].Merge(newCell);
        }

        public void DeriveGoodsForOrder(Good good, int count)
        {
            Cell necessaryCell = new Cell(good, count);

            var meetCell = _cells.Find(cell => cell.Good == necessaryCell.Good);

            meetCell.Derive(necessaryCell);
        }

        public void ShowAwaliableGoods()
        {
            foreach (var cell in _cells)
                cell.ShowInfo();
        }
    }

    public class Cell : IReadOnlyCell
    {
        public Good Good { get; private set; }

        public int Count { get; private set; }

        public Cell(Good good, int count)
        {
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            Good = good;
            Count = count;
        }

        public void Merge(Cell newCell)
        {
            if (newCell.Good != Good)
                throw new InvalidOperationException();

            Count += newCell.Count;
        }

        public void Derive(Cell targetCell)
        {
            if (targetCell.Good != Good)
                throw new InvalidOperationException();

            if (targetCell.Count > Count)
                throw new IndexOutOfRangeException(nameof(Count));

            Count -= targetCell.Count;
        }

        public void ShowInfo()
        {
            Console.WriteLine($"Good: {Good.Name}, amount avaliable: {Count}");
        }
    }

    public interface IReadOnlyCell
    {
        public Good Good { get; }
        public int Count { get; }
    }

    public class Good
    {
        public string Name { get; private set; }

        public Good(string name)
        {
            Name = name;
        }
    }
}
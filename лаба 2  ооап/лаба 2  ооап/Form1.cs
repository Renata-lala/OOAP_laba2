using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static лаба_2__ооап.Form1;

namespace лаба_2__ооап
{
    public partial class Form1 : Form
    {
        private Category clothing;
        private Category basket;
        private Category sweaters;
        private Category t_shirts;


        public Form1()
        {
            InitializeComponent();
            LoadData();
        }

        //123 - скидка 10% на категорю свитеров 
        //321 - скидка 20% на категорию футболок 
        //56 - скидка 50% на все джинсы
        //78 - скидка 30% на всю одежду
        private (decimal percent, string category) CheckPromoCodeAndGetDiscount(string promoCode)
        {
            decimal percent = 1.0m; // Начальное значение скидки
            string category = "";

            if (promoCode == "123") // Проверяем промокод
            {
                category = "sweaters";
                percent = 0.9m;
                label9.Text = "скидка 10% на категорию свитеров"; // 
            }
            else if (promoCode == "321")
            {
                category = "t_shirts";
                percent = 0.8m;
                label9.Text = "скидка 20% на категорию футболок";
            }
            else if (promoCode == "56")
            {
                category = "jeans";
                percent = 0.5m;
                label9.Text = "скидка 50% на все джинсы";
            }
            else if (promoCode == "78")
            {
                category = "clothing";
                percent = 0.3m;
                label9.Text = "скидка 30% на всю одежду";
            }
            else
            {
                label9.Text = "Неверный промокод";
            }

            return (percent, category);
        }
        private decimal ApplyDiscountToClothingProducts(string promoCode)
        {
            var (percent, category) = CheckPromoCodeAndGetDiscount(promoCode);

            decimal totalBasketPrice = 0; // Общая цена всех товаров в корзине
            foreach (IShopComponent component in basket.Components)
            {
                totalBasketPrice += component.GetPrice();
            }

            decimal skidkaPrice = 0;
            foreach (IShopComponent component in basket.Components)
            {
                if (component is Product product && IsProductInCategory(product, category))
                {
                    // Если товар принадлежит категории "Одежда", добавляем его цену к общей цене одежды
                    skidkaPrice += product.GetPrice();
                }
            }

            // Применяем скидку только к товарам из указанной категории
            decimal discountedClothingPrice = skidkaPrice * percent;

            // Финальная цена равна общей цене всех товаров в корзине, уменьшенной на сумму скидки для товаров из указанной категории
            decimal finalPrice = totalBasketPrice - (skidkaPrice - discountedClothingPrice);

            return finalPrice;
        }

        private bool IsProductInCategory(Product product, string category)
        {
            switch (category)
            {
                case "sweaters":
                    return sweaters.Components.Contains(product);
                case "t_shirts":
                    return t_shirts.Components.Contains(product);
                case "jeans":
                    return product.Name == "Jeans";
                case "clothing":
                    return clothing.Components.Contains(product) || sweaters.Components.Contains(product);
                default:
                    return false;
            }
        }

        // Общий интерфейс для компонентов магазина
        public interface IShopComponent
        {
            decimal GetPrice(); // Метод для получения цены товара или категории
        }

        // Категория товаров
        public class Category : IShopComponent
        {
            public string Name;
            public List<IShopComponent> Components = new List<IShopComponent>();

            public Category(string name)
            {
                Name = name;
            }

            // Добавление товара или подкатегории в текущую категорию
            public void AddComponent(IShopComponent component)
            {
                Components.Add(component);
            }
        

            // Получение общей цены всех товаров в категории
            public decimal GetPrice()
            {
                decimal totalPrice = 0;
                foreach (var component in Components)
                {
                    totalPrice += component.GetPrice();
                }
                return totalPrice;

            }
        }

        // Продукт
        public class Product : IShopComponent
        {
            public string Name;
            private decimal Price;

            public Product(string name, decimal price)
            {
                Name = name;
                Price = price;
            }

            // Получение цены товара
            public decimal GetPrice()
            {
                return Price;
            }
        }

        private void LoadData()
        {
            clothing = new Category("Clothing"); // Сохраняем ссылку на категорию Clothing
            basket = new Category("Basket");
            sweaters = new Category("Sweaters");
            t_shirts = new Category("t_shirts");

            Product tShirt_1 = new Product("T-shirt1", 200);
            price_t_shirt_1.Text = "200";

            Product tShirt_2 = new Product("T-shirt2", 200);
            price_t_shirt_2.Text = "200";

            Product jeans = new Product("Jeans", 500);
            price_jeans.Text = "500";

            Product sweater_1 = new Product("Sweater1", 400);
            price_sweater_1.Text = "400";

            Product sweater_2 = new Product("Sweater2", 400);
            price_sweater_2.Text = "400";

            // Добавляем продукты в категорию Clothing
            clothing.AddComponent(sweaters);
            clothing.AddComponent(t_shirts);
            clothing.AddComponent(jeans);

            sweaters.AddComponent(sweater_1);
            sweaters.AddComponent(sweater_2);

            t_shirts.AddComponent(tShirt_1);
            t_shirts.AddComponent(tShirt_2);


        }



        private Product GetProductFromCategory(string productName, Category category)
        {
            // Предполагаем, что в указанной категории есть только один продукт с заданным именем
            foreach (IShopComponent component in category.Components)
            {
                if (component is Product product && product.Name.Equals(productName, StringComparison.OrdinalIgnoreCase))
                {
                    return product;
                }
            }
            // Если продукт не найден, возвращаем null
            return null;
        }


        private void text_promo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string userInput = text_promo.Text;
                if (userInput == "123")
                {
                    label9.Text = "Промокод применён"; //промо на категорию одежда

                }
                else { label9.Text = "Неверный промокод"; }
            }
        }

        private void button_add_sweater_1_Click_1(object sender, EventArgs e)
        {
            Product sweaterProduct = GetProductFromCategory("Sweater1", sweaters);
            if (sweaterProduct != null)
            {
                basket.AddComponent(sweaterProduct); // Добавляем продукт в корзину из категории "Шерсть"
            }

        }

        private void button_add_sweater_2_Click(object sender, EventArgs e)
        {
            Product sweaterProduct = GetProductFromCategory("Sweater2", sweaters);
            if (sweaterProduct != null)
            {
                basket.AddComponent(sweaterProduct); // Добавляем продукт в корзину из категории "Шерсть"
            }

        }

        private void button_add_t_shirt_2_Click(object sender, EventArgs e)
        {
            Product tShirtProduct = GetProductFromCategory("T-shirt2", t_shirts);
            if (tShirtProduct != null)
            {
                basket.AddComponent(tShirtProduct);
            }

        }

        private void button_add_t_shirt_1_Click(object sender, EventArgs e)
        {
            Product tShirtProduct = GetProductFromCategory("T-shirt1", t_shirts);
            if (tShirtProduct != null)
            {
                basket.AddComponent(tShirtProduct);
            }
        }
        private void button_add_jeans_Click(object sender, EventArgs e)
        {
            Product jeansProduct = GetProductFromCategory("Jeans", clothing);
            if (jeansProduct != null)
            {
                basket.AddComponent(jeansProduct);
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            string userInput = text_promo.Text;
            decimal finalClothingPrice = ApplyDiscountToClothingProducts(userInput); // Применяем скидку к финальной цене продуктов из категории "Одежда" с учетом промокода
            final_price.Text = finalClothingPrice.ToString();
        }

    }


    
}



   

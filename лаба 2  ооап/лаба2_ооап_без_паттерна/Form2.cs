using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace лаба2_ооап_без_паттерна
{
    public partial class Form2 : Form
    {
        public List<prod> products = new List<prod>();
        public List<prod> sweaters = new List<prod>();
        public List<prod> basket = new List<prod>();
       
        public decimal finalPrice = 0;
        public Form2()
        {
            LoadData();
            InitializeComponent();
        }
        public class prod
        {
            public string name;
            public int price;

            public prod(string name, int price)
            {
                this.name = name;
                this.price = price;
            }

            public int GetPrice()
            {
                int price = 0;
                price = this.price;
                return price;
            }
        }

        public void LoadData()
        {


            prod jeans = new prod("Jeans", 500);

            prod sweater_1 = new prod("Sweater1", 400);

            prod sweater_2 = new prod("Sweater2", 400);

            sweaters.Add(sweater_1);
            sweaters.Add(sweater_2);
            products.Add(jeans);

        }

        //123 - скидка 10% на категорю свитеров 
        //321 - скидка 50% на всю одежду
        public decimal skidka(List<prod> basket, List<prod> productList, decimal percent, decimal fin_p)
        {
            decimal price = fin_p;

            // decimal skidka = 0;
            foreach (var product in basket)
            {
                // Проверяем, содержится ли продукт в списке продуктов категории
                if (productList.Any(p => p.name == product.name))
                {
                    price = (price - product.GetPrice() + (product.GetPrice() * percent));
                }
            }
            return price;

        }

        private void button_add_jeans_Click(object sender, EventArgs e)
        {
            foreach (var product in products)
            {
                if (product.name == "Jeans")
                {
                    finalPrice += product.GetPrice();
                    basket.Add(product); // Добавляем продукт в корзину
                    break; // Прерываем цикл после добавления продукта
                }

            }

        }

        private void button_add_sweater_2_Click(object sender, EventArgs e)
        {

            foreach (var product in sweaters)
            {
                if (product.name == "Sweater2")
                {
                    finalPrice += product.GetPrice();
                    basket.Add(product); // Добавляем продукт в корзину
                    break; // Прерываем цикл после добавления продукта
                }

            }
        }
        private void button_add_sweater_1_Click_1(object sender, EventArgs e)
        {
            foreach (var product in sweaters)
            {
                if (product.name == "Sweater1")
                {
                    finalPrice += product.GetPrice();
                    basket.Add(product); // Добавляем продукт в корзину
                    break; // Прерываем цикл после добавления продукта
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string userInput = text_promo.Text;
            //123 - скидка 10% на категорю свитеров 
            //321 - скидка 50% на всю одежду
            decimal percent = 0m;
            if (userInput == "123")
            {
                percent = 0.9m;
                label9.Text = "скидка 10% на категорю свитеров ";
                decimal sk = skidka(basket, sweaters, percent, finalPrice);
                finalPrice = sk;

            }
            else if (userInput == "321")
            {
                percent = 0.5m;
                label9.Text = "скидка 50% на всю одежду";
                finalPrice = finalPrice * percent;
               

            }
            final_price.Text = "" + finalPrice;
        }
    }
}

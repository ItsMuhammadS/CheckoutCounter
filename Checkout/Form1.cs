using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Checkout
{
    public partial class Form1 : Form
    {
        List<Product> products;
        List<Promotion> promotions;
        List<Order> orders;
        

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            orders = new List<Order>();
             products = new List<Product>() {
                new Product{id=1,name="A",price=0.50},
                new Product{id=2,name="B",price=1.00},

                new Product{id=3,name="C",price=2.50},

                new Product{id=4,name="D",price=2.00}

            };

            promotions = new List<Promotion>()
            {
                new Promotion{productId=1,promotionItems=2,specialPrice=0.75},
                new Promotion{productId=2,promotionItems=3,specialPrice=2.50},
                new Promotion{productId=4,promotionItems=2,specialPrice=3.00}
            };


           



            comboBox1.DataSource = products;
            comboBox1.DisplayMember= "name";
            comboBox1.ValueMember = "id";
            textBox1.Text = "Instructions" +
                " There are 4 products in the drop down list to select from. \n" +
                "Name=A Regular Price: 0.50. Promotional Price if bought 2 items=0.75. \n" +
                "Name=B Regular Price: 1.00. Promotional Price if bought 3 items=2.50. \n" +
                "Name=C Regular Price: 2.50. No Promotional Price. \n" +
                "Name=D Regular Price: 2.00. Promotional Price if bought 2 items=3.00. \n" +
                "Please select product and add any quantity, then press Add Product. \n" +
                "All Products will be added to the sum of total price.";


        }

        private void button1_Click(object sender, EventArgs e)
        {
            txtTotal.Text = "0";
            var obj = orders.FirstOrDefault(p => p.productId.ToString() == comboBox1.SelectedValue.ToString());

            if (obj != null)
            {
                obj.qty = obj.qty + int.Parse(txtQty.Text);
            }
            else
            {

                orders.Add(

                    new Order { id = decimal.Parse(DateTime.Now.ToString("ddMMyyyyhhmmss")), productId = int.Parse(comboBox1.SelectedValue.ToString()), qty = int.Parse(txtQty.Text.ToString()) }


                );
            }
            


            double calAmt = 0;
            foreach (var item in orders)
            {
                Promotion pro = promotions.Find((Promotion p) => p.productId == item.productId);

                #region


                //IEnumerable<Promotion> pro = promotions.Where(p => p.productId == item.productId).OrderBy(p => p.productId);


                //foreach (var item2 in pro)
                //{


                //}
                //var pro = promotions
                //    .Where(p => p.productId == item.productId)
                //    .OrderBy(p => p.productId)
                //    .Select(p => p);
                #endregion
                Product product = products.Find((Product pr) => pr.id == item.productId);
                if (pro == null)
                {
                    for (int i = 0; i < item.qty; i++)
                    {
                        calAmt += product.price;
                    }

                }
                else
                {

                    

                        int actualDiscount = 0;
                        int remainingQty = 0;
                        remainingQty = checkPromotionItems(item.qty, pro.promotionItems, out actualDiscount);
                        calAmt = calculateAmt(remainingQty, pro.promotionItems, pro.specialPrice, product.price, actualDiscount);
                        remainingQty = 0;
                        actualDiscount = 0;


                }
                txtTotal.Text = (double.Parse(txtTotal.Text) + calAmt).ToString();

            }

            

        }

        private int checkPromotionItems(int qty, int proItem, out int actualDiscount)
        {
            actualDiscount = 0;

            
            for (int i = qty; qty >= proItem;)
            {
                qty = qty - proItem;
                actualDiscount += proItem;


            }
            return qty;

            

        }

        private double calculateAmt(int rem, int pro, double priceDicounted, double priceActual, int actualDiscount)
        {

            double amt = (actualDiscount / pro) * priceDicounted;
            
            for(int i=0;i<rem;i++)
            {
                amt += priceActual;
            }

            return amt;
        }
    }
}

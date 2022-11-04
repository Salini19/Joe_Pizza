using Joe_Pizza.Models;
using System.Xml.Serialization;

namespace Jow_Pizza.Test
{
    public class Tests
    {
        [TestFixture]
        public class Test
        {
            Pizza pizza1 = new Pizza();

            [TestCase]
            public void AddPizza()
            {
                
                pizza1.ProductName = "Chicken Pizza";
                pizza1.ProductPrice = 250;
                pizza1.ProductImage = "~/Image/p9.jpg";
                pizza1.ProductDescription = "When you want to jazz up your cheese pizza with color and texture, veggies are the perfect topping. And you’re only limited by your imagination. Everything from peppers and mushrooms, to eggplant and onions make for an exciting and tasty veggie pizza.";

                bool ans = pizza1.AddPizza(pizza1);
                Assert.IsTrue(ans);

            }
            [TestCase]
            public void PizzaAvailabiltiyCheck()
            {
               
                bool ans = pizza1.AvailabilityCheck();
                Assert.AreEqual(ans, true);
            }
           
            [TestCase(7,ExpectedResult =true)]
            [TestCase(13,ExpectedResult =false)]
            public bool CheckPizza(int id)
            {             
                return  pizza1.CheckPizza(id);            
                
            }

        }
    }
}
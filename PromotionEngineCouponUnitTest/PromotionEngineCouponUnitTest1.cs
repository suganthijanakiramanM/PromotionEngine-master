using System;
using NUnit.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using PromotionCouponEngine;

namespace PromotionEngineCouponUnitTest
{
    
    public class PromotionEngineCouponUnitTest1
    {

        PromotionCalculator promotionCalculator = new PromotionCalculator();
        
        [SetUp]
        public void Setup()
        {
        }

        //Testing single product Promotion
        
        [TestCase("A", 3, 130)]
        [TestCase("B", 2, 45)]        
        [TestCase("C", 3, 60)]
        [TestCase("D", 4, 60)]
        public void Single_product_promotion(string product, int quantity, float expectedPrice)
        {
            Dictionary<string, int> salesInvoice = new Dictionary<string, int>();
            salesInvoice.Add(product, quantity);
            float totalPrice = promotionCalculator.calculatePromotion(salesInvoice);
            NUnit.Framework.Assert.AreEqual(totalPrice, expectedPrice);
        }

        [Test, TestCaseSource("ComboProduct")]
        public void Multiple_product_promotion(Dictionary<string, int> salesInvoice, float expectedPrice)
        {
            float totalPrice = promotionCalculator.calculatePromotion(salesInvoice);
            NUnit.Framework.Assert.AreEqual(totalPrice, expectedPrice);
        }
        static object[] ComboProduct =
        {
             new object[]{new Dictionary<string, int>(){{"A",1},{"B",1},{"C",1},{"D",1}},110 },
             new object[]{new Dictionary<string, int>(){{"A",5},{"B",5},{"C",1}},370 },
             new object[]{new Dictionary<string, int>(){{"A",3},{"B",5},{"C",1},{"D",1}},280 },
        };
    }
}

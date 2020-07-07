using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PromotionCouponEngine
{
    class Program
    {       
        static void Main(string[] args)
        {
            Console.WriteLine("**************Promotions Engine************");
        }
        
    }


    // The SKUId's list with unit price.
    public class ProductsList
    {
        public Dictionary<string, float> SKUID = new Dictionary<string, float>();
        public ProductsList()
        {
            SKUID.Add("A", 50);
            SKUID.Add("B", 30);
            SKUID.Add("C", 20);
            SKUID.Add("D", 15);
        }
    }

    // promotions offer rules.
    public class PromotionsOfferRules
    {
        public List<PromotionCoupon> list_of_promotions_rules = new List<PromotionCoupon>();
        public PromotionsOfferRules()
        {
            PromotionCoupon A = new PromotionCoupon();
            A.productCoupon.Add("A", 3);
            A.discountPrice = 130;
            list_of_promotions_rules.Add(A);

            PromotionCoupon B = new PromotionCoupon();
            B.productCoupon.Add("B", 2);
            B.discountPrice = 45;
            list_of_promotions_rules.Add(B);

            PromotionCoupon CD = new PromotionCoupon();
            CD.productCoupon.Add("C", 1);
            CD.productCoupon.Add("D", 1);
            CD.discountPrice = 30;
            list_of_promotions_rules.Add(CD);
        }
    }
    // Promotion rule with discount price.
    public class PromotionCoupon
    {
        public Dictionary<string, int> productCoupon = new Dictionary<string, int>();
        public string inclusiontype = "AND";
        public float discountPrice;
    }
    public class PromotionCalculator
    {
        // Total price calculation after deducting discouted price
        public float calculatePromotion(Dictionary<string, int> SalesInvoice)
        {
            ProductsList products = new ProductsList();
            PromotionsOfferRules promotions = new PromotionsOfferRules();
            float totalPrice = 0;
            float promotionsOffers = 0;

            // Apply each promotion on the sales invoice and get the toatl offervalue.     
            foreach (var promotion in promotions.list_of_promotions_rules)
            {
                var offer = applyPromotionCoupon(promotion, SalesInvoice);
                promotionsOffers = promotionsOffers + offer;
            }

            // Calculate the total price for item where default rule not applied
            var saleList = new List<string>(SalesInvoice.Keys);
            foreach (var sale in saleList)
            {
                var price = products.SKUID[sale] * SalesInvoice[sale];
                totalPrice = totalPrice + price;
                Console.WriteLine("Offer:{0} => {1}", price, totalPrice);
            }
            Console.WriteLine("Offer:{0}", totalPrice);
            return promotionsOffers + totalPrice;
        }

        // Apply promotions on the the sales
        float applyPromotionCoupon(PromotionCoupon promotions, Dictionary<string, int> SaleInvoice)
        {

            if (promotions.productCoupon.Count == 1)     // for Single coupon
            {
                var key = promotions.productCoupon.ElementAt(0).Key;
                if (SaleInvoice.ContainsKey(key))
                {
                    int quantityInvoice = SaleInvoice[key];
                    int offerQuantity = promotions.productCoupon[key];
                    int offerscount = quantityInvoice / offerQuantity;
                    int remainingQuantity = quantityInvoice % offerQuantity;
                    SaleInvoice[key] = remainingQuantity;
                    return offerscount * promotions.discountPrice;
                }
            }
            else
            {  // multi product coupon, considered for 2 products combination with AND operation
                var key1 = promotions.productCoupon.ElementAt(0).Key;
                var key2 = promotions.productCoupon.ElementAt(1).Key; ;
                if (SaleInvoice.ContainsKey(key1) && SaleInvoice.ContainsKey(key2))
                {
                    int quantityInvoice1 = SaleInvoice[key1];
                    int offerQuantity1 = promotions.productCoupon[key1];
                    int offerscount1 = quantityInvoice1 / offerQuantity1;
                    int remainingQuantity1 = quantityInvoice1 % offerQuantity1;

                    int quantityInvoice2 = SaleInvoice[key2];
                    int offerQuantity2 = promotions.productCoupon[key2];
                    int offerscount2 = quantityInvoice2 / offerQuantity2;
                    int remainingQuantity2 = quantityInvoice2 % offerQuantity2;

                    if (offerscount1 <= offerscount2) // consider combo offer
                    {
                        SaleInvoice[key1] = remainingQuantity1;
                        SaleInvoice[key2] = remainingQuantity2 + (offerQuantity2 * (offerscount2 - offerscount1));
                        return offerscount1 * promotions.discountPrice;
                    }
                    else
                    {
                        SaleInvoice[key1] = remainingQuantity1 + (offerQuantity1 * (offerscount1 - offerscount2));
                        SaleInvoice[key2] = remainingQuantity2;
                        return offerscount2 * promotions.discountPrice;
                    }
                }
            }
            return 0;
        }


    }

}

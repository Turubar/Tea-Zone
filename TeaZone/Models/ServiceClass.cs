using System.Text.RegularExpressions;

namespace TeaZone.Models
{
    public static class ServiceClass
    {
        public static string FormatLogin(string str)
        {
            if (str == null || str == "") return "";

            string result = "";
            foreach (char c in str)
            {
                if ((char.IsDigit(c)) || (c >= 65 && c <= 90) || (c >= 97 && c <= 122))
                {
                    result += c.ToString();
                }
                else
                {
                    str.Replace(c.ToString(), "");
                }
            }
            return result;
        }

        public static string FormatName(string str)
        {
            if (str == null || str == "") return "";

            string result = "";
            foreach (char c in str)
            {
                if (c >= 'А' && c <= 'я')
                {
                    result += c.ToString();
                }
                else
                {
                    str.Replace(c.ToString(), "");
                }
            }

            return result;
        }

        public static bool FormatEmail(string str)
        {
            if (str == null || str == "") return false;

            var regex = new Regex("^[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?$");
            return regex.IsMatch(str);
        }

        public static string FormatPassword(string str)
        {
            if (str == null || str == "") return "";

            string result = "";
            foreach (char c in str)
            {
                if (char.IsDigit(c) || ((c >= 65 && c <= 90) || (c >= 97 && c <= 122)) || (c >= 'А' && c <= 'я'))
                {
                    result += c.ToString();
                }
                else
                {
                    str.Replace(c.ToString(), "");
                }
            }

            return result;
        }

        public static (List<float>, List<string>, List<float>, List<float>) GetProductSet(string productSet, float productPrice)
        {
            if(productSet == null || productPrice == 0) return (null, null, null, null);

            List<float> priceGram = new List<float>();
            List<string> elementSet = new List<string>();
            List<float> priceProduct = new List<float>();
            List<float> priceDiscount = new List<float>();

            string[] set = productSet.Split('|');

            string[] pancake = set[0].Split('/');
            string[] tun = set[1].Split('/');
            string[] element = set[2].Split('/');
            string[] discount = set[3].Split('/');

            if (tun[0] != "0")
            {
                elementSet.Add("Тун (" + tun[1] + " блин.)");
                priceProduct.Add(Convert.ToInt32(tun[1]) * Convert.ToInt32(pancake[1]) * productPrice);
                priceGram.Add(productPrice);
                priceDiscount.Add(priceProduct[0]);

                if (discount[0] != "0")
                {
                    int percent = Convert.ToInt32(((Convert.ToInt32(tun[1]) * Convert.ToInt32(pancake[1])) / Convert.ToInt32(discount[1]))) * Convert.ToInt32(discount[2]);
 
                    if (percent > 0)
                    {
                        priceGram[priceGram.Count() - 1] = productPrice - (productPrice * (Convert.ToSingle(percent) / 100));
                        priceDiscount[priceDiscount.Count() - 1] = priceProduct[priceDiscount.Count() - 1] - (priceProduct[priceDiscount.Count() - 1] * (Convert.ToSingle(percent) / 100));
                    }
                }   
            }

            if (pancake[0] != "0")
            {
                elementSet.Add("Целый блин (" + pancake[1] + " г.)");
                priceProduct.Add(Convert.ToInt32(pancake[1]) * productPrice);
                priceGram.Add(productPrice);
                priceDiscount.Add(priceProduct[0]);

                if (discount[0] != "0")
                {
                    int percent = Convert.ToInt32((Convert.ToInt32(pancake[1]) / Convert.ToInt32(discount[1]))) * Convert.ToInt32(discount[2]);

                    if (percent > 0)
                    {
                        priceGram[priceGram.Count() - 1] = productPrice - (productPrice * (Convert.ToSingle(percent) / 100));
                        priceDiscount[priceProduct.Count() - 1] = priceProduct[priceProduct.Count() - 1] - (priceProduct[priceProduct.Count() - 1] * (Convert.ToSingle(percent) / 100));
                    }
                }
            }

            if (element.Length >= 1)
            {
                for (int j = 0; j < element.Length; j++)
                {
                    if (element[j] == "0")
                    {
                        continue;
                    }

                    elementSet.Add(element[j]);
                    string[] elementPart = element[j].Split(' ');
                    priceProduct.Add(Convert.ToInt32(elementPart[0]) * productPrice);
                    priceGram.Add(productPrice);
                    priceDiscount.Add(priceProduct[priceProduct.Count() - 1]);

                    if (discount[0] != "0")
                    {
                        int percent = Convert.ToInt32((Convert.ToInt32(elementPart[0]) / Convert.ToInt32(discount[1]))) * Convert.ToInt32(discount[2]);

                        if (percent > 0)
                        {

                            priceGram[priceGram.Count() - 1] = productPrice - (productPrice * (Convert.ToSingle(percent) / 100));
                            priceDiscount[priceProduct.Count() - 1] = priceProduct[priceProduct.Count() - 1] - (priceProduct[priceProduct.Count() - 1] * (Convert.ToSingle(percent) / 100));
                        }
                    }
                }
            }

            priceGram.Reverse();
            elementSet.Reverse();
            priceProduct.Reverse();
            priceDiscount.Reverse();

            return (priceGram, elementSet, priceProduct, priceDiscount);
        }

        public static (string, float, float)? GetCartSet(string productSet, float productPrice, int setNumber)
        {
            if (productSet == null || productPrice == 0) return null;

            string elementSet = "";
            float priceProduct = 0;
            float priceDiscount = 0;

            string[] set = productSet.Split('|');

            string[] pancake = set[0].Split('/');
            string[] tun = set[1].Split('/');
            string[] element = set[2].Split('/');
            string[] discount = set[3].Split('/');

             Array.Reverse(element);

            if (element.Length > 0)
            {
                if (element.Length >= setNumber + 1)
                {
                    elementSet = element[setNumber];
                    string[] elementPart = element[setNumber].Split(' ');

                    priceProduct = Convert.ToInt32(elementPart[0]) * productPrice;
                    priceDiscount = priceProduct;

                    if (discount[0] != "0")
                    {
                        int percent = Convert.ToInt32((Convert.ToInt32(elementPart[0]) / Convert.ToInt32(discount[1]))) * Convert.ToInt32(discount[2]);

                        if (percent > 0)
                        {
                            priceDiscount = priceProduct - (priceProduct * (Convert.ToSingle(percent) / 100));
                        }
                    }
                }
                else if (setNumber + 1 - element.Length == 1)
                {
                    elementSet = "Целый блин (" + pancake[1] + " г.)";
                    priceProduct = Convert.ToInt32(pancake[1]) * productPrice;
                    priceDiscount = priceProduct;

                    if (discount[0] != "0")
                    {
                        int percent = Convert.ToInt32((Convert.ToInt32(pancake[1]) / Convert.ToInt32(discount[1]))) * Convert.ToInt32(discount[2]);

                        if (percent > 0)
                        {
                            priceDiscount = priceProduct - (priceProduct * (Convert.ToSingle(percent) / 100));
                        }
                    }
                }
                else if (setNumber + 1 - element.Length == 2)
                {
                    elementSet = "Тун (" + tun[1] + " блин.)";
                    priceProduct = Convert.ToInt32(tun[1]) * Convert.ToInt32(pancake[1]) * productPrice;
                    priceDiscount = priceProduct;

                    if (discount[0] != "0")
                    {
                        int percent = Convert.ToInt32(((Convert.ToInt32(tun[1]) * Convert.ToInt32(pancake[1])) / Convert.ToInt32(discount[1]))) * Convert.ToInt32(discount[2]);

                        if (percent > 0)
                        {
                            priceDiscount = priceProduct - (priceProduct * (Convert.ToSingle(percent) / 100));
                        }
                    }
                }
            }

            return (elementSet, priceProduct, priceDiscount);
        }

        public static (List<long>, List<int>, List<int>)? GetProductCart(string? userCart)
        {
            if(userCart != null && userCart != "")
            {
                List<string> productList = userCart.Split('|').ToList();
                productList.Remove(productList[productList.Count() - 1]);

                List<long> idProduct = new List<long>();
                List<int> setNumber = new List<int>();
                List<int> countProduct = new List<int>();

                foreach (string item in productList)
                {
                    string[] partItem = item.Split("/");

                    idProduct.Add(Convert.ToInt64(partItem[0]));
                    setNumber.Add(Convert.ToInt32(partItem[1]));
                    countProduct.Add(Convert.ToInt32(partItem[2]));
                }

                return (idProduct, setNumber, countProduct);
            }
            else return null;
        }

        public static string FormatAddToCart(string userCart, string product)
        {
            List<string> productList = new List<string>();

            if(userCart != "")
            {
                productList = userCart.Split('|').ToList();
                productList.Remove(productList[productList.Count() - 1]);
            }

            
            int index = productList.FindIndex(x => x.Contains(product));

            if (index >= 0)
            {
                string[] partItem = productList[index].Split('/');
                int count = Convert.ToInt32(partItem[2]) + 1;
                productList[index] = partItem[0] + "/" + partItem[1] + "/" + count;
            }
            else
            {
                productList.Add(product + "/" + "1");
            }

            string updatedCart = "";
            foreach(string item in productList)
            {
                updatedCart += item + "|";
            }

            return updatedCart;
        }
    }
}

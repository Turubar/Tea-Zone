using MailKit.Net.Smtp;
using MimeKit;
using TeaZone.Models.Entities;

namespace TeaZone.Models
{
    public static class DataClass
    {
        public static Account? FindLogin(string? login)
        {
            using (var context = new TeaZoneDbContext())
            {
                return context.Accounts.FirstOrDefault(x => x.Login == login);
            }
        }

        public static Account? FindEmail(string email)
        {
            using (var context = new TeaZoneDbContext())
            {
                return context.Accounts.FirstOrDefault(x => x.Email == email);
            }
        }

        public static long AvailableID()
        {
            Random random = new Random();
            while (true)
            {
                using (var context = new TeaZoneDbContext())
                {
                    long id = random.Next(minValue: 3, maxValue: int.MaxValue);
                    if (context.Accounts.Find(id) == null)
                    {
                        return id;
                    }
                }
            }
        }

        public static int SendCode(string email)
        {
            int code = new Random().Next(minValue: 100000, maxValue: 1000000);

            try
            {
                MimeMessage message = new MimeMessage();
                message.From.Add(new MailboxAddress("Интернет-магазин Tea Zone (Чайная зона)", "tea.zone2023@gmail.com"));
                message.To.Add(new MailboxAddress("Клиент Чайной зоны", email));
                message.Subject = "Код для подтверждения электронной почты";
                message.Body = new BodyBuilder() { HtmlBody = $"<div style=\"color: green;\">Ваш код подтверждения: [ {code} ].</div>" }.ToMessageBody();

                using (var client = new SmtpClient())
                {
                    client.Connect("smtp.gmail.com", 465, true);
                    client.Authenticate("tea.zone2023@gmail.com", "imxgpocsnvibeuoh");
                    client.Send(message);
                    client.Disconnect(true);
                    return code;
                }
            }
            catch
            {
                return 0;
            }

        }

        public static bool VerifyEmail(string login)
        {
            try
            {
                using (var context = new TeaZoneDbContext())
                {
                    Account? user = context.Accounts.FirstOrDefault(x => x.Login == login);
                    if (user != null)
                    {
                        user.EmailVerify = "true";
                        context.SaveChanges();
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch
            {
                return false;
            }

            return true;
        }

        public static bool LoadAvatar(string login, byte[] image)
        {
            try
            {
                using (var context = new TeaZoneDbContext())
                {
                    Account? user = context.Accounts.FirstOrDefault(x => x.Login == login);
                    if (user != null)
                    {
                        user.Avatar = image;
                        context.SaveChanges();
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch
            {
                return false;
            }

            return true;
        }

        public static bool DeleteAvatar(string login)
        {
            try
            {
                using (var context = new TeaZoneDbContext())
                {
                    Account? user = context.Accounts.FirstOrDefault(x => x.Login == login);
                    if (user != null)
                    {
                        user.Avatar = null;
                        context.SaveChanges();
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch
            {
                return false;
            }

            return true;
        }

        public static bool ChangePassword(string login, string password)
        {
            try
            {
                using (var context = new TeaZoneDbContext())
                {
                    Account? user = context.Accounts.FirstOrDefault(x => x.Login == login);
                    if (user != null)
                    {
                        user.Password = password;
                        context.SaveChanges();
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch
            {
                return false;
            }

            return true;
        }

        public static TeaZone.Models.Entities.Type? GetType(int id)
        {
            try
            {
                using (var context = new TeaZoneDbContext())
                {
                    return context.Types.FirstOrDefault(x => x.Id == id);
                }
            }
            catch
            {
                return null;
            }
        }

        public static (List<long>, List<string>, List<string>, List<byte[]>, List<byte[]>, List<byte[]>, List<byte[]>, List<byte[]>, List<int>, List<List<float>>, List<List<string>>, List<List<float>>, List<List<float>>)? GetTeaProducts(string nameType)
        {
            List<long>? id = new List<long>();
            List<string>? name = new List<string>();
            List<string>? description = new List<string>();
            List<byte[]>? img1 = new List<byte[]>();
            List<byte[]>? img2 = new List<byte[]>();
            List<byte[]>? img3 = new List<byte[]>();
            List<byte[]>? img4 = new List<byte[]>();
            List<byte[]>? img5 = new List<byte[]>();
            List<int>? form = new List<int>();
            List<List<float>>? priceGram = new List<List<float>>();
            List<List<string>>? elementSet = new List<List<string>>();
            List<List<float>>? priceElement = new List<List<float>>();
            List<List<float>>? priceDiscount = new List<List<float>>();

            try
            {
                using (var context = new TeaZoneDbContext())
                {
                    IEnumerable<ITeaProduct>? product = null;
                    if (nameType == "Пуэр")
                        product = context.PuerTables.ToList();
                    else if (nameType == "Улун")
                        product = context.OolongTables.ToList();
                    else if (nameType == "Зеленый чай")
                        product = context.GreenTeaTables.ToList();
                    else if (nameType == "Красный чай")
                        product = context.RedTeaTables.ToList();
                    else if (nameType == "Белый чай")
                        product = context.WhiteTeaTables.ToList();
                    else if (nameType == "Хэй ча")
                        product = context.DarkTeaTables.ToList();

                    foreach (ITeaProduct element in product)
                    {
                        id.Add(element.IdProduct);
                        description.Add(element.Description);
                        form.Add((int)element.IdForm);

                        var tuple = ServiceClass.GetProductSet(element.ProductSet, Convert.ToSingle(element.Price));
                        priceGram.Add(tuple.Item1);
                        elementSet.Add(tuple.Item2);
                        priceElement.Add(tuple.Item3);
                        priceDiscount.Add(tuple.Item4);

                        Product? tovar = context.Products.FirstOrDefault(id => id.Id == element.IdProduct);
                        name.Add(tovar.Name);
                        img1.Add(tovar?.Image1);
                        img2.Add(tovar?.Image2);
                        img3.Add(tovar?.Image3);
                        img4.Add(tovar?.Image4);
                        img5.Add(tovar?.Image5);
                    }

                    return (id, name, description, img1, img2, img3, img4, img5, form, priceGram, elementSet, priceElement, priceDiscount);
                }
            }
            catch
            {
                return null;
            }
        }

        public static (List<long>, List<byte[]>, List<string>, List<int>, List<int>, List<string>, List<int>, List<float>, List<float>, List<int>)? GetCart(Account user)
        {
            (List<long> idProduct, List<int> setNumber, List<int> countProduct)? productCart = ServiceClass.GetProductCart(user.Cart);

            List<byte[]> imgProduct= new List<byte[]>();
            List<string> nameProduct= new List<string>();
            List<int> type = new List<int>();
            List<int> form = new List<int>();
            List<string> elementSet = new List<string>();
            List<float> productPrice = new List<float>();
            List<float> productDiscount = new List<float>();

            if(productCart != null) 
            {
                try
                {
                    using (var context = new TeaZoneDbContext())
                    {
                        ITeaProduct product = null;
                        int count = 0;
                        foreach (long id in productCart.Value.idProduct)
                        {
                            imgProduct.Add(context.Products.FirstOrDefault(x => x.Id == id).Image1);
                            nameProduct.Add(context.Products.FirstOrDefault(x => x.Id == id).Name);
                            type.Add(Convert.ToInt32(context.Products.FirstOrDefault(x => x.Id == id).IdType));

                            if (type[type.Count() - 1] == 1) product = context.PuerTables.FirstOrDefault(x => x.IdProduct == id);
                            else if (type[type.Count() - 1] == 2) product = context.OolongTables.FirstOrDefault(x => x.IdProduct == id);
                            else if (type[type.Count() - 1] == 3) product = context.GreenTeaTables.FirstOrDefault(x => x.IdProduct == id);
                            else if (type[type.Count() - 1] == 4) product = context.RedTeaTables.FirstOrDefault(x => x.IdProduct == id);
                            else if (type[type.Count() - 1] == 5) product = context.WhiteTeaTables.FirstOrDefault(x => x.IdProduct == id);
                            else if (type[type.Count() - 1] == 6) product = context.DarkTeaTables.FirstOrDefault(x => x.IdProduct == id);

                            form.Add(Convert.ToInt32(product.IdForm));

                            (string elementSet, float productPrice, float productDiscount)? cartSet = ServiceClass.GetCartSet(product.ProductSet, Convert.ToSingle(product.Price), productCart.Value.setNumber[count]);

                            elementSet.Add(cartSet.Value.elementSet);
                            productPrice.Add(cartSet.Value.productPrice);
                            productDiscount.Add(cartSet.Value.productDiscount);

                            count++;
                        }
                    }
                }
                catch
                {
                    return null;
                }

                return (productCart.Value.idProduct, imgProduct, nameProduct, type, form, elementSet, productCart.Value.countProduct, productPrice, productDiscount, productCart.Value.setNumber);
            }
            else return null;
        }

        public static string AddToCart(Account user, string product)
        {
            string? userCart = user.Cart;
            string newCart = "";

            if (userCart == null) newCart = ServiceClass.FormatAddToCart("", product);
            else newCart = ServiceClass.FormatAddToCart(userCart, product);

            try
            {
                using (var context = new TeaZoneDbContext())
                {
                    var account = context.Accounts.FirstOrDefault(x => x.Id == user.Id);
                    if (account != null)
                    {
                        account.Cart = newCart;
                        context.SaveChanges();

                        return "1";
                    }
                    else return "0";
                }
            }
            catch
            {
                return "0";
            }
        }

        public static string ChangeCountProduct(Account user, string changeCount)
        {
            (List<long> idProduct, List<int> setNumber, List<int> countProduct)? productCart = ServiceClass.GetProductCart(user.Cart);

            if(productCart != null)
            {
                string[] arrayChangeCount = changeCount.Split("/");
                int deleteIndex = -1;

                for(int i = 0; i < productCart.Value.idProduct.Count(); i++) 
                {
                    if (productCart.Value.idProduct[i] == Convert.ToInt64(arrayChangeCount[0]) && productCart.Value.setNumber[i] == Convert.ToInt32(arrayChangeCount[1])) 
                    {
                        if (Convert.ToInt32(arrayChangeCount[2]) > 0 && Convert.ToInt32(arrayChangeCount[2]) <= 100)
                        {
                            productCart.Value.countProduct[i] = Convert.ToInt32(arrayChangeCount[2]);
                        }
                        else deleteIndex = i;
                    }
                }

                if(deleteIndex != -1)
                {
                    productCart.Value.idProduct.RemoveAt(deleteIndex);
                    productCart.Value.setNumber.RemoveAt(deleteIndex);
                    productCart.Value.countProduct.RemoveAt(deleteIndex);
                }

                string updatedCart = "";
                for (int i = 0; i < productCart.Value.idProduct.Count(); i++)
                {
                    string product = productCart.Value.idProduct[i] + "/" + productCart.Value.setNumber[i] + "/" + productCart.Value.countProduct[i];
                    updatedCart += product + "|";
                }

                try
                {
                    using (var context = new TeaZoneDbContext())
                    {
                        var account = context.Accounts.FirstOrDefault(x => x.Id == user.Id);
                        if (account != null)
                        {
                            if (updatedCart != "" && updatedCart != null) account.Cart = updatedCart;
                            else account.Cart = null;

                            context.SaveChanges();

                            return "1";
                        }
                        else return "0";
                    }
                }
                catch
                {
                    return "0";
                }
            }
            return "0";
        }
    }
}
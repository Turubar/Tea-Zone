using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TeaZone.Models.Entities;
using System.ComponentModel.DataAnnotations;
using TeaZone.Models;

namespace TeaZone.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult EntryPage()
        {
            return View(model: "");
        }

        [HttpPost]
        public IActionResult EntryPage(string login, string password)
        {
            if (login != null && password != null)
            {
                try
                {
                    using (var context = new TeaZoneDbContext())
                    {
                        var account = context.Accounts.FirstOrDefault(x => (x.Login == login || x.Email == login) && x.Password == password);

                        if (account != null)
                        {
                            var claims = new List<Claim>()
                            {
                                new Claim("Login", account.Login),
                                new Claim("Name", account.Name),
                                new Claim("Role", context.Roles.First(x=> x.Id == account.IdRole).Name)
                            };

                            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                            HttpContext.SignInAsync(claimsPrincipal);
                            ViewData.Clear();
                            return Redirect("~/");
                        }
                        else
                        {
                            ViewBag.Entry = "Неверный логин или пароль";
                            return View(model: login);
                        }
                    }
                }
                catch
                {
                    ViewBag.Entry = "Что то пошло не так";
                    return View(model: login);
                }
            }
            else
            {
                ViewBag.Entry = "Неверный логин или пароль";
                return View(model: "");
            }
        }

        public IActionResult RegistrationPage()
        {
            return View(("", "", ""));
        }

        [HttpPost]
        public IActionResult RegistrationPage(string login, string nameuser, string email, string password, string conpassword)
        {
            string userLogin = "";
            if (login != null && login != "")
            {
                userLogin = ServiceClass.FormatLogin(login);
                if (userLogin != login)
                {
                    ViewBag.Login = "Недопустимые символы в логине";
                    return View((login, nameuser, email));
                }

                if (userLogin.Length < 4 || userLogin.Length > 16)
                {
                    ViewBag.Login = "Длина логина от 4 до 16 символов";
                    return View((userLogin, nameuser, email));
                }

                if (DataClass.FindLogin(userLogin) != null)
                {
                    ViewBag.Login = $"Логин [{userLogin}] уже занят";
                    return View((userLogin, nameuser, email));
                }
            }
            else
            {
                ViewBag.Login = "Неверно заполнено поле";
                return View((login, nameuser, email));
            }

            string userName = "";
            if (nameuser != null && nameuser != "")
            {
                nameuser = nameuser.ToLower();
                string first = nameuser[0].ToString().ToUpper();
                nameuser = nameuser.Remove(0, 1);
                nameuser = first + nameuser;

                userName = ServiceClass.FormatName(nameuser);
                if (userName != nameuser)
                {
                    ViewBag.Name = "Недопустимые символы в имени";
                    return View((userLogin, nameuser, email));
                }

                if (userName.Length < 2 || userName.Length > 16)
                {
                    ViewBag.Name = "Длина имени от 2 до 16 символов";
                    return View((userLogin, userName, email));
                }
            }
            else
            {
                ViewBag.Name = "Неверно заполнено поле";
                return View((userLogin, nameuser, email));
            }

            string userEmail = "";
            if (email != null && email != "")
            {
                if (!ServiceClass.FormatEmail(email) || !new EmailAddressAttribute().IsValid(email))
                {
                    ViewBag.Email = "Неверно заполнено поле";
                    return View((userLogin, userName, email));
                }

                if (DataClass.FindEmail(email) != null)
                {
                    ViewBag.Email = $"Этот email уже занят";
                    return View((userLogin, nameuser, email));
                }

                userEmail = email;
            }
            else
            {
                ViewBag.Email = "Неверно заполнено поле";
                return View((userLogin, userName, email));
            }

            string userPassword = "";
            if (password != null && password != "" && conpassword != null && conpassword != "")
            {
                if (password != conpassword)
                {
                    ViewBag.Password = "Пароли не совпадают";
                    ViewBag.Conpassword = "Пароли не совпадают";
                    return View((userLogin, userName, email));
                }

                userPassword = ServiceClass.FormatPassword(password);
                if (userPassword != password)
                {
                    ViewBag.Password = "Недопустимые символы в пароле";
                    ViewBag.Conpassword = "Недопустимые символы в пароле";
                    return View((userLogin, userName, email));
                }

                if (userPassword.Length < 8 || userPassword.Length > 16)
                {
                    ViewBag.Password = "Длина пароля от 8 до 16 символов";
                    ViewBag.Conpassword = "Длина пароля от 8 до 16 символов";
                    return View((userLogin, userName, email));
                }
            }
            else
            {
                ViewBag.Password = "Неверно заполнено поле";
                ViewBag.Conpassword = "Неверно заполнено поле";
                return View((userLogin, userName, email));
            }

            using (var context = new TeaZoneDbContext())
            {
                Account user = new Account()
                {
                    Id = DataClass.AvailableID(),
                    Login = userLogin,
                    Name = userName,
                    Email = userEmail,
                    EmailVerify = "false",
                    Password = userPassword,
                    IdRole = 3
                };

                context.Accounts.Add(user);
                context.SaveChanges();
            }

            ViewData.Clear();
            return Redirect("~/Account/ValidRegistrationPage");
        }

        public IActionResult ValidRegistrationPage()
        {
            return View();
        }

        public IActionResult LogoutPage()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("~/");
        }

        public IActionResult ProfilePage()
        {
            Account? user = DataClass.FindLogin(User?.FindFirst("Login")?.Value.ToString());
            if (user != null) return View(model: user);
            else return Redirect("/Account/LogoutPage");
        }

        [HttpPost]
        public string SendCode(string email)
        {
            if (email != null && email != "")
            {
                int code = DataClass.SendCode(email);
                return code.ToString();
            }
            else
            {
                Redirect("~/");
                return "";
            }
        }

        [HttpPost]
        public string VerifyCode(string codeInput, string codeButton)
        {
            if (codeInput == null || codeButton == null)
            {
                return "false";
            }

            bool input = int.TryParse(codeInput, out int codeUser);
            bool button = int.TryParse(codeButton, out int codeServer);

            if (input && button)
            {
                if (codeUser == codeServer)
                {
                    string userLogin = User.FindFirstValue("Login");
                    if (userLogin != null)
                    {
                        bool resultVerify = DataClass.VerifyEmail(userLogin);
                        if (resultVerify) return "true";
                        else return "false";
                    }
                    else
                    {
                        return "false";
                    }
                }
                else
                {
                    return "false";
                }
            }
            else
            {
                return "false";
            }
        }

        [HttpPost]
        public IActionResult LoadPhoto(IFormFile Avatar)
        {
            if(Avatar != null)
            {
				Account? user = DataClass.FindLogin(User?.FindFirst("Login")?.Value.ToString());
                if(user != null)
                {
					byte[] arrayByte = null;
					using (var reader = new BinaryReader(Avatar.OpenReadStream()))
					{
						arrayByte = reader.ReadBytes((int)Avatar.Length);
					}

					DataClass.LoadAvatar(user.Login, arrayByte);

					return Redirect("/Account/ProfilePage");
				}
                else
                {
					return Redirect("/Account/LogoutPage");
				}
				
			}
            else
            {
                return Redirect("/Account/ProfilePage");
            }
        }

        public IActionResult DeletePhoto()
        {
			Account? user = DataClass.FindLogin(User?.FindFirst("Login")?.Value.ToString());
			if (user != null)
            {
                DataClass.DeleteAvatar(user.Login);
                return Redirect("/Account/ProfilePage");
            }
            else
            {
                return Redirect("/Account/LogoutPage");
            }
		}

        [HttpPost]
        public string ChangePassword(string oldPassword, string newPassword, string confirmPassword)
        {
			Account? user = DataClass.FindLogin(User?.FindFirst("Login")?.Value.ToString());
            if(user != null)
            {
				if (oldPassword != null && oldPassword != "" && newPassword != null && newPassword != "" && confirmPassword != null && confirmPassword != "")
				{
					if (newPassword != confirmPassword)
						return "(Пароли не совпадают!)";

                    if (user.Password != oldPassword)
                        return "(Неверно введен старый пароль!)";

					string password = ServiceClass.FormatPassword(newPassword);
					if (password != newPassword)
						return "(Недопустимые символы в пароле!)";

					if (password.Length < 8 || password.Length > 16)
						return "(Пароль должен быть от 8 до 16 символов!)";

					if (user.Password == newPassword)
						return "(Нельзя изменить пароль на текущий!)";

					if (DataClass.ChangePassword(user.Login, password))
						return "(Пароль успешно изменен!)";
                    else
						return "(Что-то пошло не так, пароль не был изменен!)";
				}
				else
				{
					return "(Что-то пошло не так, пароль не был изменен!)";
				}
			}
            else
            {
                return "(Что-то пошло не так, пароль не был изменен!)";
			}
        }

        public IActionResult CartPage()
        {
            Account? user = DataClass.FindLogin(User?.FindFirst("Login")?.Value.ToString());
            if (user != null)
            {
                var cart = DataClass.GetCart(user);
                if (cart != null) return View(model: cart);
                else return View(model: (new List<long>(), new List<byte[]>(), new List<string>(), new List<int>(), new List<int>(), new List<string>(), new List<int>(), new List<float>(), new List<float>(), new List<int>()));
            }
            else return Redirect("/Account/EntryPage");
        }

        [HttpPost]
        public string AddToCart(string product)
        {
            if (product != null && product != "")
            {
                Account? user = DataClass.FindLogin(User?.FindFirst("Login")?.Value.ToString());
                if (user != null) return DataClass.AddToCart(user, product);
                else return "0";
            }
            else return "0";
        }

        [HttpPost]
        public string ChangeCountProduct(string changeCount)
        {
            if(changeCount != null && changeCount != "")
            {
                Account? user = DataClass.FindLogin(User?.FindFirst("Login")?.Value.ToString());
                if (user != null) return DataClass.ChangeCountProduct(user, changeCount);
                else return "0";
            }
            else return "0";
        }
    }
}

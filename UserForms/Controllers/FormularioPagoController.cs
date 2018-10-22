using Repository.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace web_forms.Controllers
{
    public class FormularioPagoController : Controller
    {
        SubscriptionsRepository subscriptionsRepository = new SubscriptionsRepository();
        ProductsFormsRepository productsFormsRepository = new ProductsFormsRepository();
        ProductsRepository productsRepository = new ProductsRepository();
        UsersRepository usersRepository = new UsersRepository();

        // GET: FormularioPago
        [HttpGet]
        public ActionResult Index(string form, int plan, string user)
        {

            ViewBag.form = form;
            ViewBag.plan = plan;
            ViewBag.user = user;

            return View();
        }

        [HttpPost]
        public ActionResult Next(string form, int plan, string user, int result)
        {
            //obtengo el formulario
            var formulario = productsFormsRepository.GetProductForm(form);

            //verificar existencia de la app token
            //obtiene ID de producto
            int idProduct = (int)formulario.IdProduct;
            if (idProduct == 0)
                return Content("ERROR: No se encontro el producto");

            //obtener usuario segun el app token y external code
            var user_r = usersRepository.GetUser(user, idProduct);
            if (user_r == null)
                return Content("ERROR: El usuario no existe");

            //paginas
            var url_ok = user_r.Products.BillingUrlOk;
            var url_error = user_r.Products.BillingUrlError;

            //si se selecciona resultado con error en el pago, retorna a pantalla de error antes de cambiar la suscripcion
            if (result == 0)
            {
                return Redirect(url_error);
            }

            //verificar si existe una suscripcion actual. Si existe se cambia el IDPERFIL. 
            var sub = subscriptionsRepository.GetUserCurrentSubscription(user_r.IdUser);

            if (sub == null)
            {
                //Si no existe, se crea la suscripcion.

                subscriptionsRepository.NewSubscription(new Repository.EntityFramework.Subscriptions
                {
                    Active = true,
                    DateCreated = DateTime.Now,
                    ExternalCode = "",
                    IdProfile = plan,
                    IdUser = user_r.IdUser,
                    IsCurrent = true,
                    RenewalDay = DateTime.Now.Day
                });
            }
            else {
                subscriptionsRepository.SetSubscriptionProfile(sub.IdSubscription, plan);
            }

            //retorna a la pagina de retorno EXITOSO del producto
            return Redirect(user_r.Products.BillingUrlOk);
        }
    }
}
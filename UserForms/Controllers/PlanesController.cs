using Repository.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using UserForms.Models;
using System.Web.Cors;
using web_forms;
using System.Diagnostics.Contracts;
using System.Text;

namespace UserForms.Controllers
{
    //var url = "notification/inittester?IDApp=asdasd&IDPlan=2342342&IDCountry=CL&UrlOk=google.cl&UrlError=google.cl&UrlNotify=google.cl&CommerceID=67565454";

    // Retorno desde el Motor de Pagos
    // HORRIBLE y PÉSIMO que esto se encuentre aquí, debería estar en la capa Models como corresponde
    public class RetornoInit
    {
        public int code { get; set; }
        public string message { get; set; }
        public string result { get; set; }  // trx del motor encriptada
        public string paymentForm { get; set; }
    }

    public class PlanesController : Controller
    {

        ProductsFormsRepository productsFormsRepository = new ProductsFormsRepository();
        ProfilesRepository profilesRepository = new ProfilesRepository();
        CobrosRepository cobrosRepository = new CobrosRepository();
        UsersRepository usersRepository = new UsersRepository();
        ConfigurationsRepository configurationRepository = new ConfigurationsRepository();
        SubscriptionsRepository subscriptionsRepository = new SubscriptionsRepository();
        ProductsRepository productsRepository = new ProductsRepository();

        // GET: PlanesSelector
        [HttpGet]
        public ActionResult Index(string form, string user, string country)
        {
            //validaciones iniciales
            if (String.IsNullOrWhiteSpace(form))
                throw new HttpException(404, "Debes indicar el parametro FORM");

            if (String.IsNullOrWhiteSpace(user))
                throw new HttpException(404, "Debes indicar el parametro USER");



            ViewBag.Pais = country;
            ViewBag.Formulario = form;

            //obtengo el formulario
            var formulario = productsFormsRepository.GetProductForm(form);

            if (formulario == null)
                throw new HttpException(404, "Formulario no existe");

            if (formulario.IdProduct == null)
                throw new HttpException(404, "Formulario no relacionado a un producto");

            if (!usersRepository.UserExist(user, (int)formulario.IdProduct))
            {
                // throw new HttpException(404, "El usuario no existe para este producto");
                //usersRepository.CreateUser(user, (int)formulario.IdProduct);
            }
               

            //obtengo listado de perfiles
            var perfiles = profilesRepository.GetProfiles((int)formulario.IdProduct).Where(e=>e.Featured == true);

            //obtengo perfiles del servicio de COBRO
            var cobros = cobrosRepository.GetCobros((int)formulario.IdProduct); //IDAPP es IDPROD

            List<Planes> planes = new List<Planes>();

            foreach (var item in perfiles)
            {
                var cobro = cobros.FirstOrDefault(e=>e.IdPlan == item.IdProfile && e.Principal == 1);

                planes.Add(new Planes
                {
                    Nombre = item.Name,
                    Valor = (cobro!=null)? cobro.Monto : "-",
                    Plan = item.IdProfile,
                    Caracteristicas = item.Description.Split(','),
                    Featured = item.Featured == null ? false : (bool)item.Featured,
                    Motivator = item.MotivatorText,
                    ShortDescription = item.ShortDescription,
                });
            }

            ViewBag.User = user;

            //obtengo usuario
            var u = usersRepository.GetUser(user, (int)formulario.IdProduct);

            //obtengo suscripcion actual del usuario
            var subs = subscriptionsRepository.GetUserCurrentSubscription(u.IdUser);

            int IdProfile = 0;

            if (subs == null)
            {
                //si no tiene suscripcion, le crea una en el estándar de suscrito sin suscripción.
                var profile = profilesRepository.GetStandardFreeSuscription(u.IdUser);
                IdProfile = profile.IdProfile;
            }
            else
            {
                IdProfile = (int)subs.IdProfile;

                if (subs.PromoActive == true)
                    ViewBag.ActivePromo = 1;
            }

            //datos del formulario
            ViewBag.UrlLogo = formulario.UrlLogo;
            ViewBag.UrlBackground = formulario.UrlBackground;
            ViewBag.ColorSuperiorBar = formulario.ColorSuperiorBar;
            ViewBag.CssClassFeatured = formulario.CssClassFeatured;
            ViewBag.CssClassActual = formulario.CssClassActual;
            ViewBag.TitleColor = formulario.TitleColor;
            ViewBag.FormTitle = formulario.Title;
            ViewBag.CurrentIdProfile = IdProfile;

            return View(planes);
        }

        [AllowCrossSiteAttribute]
        [HttpGet]
        /// <summary>Inicializa transacción con Motor de Pagos</summary>
        /// <param name="productToken">Token del producto en Freemium</param>
        /// <param name="pais">Sigla (2 dígitos) que representa al país</param>
        /// <param name="user">ID usuario externo del 3rd party</param>
        /// <param name="m">Monto (valor) codificado de la transacción</param>
        /// <param name="idPerfil">(opcional) Recibe el perfil que debe ser seteado (para flujo distinto)</param>
        /// <returns>ActionResult, redirección hacia ruta correspondiente</returns>
        public ActionResult PagarPerfilPremium(string productToken, string pais, string user, string m, string idPerfil)
        {
            string baseUrlBillingService = configurationRepository.GetConfig("BaseUrlBillingService");
            string methodUrlBillingService = configurationRepository.GetConfig("MethodUrlBillingService");

            // Descodifico el monto recibido
            byte[] data = Convert.FromBase64String(m);
            string decodedString = Encoding.UTF8.GetString(data);
            string[] arrAmount = decodedString.Split('@'); // valor multiplicado @ ghost @ multiplicador
            int total = Convert.ToInt32(arrAmount[0]);
            int multiplier = Convert.ToInt32(arrAmount[arrAmount.Length - 1]);
            int amount = total / multiplier;

            // Obtengo el producto
            var product_id = productsRepository.GetProductId(productToken); // productToken = idApp
            var product = productsRepository.GetProduct(product_id);
            var perfilPremium = profilesRepository.GetPaidProfiles(product_id).FirstOrDefault();

            //URLS
            string FreemiumUrlOk = product.BillingUrlOk;
            string FreemiumUrlError = product.BillingUrlError;
            string FreemiumUrlNotify = configurationRepository.GetConfig("FreemiumUrlNotify"); //notifica a freemium sobre la suscripcion
            
            
            string FreemiumCommerceId = configurationRepository.GetConfig("FreemiumCommerceId");

            if (product_id == 1) // MG
            {
                FreemiumCommerceId = configurationRepository.GetConfig("FreemiumCommerceIdMG");
            }
            else if (product_id == 2) // SSZ
            {
                FreemiumCommerceId = configurationRepository.GetConfig("FreemiumCommerceIdSSZ");
            }
            else if (product_id == 5) // Zana
            {
                FreemiumCommerceId = configurationRepository.GetConfig("FreemiumCommerceIdZana");
            }

            if (product.DemoMode == true)
            {
                //en caso de que el propduicto esté en modo demo, abre la pantalla de pago de prueba
                //return RedirectToAction("Index", "FormularioPago", new { form = form, plan = plan, user = user });
            }

            // Llamado a InitTransaction del Motor de Pagos
            Wach.WachHelper billingService = new Wach.WachHelper(baseUrlBillingService);

            // Creo un codExternal con los datos obtenidos
            Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            // idUserExternal, idPlan, idApp son requeridos para notify billing
            string idPerfilX = idPerfil == null ? perfilPremium.IdProfile.ToString() : idPerfil;

            string codExternal = product_id + "-" 
                                + idPerfilX + "-"           // idPlan (perfil)
                                + user + "-"                // idUserExternal
                                + productToken + "-"        // idApp
                                + unixTimestamp.ToString();

            var content = new FormUrlEncodedContent(
                new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("idUserExternal", user),
                    new KeyValuePair<string, string>("codExternal", codExternal),     // requiere trx de wp
                    new KeyValuePair<string, string>("urlOk", FreemiumUrlOk),
                    new KeyValuePair<string, string>("urlError", FreemiumUrlError),
                    new KeyValuePair<string, string>("urlNotify", FreemiumUrlNotify),
                    new KeyValuePair<string, string>("commerceID", FreemiumCommerceId),
                    new KeyValuePair<string, string>("amount", amount.ToString())
                });


            // Respuesta del motor de pagos
            var respuesta = billingService.PostSimple<RetornoInit>(methodUrlBillingService, content);

            new EventLogRepository().SetLog("Seleccion planes", "Realizando POST a " + respuesta.message);

            if (respuesta.code == 0) {
                // Transacción inicializada correctamente en el motor de pagos
                // Redirecciona a formulario de pago con valor de transacción
                return Redirect(respuesta.paymentForm + respuesta.result);

            } else {
                // El motor retorna error
                return Content(respuesta.message);
            }

        }

        [HttpGet]
        public ActionResult Seleccionar(string form, string pais, int plan, string user)
        {
            string baseUrlBillingService = configurationRepository.GetConfig("BaseUrlBillingService");
            string methodUrlBillingService = configurationRepository.GetConfig("MethodUrlBillingService");

            // Obtengo el formulario
            var formulario = productsFormsRepository.GetProductForm(form);

            // URLS
            string FreemiumUrlOk = formulario.Products.BillingUrlOk;
            string FreemiumUrlError = formulario.Products.BillingUrlError;
            string FreemiumUrlNotify = configurationRepository.GetConfig("FreemiumUrlNotify"); //notifica a freemium sobre la suscripcion
            string FreemiumCommerceId = configurationRepository.GetConfig("FreemiumCommerceId");

            if (formulario.Products.DemoMode == true) {
                // En caso de que el propduicto esté en modo demo, abre la pantalla de pago de prueba
                return RedirectToAction("Index", "FormularioPago", new { form = form, plan = plan, user = user });
            }

            // Se invoca InitTransaction del Motor de pagos
            Wach.WachHelper billingService = new Wach.WachHelper(baseUrlBillingService);

            // Creo un codExternal con los datos obtenidos
            string codExternal = pais + "-" + plan + "-" + user + "-" + formulario.Products.Token;

            var content = new FormUrlEncodedContent(
                new List<KeyValuePair<string, string>>
                {

                    new KeyValuePair<string, string>("idUserExternal", user),
                    new KeyValuePair<string, string>("codExternal", codExternal),     // requiere trx de wp
                    new KeyValuePair<string, string>("urlOk", FreemiumUrlOk),
                    new KeyValuePair<string, string>("urlError", FreemiumUrlError),
                    new KeyValuePair<string, string>("urlNotify", FreemiumUrlNotify),
                    new KeyValuePair<string, string>("commerceID", FreemiumCommerceId),
                    new KeyValuePair<string, string>("amount", "0")

                });

            //new EventLogRepository().SetLog("Seleccion planes", "Realizando POST a " + methodUrlBillingService+ " / COD ANALYTICS: "+formulario.Products.CodeAnalytics);

            var respuesta = billingService.PostSimple<RetornoInit>(methodUrlBillingService, content);

            //new EventLogRepository().SetLog("Selección planes", "Respuesta: [" + respuesta.errNumber + "]:" + respuesta.errMessage);

            if (respuesta.code == 0) {
                //return Redirect(respuesta.urlFrmPago.Replace("{TRX}",respuesta.trx).Replace("{COMM}",FreemiumCommerceId));
            } else {
                // El motor retorna error
                return Content(respuesta.message);
            }

            return Content("Error, no se pudo redireccionar");

        }



    }
}

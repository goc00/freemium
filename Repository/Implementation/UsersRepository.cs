using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository.EntityFramework;

namespace Repository.Implementation
{
    public class UsersRepository
    {
        public EntityFramework.FriPriEntities db = new EntityFramework.FriPriEntities();

        /// <summary>
        /// Get data user in function of user and product
        /// </summary>
        /// <param name="UserCode">ID Client (ExternalCode)</param>
        /// <param name="idProduct">ID Product</param>
        /// <returns></returns>
        public Users GetUserv2(string UserCode, int idProduct)
        {
            return db.Users.FirstOrDefault(e => e.ExternalCode != "0" && e.ExternalCode != "" && e.ExternalCode != null && e.ExternalCode == UserCode && e.IdProduct == idProduct);
        }


        // -------------------------------------------------- DEPRECATED --------------------------------------------------

        public Users GetUser(string UserCode, int idProduct)
        {
            var usuario = db.Users.FirstOrDefault(e => e.ExternalCode != "0" && e.ExternalCode != "" && e.ExternalCode !=null && e.ExternalCode == UserCode && e.IdProduct == idProduct);


            //si el usuario no es encontrado y el codigo de usuario tiene un valor distinto a vacio o 0, osea, que tiene un ID... le crea un usuario.
            if (usuario == null && UserCode != String.Empty && UserCode != null && UserCode!= "0")
            {
                usuario = new Users
                {
                    Active = true,
                    ExternalCode = UserCode,
                    IdProduct = idProduct
                };

                db.Users.Add(usuario);
                db.SaveChanges();
            }

            return usuario;
        }

        public bool UserExist(string UserExternalCode, int idProduct)
        {
            var user = db.Users.FirstOrDefault(e=>e.IdProduct == idProduct && e.ExternalCode == UserExternalCode);

            if (user == null)
            {
                return false;
            }
            else
                return true;
        }

        public Repository.EntityFramework.Users CreateUser(string UserExternalCode, int idProduct)
        {
            var user = new Users { Active = true, ExternalCode=UserExternalCode, IdProduct = idProduct };
            db.Users.Add(user);
            
            return user;
        }
    }
}

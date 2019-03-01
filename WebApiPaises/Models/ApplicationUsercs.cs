using Microsoft.AspNetCore.Identity;//sistema de usuarios por defecto
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiPaises.Models
{
    //hereda de identity user propiedades extra usuarios
    public class ApplicationUsercs : IdentityUser
    {
    }
}

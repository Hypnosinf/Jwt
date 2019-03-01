using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiPaises.Models
{
    //entidad para representar los paises
    public class Pais
    {

        public Pais()
        {
            Provincias = new List<Provincia>();
        }

        public int Id { get; set; }
        [StringLength(30)]
        public string Name { get; set; }
        public List<Provincia> Provincias { get; set; }
    }
}

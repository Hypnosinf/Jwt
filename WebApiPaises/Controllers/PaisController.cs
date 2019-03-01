using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiPaises.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace WebApiPaises.Controllers
{
    //1 inyeccion de dependencia para traer Dbcontext

    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PaisController : ControllerBase
    {

        //name space

        private readonly ApplicationDbContext context;


        public PaisController(ApplicationDbContext context)
        {
            this.context = context;
        }

        //no obligaco nombre accion (Get), nombre de las acciones iirelevante, se utiliza el developer
        [HttpGet]
        public IEnumerable<Pais> Get()
        {
            //return context.Paises.Include(x => x.Provincias ).ToList();
            return context.Paises.ToList();
        }


        //poner nombre a ruta 
        [HttpGet("{id}", Name ="PaisCreado")]
        public IActionResult GetById(int id)
        {
            //bsucar pais por su id
            var pais = context.Paises.Include(x => x.Provincias).FirstOrDefault(x => x.Id == id);

            if(pais == null)
            {
                return NotFound();
            }

            return Ok(pais);
        }

        //fromBody sacar del cuerpo de la peticion Http
        [HttpPost]
        public IActionResult Post([FromBody] Pais pais)
        {
            if (ModelState.IsValid)
            {
                context.Paises.Add(pais);
                context.SaveChanges();
                //segundo argumento agrega los routes values id
                return new CreatedAtRouteResult("PaisCreado", new { id = pais.Id }, pais);
            }

            return BadRequest(ModelState);
        }


        [HttpPut("{id}")]
        public IActionResult Put([FromBody] Pais pais, int id)
        {
           if(pais.Id != id)
            {
                return BadRequest();
            }


            context.Entry(pais).State = EntityState.Modified;
            context.SaveChanges();

            return Ok();
        }


        //solo pasamos id
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {

            var pais = context.Paises.FirstOrDefault(x => x.Id == id);

            if(pais == null)
            {
                return NotFound();
            }

            context.Paises.Remove(pais);
            context.SaveChanges();
            return Ok(pais);
        }

    }
}
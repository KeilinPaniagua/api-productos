using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Models;
using Microsoft.AspNetCore.Cors;

namespace API.Controllers
{
    [EnableCors("ReglasCors")]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductoController : ControllerBase
    {
        public readonly DbapiContext dbapiContext;

        public ProductoController(DbapiContext context)
        {
            dbapiContext = context;
        }

        [HttpGet]
        [Route("Lista")]
        public IActionResult Lista()
        {
            List<Producto> lista = new List<Producto>();

            try
            {
                lista = dbapiContext.Productos.Include(p => p.oCategoria).ToList();
                return StatusCode(StatusCodes.Status200OK , new { mensaje = "ok", response = lista });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { mensaje = ex.Message, response = lista });
                throw;
            }
        }

        [HttpGet]
        [Route("Obtener/{idProducto:int}")]
        public IActionResult Obtener(int idProducto)
        {
            Producto? oProducto = dbapiContext.Productos.Find(idProducto);

            if (oProducto == null)
            { 
                return BadRequest("Producto no encontrado");
            }

            try
            {
                oProducto = dbapiContext.Productos.Include(c => c.oCategoria).Where(p => p.IdProducto == idProducto).FirstOrDefault();
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = oProducto });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { mensaje = ex.Message, response = oProducto });
                throw;
            }
        }

        [HttpPost]
        [Route("Guardar")]
        public IActionResult Guardar([FromBody] Producto objecto)
        {
            try
            {
                dbapiContext.Productos.Add(objecto);
                dbapiContext.SaveChanges();
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok"});
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { mensaje = ex.Message });
                throw;
            }
        }

        [HttpPut]
        [Route("Editar")]
        public IActionResult Editar([FromBody] Producto objecto)
        {
            Producto? oProducto = dbapiContext.Productos.Find(objecto.IdProducto);

            if (oProducto == null)
            {
                return BadRequest("Producto no encontrado");
            }

            try
            {
                oProducto.CodigoBarra = objecto.CodigoBarra is null ? oProducto.CodigoBarra : objecto.CodigoBarra;
                oProducto.Descripcion = objecto.Descripcion is null ? oProducto.Descripcion : objecto.Descripcion;
                oProducto.Marca = objecto.Marca is null ? oProducto.Marca : objecto.Marca;
                oProducto.IdCategoria = objecto.IdCategoria is null ? oProducto.IdCategoria : objecto.IdCategoria;
                oProducto.Precio = objecto.Precio is null ? oProducto.Precio : objecto.Precio;

                dbapiContext.Productos.Update(oProducto);
                dbapiContext.SaveChanges();
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { mensaje = ex.Message });
                throw;
            }
        }

        [HttpDelete]
        [Route("Eliminar/{idProducto:int}")]
        public IActionResult Eliminar(int idProducto)
        {
            Producto? oProducto = dbapiContext.Productos.Find(idProducto);

            if (oProducto == null)
            {
                return BadRequest("Producto no encontrado");
            }

            try
            {

                dbapiContext.Productos.Remove(oProducto);
                dbapiContext.SaveChanges();
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { mensaje = ex.Message });
                throw;
            }
        }
    }
}

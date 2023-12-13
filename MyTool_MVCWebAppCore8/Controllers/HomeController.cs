using System.Diagnostics;
using ImageProcessor;
using ImageProcessor.Plugins.WebP.Imaging.Formats;
using Microsoft.AspNetCore.Mvc;
using MyTool_MVCWebAppCore8.Models;

namespace MyTool_MVCWebAppCore8.Controllers
{
    public class HomeController : Controller
    {
        private readonly IWebHostEnvironment _environment;

        public HomeController(IWebHostEnvironment webHostEnvironment)
        {
            _environment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [DisableRequestSizeLimit]
        public IActionResult Index(List<IFormFile> imagenes)
        {
            List<Imagen> listaImagenes = new List<Imagen>();

            foreach (var imagen in imagenes)
            {
                //se cambia la extencion de la imagen a webp
                string webpImagen = Path.GetFileNameWithoutExtension(imagen.FileName) + ".webp";
                //se define la ruta donde se guardara la imagen webp en la carpeta Images
                string rutaImagenWebP = Path.Combine(_environment.WebRootPath, "Images", webpImagen);

                //convierte la imagen webp
                using (var webpImageFileStream = new FileStream(rutaImagenWebP, FileMode.Create))
                {
                    using (ImageFactory imageFactory = new ImageFactory(preserveExifData: false))
                    {
                        imageFactory.Load(imagen.OpenReadStream()).Format(new WebPFormat()).Quality(65).Save(webpImageFileStream);
                    }
                }

                // Agregar la información de la imagen a la lista
                listaImagenes.Add(new Imagen
                {
                    NomImagen = webpImagen,
                    RutaImagen = "/Images/" + webpImagen,
                });
            }

            return View(listaImagenes);
        }
    }
}

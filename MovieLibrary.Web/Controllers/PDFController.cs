using AutoMapper;
using MovieLibrary.Business;
using MovieLibrary.Web.Helpers;
using MovieLibrary.Web.Models;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Mvc;

namespace MovieLibrary.Web.Controllers
{
    public class PDFController : Controller
    {
        // GET: PDF
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public void Get(int id)
        {
            //Get path from Web.Config file AppSettings.  
            string path = Server.MapPath(ConfigurationManager.AppSettings["PdfPath"]);
            var user = UsersRepository.Get(id);
            PDFGenerator pdf = new PDFGenerator();
            if (user == null)
            {
                pdf.CreateEmptyDocument();
                pdf.ExportDocument(path, pdf.document);
            }
            else
            {
                pdf.user = Mapper.Map<UserDisplayModel>(user);
                var rented = MoviesRepository.RentedMovies(id);
                pdf.rentedMovieModels = Mapper.Map<List<RentedMovieModel>>(rented);
                pdf.CreateDocument();
                pdf.ExportDocument(path, pdf.document);
            }
        }




    }
}
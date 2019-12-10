using MovieLibrary.Business;
using MovieLibrary.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using MovieLibrary.Business.DTO;
using System.Net;
using log4net;
using MovieLibrary.Web.CutomFilters;
using System.IO;
using System.Configuration;
using OfficeOpenXml;
using System.Globalization;
using System.Threading;
using System.Text;
using PagedList;
using System.Diagnostics;

namespace MovieLibrary.Web.Controllers
{
    [LogAction]
    public class MovieLibraryController : Controller
    {
        //private static readonly ILog Log = LogManager.GetLogger(typeof(MovieLibraryController));

        // GET: MovieLibrary
        public ActionResult Index()
        {
            return View();
            //return RedirectToAction("Movies");
        }

        // GET: Movies
        [HttpGet]
        public ActionResult Movies()
        {

            //var movies = MoviesRepository.GetAllActiveMovies();
            //var model = Mapper.Map<List<MovieModel>>(movies);
            //return View(model);
            return View();
        }

        // GET: Movies
        [HttpGet]
        public JsonResult GetAllActiveMovies(GridMovieFilter filter)
        {
            var filterGet = Mapper.Map<GridMovieFilterDTO>(filter);
            var movies = MoviesRepository.GetAllActiveGridMovies(filterGet);
            var model = Mapper.Map<GridMovieResult>(movies);

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult AddMovie()
        {
            MovieAddModel movieAdd = new MovieAddModel();
            var genres = GenresRepository.GetAllActiveGenres();
            var directors = DirectorsRepository.GetAllActiveDirectors();
            movieAdd.SelectedGenreModel.GenreCollection = Mapper.Map<List<GenreLightModel>>(genres);
            movieAdd.SelectedDirectorModel.DirectorCollection = Mapper.Map<List<DirectorLightModel>>(directors);
            return View(movieAdd);
        }

        [HttpPost]
        public ActionResult AddMovie(MovieAddModel movieModel)
        {
            if (ModelState.IsValid)
            {
                if (movieModel.ImageFile != null)
                {
                    string FileName = Path.GetFileNameWithoutExtension(movieModel.ImageFile.FileName);
                    //Get File Extension  
                    string FileExtension = Path.GetExtension(movieModel.ImageFile.FileName);
                    //Get Upload path from Web.Config file AppSettings.  
                    string UploadPath = ConfigurationManager.AppSettings["MovieImagePath"].ToString();
                    if (movieModel.ImageFile.ContentLength > 0)
                    {
                        movieModel.ImagePath = UploadPath + FileName + FileExtension;
                        movieModel.ImageTitle = FileName + FileExtension;
                    }
                }
                var data = Mapper.Map<MovieDTO>(movieModel);
                MoviesRepository.Insert(data);
                return RedirectToAction("Movies");
            }
            return View(movieModel);
        }

        public ActionResult ImportMovies()
        {
            //Read the CSV file: Get Upload path from Web.Config file AppSettings.  
            string rootPath = ConfigurationManager.AppSettings["CsvPath"].ToString();
            string fileName = @"MoviesForImport.csv";
            string path = rootPath + fileName;
            FileInfo file = new FileInfo(Path.Combine(rootPath, fileName));
            //set the formatting options
            ExcelTextFormat format = new ExcelTextFormat();
            format.Delimiter = ';';
            format.Culture = new CultureInfo(Thread.CurrentThread.CurrentCulture.ToString());
            format.Culture.DateTimeFormat.ShortDatePattern = "dd-mm-yyyy";
            format.Encoding = new UTF8Encoding();
            //create a new Excel package
            using (ExcelPackage excelPackage = new ExcelPackage())
            {
                //create a WorkSheet
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("Movie");
                //using (var stream = file.OpenRead())
                //{
                //    excelPackage.Load(stream);
                //}
                //load the CSV data into cell A1
                worksheet.Cells["A1"].LoadFromText(file, format);
                Debug.WriteLine(worksheet.Cells["A1"].Value.ToString());

                //var worksheet = excelPackage.Workbook.Worksheets["Sheet1"];
                int totalRows = worksheet.Dimension.Rows;
                List<MovieImportModel> movieList = new List<MovieImportModel>();
                //i=1:is a file header
                for (int i = 2; i <= totalRows; i++)
                {
                    MovieImportModel mi = new MovieImportModel();
                    mi.Caption = worksheet.Cells[i, 1].Value.ToString();
                    mi.ReleaseYear = Int32.Parse(worksheet.Cells[i, 2].Value.ToString());
                    mi.SubmittedBy = worksheet.Cells[i, 3].Value.ToString();
                    mi.NumberOfAvailableCopies = Int32.Parse(worksheet.Cells[i, 4].Value.ToString());
                    mi.MovieGenres = new List<string>();
                    mi.MovieDirectors = new List<string>();
                    string[] gen = worksheet.Cells[i, 5].Value.ToString().Split(',');
                    foreach (var g in gen)
                    {
                        mi.MovieGenres.Add(g);
                    }
                    string[] dir = worksheet.Cells[i, 6].Value.ToString().Split(',');
                    foreach (var d in dir)
                    {
                        mi.MovieDirectors.Add(d);
                    }
                    movieList.Add(mi);
                }
                var data = Mapper.Map<List<MovieImportDTO>>(movieList);
                MoviesRepository.InsertRange(data);
                return RedirectToAction("Movies");
            }
        }

        //GET: MovieLibrary/MovieEdit/1
        [HttpGet]
        public ActionResult MovieEdit(int? id)
        {
            if (!id.HasValue)
            {
                return HttpNotFound();
            }
            var movie = MoviesRepository.GetActive(id.Value);
            if (movie == null)
            {
                return HttpNotFound();
            }
            var movieModel = Mapper.Map<MovieEditModel>(movie);
            var genres = GenresRepository.GetAllActiveGenres();
            var directors = DirectorsRepository.GetAllActiveDirectors();
            var directorIDs = directors.Select(d => d.DirectorID.ToString()).ToArray();
            var genreIDs = genres.Select(d => d.GenreID.ToString()).ToArray();
            movieModel.AllGenreModel.GenreCollection = Mapper.Map<List<GenreLightModel>>(genres);
            movieModel.AllDirectorModel.DirectorCollection = Mapper.Map<List<DirectorLightModel>>(directors);
            movieModel.AllDirectorModel.DirectorIDs = directorIDs;
            movieModel.AllGenreModel.GenreIDs = genreIDs;
            return View(movieModel);
        }

        //POST: MovieLibrary/MovieEdit/1
        [HttpPost]
        public ActionResult MovieEdit(MovieEditModel movieModel)
        {
            if (ModelState.IsValid)
            {
                if ((movieModel.ImageFile != null) && (movieModel.ImageFile.ContentLength > 0))
                {
                    string FileName = Path.GetFileNameWithoutExtension(movieModel.ImageFile.FileName);
                    //Get File Extension  
                    string FileExtension = Path.GetExtension(movieModel.ImageFile.FileName);
                    //Get Upload path from Web.Config file AppSettings.  
                    string UploadPath = ConfigurationManager.AppSettings["MovieImagePath"].ToString();
                    if (movieModel.ImageFile.ContentLength > 0)
                    {
                        movieModel.ImagePath = UploadPath + FileName + FileExtension;
                        movieModel.ImageTitle = FileName + FileExtension;
                    }
                }
                var data = Mapper.Map<MovieDTO>(movieModel);
                MoviesRepository.Update(data);
                return RedirectToAction("Movies");
            }
            return View(movieModel);
        }

        //POST: MovieLibrary/MovieEdit/1
        [HttpPost]
        public ActionResult MovieEditGrid(MovieModel movieModel)
        {
            if (ModelState.IsValid)
            {
                var data = Mapper.Map<MovieDTO>(movieModel);
                MoviesRepository.Update(data);
            }
            return RedirectToAction("Movies");
        }

        //GET: MovieLibrary/MovieDelete/1
        [HttpGet]
        public ActionResult MovieDelete(int? id)
        {
            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var result = MoviesRepository.Delete(id.Value);
            if (!result)
            {
                return HttpNotFound();
            }
            return RedirectToAction("Movies");
        }

        [HttpGet]
        public ActionResult MovieDeleteGrid(int? id)
        {
            if (!id.HasValue)
            {
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                return Json(new { response = false });
            }
            var result = MoviesRepository.Delete(id.Value);
            if (!result)
            {
                // return HttpNotFound();
                return Json(new { response = result });
            }       
            //return RedirectToAction("Movies");  
            return Json(new { response = result }, JsonRequestBehavior.AllowGet);            
        }

        [HttpGet]
        public ActionResult Users(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewBag.CurrentFilter = searchString;
            int pageSize = 5;
            int pageNumber = (page ?? 1) - 1;
            int totalCount;
            var activeUsers = UsersRepository.GetAllActiveUsers(sortOrder, searchString, pageNumber, pageSize, out totalCount);
            var model = Mapper.Map<List<UserDisplayModel>>(activeUsers);
            IPagedList<UserDisplayModel> userPaged = new StaticPagedList<UserDisplayModel>(model, pageNumber + 1, pageSize, totalCount);
            return View(userPaged);
        }

        [HttpGet]
        public ActionResult UserGet(int? id)
        {
            if (!id.HasValue)
            {
                return HttpNotFound();
            }
            var user = UsersRepository.Get(id.Value);
            if (user == null)
            {
                return HttpNotFound();
            }
            var userModel = Mapper.Map<UserDisplayModel>(user);
            return View(userModel);
        }

        [HttpGet]
        public ActionResult AddUser()
        {
            UserAddModel userAdd = new UserAddModel();
            var maritalStatuses = MaritalStatuseRepository.GetAllMaritalStatuses();
            userAdd.MaritalStatuses = Mapper.Map<List<MaritalStatuseModel>>(maritalStatuses);
            userAdd.InsertDate = DateTime.Now;
            return View(userAdd);
        }

        [HttpPost]
        public ActionResult AddUser(UserAddModel userModel)
        {
            if (ModelState.IsValid)
            {
                if (userModel.ImageFile != null)
                {
                    string FileName = Path.GetFileNameWithoutExtension(userModel.ImageFile.FileName);
                    //Get File Extension  
                    string FileExtension = Path.GetExtension(userModel.ImageFile.FileName);
                    //Get Upload path from Web.Config file AppSettings.  
                    string UploadPath = ConfigurationManager.AppSettings["UserImagePath"].ToString();
                    if (userModel.ImageFile.ContentLength > 0)
                    {
                        userModel.ImagePath = UploadPath + FileName + FileExtension;
                        userModel.ImageTitle = FileName + FileExtension;
                    }
                }
                var data = Mapper.Map<UserDTO>(userModel);
                UsersRepository.Insert(data);
                return RedirectToAction("Users");
            }
            //if (userModel.MaritalStatuses.Count == 0)
            //{
            //    var maritalStatuses = MaritalStatuseRepository.GetMaritalStatusById(userModel.MaritalStatusID);

            //    userModel.MaritalStatuses.Add(Mapper.Map<MaritalStatuseModel>(maritalStatuses));
            //}
            return View(userModel);
        }

        //GET: MovieLibrary/UserEdit/1
        [HttpGet]
        public ActionResult UserEdit(int? id)
        {
            if (!id.HasValue)
            {
                return HttpNotFound();
            }
            var user = UsersRepository.Get(id.Value);
            if (user == null)
            {
                return HttpNotFound();
            }
            var userModel = Mapper.Map<UserEditModel>(user);
            var maritalStatuses = MaritalStatuseRepository.GetAllMaritalStatuses();
            userModel.MaritalStatuses = Mapper.Map<List<MaritalStatuseModel>>(maritalStatuses);
            return View(userModel);
        }

        //POST: MovieLibrary/UserEdit/1
        [HttpPost]
        public ActionResult UserEdit(UserEditModel userModel)
        {
            if (ModelState.IsValid)
            {
                if ((userModel.ImageFile != null) && (userModel.ImageFile.ContentLength > 0))
                {
                    string FileName = Path.GetFileNameWithoutExtension(userModel.ImageFile.FileName);
                    //Get File Extension  
                    string FileExtension = Path.GetExtension(userModel.ImageFile.FileName);
                    //Get Upload path from Web.Config file AppSettings.  
                    string UploadPath = ConfigurationManager.AppSettings["UserImagePath"].ToString();
                    if (userModel.ImageFile.ContentLength > 0)
                    {
                        userModel.ImagePath = UploadPath + FileName + FileExtension;
                        userModel.ImageTitle = FileName + FileExtension;
                    }
                }
                var data = Mapper.Map<UserDTO>(userModel);
                UsersRepository.Update(data);
                return RedirectToAction("Users");
            }
            return View();
        }

        //GET: MovieLibrary/UserDelete/1
        [HttpGet]
        public ActionResult UserDelete(int? id)
        {
            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var result = UsersRepository.Delete(id.Value);
            if (!result)
            {
                return HttpNotFound();
            }
            return RedirectToAction("Users");
        }

        [HttpGet]
        public ActionResult UserRentAMovie(int? id, string sortOrder, string currentFilter, string searchString, int? page)
        {
            if (!id.HasValue)
            {
                return HttpNotFound();
            }
            var user = UsersRepository.Get(id.Value);
            if (user == null)
            {
                return HttpNotFound();
            }
            ViewBag.CurrentSort = sortOrder;
            ViewBag.YearSortParm = (String.IsNullOrEmpty(sortOrder) || sortOrder.Equals("year")) ? "year_desc" : "year";
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewBag.CurrentFilter = searchString;
            int pageSize = 5;
            int pageNumber = (page ?? 1) - 1;
           // int totalCount;
            var movies = MoviesRepository.GetAllActiveAvailableMovies(sortOrder, searchString, pageNumber, pageSize, out int totalCount);
            var modelMovie = Mapper.Map<List<MovieModel>>(movies);
            var userModel = Mapper.Map<UserDisplayModel>(user);
            var wishMoviesListIds = MoviesRepository.WishListMovieIds(user.UserID);
            IPagedList<MovieModel> userPaged = new StaticPagedList<MovieModel>(modelMovie, pageNumber + 1, pageSize, totalCount);
            UserRentAMovieModel userRentAMovieModel = new UserRentAMovieModel()
            {
                MovieModels = userPaged,
                UserDisplayModel = userModel,
                WishMoviesListIds = wishMoviesListIds
            };
            return View(userRentAMovieModel);
        }

        [HttpGet]
        public ActionResult Rent(int? movieId, int? userId)
        {
            if (!movieId.HasValue || !userId.HasValue)
            {
                return HttpNotFound();
            }
            var user = UsersRepository.Get(userId.Value);
            if (user == null)
            {
                return HttpNotFound();
            }
            var movie = MoviesRepository.Get(movieId.Value);
            if (movie == null)
            {
                return HttpNotFound();
            }
            RentedMovieModel rentMovie = new RentedMovieModel()
            {
                UserID = userId.Value,
                MovieID = movieId.Value,
                IssuedDate = DateTime.Now,
                Movie = Mapper.Map<MovieModel>(movie),
                User = Mapper.Map<UserEditModel>(user)
            };
            return View(rentMovie);
        }

        [HttpPost]
        public ActionResult Rent(RentedMovieModel model)
        {
            if (ModelState.IsValid)
            {
                var data = Mapper.Map<RentedMovieDTO>(model);
                var response = MoviesRepository.RentMovieAdd(data);
                return RedirectToAction("RentedMovies", new { id = model.UserID });
            }
            return View();
        }

        [HttpGet]
        public ActionResult AddToWishList(int? movieId, int? userId)
        {
            if (!movieId.HasValue || !userId.HasValue)
            {
                return HttpNotFound();
            }
            var user = UsersRepository.Get(userId.Value);
            if (user == null)
            {
                return HttpNotFound();
            }
            var movie = MoviesRepository.Get(movieId.Value);
            if (movie == null)
            {
                return HttpNotFound();
            }
            MoviesRepository.AddToWishList(userId.Value, movieId.Value);
            return RedirectToAction("UserRentAMovie", new { id = userId.Value });
        }

        [HttpPost]
        public ActionResult RemoveFromWishList(int? movieId, int? userId)
        {
            if (!movieId.HasValue || !userId.HasValue)
            {
                return HttpNotFound();
            }
            var user = UsersRepository.Get(userId.Value);
            if (user == null)
            {
                return HttpNotFound();
            }
            var movie = MoviesRepository.Get(movieId.Value);
            if (movie == null)
            {
                return HttpNotFound();
            }
            return View();
        }

        //view wish list
        [HttpGet]
        public ActionResult WishList(int? id)
        {
            if (!id.HasValue)
            {
                return HttpNotFound();
            }
            var user = UsersRepository.Get(id.Value);
            if (user == null)
            {
                return HttpNotFound();
            }
            var userModel = Mapper.Map<UserDisplayModel>(user);
            var wishMovieList = MoviesRepository.WishList(id.Value);
            var wishMovieModel = Mapper.Map<List<MovieModel>>(wishMovieList);
            return View(Tuple.Create(userModel, wishMovieModel));
        }

        [HttpGet]
        public ActionResult RentedMovies(int? id)
        {
            if (!id.HasValue)
            {
                return HttpNotFound();
            }
            var user = UsersRepository.Get(id.Value);
            if (user == null)
            {
                return HttpNotFound();
            }
            var userModel = Mapper.Map<UserDisplayModel>(user);
            var rented = MoviesRepository.RentedMovies(id.Value);
            var rentedModel = Mapper.Map<List<RentedMovieModel>>(rented);
            return View(Tuple.Create(userModel, rentedModel));
        }

        [HttpGet]
        public ActionResult CloseRentedMovie(int? id, int userId)
        {
            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var result = MoviesRepository.CloseRentedMovie(id.Value);
            if (!result)
            {
                return HttpNotFound();
            }
            //return RedirectToAction("Users");           
            return RedirectToAction("RentedMovies", new { id = userId });
        }

        [HttpGet]
        public ActionResult WatchedMovies(int? id)
        {
            if (!id.HasValue)
            {
                return HttpNotFound();
            }
            var user = UsersRepository.Get(id.Value);
            if (user == null)
            {
                return HttpNotFound();
            }
            var userModel = Mapper.Map<UserDisplayModel>(user);
            var rented = MoviesRepository.WatchedMovies(id.Value);
            var watchedModel = Mapper.Map<List<RentedMovieModel>>(rented);
            return View(Tuple.Create(userModel, watchedModel));
        }

        // GET: Directors
        [HttpGet]
        public ActionResult Directors()
        {
            var directors = DirectorsRepository.GetAllActiveDirectors();
            var model = Mapper.Map<List<DirectorModel>>(directors);
            return View(model);
        }

        [HttpGet]
        public ActionResult AddDirector()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddDirector(DirectorAddModel directorModel)
        {
            if (ModelState.IsValid)
            {
                var data = Mapper.Map<DirectorDTO>(directorModel);
                DirectorsRepository.Insert(data);
                return RedirectToAction("Directors");
            }
            return View(directorModel);
        }

        //GET: MovieLibrary/DirectorEdit/1
        [HttpGet]
        public ActionResult DirectorEdit(int? id)
        {
            if (!id.HasValue)
            {
                return HttpNotFound();
            }
            var director = DirectorsRepository.Get(id.Value);
            if (director == null)
            {
                return HttpNotFound();
            }
            var directorModel = Mapper.Map<DirectorAddModel>(director);
            return View(directorModel);

        }

        //POST: MovieLibrary/DirectorEdit/1
        [HttpPost]
        public ActionResult DirectorEdit(DirectorAddModel directorModel)
        {
            if (ModelState.IsValid)
            {
                var data = Mapper.Map<DirectorDTO>(directorModel);
                DirectorsRepository.Update(data);
                return RedirectToAction("Directors");
            }
            return View(directorModel);
        }

        //GET: MovieLibrary/DirectorDelete/1
        [HttpGet]
        public ActionResult DirectorDelete(int? id)
        {
            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var result = DirectorsRepository.Delete(id.Value);
            if (!result)
            {
                return HttpNotFound();
            }
            return RedirectToAction("Directors");
        }

        // GET: Genres
        [HttpGet]
        public ActionResult Genres()
        {
            var genres = GenresRepository.GetAllActiveGenres();
            var model = Mapper.Map<List<GenreModel>>(genres);
            return View(model);
        }

        [HttpGet]
        public ActionResult AddGenre()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddGenre(GenreAddModel genreModel)
        {
            if (ModelState.IsValid)
            {
                var data = Mapper.Map<GenreDTO>(genreModel);
                GenresRepository.Insert(data);
                return RedirectToAction("Genres");
            }
            return View(genreModel);
        }

        //GET: MovieLibrary/GenreEdit/1
        [HttpGet]
        public ActionResult GenreEdit(int? id)
        {
            if (!id.HasValue)
            {
                return HttpNotFound();
            }
            var genre = GenresRepository.Get(id.Value);
            if (genre == null)
            {
                return HttpNotFound();
            }
            var genreModel = Mapper.Map<GenreAddModel>(genre);
            return View(genreModel);
        }

        //POST: MovieLibrary/GenreEdit/1
        [HttpPost]
        public ActionResult GenreEdit(GenreAddModel genreModel)
        {
            if (ModelState.IsValid)
            {
                var data = Mapper.Map<GenreDTO>(genreModel);
                GenresRepository.Update(data);
                return RedirectToAction("Genres");
            }
            return View(genreModel);
        }

        //GET: MovieLibrary/GenreDelete/1
        [HttpGet]
        public ActionResult GenreDelete(int? id)
        {
            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var result = GenresRepository.Delete(id.Value);
            if (!result)
            {
                return HttpNotFound();
            }
            return RedirectToAction("Genres");
        }

    }
}
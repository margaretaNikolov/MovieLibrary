using MovieLibrary.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using MovieLibrary.Business.DTO;
using AutoMapper;
using System.Data.Entity;
using System.IO;
using System.Drawing;

namespace MovieLibrary.Business
{
    public static class MoviesRepository
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //private static readonly log4net.ILog log = LogHelper.GetLogger();
        public static void Insert(string caption, string subbmittedBy, DateTime insertDate)
        {
            var context = new MovieLibraryContext();

            try
            {
                var test = log4net.LogManager.GetRepository().Configured;
                var movie = new MovieDTO()
                {
                    Caption = caption,
                    SubmittedBy = subbmittedBy,
                    InsertDate = insertDate
                };
                var dbMovie = Mapper.Map<Movy>(movie);
                context.Movies.Add(dbMovie);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                log.Error("Movies Repository Insert method Error: ", ex);
            }
            finally
            {
                if (context != null)
                    ((IDisposable)context).Dispose();
            }
        }

        public static List<MovieDTO> GetAllMovies()
        {
            var context = new MovieLibraryContext();
            try
            {
                var test = log4net.LogManager.GetRepository().Configured;
                List<Movy> dbMovies = context.Movies.OrderByDescending(m => m.ReleaseYear).ToList<Movy>();
                return Mapper.Map<List<MovieDTO>>(dbMovies);
            }
            catch (Exception ex)
            {
                log.Error("Movies Repository GetAllMovies method Error: ", ex);
                return null;
            }
            finally
            {
                if (context != null)
                    ((IDisposable)context).Dispose();
            }
        }

        public static List<MovieDTO> GetAllActiveMovies()
        {
            var context = new MovieLibraryContext();
            try
            {
                List<Movy> dbMovies = context.Movies
                    .Include(m => m.MovieDirectors.Select(d => d.Director))
                    .Include(m => m.MovieGenres.Select(g => g.Genre))
                    .Where(m => m.DeleteDate == null)
                    .OrderByDescending(m => m.ReleaseYear).ToList<Movy>();
                return Mapper.Map<List<MovieDTO>>(dbMovies);
            }
            catch (Exception ex)
            {
                log.Error("Movies Repository GetAllActiveMovies method Error: ", ex);
                return null;
            }
            finally
            {
                if (context != null)
                    ((IDisposable)context).Dispose();
            }
        }

        public static GridMovieResultDTO GetAllActiveGridMovies(GridMovieFilterDTO filter)
        {
            var context = new MovieLibraryContext();
            try
            {
                List<Movy> dbMovies = null;
                if (filter.sortOrder == null)
                {
                    dbMovies = context.Movies
                        .Include(m => m.MovieDirectors.Select(d => d.Director))
                        .Include(m => m.MovieGenres.Select(g => g.Genre))
                        .Where(m => m.DeleteDate == null)
                        .OrderByDescending(m => m.ReleaseYear)
                        .Skip(filter.Skip)
                        .Take(filter.pageSize)
                        .ToList<Movy>();
                }
                if (filter.sortOrder == "asc")
                {
                    switch (filter.sortField)
                    {
                        case "Caption":
                            dbMovies = context.Movies
                                .Include(m => m.MovieDirectors.Select(d => d.Director))
                                .Include(m => m.MovieGenres.Select(g => g.Genre))
                                .Where(m => m.DeleteDate == null).OrderBy(m => m.Caption).Skip(filter.Skip).Take(filter.pageSize).ToList<Movy>();
                            break;
                        case "Release Year":
                            dbMovies = context.Movies
                               .Include(m => m.MovieDirectors.Select(d => d.Director))
                               .Include(m => m.MovieGenres.Select(g => g.Genre))
                               .Where(m => m.DeleteDate == null).OrderBy(m => m.ReleaseYear).Skip(filter.Skip).Take(filter.pageSize).ToList<Movy>();
                            break;
                        case "SubmittedBy":
                            dbMovies = context.Movies
                               .Include(m => m.MovieDirectors.Select(d => d.Director))
                               .Include(m => m.MovieGenres.Select(g => g.Genre))
                               .Where(m => m.DeleteDate == null).OrderBy(m => m.SubmittedBy).Skip(filter.Skip).Take(filter.pageSize).ToList<Movy>();
                            break;
                        case "NumberOfAvailableCopies":
                            dbMovies = context.Movies
                               .Include(m => m.MovieDirectors.Select(d => d.Director))
                               .Include(m => m.MovieGenres.Select(g => g.Genre))
                               .Where(m => m.DeleteDate == null).OrderBy(m => m.NumberOfAvailableCopies).Skip(filter.Skip).Take(filter.pageSize).ToList<Movy>();
                            break;
                        case "Director":
                            dbMovies = context.Movies
                               .Include(m => m.MovieDirectors.Select(d => d.Director).OrderBy(d => d.FirstName))
                               .Include(m => m.MovieGenres.Select(g => g.Genre))
                               .Where(m => m.DeleteDate == null).Skip(filter.Skip).Take(filter.pageSize).ToList<Movy>();
                            break;
                        case "Genre":
                            dbMovies = context.Movies
                               .Include(m => m.MovieDirectors.Select(d => d.Director))
                               .Include(m => m.MovieGenres.Select(g => g.Genre).OrderBy(d => d.Name))
                               .Where(m => m.DeleteDate == null).Skip(filter.Skip).Take(filter.pageSize).ToList<Movy>();
                            break;
                        default:
                            dbMovies = context.Movies
                                .Include(m => m.MovieDirectors.Select(d => d.Director))
                                .Include(m => m.MovieGenres.Select(g => g.Genre))
                                .Where(m => m.DeleteDate == null).OrderBy(m => m.ReleaseYear).Skip(filter.Skip).Take(filter.pageSize).ToList<Movy>();
                            break;
                    }
                }
                if (filter.sortOrder == "desc")
                {
                    switch (filter.sortField)
                    {
                        case "Caption":
                            dbMovies = context.Movies
                                .Include(m => m.MovieDirectors.Select(d => d.Director))
                                .Include(m => m.MovieGenres.Select(g => g.Genre))
                                .Where(m => m.DeleteDate == null).OrderByDescending(m => m.Caption).Skip(filter.Skip).Take(filter.pageSize).ToList<Movy>();
                            break;
                        case "Release Year":
                            dbMovies = context.Movies
                               .Include(m => m.MovieDirectors.Select(d => d.Director))
                               .Include(m => m.MovieGenres.Select(g => g.Genre))
                               .Where(m => m.DeleteDate == null).OrderByDescending(m => m.ReleaseYear).Skip(filter.Skip).Take(filter.pageSize).ToList<Movy>();
                            break;
                        case "SubmittedBy":
                            dbMovies = context.Movies
                               .Include(m => m.MovieDirectors.Select(d => d.Director))
                               .Include(m => m.MovieGenres.Select(g => g.Genre))
                               .Where(m => m.DeleteDate == null).OrderByDescending(m => m.SubmittedBy).Skip(filter.Skip).Take(filter.pageSize).ToList<Movy>();
                            break;
                        case "NumberOfAvailableCopies":
                            dbMovies = context.Movies
                               .Include(m => m.MovieDirectors.Select(d => d.Director))
                               .Include(m => m.MovieGenres.Select(g => g.Genre))
                               .Where(m => m.DeleteDate == null).OrderByDescending(m => m.NumberOfAvailableCopies).Skip(filter.Skip).Take(filter.pageSize).ToList<Movy>();
                            break;
                        case "Director":
                            dbMovies = context.Movies
                               .Include(m => m.MovieDirectors.Select(d => d.Director).OrderByDescending(d => d.FirstName))
                               .Include(m => m.MovieGenres.Select(g => g.Genre))
                               .Where(m => m.DeleteDate == null).Skip(filter.Skip).Take(filter.pageSize).ToList<Movy>();
                            break;
                        case "Genre":
                            dbMovies = context.Movies
                               .Include(m => m.MovieDirectors.Select(d => d.Director))
                               .Include(m => m.MovieGenres.Select(g => g.Genre).OrderByDescending(d => d.Name))
                               .Where(m => m.DeleteDate == null).Skip(filter.Skip).Take(filter.pageSize).ToList<Movy>();
                            break;
                        default:
                            dbMovies = context.Movies
                                .Include(m => m.MovieDirectors.Select(d => d.Director))
                                .Include(m => m.MovieGenres.Select(g => g.Genre))
                                .Where(m => m.DeleteDate == null).OrderByDescending(m => m.ReleaseYear).Skip(filter.Skip).Take(filter.pageSize).ToList<Movy>();
                            break;
                    }
                }//end of sorting
                //filtering
                dbMovies = context.Movies
                        .Include(m => m.MovieDirectors.Select(d => d.Director))
                        .Include(m => m.MovieGenres.Select(g => g.Genre))
                        .Where(m => m.DeleteDate == null)
                        .Where(m =>
                    ((String.IsNullOrEmpty(filter.Caption.ToString())) ? true : m.Caption.ToUpper().Contains(filter.Caption.ToString().ToUpper())) &&
                    ((!filter.ReleaseYear.HasValue) ? true : m.ReleaseYear == filter.ReleaseYear) &&
                    ((String.IsNullOrEmpty(filter.SubmittedBy.ToString())) ? true : m.SubmittedBy.ToString().ToUpper() == filter.SubmittedBy.ToString().ToUpper()) &&
                    ((!filter.NumberOfAvailableCopies.HasValue) ? true : m.NumberOfAvailableCopies == filter.NumberOfAvailableCopies)
                    )
                    .Where(m =>
                    ((String.IsNullOrEmpty(filter.Director.ToString())) ? true : m.MovieDirectors.Any(d => d.Director.FirstName.ToUpper().Contains(filter.Director.ToString().ToUpper()) || d.Director.LastName.ToUpper().Contains(filter.Director.ToString().ToUpper()))))
                    .Where(m =>
                    ((String.IsNullOrEmpty(filter.Genre.ToString())) ? true : m.MovieGenres.Any(d => d.Genre.Name.ToUpper().Contains(filter.Genre.ToString().ToUpper())))
                    ).
                    ToList<Movy>();

                var data = Mapper.Map<List<MovieDTO>>(dbMovies);
                var itemsCount = context.Movies.Where(m => m.DeleteDate == null).Count();
                return new GridMovieResultDTO { data = data, itemsCount = itemsCount };
            }
            catch (Exception ex)
            {
                log.Error("Movies Repository GetAllActiveGridMovies method Error: ", ex);
                return null;
            }
            finally
            {
                if (context != null)
                    ((IDisposable)context).Dispose();
            }
        }

        public static List<MovieDTO> GetAllActiveAvailableMovies()
        {
            var context = new MovieLibraryContext();
            try
            {
                List<Movy> dbMovies = context.Movies
                    .Where(m => m.DeleteDate == null && m.NumberOfAvailableCopies > 0)
                    .OrderByDescending(m => m.ReleaseYear).ToList<Movy>();
                return Mapper.Map<List<MovieDTO>>(dbMovies);
            }
            catch (Exception ex)
            {
                log.Error("Movies Repository GetAllActiveAvailableMovies method Error: ", ex);
                return null;
            }
            finally
            {
                if (context != null)
                    ((IDisposable)context).Dispose();
            }
        }

        public static List<MovieDTO> GetAllActiveAvailableMovies(string sortOrder, string searchString, int page, int pageSize, out int totalCount)
        {
            var context = new MovieLibraryContext();
            try
            {
                var dbMovies = context.Movies.Where(m => m.DeleteDate == null && m.NumberOfAvailableCopies > 0);
                if (!String.IsNullOrEmpty(searchString))
                {
                    dbMovies = dbMovies.Where(m => m.Caption.Contains(searchString));
                }
                switch (sortOrder)
                {
                    case "year_desc":
                        dbMovies = dbMovies.OrderByDescending(s => s.ReleaseYear);
                        break;
                    case "year":
                        dbMovies = dbMovies.OrderBy(s => s.ReleaseYear);
                        break;
                    default:  // Name ascending 
                        dbMovies = dbMovies.OrderBy(s => s.Caption);
                        break;
                }
                totalCount = dbMovies.Count();
                var movies = dbMovies.Skip(pageSize * page).Take(pageSize).ToList();
                return Mapper.Map<List<MovieDTO>>(movies);
            }
            catch (Exception ex)
            {
                log.Error("Movies Repository GetAllActiveAvailableMovies method Error: ", ex);
                totalCount = 0;
                return null;
            }
            finally
            {
                if (context != null)
                    ((IDisposable)context).Dispose();
            }
        }

        public static MovieDTO Get(int movieId)
        {
            var context = new MovieLibraryContext();
            try
            {
                var dbMovie = context.Movies.Where(m => m.MovieID == movieId).FirstOrDefault();
                return Mapper.Map<MovieDTO>(dbMovie);
            }
            catch (Exception ex)
            {
                log.Error("Movies Repository Get method Error: ", ex);
                return null;
            }
            finally
            {
                if (context != null)
                    ((IDisposable)context).Dispose();
            }
        }

        public static MovieDTO GetActive(int movieId)
        {
            var context = new MovieLibraryContext();
            try
            {
                var dbMovie = context.Movies
                    .Include(m => m.MovieDirectors)
                    .Include(m => m.MovieGenres)
                    .Where(m => m.MovieID == movieId && m.DeleteDate == null).FirstOrDefault();

                var result = Mapper.Map<MovieDTO>(dbMovie);
                result.SelectedDirectorIDs = dbMovie.MovieDirectors.Select(m => m.DirectorID.ToString()).ToArray();
                result.SelectedGenreIDs = dbMovie.MovieGenres.Select(m => m.GenreID.ToString()).ToArray();
                return result;
            }
            catch (Exception ex)
            {
                log.Error("Movies Repository GetActive method Error: ", ex);
                return null;
            }
            finally
            {
                if (context != null)
                    ((IDisposable)context).Dispose();
            }
        }

        public static MovieDTO Insert(MovieDTO movie)
        {
            using (var context = new MovieLibraryContext())
            {
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {

                    try
                    {
                        if (!String.IsNullOrEmpty(movie.ImagePath))
                        {
                            FileStream fs = new FileStream(movie.ImagePath, FileMode.Open, FileAccess.Read);
                            int length = (int)fs.Length;
                            movie.Image = new byte[length];
                            fs.Read(movie.Image, 0, length);
                            fs.Close();
                        }
                        var dbMovie = Mapper.Map<Movy>(movie);
                        var data = context.Movies.Add(dbMovie);
                        context.SaveChanges();

                        //insert movie directors
                        foreach (var item in movie.SelectedDirectorIDs)
                        {
                            MovieDirectorDTO movieDirectorDTO = new MovieDirectorDTO()
                            {
                                MovieID = dbMovie.MovieID,
                                DirectorID = int.Parse(item)
                            };
                            var dbMovieDirector = Mapper.Map<MovieDirector>(movieDirectorDTO);
                            context.MovieDirectors.Add(dbMovieDirector);
                            context.SaveChanges();
                        }

                        //insert movie genres
                        foreach (var item in movie.SelectedGenreIDs)
                        {
                            MovieGenreDTO movieGenreDTO = new MovieGenreDTO()
                            {
                                MovieID = dbMovie.MovieID,
                                GenreID = int.Parse(item)
                            };
                            var dbMovieGenre = Mapper.Map<MovieGenre>(movieGenreDTO);
                            context.MovieGenres.Add(dbMovieGenre);
                            context.SaveChanges();
                        }
                        transaction.Commit();
                        return Mapper.Map<MovieDTO>(data);
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        log.Error("Movies Repository Insert method Error: ", ex);
                        return null;
                    }
                }
            }
        }

        public static void InsertRange(List<MovieImportDTO> movie)
        {
            using (var context = new MovieLibraryContext())
            {
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {

                    try
                    {
                        var dbMovies = Mapper.Map<List<Movy>>(movie);
                        var data = context.Movies.AddRange(dbMovies);
                        context.SaveChanges();
                        List<int> movieIDs = dbMovies.Select(m => m.MovieID).ToList();
                        List<MovieDirector> md = new List<MovieDirector>();
                        List<MovieGenre> mg = new List<MovieGenre>();
                        int i = 0;
                        //insert movie directors
                        foreach (var list in movie)
                        {
                            var movieID = movieIDs[i];
                            foreach (var item in list.MovieDirectors)
                            {
                                string trim = item.Replace(" ", "");
                                int directorId = context.Directors
                                    .Where(m => String.Equals((m.FirstName.ToUpper() + m.LastName.ToUpper()).Replace(" ",""), trim.ToUpper()) || String.Equals((m.LastName.ToUpper() + m.FirstName.ToUpper()).Replace(" ", ""), trim.ToUpper()))
                                    .Select(m => m.DirectorID).FirstOrDefault();
                                if (directorId == 0) 
                                {
                                    transaction.Rollback(); 
                                    return; 
                                }
                                MovieDirector movieDirector = new MovieDirector()
                                {
                                    MovieID = movieID,
                                    DirectorID = directorId
                                };
                                md.Add(movieDirector);                                
                            }
                            context.MovieDirectors.AddRange(md);
                            //insert movie genres
                            foreach (var item in list.MovieGenres)
                            {
                                int genreId = context.Genres
                                  .Where(m => String.Equals(m.Name.ToUpper(), item.ToUpper()))
                                  .Select(m => m.GenreID).FirstOrDefault();
                                if (genreId == 0) 
                                { 
                                    transaction.Rollback(); 
                                    return; 
                                }
                                MovieGenre movieGenre = new MovieGenre()
                                {
                                    MovieID = movieID,
                                    GenreID = genreId
                                };
                                mg.Add(movieGenre);                                
                            }
                            context.MovieGenres.AddRange(mg);
                            i++;
                        }
                        context.SaveChanges();
                        transaction.Commit();                        
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        log.Error("Movies Repository InsertRange method Error: ", ex);

                    }
                }
            }
        }

        public static void Update(MovieDTO movie)
        {
            using (var context = new MovieLibraryContext())
            {
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {

                    try
                    {
                        var dbMovie = context.Movies
                            .Include(m => m.MovieDirectors)
                            .Include(m => m.MovieGenres)
                            .SingleOrDefault(m => m.MovieID.Equals(movie.MovieID));
                        var dbMovieDirectorIDs = dbMovie.MovieDirectors.Select(m => m.DirectorID).ToList();
                        var dbMovieGenreIDs = dbMovie.MovieGenres.Select(m => m.GenreID).ToList();

                        if (dbMovie != null)
                        {
                            //update movie directors                          
                            foreach (var item in movie.SelectedDirectorIDs)
                            {
                                if (!dbMovieDirectorIDs.Contains(int.Parse(item)))
                                {
                                    MovieDirectorDTO movieDirectorDTO = new MovieDirectorDTO()
                                    {
                                        MovieID = movie.MovieID,
                                        DirectorID = int.Parse(item)
                                    };
                                    var dbMovieDirector = Mapper.Map<MovieDirector>(movieDirectorDTO);
                                    context.MovieDirectors.Add(dbMovieDirector);
                                }
                            }
                            //remove from moviedirectors all old records
                            var list = LeftJoinList(dbMovieDirectorIDs, movie.SelectedDirectorIDs);
                            if (list.Count > 0)
                            {
                                foreach (var item in list)
                                {
                                    var dbMovieDirector = dbMovie.MovieDirectors.SingleOrDefault(m => m.DirectorID.Equals(item));
                                    if (dbMovieDirector != null)
                                    {
                                        context.MovieDirectors.Remove(dbMovieDirector);
                                    }
                                }
                            }
                            //update movie genres
                            foreach (var item in movie.SelectedGenreIDs)
                            {
                                if (!dbMovieGenreIDs.Contains(int.Parse(item)))
                                {
                                    MovieGenreDTO movieGenreDTO = new MovieGenreDTO()
                                    {
                                        MovieID = movie.MovieID,
                                        GenreID = int.Parse(item)
                                    };
                                    var dbMovieGenre = Mapper.Map<MovieGenre>(movieGenreDTO);
                                    context.MovieGenres.Add(dbMovieGenre);
                                }
                            }
                            //remove from moviegenres all old records
                            var listg = LeftJoinList(dbMovieGenreIDs, movie.SelectedGenreIDs);
                            if (listg.Count > 0)
                            {
                                foreach (var item in listg)
                                {
                                    var dbMovieGenre = dbMovie.MovieGenres.SingleOrDefault(m => m.GenreID.Equals(item));
                                    if (dbMovieGenre != null)
                                    {
                                        context.MovieGenres.Remove(dbMovieGenre);
                                    }
                                }
                            }
                            if (!String.IsNullOrEmpty(movie.ImageTitle))
                            {
                                if (dbMovie.ImageTitle != movie.ImageTitle)
                                {
                                    FileStream fs = new FileStream(movie.ImagePath, FileMode.Open, FileAccess.Read);
                                    int length = (int)fs.Length;
                                    movie.Image = new byte[length];
                                    fs.Read(movie.Image, 0, length);
                                    fs.Close();
                                    var movieEntry = context.Entry(dbMovie);
                                    movieEntry.CurrentValues.SetValues(movie);
                                }
                                else
                                {
                                    // Update scalar properties 
                                    var movieEntry = context.Entry(dbMovie);
                                    movieEntry.CurrentValues.SetValues(movie);
                                    movieEntry.Property(m => m.Image).IsModified = false;
                                    movieEntry.Property(m => m.ImageTitle).IsModified = false;
                                }
                            }
                            else
                            {
                                // Update scalar properties 
                                var movieEntry = context.Entry(dbMovie);
                                movieEntry.CurrentValues.SetValues(movie);
                                movieEntry.Property(m => m.Image).IsModified = false;
                                movieEntry.Property(m => m.ImageTitle).IsModified = false;
                            }
                        }
                        context.SaveChanges();
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        log.Error("Movies Repository Update method Error: ", ex);
                    }
                }
            }
        }

        public static List<int> LeftJoinList(List<int> l1, string[] l2)
        {
            List<int> response = new List<int>();
            foreach (var item in l1)
            {
                if (!l2.Contains(item.ToString()))
                {
                    response.Add(item);
                }
            }
            return response;
        }

        public static bool Delete(int movieId)
        {
            var context = new MovieLibraryContext();
            try
            {
                Movy movie = context.Movies.Find(movieId);
                if (movie != null)
                {
                    //context.Movies.Remove(movie);
                    movie.DeleteDate = DateTime.Now;
                    context.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                log.Error("Movies Repository Delete method Error: ", ex);
                return false;
            }
            finally
            {
                if (context != null)
                    ((IDisposable)context).Dispose();
            }
        }

        public static RentedMovieDTO RentMovieAdd(RentedMovieDTO movie)
        {
            using (var context = new MovieLibraryContext())
            {
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        Movy movieUpdate = context.Movies.Find(movie.MovieID);
                        if (movieUpdate.NumberOfAvailableCopies > 0)
                        {
                            var dbMovie = Mapper.Map<RentedMovy>(movie);
                            var data = context.RentedMovies.Add(dbMovie);
                            context.SaveChanges();
                            //update movie with available copies

                            movieUpdate.NumberOfAvailableCopies--;
                            context.SaveChanges();
                            transaction.Commit();
                            return Mapper.Map<RentedMovieDTO>(data);
                        }
                        else
                        {
                            return new RentedMovieDTO();
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        log.Error("Movies Repository RentMovieAdd method Error: ", ex);
                        return null;
                    }
                }
            }
        }

        public static List<RentedMovieDTO> RentedMovies(int userId)
        {
            using (var context = new MovieLibraryContext())
            {
                try
                {
                    var rentedMovies = context.RentedMovies
                        .Include(m => m.Movy)
                        .Include(m => m.User)
                        .Where(m => m.UserID == userId)
                        .Where(m => m.ReturnedDate == null)
                        .OrderByDescending(m => m.IssuedDate).ToList();
                    return Mapper.Map<List<RentedMovieDTO>>(rentedMovies);
                }
                catch (Exception ex)
                {
                    log.Error("Movies Repository RentedMovies method Error: ", ex);
                    return null;
                }
            }
        }

        public static List<RentedMovieDTO> WatchedMovies(int userId)
        {
            using (var context = new MovieLibraryContext())
            {
                try
                {
                    var rentedMovies = context.RentedMovies
                        .Include(m => m.Movy)
                        .Include(m => m.User)
                        .Where(m => m.UserID == userId)
                        .Where(m => m.ReturnedDate != null)
                        .OrderByDescending(m => m.IssuedDate).ToList();
                    return Mapper.Map<List<RentedMovieDTO>>(rentedMovies);
                }
                catch (Exception ex)
                {
                    log.Error("Movies Repository WatchedMovies method Error: ", ex);
                    return null;
                }
            }
        }

        public static bool CloseRentedMovie(int rentedMovieId)
        {
            using (var context = new MovieLibraryContext())
            {
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var rentedMovie = context.RentedMovies.Find(rentedMovieId);
                        if (rentedMovie != null)
                        {
                            //context.Movies.Remove(movie);
                            rentedMovie.ReturnedDate = DateTime.Now;
                            context.SaveChanges();
                            //update movie with available copies
                            Movy movieUpdate = context.Movies.Find(rentedMovie.MovieID);
                            movieUpdate.NumberOfAvailableCopies++;
                            context.SaveChanges();
                            transaction.Commit();
                            return true;
                        }
                        return false;
                    }
                    catch (Exception ex)
                    {
                        log.Error("Movies Repository CloseRentedMovie method Error: ", ex);
                        return false;
                    }
                }
            }
        }

        public static void AddToWishList(int userId, int movieId)
        {
            var context = new MovieLibraryContext();
            try
            {
                Movy movie = context.Movies.Find(movieId);
                if (movie == null)
                {
                    return;
                }
                User user = context.Users.Find(userId);
                if (user == null)
                {
                    return;
                }
                var wwlCount = context.WWLists.Where(m => m.UserID == userId && m.MovieID == movieId).ToList().Count;
                if (wwlCount == 0)
                {
                    WWListDTO wwl = new WWListDTO()
                    {
                        UserID = userId,
                        MovieID = movieId,
                        InsertDate = DateTime.Now
                    };
                    var dbWWL = Mapper.Map<WWList>(wwl);
                    context.WWLists.Add(dbWWL);
                    context.SaveChanges();
                }
                //else movie is already in wish list
            }
            catch (Exception ex)
            {
                log.Error("Movies Repository AddToWishList method Error: ", ex);
                return;
            }
            finally
            {
                if (context != null)
                    ((IDisposable)context).Dispose();
            }
        }

        public static void RemoveFromWishList(int userId, int movieId)
        {
            var context = new MovieLibraryContext();
            try
            {
                Movy movie = context.Movies.Find(movieId);
                if (movie == null)
                {
                    return;
                }
                User user = context.Users.Find(userId);
                if (user == null)
                {
                    return;
                }
                var wwl = context.WWLists.SingleOrDefault(m => m.UserID == userId && m.MovieID == movieId);
                if (wwl != null)
                {
                    wwl.DeleteDate = DateTime.Now;
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                log.Error("Movies Repository RemoveFromWishList method Error: ", ex);
                return;
            }
            finally
            {
                if (context != null)
                    ((IDisposable)context).Dispose();
            }
        }

        public static List<MovieDTO> WishList(int userId)
        {
            var context = new MovieLibraryContext();
            try
            {
                User user = context.Users.Find(userId);
                if (user == null)
                {
                    return null;
                }
                var mids = context.WWLists.Where(w => w.UserID == userId).Select(w => w.MovieID).ToList();
                var dataMovies = context.Movies.Where(m => mids.Contains(m.MovieID)).ToList();
                return Mapper.Map<List<MovieDTO>>(dataMovies);
            }
            catch (Exception ex)
            {
                log.Error("Movies Repository WishList method Error: ", ex);
                return null;
            }
            finally
            {
                if (context != null)
                    ((IDisposable)context).Dispose();
            }
        }

        public static List<string> WishListMovieIds(int userId)
        {
            var context = new MovieLibraryContext();
            try
            {
                User user = context.Users.Find(userId);
                if (user == null)
                {
                    return null;
                }
                var mids = context.WWLists.Where(w => w.UserID == userId).Select(w => w.MovieID.ToString()).ToList();
                return mids;
            }
            catch (Exception ex)
            {
                log.Error("Movies Repository WishListMovieIds method Error: ", ex);
                return null;
            }
            finally
            {
                if (context != null)
                    ((IDisposable)context).Dispose();
            }
        }        

    }
}

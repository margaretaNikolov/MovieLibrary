using AutoMapper;
using MovieLibrary.Business.DTO;
using MovieLibrary.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MovieLibrary.Business
{
    public static class GenresRepository
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static List<GenreDTO> GetAllGenres()
        {
            var context = new MovieLibraryContext();
            try
            {
                var dbGenres = context.Genres.ToList<Genre>();
                return Mapper.Map<List<GenreDTO>>(dbGenres);
            }
            catch (Exception ex)
            {
                log.Error("Genres Repository GetAllGenres method Error: ", ex);
                return null;
            }
            finally
            {
                if (context != null)
                    ((IDisposable)context).Dispose();
            }
        }

        public static List<GenreDTO> GetAllActiveGenres()
        {
            var context = new MovieLibraryContext();
            try
            {
                var dbGenres = context.Genres.Where(m => m.DeleteDate == null).ToList<Genre>();
                return Mapper.Map<List<GenreDTO>>(dbGenres);
            }
            catch (Exception ex)
            {
                log.Error("Genres Repository GetAllActiveGenres method Error: ", ex);
                return null;
            }
            finally
            {
                if (context != null)
                    ((IDisposable)context).Dispose();
            }
        }

        public static List<GenreDTO> GetOtherActiveGenres(List<int> selectedGenreIDs)
        {
            var context = new MovieLibraryContext();
            try
            {
                var dbGenres = context.Genres
                    .Where(m => m.DeleteDate == null)
                    .Where(m => !selectedGenreIDs.Contains(m.GenreID))
                    .ToList<Genre>();
           
                return Mapper.Map<List<GenreDTO>>(dbGenres);
            }
            catch (Exception ex)
            {
                log.Error("Genres Repository GetOtherActiveGenres method Error: ", ex);
                return null;
            }
            finally
            {
                if (context != null)
                    ((IDisposable)context).Dispose();
            }
        }

        public static GenreDTO Insert(GenreDTO genre)
        {
            var context = new MovieLibraryContext();
            try
            {
                var dbGenre = Mapper.Map<Genre>(genre);
                var data = context.Genres.Add(dbGenre);
                context.SaveChanges();
                return Mapper.Map<GenreDTO>(data);
            }
            catch (Exception ex)
            {
                log.Error("Genres Repository Insert method Error: ", ex);
                return null;
            }
            finally
            {
                if (context != null)
                    ((IDisposable)context).Dispose();
            }
        }

        public static GenreDTO Get(int genreId)
        {
            var context = new MovieLibraryContext();
            try
            {
                var dbGenre = context.Genres.Where(m => m.GenreID == genreId).FirstOrDefault();
                return Mapper.Map<GenreDTO>(dbGenre);
            }
            catch (Exception ex)
            {
                log.Error("Genres Repository Get method Error: ", ex);
                return null;
            }
            finally
            {
                if (context != null)
                    ((IDisposable)context).Dispose();
            }
        }

        public static void Update(GenreDTO genre)
        {
            var context = new MovieLibraryContext();
            try
            {
                var dbGenre = context.Genres.SingleOrDefault(m => m.GenreID.Equals(genre.GenreID));
                if (dbGenre != null)
                {
                    Mapper.Map(genre, dbGenre);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                log.Error("Genres Repository Update method Error: ", ex);
            }
            finally
            {
                if (context != null)
                    ((IDisposable)context).Dispose();
            }
        }

        public static bool Delete(int genreId)
        {
            var context = new MovieLibraryContext();
            try
            {
                Genre genre = context.Genres.Find(genreId);
                if (genre != null)
                {
                    genre.DeleteDate = DateTime.Now;
                    context.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                log.Error("Genres Repository Delete method Error: ", ex);
                return false;
            }
            finally
            {
                if (context != null)
                    ((IDisposable)context).Dispose();
            }
        }

    }
}

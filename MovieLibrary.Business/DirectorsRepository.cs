using AutoMapper;
using MovieLibrary.Business.DTO;
using MovieLibrary.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MovieLibrary.Business
{
    public static class DirectorsRepository
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static List<DirectorDTO> GetAllDirectors()
        {
            var context = new MovieLibraryContext();
            try
            {
                var dbDirectors = context.Directors.ToList<Director>();
                return Mapper.Map<List<DirectorDTO>>(dbDirectors);
            }
            catch (Exception ex)
            {
                log.Error("Directors Repository GetAllDirectors method Error: ", ex);
                return null;
            }
            finally
            {
                if (context != null)
                    ((IDisposable)context).Dispose();
            }
        }

        public static List<DirectorDTO> GetAllActiveDirectors()
        {
            var context = new MovieLibraryContext();
            try
            {
                var dbDirectors = context.Directors.Where(m => m.DeleteDate == null).ToList<Director>();
                return Mapper.Map<List<DirectorDTO>>(dbDirectors);
            }
            catch (Exception ex)
            {
                log.Error("Directors Repository GetAllActiveDirectors method Error: ", ex);
                return null;
            }
            finally
            {
                if (context != null)
                    ((IDisposable)context).Dispose();
            }
        }

        public static DirectorDTO Insert(DirectorDTO director)
        {
            var context = new MovieLibraryContext();
            try
            {            
                var dbDirector = Mapper.Map<Director>(director);
                var data = context.Directors.Add(dbDirector);
                context.SaveChanges();
                return Mapper.Map<DirectorDTO>(data);
            }
            catch (Exception ex)
            {
                log.Error("Directors Repository Insert method Error: ", ex);
                return null;
            }
            finally
            {
                if (context != null)
                    ((IDisposable)context).Dispose();
            }
        }

        public static DirectorDTO Get(int directorId)
        {
            var context = new MovieLibraryContext();
            try
            {
                var dbDirector = context.Directors.Where(m => m.DirectorID==directorId).FirstOrDefault();
                return Mapper.Map<DirectorDTO>(dbDirector);
            }
            catch (Exception ex)
            {
                log.Error("Directors Repository Get method Error: ", ex);
                return null;
            }
            finally
            {
                if (context != null)
                    ((IDisposable)context).Dispose();
            }
        }

        public static void Update(DirectorDTO director)
        {
            var context = new MovieLibraryContext();
            try
            {
                var dbDirector = context.Directors.SingleOrDefault(m => m.DirectorID.Equals(director.DirectorID));
                if (dbDirector != null)
                {
                    Mapper.Map(director, dbDirector);                   
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                log.Error("Directors Repository Update method Error: ", ex);
            }
            finally
            {
                if (context != null)
                    ((IDisposable)context).Dispose();
            }
        }

        public static bool Delete(int directorId)
        {
            var context = new MovieLibraryContext();
            try
            {
                Director director = context.Directors.Find(directorId);
                if (director != null)
                {
                    director.DeleteDate = DateTime.Now;
                    context.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                log.Error("Directors Repository Delete method Error: ", ex);
                return false;
            }
            finally
            {
                if (context != null)
                    ((IDisposable)context).Dispose();
            }
        }

        public static List<DirectorDTO> GetOtherActiveDirectors(List<int> selectedDirectorIDs)
        {
            var context = new MovieLibraryContext();
            try
            {
                var dbDirectors = context.Directors
                    .Where(m => m.DeleteDate == null)
                    .Where(m => !selectedDirectorIDs.Contains(m.DirectorID))
                    .ToList<Director>();

                return Mapper.Map<List<DirectorDTO>>(dbDirectors);
            }
            catch (Exception ex)
            {
                log.Error("Genres Repository GetOtherActiveDirectors method Error: ", ex);
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

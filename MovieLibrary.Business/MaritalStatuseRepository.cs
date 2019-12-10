using MovieLibrary.Business.DTO;
using MovieLibrary.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;

namespace MovieLibrary.Business
{
    public class MaritalStatuseRepository
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static List<string> GetAllCaptions()
        {
            var context = new MovieLibraryContext();
            try
            {
                var dbCaptions = context.MaritalStatuses.Select(m =>  m.Caption ).Distinct().ToList();
                return dbCaptions;
            }
            catch (Exception ex)
            {
                log.Error("Marital Statuse Repository GetAllCaptions method Error: ", ex);
                return null;
            }
            finally
            {
                if (context != null)
                    ((IDisposable)context).Dispose();
            }
        }

        public static List<MaritalStatusDTO> GetAllMaritalStatuses()
        {
            var context = new MovieLibraryContext();
            try
            {
                var dbMaritalStatuse = context.MaritalStatuses.Distinct().ToList();
                var data = Mapper.Map<List<MaritalStatusDTO>>(dbMaritalStatuse);
                return data;
            }
            catch (Exception ex)
            {
                log.Error("Marital Statuse Repository GetAllMaritalStatuses method Error: ", ex);
                return null;
            }
            finally
            {
                if (context != null)
                    ((IDisposable)context).Dispose();
            }
        }

        public static MaritalStatusDTO GetMaritalStatusById(int id)
        {
            var context = new MovieLibraryContext();
            try
            {
                var dbMaritalStatuse = context.MaritalStatuses.SingleOrDefault(m => m.MaritalStatusID == id);
                var data = Mapper.Map<MaritalStatusDTO>(dbMaritalStatuse);
                return data;
            }
            catch (Exception ex)
            {
                log.Error("Marital Statuse Repository GetMaritalStatusById method Error: ", ex);
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

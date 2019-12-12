using AutoMapper;
using MovieLibrary.Business.DTO;
using MovieLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;

namespace MovieLibrary.Business
{
    public static class UsersRepository
    {
        //private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly log4net.ILog log = LogHelper.GetLogger();
        public static List<UserDTO> GetAllUsers()
        {
            var context = new MovieLibraryContext();
            try
            {
                List<User> dbUsers = context.Users.ToList<User>();
                return Mapper.Map<List<UserDTO>>(dbUsers);
            }
            catch (Exception ex)
            {
                log.Error("Users Repository GetAllUsers method Error: ", ex);
                return null;
            }
            finally
            {
                if (context != null)
                    ((IDisposable)context).Dispose();
            }
        }

        public static List<UserDTO> GetAllActiveUsers()
        {
            var context = new MovieLibraryContext();
            try
            {
                var dbUsers = context.Users
                    .Include(m => m.MaritalStatus).Where(m => m.DeleteDate == null).ToList();
                return Mapper.Map<List<UserDTO>>(dbUsers);
            }
            catch (Exception ex)
            {
                log.Error("Users Repository  GetAllActiveUsers method Error: ", ex);
                return null;
            }
            finally
            {
                if (context != null)
                    ((IDisposable)context).Dispose();
            }
        }

        public static List<UserDTO> GetAllActiveUsers(string sortOrder, string searchString, int page, int pageSize, out int totalCount)
        {
            var context = new MovieLibraryContext();
            try
            {
                var dbUsers = context.Users.Include(m => m.MaritalStatus).Where(m => m.DeleteDate == null);
                if (!String.IsNullOrEmpty(searchString))
                {
                    dbUsers = dbUsers.Where(m => m.LastName.Contains(searchString) || m.FirstName.Contains(searchString));                   
                }
                switch (sortOrder)
                {
                    case "name_desc":
                        dbUsers = dbUsers.OrderByDescending(s => s.LastName);
                        break;
                    case "Date":
                        dbUsers = dbUsers.OrderBy(s => s.InsertDate);
                        break;
                    case "date_desc":
                        dbUsers = dbUsers.OrderByDescending(s => s.InsertDate);
                        break;
                    default:  // Name ascending 
                        dbUsers = dbUsers.OrderBy(s => s.LastName);
                        break;
                }              
                totalCount = dbUsers.Count();
                var users = dbUsers.Skip(pageSize*page).Take(pageSize).ToList();
                return Mapper.Map<List<UserDTO>>(users);
            }
            catch (Exception ex)
            {
                log.Error("Users Repository  GetAllActiveUsers method Error: ", ex);
                totalCount = 0;
                return null;
            }
            finally
            {
                if (context != null)
                    ((IDisposable)context).Dispose();
            }
        }


        public static UserDTO Get(int userId)
        {
            var context = new MovieLibraryContext();
            try
            {
                var dbUser = context.Users.Include(m => m.MaritalStatus).Where(m => m.UserID == userId).FirstOrDefault();
                return Mapper.Map<UserDTO>(dbUser);
            }
            catch (Exception ex)
            {
                log.Error("Users Repository Get method Error: ", ex);
                return null;
            }
            finally
            {
                if (context != null)
                    ((IDisposable)context).Dispose();
            }
        }

        public static UserDTO Insert(UserDTO user)
        {
            var context = new MovieLibraryContext();
            try
            {
                if (!String.IsNullOrEmpty(user.ImagePath))
                {
                    FileStream fs = new FileStream(user.ImagePath, FileMode.Open, FileAccess.Read);
                    int length = (int)fs.Length;
                    user.Image = new byte[length];
                    fs.Read(user.Image, 0, length);
                    fs.Close();
                }
                var dbUser = Mapper.Map<User>(user);
                var data = context.Users.Add(dbUser);
                context.SaveChanges();
                return Mapper.Map<UserDTO>(data);
            }
            catch (Exception ex)
            {
                log.Error("Users Repository Insert method Error: ", ex);
                return null;
            }
            finally
            {
                if (context != null)
                    ((IDisposable)context).Dispose();
            }
        }

        public static void Update(UserDTO user)
        {
            var context = new MovieLibraryContext();
            try
            {
                var dbUser = context.Users.SingleOrDefault(m => m.UserID.Equals(user.UserID));
                if (dbUser != null)
                {
                    if (!String.IsNullOrEmpty(user.ImageTitle))
                    {
                        if (dbUser.ImageTitle != user.ImageTitle)
                        {
                            FileStream fs = new FileStream(user.ImagePath, FileMode.Open, FileAccess.Read);
                            int length = (int)fs.Length;
                            user.Image = new byte[length];
                            fs.Read(user.Image, 0, length);
                            fs.Close();
                            var userEntry = context.Entry(dbUser);
                            userEntry.CurrentValues.SetValues(user);
                        }
                        else
                        {
                            // Update scalar properties 
                            var userEntry = context.Entry(dbUser);
                            userEntry.CurrentValues.SetValues(user);
                            userEntry.Property(m => m.Image).IsModified = false;
                            userEntry.Property(m => m.ImageTitle).IsModified = false;
                        }
                    }
                    else
                    {
                        // Update scalar properties 
                        var userEntry = context.Entry(dbUser);
                        userEntry.CurrentValues.SetValues(user);
                        userEntry.Property(m => m.Image).IsModified = false;
                        userEntry.Property(m => m.ImageTitle).IsModified = false;
                    }
                }
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                log.Error("Users Repository Update method Error: ", ex);
            }
            finally
            {
                if (context != null)
                    ((IDisposable)context).Dispose();
            }
        }

        public static bool Delete(int userId)
        {
            var context = new MovieLibraryContext();
            try
            {
                User user = context.Users.Find(userId);
                if (user != null)
                {
                    user.DeleteDate = DateTime.Now;
                    context.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                log.Error("Users Repository Delete method Error: ", ex);
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

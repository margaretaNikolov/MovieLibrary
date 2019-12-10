using AutoMapper;
using MovieLibrary.Business.DTO;
using MovieLibrary.Data;
using MovieLibrary.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MovieLibrary.Web.WebAutoMapper
{
    public static class MVCMapperConfiguration
    {
        public static void Configure()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile(new MovieProfile());
                cfg.AddProfile(new UserProfile());
                cfg.AddProfile(new MaritalStatuseProfile());
                cfg.AddProfile(new WWListProfile());
                cfg.AddProfile(new DirectorProfile());
                cfg.AddProfile(new GenreProfile());
            });
        }
    }

    public class MovieProfile : Profile
    {
        public MovieProfile()
        {
            CreateMap<Movy, MovieDTO>()
                   .ForMember(d => d.SelectedDirectorIDs, opt => opt.Ignore())
                   .ForMember(d => d.SelectedGenreIDs, opt => opt.Ignore());
            CreateMap<MovieDTO, Movy>()
                .ForMember(d => d.MovieDirectors, opt => opt.Ignore())
                .ForMember(d => d.MovieGenres, opt => opt.Ignore());
                //.ForMember(d => d.Image, opt => opt.Condition(src => (src.Image!=null)))
                //.ForMember(d => d.ImageTitle, opt => opt.Condition(src => (!String.IsNullOrEmpty(src.ImageTitle))));

            CreateMap<MovieAddModel, MovieDTO>()
                   .ForMember(d => d.InsertDate, opt => opt.MapFrom(src => DateTime.Now))
                   .ForMember(d => d.SelectedDirectorIDs, opt => opt.MapFrom(src => src.SelectedDirectorModel.SelectedDirectorIDs))
                   .ForMember(d => d.SelectedGenreIDs, opt => opt.MapFrom(src => src.SelectedGenreModel.SelectedGenreIDs));

            CreateMap<MovieModel, MovieDTO>()
                .ForMember(d => d.MovieDirectors, opt => opt.Ignore())
                .ForMember(d => d.MovieGenres, opt => opt.Ignore());
            CreateMap<MovieDTO, MovieModel>();

            CreateMap<MovieDTO, MovieEditModel>();
            CreateMap<MovieEditModel, MovieDTO>()
                  .ForMember(d => d.SelectedDirectorIDs, opt => opt.MapFrom(src => src.AllDirectorModel.DirectorIDs))
                  .ForMember(d => d.SelectedGenreIDs, opt => opt.MapFrom(src => src.AllGenreModel.GenreIDs));

            CreateMap<RentedMovy, RentedMovieDTO>()
                 .ForMember(d => d.Movie, opt => opt.MapFrom(src => src.Movy));
            CreateMap<RentedMovieDTO, RentedMovy>()
                 .ForMember(d => d.Movy, opt => opt.MapFrom(src => src.Movie));

            CreateMap<RentedMovieDTO, RentedMovieModel>();
            CreateMap<RentedMovieModel, RentedMovieDTO>();

            CreateMap<MovieDirector, MovieDirectorDTO>();
            CreateMap<MovieDirectorDTO, MovieDirector>();

            CreateMap<MovieDirectorDTO, DirectorLightModel>()
                 .ForMember(d => d.Name, opt => opt.MapFrom(src => src.Director.FirstName + " " + src.Director.LastName));
            CreateMap<MovieDirectorDTO, DirectorDTO>()
                .ForMember(d => d.FirstName, opt => opt.MapFrom(src => src.Director.FirstName))
                .ForMember(d => d.LastName, opt => opt.MapFrom(src => src.Director.LastName));

            CreateMap<MovieGenre, MovieGenreDTO>();
            CreateMap<MovieGenreDTO, MovieGenre>();

            CreateMap<MovieGenreDTO, GenreLightModel>()
                .ForMember(d => d.Name, opt => opt.MapFrom(src => src.Genre.Name));

            CreateMap<MovieGenreDTO, GenreDTO>()
                .ForMember(d => d.Name, opt => opt.MapFrom(src => src.Genre.Name));

            CreateMap<GridMovieFilter, GridMovieFilterDTO>();
            CreateMap<GridMovieFilterDTO, GridMovieFilter>();

            CreateMap<GridMovieResult, GridMovieResultDTO>();
            CreateMap<GridMovieResultDTO, GridMovieResult>();

            CreateMap<MovieImportDTO, Movy>()
                  .ForMember(d => d.MovieID, opt => opt.Ignore())
                  .ForMember(d => d.InsertDate, opt => opt.MapFrom(src => DateTime.Now))
                  .ForMember(d => d.DeleteDate, opt => opt.Ignore())
                  .ForMember(d => d.Image, opt => opt.Ignore())
                  .ForMember(d => d.ImageTitle, opt => opt.Ignore())
                  .ForMember(d => d.RentedMovies, opt => opt.Ignore())
                  .ForMember(d => d.WWLists, opt => opt.Ignore())
                  .ForMember(d => d.MovieGenres, opt => opt.Ignore())
                  .ForMember(d => d.MovieDirectors, opt => opt.Ignore());

            CreateMap<MovieImportModel, MovieImportDTO>();
            CreateMap<MovieImportDTO, MovieImportModel>();
        }
    }

    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDTO>();
            CreateMap<UserDTO, User>();

            CreateMap<UserModel, UserDTO>();
            CreateMap<UserDTO, UserModel>();

            CreateMap<UserEditModel, UserDisplayModel>();

            CreateMap<UserAddModel, UserDTO>();

            CreateMap<UserDTO, UserDisplayModel>()
                .ForMember(d => d.Caption, opt => opt.MapFrom(src => src.MaritalStatus.Caption))             
                .ReverseMap()
                .ForPath(s => s.MaritalStatus.Caption, opt => opt.MapFrom(src => src.Caption.FirstOrDefault()));

            CreateMap<UserEditModel, UserDTO>()
                .ForMember(d => d.MaritalStatus, opt => opt.Ignore());
            CreateMap<UserDTO, UserEditModel>()
                .ForMember(d => d.MaritalStatuses, opt => opt.Ignore());
        }
    }

    public class MaritalStatuseProfile : Profile
    {
        public MaritalStatuseProfile()
        {
            CreateMap<MaritalStatus, MaritalStatusDTO>();
            CreateMap<MaritalStatusDTO, MaritalStatus>();

            CreateMap<MaritalStatuseModel, MaritalStatusDTO>();
            CreateMap<MaritalStatusDTO, MaritalStatuseModel>();
        }
    }

    public class WWListProfile : Profile
    {
        public WWListProfile()
        {
            CreateMap<WWList, WWListDTO>();
            //.ForMember(d => d.Movie, opt => opt.MapFrom(src => src.Movy));
            CreateMap<WWListDTO, WWList>();
            //.ForMember(d => d.Movy, opt => opt.MapFrom(src => src.Movie));
        }
    }

    public class DirectorProfile : Profile
    {
        public DirectorProfile()
        {
            CreateMap<DirectorModel, DirectorDTO>();
            CreateMap<DirectorDTO, DirectorModel>();

            CreateMap<Director, DirectorDTO>();
            CreateMap<DirectorDTO, Director>();

            CreateMap<DirectorAddModel, DirectorDTO>()
                 .ForMember(d => d.InsertDate, opt => opt.MapFrom(src => DateTime.Now));
            CreateMap<DirectorDTO, DirectorAddModel>();

            CreateMap<DirectorDTO, DirectorLightModel>()
                 .ForMember(d => d.DirectorID, opt => opt.MapFrom(src => src.DirectorID))
                 .ForMember(d => d.Name, opt => opt.MapFrom(src => src.FirstName + " " + src.LastName));
        }
    }

    public class GenreProfile : Profile
    {
        public GenreProfile()
        {
            CreateMap<GenreModel, GenreDTO>();
            CreateMap<GenreDTO, GenreModel>();

            CreateMap<GenreAddModel, GenreDTO>()
                 .ForMember(d => d.InsertDate, opt => opt.MapFrom(src => DateTime.Now));
            CreateMap<GenreDTO, GenreAddModel>();

            CreateMap<Genre, GenreDTO>();
            CreateMap<GenreDTO, Genre>();

            CreateMap<GenreDTO, GenreLightModel>();
        }
    }

}
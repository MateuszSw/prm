using System;
using System.Linq;
using AutoMapper;
using Prm.API.Dtos;
using Prm.API.Models;

namespace Prm.API.Helpers {
    public class AutoMapperProfiles : Profile {
        public AutoMapperProfiles () {
            CreateMap<User, UserListDto> ()
                .ForMember (dest => dest.PhotoUrl, opt => {
                    opt.MapFrom (src => src.Photos.FirstOrDefault (p => p.Main).Url);
                });

            CreateMap<User, UserDetailedDto> ()
                .ForMember (dest => dest.PhotoUrl, opt => {
                    opt.MapFrom (src => src.Photos.FirstOrDefault (p => p.Main).Url);
                })
                .ForMember (dest => dest.CreatedArticles, opt => {
                    opt.MapFrom (src => src.Articles);
                })
                .ForMember (dest => dest.SubscribedArticles, opt => {
                    opt.MapFrom (src => src.ArticleStudents.Select (s => s.Article));
                });
            CreateMap<Photo, PhotosDetailedDto> ();
                        CreateMap<UserRegisterDto, User> ();
            CreateMap<ArticleForCreationEdition, Article> ().ReverseMap ();
            CreateMap<UserForUpdateDto, User> ();
            CreateMap<Photo, PhotosDetailedDto> ();
            CreateMap<Article, ArticleForListDto> ();
            CreateMap<Article, ArticleForDetailesDto> ()
                .ForMember (m => m.Students, opt => opt
                    .MapFrom (u => u.Students.Select (s => s.Student)))
                .ForMember (m => m.Questions, opt => opt
                    .MapFrom (u => (u.Test ?? string.Empty).Split ("\n*\n", StringSplitOptions.None).Select (
                        q => new QuestionDto {
                            Value = q.Split ('\n', StringSplitOptions.None).First (),
                                Answers = q.Split ('\n', StringSplitOptions.None).Skip (1).Select (
                                    a => new AnswerDto {
                                        Value = a.FirstOrDefault () == '#' ? a.Substring (1) : a,
                                        IsCorrect = a.FirstOrDefault () == '#'
                                    }
                                )
                        }

                    )));
            CreateMap<MessageCreationDto, Message> ().ReverseMap ();
            CreateMap<Message, MessageDto> ()
                .ForMember (m => m.SenderImage, opt => opt
                    .MapFrom (u => u.Sender.Photos.FirstOrDefault (p => p.Main).Url))
                .ForMember (m => m.RecipientImage, opt => opt
                    .MapFrom (u => u.Recipient.Photos.FirstOrDefault (p => p.Main).Url));

        }
    }
}
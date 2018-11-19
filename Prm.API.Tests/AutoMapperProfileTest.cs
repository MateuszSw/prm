using AutoMapper;
using System;
using Xunit;
using System.Linq;
using Prm.API.Dtos;
using Prm.API.Helpers;
using Prm.API.Models;
using System.Collections.Generic;

namespace prm.api.tests
{
    public class AutoMapperProfileTest
    {
        static AutoMapperProfileTest()
        {
            Mapper.Initialize(cfg=> cfg.AddProfile( new AutoMapperProfiles()));
        }

        [Fact]
        public void CheckUrlMappingTest()
        {
            // arange            

            User user = new User 
            {
                Photos = new List<Photo>
                {
                    new Photo
                    {
                        Url = "test1"
                    },
                    new Photo
                    {
                        Url = "test2",
                        Main = true
                    },
                    new Photo
                    {
                        Url = "test3"
                    }
                }

            };

            // act
            var score =Mapper.Map<User, UserDetailedDto>(user);

            // assert
            Assert.NotEqual("test1", score.PhotoUrl);
            Assert.Equal("test2", score.PhotoUrl);
            Assert.NotEqual("test3", score.PhotoUrl);
        }

        [Fact]
        public void CheckQuestionsMappingTest()
        {
            // arange
            var article = new Article
            {
                Test = "Q1\n#A1 - correct\nA2 - not correct\nA3 - not correct\n*\nTwo plus two?\nOne\n#Four\nSix"
            };

            // act
            var score =Mapper.Map<Article, ArticleForDetailesDto>(article);
            System.Console.WriteLine(score.Test);

            // assert
            Assert.NotNull(score.Questions);
            Assert.Equal(2, score.Questions.Length);
            Assert.Equal("Two plus two?", score.Questions[1].Value);
            Assert.Equal(true, score.Questions[0].Answers.First().IsCorrect);
            Assert.Equal("Six", score.Questions[1].Answers.Last().Value);
        }
    }
}

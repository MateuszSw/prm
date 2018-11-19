using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Prm.API.Data;
using Prm.API.Dtos;
using Prm.API.Helpers;
using Prm.API.Models;

namespace Prm.API.Controllers {
    [ServiceFilter (typeof (LogUser))]
    [Route ("api/article/{userId}/[action]")]
    [ApiController]
    public class ArticleController : ControllerBase {
        private readonly IPrmRepository _repo;
        private readonly IMapper _mapper;
        public ArticleController (IPrmRepository repo, IMapper mapper) {
            _mapper = mapper;
            _repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetArticles (int userId, [FromQuery] ArticleParams articleParams) {
            articleParams.UserId = userId;

            if (articleParams.UserId != int.Parse (User.FindFirst (ClaimTypes.NameIdentifier).Value))
                return Unauthorized ();

            var articlesFromRepo = await _repo.GetArticlesForUser (articleParams);

            if (articlesFromRepo == null)
                return NotFound ();

            Response.AddPagination (articlesFromRepo.CurrentPage, articlesFromRepo.SizePage,
                articlesFromRepo.TotalCount, articlesFromRepo.Total);
            var articleReturn = Mapper.Map<IList<ArticleForListDto>> (articlesFromRepo);
            return Ok (articleReturn);
        }

        [HttpGet]
        public async Task<IActionResult> GetArticle (int userId, int? id) {
            var articleFromRepo = await _repo.GetArticle (id.Value);

            if (articleFromRepo == null)
                return NotFound ();
            var articleReturn = Mapper.Map<ArticleForDetailesDto> (articleFromRepo);
            return Ok (articleReturn);
        }

        [HttpPost]
        public async Task<IActionResult> CreateArticle (int userId, [FromForm] ArticleForCreationEdition articleForCreation) {
            var author = await _repo.GetUser (userId, false);

            if (!int.TryParse (User.FindFirstValue (ClaimTypes.NameIdentifier), out int id) || author.Id != id) {
                return Unauthorized ();
            }

            articleForCreation.AuthorId = userId;

            var article = _mapper.Map<Article> (articleForCreation);

            article.Author = author;

            _repo.Add (article);

            if (await _repo.SaveAll ()) {
                return Ok ();
            }

            throw new Exception ("Dodawanie artykułu na serwer nie powiodło sie");
        }

        [HttpPost]
        public async Task<IActionResult> EditArticle (int userId, [FromBody] ArticleForCreationEdition articleForEdition) {
            var article = await _repo.GetArticle (articleForEdition.Id);

            if (!int.TryParse (User.FindFirstValue (ClaimTypes.NameIdentifier), out int id) 
            || (article.AuthorId != id && !User.IsInRole ("Admin"))) {
                return Unauthorized ();
            }

			if (article.Title == articleForEdition.Title 
			&& article.Content == articleForEdition.Content
			&& article.Test == articleForEdition.Test) {
                return Ok ();
            }
			
            article.Title = articleForEdition.Title;
            article.Content = articleForEdition.Content;
            article.Test = articleForEdition.Test;

            if (await _repo.SaveAll ()) {
                return Ok ();
            }

            throw new Exception ("Editing the article failed on save");
        }

        [HttpGet]
        public async Task<IActionResult> DeleteArticle (int? id, int userId) {
            var articleFromRepo = await _repo.GetArticle (id.Value);
            if (userId != int.Parse (User.FindFirst (ClaimTypes.NameIdentifier).Value) ||
                (articleFromRepo.AuthorId != userId && !User.IsInRole ("Admin"))) {
                return Unauthorized ();
            }

            foreach (var ar in articleFromRepo.Students) {
                _repo.Delete (ar);
            }

            _repo.Delete (articleFromRepo);

            if (await _repo.SaveAll ())
                return NoContent ();

            throw new Exception ("Error deleting the article");
        }

        [HttpPost]
        public async Task<IActionResult> AddToArticle ([FromForm] int? id, int userId) {
            if (userId != int.Parse (User.FindFirst (ClaimTypes.NameIdentifier).Value) ||
                !User.IsInRole ("Student"))
                return Unauthorized ();

            var articleFromRepo = await _repo.GetArticle (id.Value);
            var userFromRepo = await _repo.GetUser (userId, true);

            ArticleStudent ast = new ArticleStudent {
                Article = articleFromRepo,
                Student = userFromRepo
            };

            articleFromRepo.Students.Add (ast);

            if (await _repo.SaveAll ())
                return Ok ();

            throw new Exception ("Error deleting the message");
        }

        public async Task<IActionResult> RemoveFromArticle ([FromForm] int? id, int userId) {
            if (userId != int.Parse (User.FindFirst (ClaimTypes.NameIdentifier).Value) ||
                !User.IsInRole ("Student"))
                return Unauthorized ();

            var articleFromRepo = await _repo.GetArticle (id.Value);

            var ast = articleFromRepo.Students.FirstOrDefault (s => s.StudentId == userId && s.ArticleId == id);

            if (ast != null) {
                articleFromRepo.Students.Remove (ast);

                if (await _repo.SaveAll ())
                    return Ok ();
            }

            throw new Exception ("Error deleting the message");
        }

    }
}
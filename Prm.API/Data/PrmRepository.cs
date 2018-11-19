using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Prm.API.Helpers;
using Prm.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Prm.API.Data
{
    public class PrmRepository : IPrmRepository
    {
        private readonly DataContext _context;
        public PrmRepository(DataContext context)
        {
            _context = context;
        }
        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        

        public async Task<Photo> GetPhoto(int id)
        {
            var photo = await _context.Photos.IgnoreQueryFilters()
                .FirstOrDefaultAsync(ph => ph.Id == id);

            return photo;
        }

        public async Task<User> GetUser(int id, bool isCurrentUser)
        {
            var query = _context.Users
            .Include(p => p.Photos)
            .Include(u=>u.Articles)
            .Include(u=>u.ArticleStudents).ThenInclude(u=>u.Article)
            .AsQueryable();

            if (isCurrentUser)
                query = query.IgnoreQueryFilters();

            var user = await query.FirstOrDefaultAsync(u => u.Id == id);

            return user;
        }

        public async Task<PagedList<User>> GetUsers(UserParams userParams)
        {
            var users = _context.Users.Include(p => p.Photos)
                .OrderByDescending(u => u.LastActive).AsQueryable();

            users = users.Where(u => u.Id != userParams.UserId);

            users = users.Where(u => u.Status == userParams.Status);



            if (!string.IsNullOrEmpty(userParams.OrderBy))
            {
                switch (userParams.OrderBy)
                {
                    case "created":
                        users = users.OrderByDescending(u => u.Created);
                        break;
                    default:
                        users = users.OrderByDescending(u => u.LastActive);
                        break;
                }
            }

            return await PagedList<User>.CreateAsync(users, userParams.PageNumber, userParams.SizePage);
        }


        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<Message> GetMessage(int id)
        {
            return await _context.Messages.FirstOrDefaultAsync(message => message.Id == id);
        }

        private IQueryable<Article> GetArticlesForUser(int userId, Boolean showAll) 
        {
            var articles = _context.Articles
                .Include(a => a.Author)
                .Include(a => a.Students).ThenInclude(sa => sa.Student)
                .AsQueryable();

            var roles = _context.UserRoles.Where(u=>u.UserId == userId).Select(u=>u.Role.Name).ToList();

            if(!roles.Contains("Admin") && !showAll)
            {
                if(roles.Contains("Teacher") && roles.Contains("Student"))
                {
                    articles = articles.Where(a=>a.AuthorId == userId || a.Students.Any(s=>s.Student.Id == userId));
                } else if(roles.Contains("Teacher"))
                {
                    articles = articles.Where(a=>a.AuthorId == userId);
                } else if(roles.Contains("Student")){
                    articles = articles.Where(a=> a.Students.Any(s=>s.Student.Id == userId));
                } else
                {
                    articles = articles.Where(a=> false);
                }

            }

            return articles;

        }

        public async Task<PagedList<Article>> GetArticlesForUser(ArticleParams articleParams)
        {
            var articles = GetArticlesForUser(articleParams.UserId, articleParams.ShowAll);
            
            articles = articles.OrderByDescending(d => d.Id);

            return await PagedList<Article>.CreateAsync(articles, articleParams.PageNumber, articleParams.SizePage);
        }

        public async Task<PagedList<Message>> GetMessagesUser (MessageParams messageParams)
        {
            var messages = _context.Messages
                .Include(u => u.Sender)
                .Include(u => u.Recipient)
                .AsQueryable();

            switch (messageParams.Container)
            {
                case "Outbox":
                    messages = messages.Where(user => user.IdSender == messageParams.UserId 
                        && user.SenderDeleted == false);
                    break;
                case "Inbox":
                    messages = messages.Where(user => user.RecipientId == messageParams.UserId 
                        && user.RecipientDeleted == false);
                    break;
                default:
                    messages = messages.Where(user => user.RecipientId == messageParams.UserId 
                        && user.RecipientDeleted == false && user.IsRead == false);
                    break;
            }
            messages = messages.OrderByDescending(d => d.MessageSent);
            return await PagedList<Message>.CreateAsync(messages, messageParams.PageNumber, messageParams.SizePage);
        }

        public async Task<IEnumerable<Message>> GetConversation (int userId, int recipientId)
        {
            var messages = await _context.Messages
                .Include(user => user.Sender).ThenInclude(p => p.Photos)
                .Include(user => user.Recipient).ThenInclude(p => p.Photos)
                .Where(message => message.RecipientId == userId && message.RecipientDeleted == false 
                    && message.IdSender == recipientId 
                    || message.RecipientId == recipientId && message.IdSender == userId 
                    && message.SenderDeleted == false)
                .OrderByDescending(message => message.MessageSent)
                .ToListAsync();

            return messages;
        }
        public async Task<Article> GetArticle(int id)
        {
             return await _context.Articles
             .Include(a=>a.Author)
             .Include(a => a.Students).ThenInclude(sa => sa.Student)
             .FirstOrDefaultAsync(m => m.Id == id);
        }
    }
}
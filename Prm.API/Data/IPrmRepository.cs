using System.Collections.Generic;
using System.Threading.Tasks;
using Prm.API.Helpers;
using Prm.API.Models;

namespace Prm.API.Data
{
    public interface IPrmRepository
    {
         void Add<T>(T entity) where T: class;
         void Delete<T>(T entity) where T: class;
         Task<bool> SaveAll();
         Task<PagedList<User>> GetUsers(UserParams userParams);
         Task<User> GetUser(int id, bool isCurrentUser);
         Task<Photo> GetPhoto(int id);
         Task<Message> GetMessage(int id);
         Task<Article> GetArticle(int id);

         Task<PagedList<Message>> GetMessagesUser (MessageParams messageParams);
         Task<PagedList<Article>> GetArticlesForUser(ArticleParams articleParams);
         Task<IEnumerable<Message>> GetConversation (int userId, int recipientId);
    }
}
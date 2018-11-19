using System.Collections.Generic;
using System.Threading.Tasks;
using Prm.API.Helpers;
using Prm.API.Models;

namespace Prm.API.Data
{
    public interface IMessageRepository
    {
         void Add<T>(T entity) where T: class;
         void Delete<T>(T entity) where T: class;
        Task<Message> GetMessage(int id);
         Task<Article> GetArticle(int id);

         Task<PagedList<Message>> GetMessagesUser (MessageParams messageParams);
         Task<IEnumerable<Message>> GetConversation (int userId, int recipientId);
    }
}
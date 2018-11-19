using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Prm.API.Helpers
{
    public class PagedList<T> : List<T>
    {
        public int CurrentPage { get; set; }
        public int Total { get; set; }
        public int SizePage { get; set; }
        public int TotalCount { get; set; }

        public PagedList(List<T> items, int count, int page, int size)
        {
            TotalCount = count;
            SizePage = size;
            CurrentPage = page;
            Total = (int)Math.Ceiling(count / (double)size);
            this.AddRange(items);
        }

        public static async Task<PagedList<T>> CreateAsync(IQueryable<T> source, 
            int page, int size)
        {
            var count = await source.CountAsync();
            var items = await source.Skip((page - 1) * size).Take(size).ToListAsync();
            return new PagedList<T>(items, count, page, size);
        }

        public static PagedList<T> Create(IQueryable<T> source, 
            int page, int size)
        {
            var count = source.Count();
            var items = source.Skip((page - 1) * size).Take(size).ToList();
            return new PagedList<T>(items, count, page, size);
        }
    }
}
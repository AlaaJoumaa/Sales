using ApplicationLib.Helpers;
using Microsoft.EntityFrameworkCore;
using SalesLib.Data;
using SalesLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SalesLib.Repositories
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetAll(string search);
        Task<Order> Get(int id);
        Task Add(Order model);
        Task Edit(Order model);
        Task Delete(int id);
    }
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext Context;

        public OrderRepository(ApplicationDbContext dbContext)
        {
            Context = dbContext;
        }

        public async Task Add(Order model)
        {
            Context.Orders.Add(model);
            await Context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var entity = await Context.Orders.SingleOrDefaultAsync(x => x.Id == id);
            Context.Orders.Remove(entity);
            await Context.SaveChangesAsync();
        }

        public async Task Edit(Order model)
        {
            var entity = Context.Attach(model);
            entity.State = EntityState.Modified;
            await Context.SaveChangesAsync();
        }

        public async Task<Order> Get(int id)
        {
            return await Context.Orders.SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<Order>> GetAll(string search)
        {
            Expression<Func<Order, bool>> filterExpression = x => string.IsNullOrEmpty(search) ? true :
                                                                 (
                                                                    x.Name.Contains(search) ||
                                                                    x.PhoneNumber.Contains(search) ||
                                                                    x.ServiceType.Contains(search) ||
                                                                    x.FromAddress.Contains(search) ||
                                                                    x.ToAddress.Contains(search) ||
                                                                    x.Email.Contains(search) ||
                                                                   (string.IsNullOrEmpty(x.Note) ? true : x.Note.Contains(search))                                                              
                                                                 );
            return await Context.Orders.Where(filterExpression).Select(x=>x).ToListAsync();
        }
    }
}

using DKCrm.Server.Data;
using DKCrm.Server.Interfaces;
using DKCrm.Shared.Constants;
using DKCrm.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace DKCrm.Server.Services
{
    public class InternalCompanyDataService : IInternalCompanyDataService
    {
        private readonly ApplicationDBContext _context;
        private readonly UserDbContext _contextUser;
        private readonly IConfiguration _configuration;
        public InternalCompanyDataService(ApplicationDBContext context, UserDbContext contextUser, , IConfiguration configuration)
        {
            _context = context;
            _contextUser = contextUser;
            _configuration = configuration;
        }

        public async Task<InternalCompanyData> GetAsync()
        {
            var data = await _context.InternalCompanyData.FirstOrDefaultAsync();
            if (data == null)
            {
                if (Initialize())
                    data = await _context.InternalCompanyData.FirstOrDefaultAsync();
                else
                    throw new InvalidOperationException();
            }
            return data!;
        }

        public async Task<Guid> PostAsync(InternalCompanyData data)
        {
            _context.Add(data);
            await _context.SaveChangesAsync();
            return data.Id;
        }
        public async Task MigrateProd(string pass)
        {
            var mPass = _configuration[$"mpass"];
            if (mPass == pass)
                await _context.Database.MigrateAsync();
        }
        public async Task MigrateUser(string pass)
        {
            var mPass = _configuration[$"mpass"];
            if (mPass == pass)
                await _contextUser.Database.MigrateAsync();
        }
        public async Task<Guid> PutAsync(InternalCompanyData data)
        {
            _context.Entry(data).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return data.Id;
        }
        public bool Initialize()
        {
            _context.InternalCompanyData.Add(
                new InternalCompanyData() { Id = Guid.NewGuid(), CurrencyPercent = 0, KeyFns = "", LocalCurrency = "RUB" ,Nds = 0}
            );
            return _context.SaveChangesAsync().IsCompletedSuccessfully;
        }
    }
}

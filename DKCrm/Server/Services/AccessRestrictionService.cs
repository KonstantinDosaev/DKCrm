﻿using DKCrm.Server.Data;
using DKCrm.Shared.Constants;
using DKCrm.Shared.Models.CompanyModels;
using DKCrm.Shared.Models.OrderModels;
using DKCrm.Shared.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.util;
using DKCrm.Server.Interfaces;
using Org.BouncyCastle.Asn1.IsisMtt.X509;

namespace DKCrm.Server.Services
{
    public class AccessRestrictionService :  IAccessRestrictionService
    {
        private readonly ApplicationDBContext _context;

        public AccessRestrictionService(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<bool> CheckExistAccessAndContainsUserInArrayToComponentAsync(Guid componentId, ClaimsPrincipal user)
        {
            var userId = user.Claims.Where(a => a.Type == ClaimTypes.NameIdentifier).Select(a => a.Value).FirstOrDefault();
            if (userId == null) return false;
            var acc = await _context.AccessRestrictions
                .FirstOrDefaultAsync(h => h.AccessedComponentId == componentId);
            if (acc == null)
                return true;
            var accessed = acc.AccessUsersToComponent.Contains(userId);
            return accessed;
        }
        public async Task<AccessRestriction> GetAccessFromComponentAsync(Guid componentId)
        {
            return await _context.AccessRestrictions
                .FirstOrDefaultAsync(h => h.AccessedComponentId == componentId) ?? new AccessRestriction();
        }
       
        public async Task<int> EditAccessToComponentAsync(AccessRestriction restriction, ClaimsPrincipal user)
        {
            var userRole = user.Claims.Where(a => a.Type == ClaimTypes.Role).Select(a => a.Value).FirstOrDefault();
            if (userRole == null || userRole != RoleNames.SuAdmin) return 0;
            
            if (restriction.Id != Guid.Empty)
            {
                var currentRestriction = await GetAccessFromComponentAsync(restriction.AccessedComponentId);
                if (currentRestriction != null)
                {
                    if (!restriction.AccessUsersToComponent.Any())
                        _context.Entry(currentRestriction).State = EntityState.Deleted;
                    else
                    {
                        currentRestriction.AccessUsersToComponent = restriction.AccessUsersToComponent;
                        _context.Entry(currentRestriction).State = EntityState.Modified;
                    }
                }
            }
            else
            {
                var userId = user.Claims.Where(a => a.Type == ClaimTypes.NameIdentifier).Select(a => a.Value).FirstOrDefault();
                var res = restriction.AccessUsersToComponent.Select(s => s).ToList();
               res.Add(userId);
                restriction.AccessUsersToComponent = res.ToArray();
                _context.Entry(restriction).State = EntityState.Added;
            }

            return await _context.SaveChangesAsync();
        }

        public async Task<int> RemoveAccessAsync(Guid accessId)
        {
            var currentRestriction = await GetAccessFromComponentAsync(accessId);
            if (currentRestriction != null)
            {
                _context.Entry(currentRestriction).State = EntityState.Deleted;
            }
            return await _context.SaveChangesAsync(); ;
        }
    }
}
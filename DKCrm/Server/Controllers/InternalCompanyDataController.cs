﻿using DKCrm.Server.Interfaces;
using DKCrm.Shared.Constants;
using DKCrm.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DKCrm.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class InternalCompanyDataController: ControllerBase
    {
        private readonly IInternalCompanyDataService _internalCompanyDataService;

        public InternalCompanyDataController(IInternalCompanyDataService internalCompanyDataService)
        {
            _internalCompanyDataService = internalCompanyDataService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _internalCompanyDataService.GetAsync());
        }

        [HttpPost]
        public async Task<IActionResult> Post(InternalCompanyData data)
        {
            return Ok(await _internalCompanyDataService.PostAsync(data));
        }

        [HttpPut]
        public async Task<IActionResult> Put(InternalCompanyData data)
        {
            return Ok(await _internalCompanyDataService.PutAsync(data));
        }

        [HttpGet("user/{pass}")]
        [Authorize(Roles = $"{RoleNames.Admin}, {RoleNames.SuAdmin}")]
        public async Task MigrateUser(string pass)
        {
           await _internalCompanyDataService.MigrateUser(pass);
        }
        [HttpGet("product/{pass}")]
        [Authorize(Roles = $"{RoleNames.Admin}, {RoleNames.SuAdmin}")]
        public async Task MigrateProd(string pass)
        {
           await _internalCompanyDataService.MigrateProd(pass);
        }
    }
}

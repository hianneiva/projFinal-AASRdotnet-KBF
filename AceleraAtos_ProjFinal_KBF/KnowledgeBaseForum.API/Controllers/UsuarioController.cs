﻿using KnowledgeBaseForum.API.Utils;
using KnowledgeBaseForum.DataAccessLayer.Model;
using KnowledgeBaseForum.DataAccessLayer.Repository;
using KnowledgeBaseForum.DataAccessLayer.Repository.Impl;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KnowledgeBaseForum.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly UsuarioDao dao;

        public UsuarioController(KbfContext context)
        {
            dao = new UsuarioDao(context);
        }

        [HttpGet]
        [Authorize(Roles = "ADMIN")]
        public async Task<IEnumerable<Usuario>> ReadAll() => await dao.All();

        [HttpGet("{login}")]
        [Authorize(Roles = "ADMIN,NORMAL")]
        public async Task<Usuario?> Read(string login) => await dao.Get(login);

        //[HttpPost]
        //public async Task<IActionResult> Create(Usuario entry)
        //{
        //    try
        //    {
        //        await dao.Add(entry);
        //        return Created(new Uri(Request.GetEncodedUrl()), entry);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}

        [HttpPut]
        [Authorize(Roles = "ADMIN,NORMAL")]
        public async Task<IActionResult> Update(Usuario entry)
        {
            try
            {
                if (entry == null)
                {
                    throw new KeyNotFoundException(Constants.ENTITY_NOT_FOUND_IN_DB);
                }

                await dao.Update(entry);
                return Ok(entry);
            }
            catch (Exception ex)
            {
                if (ex is KeyNotFoundException)
                {
                    return NotFound(ex.Message);
                }

                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{login}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Deactivate(string login)
        {
            try
            {
                Usuario? found = await dao.Get(login);

                if (found == null)
                {
                    return NotFound(Constants.ENTITY_NOT_FOUND_IN_DB);
                }

                found.Status = false;
                await dao.Update(found);
                return Ok(true);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{login}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Activate(string login)
        {
            try
            {
                Usuario? found = await dao.Get(login);

                if (found == null)
                {
                    return NotFound(Constants.ENTITY_NOT_FOUND_IN_DB);
                }

                found.Status = true;
                await dao.Update(found);
                return Ok(true);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

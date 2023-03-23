﻿using KnowledgeBaseForum.API.Model;
using KnowledgeBaseForum.API.Utils;
using KnowledgeBaseForum.DataAccessLayer.Model;
using KnowledgeBaseForum.DataAccessLayer.Repository;
using KnowledgeBaseForum.DataAccessLayer.Repository.Impl;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace KnowledgeBaseForum.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "ADMIN,NORMAL")]
    public class TopicoController : Controller
    {
        private readonly TopicoDao dao;

        public TopicoController(KbfContext context)
        {
            dao = new TopicoDao(context);
        }

        [HttpGet]
        public async Task<IEnumerable<Topico>> ReadAll() => await dao.All();

        [HttpGet("{topico}")]
        public async Task<Topico?> Read(Guid topico) => await dao.Get(topico);

        [HttpPost("search")]
        public async Task<IEnumerable<Topico>> Search(TopicoSearchRequest searchParams) => await dao.Search(searchParams.Filter,
                                                                                                            searchParams.Author,
                                                                                                            searchParams.Tags ?? new List<string>());

        [HttpGet("fromAuthor")]
        public async Task<IEnumerable<Topico>> FromAuthor(string login) => await dao.FromAuthor(login);

        [HttpPost]
        public async Task<IActionResult> Create(Topico entry)
        {
            try
            {
                await dao.Add(entry);
                return Created(new Uri(Request.GetEncodedUrl()), entry);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{entry}")]
        public async Task<IActionResult> Update(Topico entry)
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

        [HttpDelete("{entry}")]
        public async Task<IActionResult> Delete(Guid entry)
        {
            try
            {
                Topico? found = await dao.Get(entry);

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
    }
}

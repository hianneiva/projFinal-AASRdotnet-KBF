﻿using KnowledgeBaseForum.API.Utils;
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
    public class AlertaController : Controller
    {
        private readonly AlertaDao dao;

        public AlertaController(KbfContext context)
        {
            dao = new AlertaDao(context);
        }

        [HttpGet]
        public async Task<IEnumerable<Alerta>> ReadAll() => await dao.All();

        [HttpGet("{userId}")]
        public async Task<IEnumerable<Alerta>?> Read(string userId) => await dao.AllForUser(userId);

        [HttpGet("{userId}/{topicId}")]
        public async Task<Alerta?> Read(string userId, Guid topicId) => await dao.SingleForUser(userId, topicId);

        [HttpPost]
        public async Task<IActionResult> Create(Alerta entry)
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

        [HttpPost("{userId}")]
        public async Task<IActionResult> DismissForUser(string userId)
        {
            try
            {
                IEnumerable<Alerta> alerts = await dao.AllForUser(userId);

                foreach (Alerta entry in alerts)
                {
                    entry.Atualizacao = false;
                    await dao.Update(entry);
                }

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, bool toggleMode, bool dismiss)
        {
            try
            {
                Alerta? entry = await dao.Get(id);

                if (entry == null)
                {
                    throw new KeyNotFoundException(Constants.ENTITY_NOT_FOUND_IN_DB);
                }

                entry.ModoAlerta = (toggleMode && entry.ModoAlerta < 1) ? 1 : 0;

                if (dismiss)
                {
                    entry.Atualizacao = false;
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
                Alerta? found = await dao.Get(entry);

                if (found == null)
                {
                    return NotFound(Constants.ENTITY_NOT_FOUND_IN_DB);
                }

                await dao.Delete(found);
                return Ok(true);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

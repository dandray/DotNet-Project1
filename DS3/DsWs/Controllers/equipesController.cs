using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using DsWs.Models;

namespace DsWs.Controllers
{
    public class equipesController : ApiController
    {
        private ds3Entities db = new ds3Entities();

        // GET: api/equipes
        public IQueryable<equipes> Getequipes()
        {
            return db.equipes;
        }

        // GET: api/equipes/5
        [ResponseType(typeof(equipes))]
        public IHttpActionResult Getequipes(string id)
        {
            equipes equipes = db.equipes.Find(id);
            if (equipes == null)
            {
                return NotFound();
            }

            return Ok(equipes);
        }

        // GET : api/equipes?name=Name
        public IHttpActionResult GetequipesByName(string name)
        {
            return Ok(from r in db.equipes
                        where r.nom == name
                        select r);
        }

        // PUT: api/equipes/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Putequipes(string id, equipes equipes)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != equipes.code)
            {
                return BadRequest();
            }

            db.Entry(equipes).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!equipesExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/equipes
        [ResponseType(typeof(equipes))]
        public IHttpActionResult Postequipes(equipes equipes)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.equipes.Add(equipes);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (equipesExists(equipes.code))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = equipes.code }, equipes);
        }

        // DELETE: api/equipes/5
        [ResponseType(typeof(equipes))]
        public IHttpActionResult Deleteequipes(string id)
        {
            equipes equipes = db.equipes.Find(id);
            if (equipes == null)
            {
                return NotFound();
            }

            db.equipes.Remove(equipes);
            db.SaveChanges();

            return Ok(equipes);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool equipesExists(string id)
        {
            return db.equipes.Count(e => e.code == id) > 0;
        }
    }
}
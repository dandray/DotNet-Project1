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
    public class coureursController : ApiController
    {
        private ds3Entities db = new ds3Entities();

        // GET: api/coureurs
        public IQueryable<coureurs> Getcoureurs()
        {
            return db.coureurs;
        }

        // GET: api/coureurs/5
        [ResponseType(typeof(coureurs))]
        public IHttpActionResult Getcoureurs(decimal id)
        {
            coureurs coureurs = db.coureurs.Find(id);
            if (coureurs == null)
            {
                return NotFound();
            }

            return Ok(coureurs);
        }

        // PUT: api/coureurs/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Putcoureurs(decimal id, coureurs coureurs)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != coureurs.dossard)
            {
                return BadRequest();
            }

            db.Entry(coureurs).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!coureursExists(id))
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

        // POST: api/coureurs
        [ResponseType(typeof(coureurs))]
        public IHttpActionResult Postcoureurs(coureurs coureurs)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.coureurs.Add(coureurs);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (coureursExists(coureurs.dossard))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = coureurs.dossard }, coureurs);
        }

        // DELETE: api/coureurs/5
        [ResponseType(typeof(coureurs))]
        public IHttpActionResult Deletecoureurs(decimal id)
        {
            coureurs coureurs = db.coureurs.Find(id);
            if (coureurs == null)
            {
                return NotFound();
            }

            db.coureurs.Remove(coureurs);
            db.SaveChanges();

            return Ok(coureurs);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool coureursExists(decimal id)
        {
            return db.coureurs.Count(e => e.dossard == id) > 0;
        }
    }
}
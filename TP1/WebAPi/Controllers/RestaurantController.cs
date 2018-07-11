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
using WebAPi.Models;

namespace WebAPi.Controllers
{
    public class RestaurantController : ApiController
    {
        private TripAdvisorEntities db = new TripAdvisorEntities();

        // GET: api/Restaurant
        public IQueryable<T_E_RESTAURANT_RES> GetRestaurant()
        {
            return db.T_E_RESTAURANT_RES;
        }

        // GET: api/Restaurant/5
        [ResponseType(typeof(T_E_RESTAURANT_RES))]
        public IHttpActionResult GetRestaurant(int id)
        {
            T_E_RESTAURANT_RES t_E_RESTAURANT_RES = db.T_E_RESTAURANT_RES.Find(id);
            if (t_E_RESTAURANT_RES == null)
            {
                return NotFound();
            }

            return Ok(t_E_RESTAURANT_RES);
        }

        // GET : api/Restaurant?name=Name&exact=True
        public IHttpActionResult GetRestaurantByName(string name, Boolean exact)
        {
            if (exact) // exact name
            {
                return Ok(from r in db.T_E_RESTAURANT_RES
                          where r.RES_NOM == name
                          select r);
            }
            // else like
            return Ok(from r in db.T_E_RESTAURANT_RES
                      where r.RES_NOM.ToUpper().Contains(name.ToUpper())
                      select r);
        }

        // PUT: api/Restaurant/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutRestaurant(int id, T_E_RESTAURANT_RES t_E_RESTAURANT_RES)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != t_E_RESTAURANT_RES.RES_ID)
            {
                return BadRequest();
            }

            db.Entry(t_E_RESTAURANT_RES).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!T_E_RESTAURANT_RESExists(id))
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

        // POST: api/Restaurant
        [ResponseType(typeof(T_E_RESTAURANT_RES))]
        public IHttpActionResult PostRestaurant(T_E_RESTAURANT_RES t_E_RESTAURANT_RES)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.T_E_RESTAURANT_RES.Add(t_E_RESTAURANT_RES);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = t_E_RESTAURANT_RES.RES_ID }, t_E_RESTAURANT_RES);
        }

        // DELETE: api/Restaurant/5
        [ResponseType(typeof(T_E_RESTAURANT_RES))]
        public IHttpActionResult DeleteRestaurant(int id)
        {
            T_E_RESTAURANT_RES t_E_RESTAURANT_RES = db.T_E_RESTAURANT_RES.Find(id);
            if (t_E_RESTAURANT_RES == null)
            {
                return NotFound();
            }

            db.T_E_RESTAURANT_RES.Remove(t_E_RESTAURANT_RES);
            db.SaveChanges();

            return Ok(t_E_RESTAURANT_RES);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool T_E_RESTAURANT_RESExists(int id)
        {
            return db.T_E_RESTAURANT_RES.Count(e => e.RES_ID == id) > 0;
        }
    }
}
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
using WebServer.Models;

namespace WebServer.Controllers
{
    public class RanksController : ApiController
    {
        private UserContext db = new UserContext();
        
        /// <summary>
        /// return all the ranks
        /// </summary>
        /// <returns></returns>
        // GET: api/Ranks
        public IQueryable<Rank> GetRanks()
        {
            return db.Ranks;
        }

        /// <summary>
        /// get given rank by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: api/Ranks/5
        [ResponseType(typeof(Rank))]
        public IHttpActionResult GetRank(string id)
        {
            Rank rank = db.Ranks.Find(id);
            if (rank == null)
            {
                return NotFound();
            }

            return Ok(rank);
        }

        // put new rank
        // PUT: api/Ranks/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutRank(string id, Rank rank)
        {
            // check for bad arguments
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // if the given username don't match the rank username
            if (id != rank.Id)
            {
                return BadRequest();
            }

            db.Entry(rank).State = EntityState.Modified;

            // try to save change
            try
            {
                db.SaveChanges();
            }
            // if the save failed
            catch (DbUpdateConcurrencyException)
            {
                if (!RankExists(id))
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

        /// <summary>
        /// add new rank
        /// </summary>
        /// <param name="rank"></param>
        /// <returns></returns>
        // POST: api/Ranks
        [ResponseType(typeof(Rank))]
        public IHttpActionResult PostRank(Rank rank)
        {
            // check for bad arguments
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // add new rank
            db.Ranks.Add(rank);

            // try to save
            try
            {
                db.SaveChanges();
            }
            // catch if the save failed
            catch (DbUpdateException)
            {
                if (RankExists(rank.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = rank.Id }, rank);
        }

        /// <summary>
        /// delete given rank
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // DELETE: api/Ranks/5
        [ResponseType(typeof(Rank))]
        public IHttpActionResult DeleteRank(string id)
        {
            Rank rank = db.Ranks.Find(id);
            if (rank == null)
            {
                return NotFound();
            }

            // remove the rank and save
            db.Ranks.Remove(rank);
            db.SaveChanges();

            return Ok(rank);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// check if the rank is exist.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        private bool RankExists(string id)
        {
            return db.Ranks.Count(e => e.Id == id) > 0;
        }
    }
}
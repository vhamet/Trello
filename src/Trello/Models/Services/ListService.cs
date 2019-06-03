
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Trello.Models
{
    public class ListService
    {
        private TrelloDbContext db;

        public ListService(TrelloDbContext context)
        {
            db = context;
        }

        public List Create(List list) {
            try
            {
                db.tblList.Add(list);
                db.SaveChanges();
                
                return list;
            }
            catch (Exception e)
            {
                throw e;
                // return null;
            }
        }

        public bool UpdateTitle(List list)
        {
            try
            {
                var record = db.tblList.Find(list.ListId);
                if (record != null)
                {
                    record.Title = list.Title;
                    return db.SaveChanges() == 1;
                }

                return false;   
            }
            catch (Exception e)
            {
                throw e;
                // return false;
            }
        }

        public bool UpdatePosition(List list)
        {
            try
            {
                var record = db.tblList.Find(list.ListId);
                if (record != null)
                {
                    record.Position = list.Position;
                    return db.SaveChanges() == 1;
                }

                return false;   
            }
            catch (Exception e)
            {
                throw e;
                // return false;
            }
        }

        public bool Delete(List list)
        {
            try
            {
                var cards = db.tblCard.Where(c => c.ListId == list.ListId);
                foreach (var card in cards)
                    db.tblCard.Remove(card);

                db.tblList.Remove(list);
                db.SaveChanges();  

                return true;
            }
            catch (Exception e)
            {
                throw e;
                // return false;
            }
        }
    }
}
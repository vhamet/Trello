using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Trello.Models
{
    public class CardService
    {
        private TrelloDbContext db;

        public CardService(TrelloDbContext context)
        {
            db = context;
        }

        public Card Create(Card card) {
            try
            {
                db.tblCard.Add(card);
                db.SaveChanges();
                
                return card;
            }
            catch (Exception e)
            {
                throw e;
                // return null;
            }
        }


        public bool UpdatePosition(Card card)
        {
            try
            {
                var record = db.tblCard.Find(card.CardId);
                if (record != null)
                {
                    record.ListId = card.ListId;
                    record.Position = card.Position;
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

        public bool UpdateTitle(Card card)
        {
            try
            {
                var record = db.tblCard.Find(card.CardId);
                if (record != null)
                {
                    record.Title = card.Title;
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


        public bool Delete(Card card)
        {
            try
            {
                db.tblCard.Remove(card);
                return db.SaveChanges() == 1;
            }
            catch (Exception e)
            {
                throw e;
                // return false;
            }
        }
    }
}
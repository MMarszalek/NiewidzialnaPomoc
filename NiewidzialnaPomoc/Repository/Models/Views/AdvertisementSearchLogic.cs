using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Repository.Models.Views
{
    public class AdvertisementSearchLogic
    {
        private ApplicationDbContext Context;
        public AdvertisementSearchLogic()
        {
            Context = new ApplicationDbContext();
        }

        public IQueryable<Advertisement> GetAdvertisements(AdvertisementSearchModel searchModel)
        {
            var result = Context.Advertisements.AsQueryable();

            if(searchModel != null)
            {
                if(!string.IsNullOrEmpty(searchModel.TitleContent))
                {
                    result = result.Where(a => a.Title.Contains(searchModel.TitleContent)
                                       || a.Content.Contains(searchModel.TitleContent));
                }
                if(searchModel.LocationId.HasValue)
                {
                    result = result.Where(a => a.LocationId == searchModel.LocationId);
                }
            }

            return result;
        }
    }
}
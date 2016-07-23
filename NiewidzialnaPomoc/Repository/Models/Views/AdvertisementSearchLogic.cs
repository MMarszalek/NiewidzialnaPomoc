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

                if (searchModel.PostedCategories.CategoriesIds != null)
                {
                    foreach (var c in searchModel.PostedCategories.CategoriesIds)
                    {
                        result = result.Where(a => a.Categories.Any(x => x.Id.ToString().Equals(c)));
                    }
                }

                if (searchModel.PostedDifficulties.DifficultiesIds != null)
                {
                    foreach (var d in searchModel.PostedDifficulties.DifficultiesIds)
                    {
                        result = result.Where(a => a.DifficultyId.ToString().Equals(d));
                    }
                }

            }

            return result;
        }
    }
}
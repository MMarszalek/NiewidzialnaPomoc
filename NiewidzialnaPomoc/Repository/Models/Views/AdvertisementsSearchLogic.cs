using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Repository.Models.Views
{
    public class AdvertisementsSearchLogic
    {
        private ApplicationDbContext Context;
        public AdvertisementsSearchLogic()
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
                    result = result.Where(a => a.Categories.Select(f => 
                    f.Id.ToString()).Intersect(searchModel.PostedCategories.CategoriesIds).Any());
                }

                if (searchModel.PostedDifficulties.DifficultiesIds != null)
                {
                    result = result.Where(a => 
                    searchModel.PostedDifficulties.DifficultiesIds.Contains(a.DifficultyId.ToString()));
                }

            }

            return result;
        }
    }
}
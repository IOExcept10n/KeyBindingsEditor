using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyBindingsEditor.Configuration
{
    class CategoryManager
    {
        public List<GameplayCategory> Categories { get; set; }

        public CategoryManager(List<GameplayCategory> categories) 
        {
            Categories = categories;
        }

        public ActionInfo GetAction(string categoryName, string actionName)
        {
            var category = Categories.FirstOrDefault(x => x.Name == categoryName);
            if (category == null) return ActionInfo.Empty;
            return category.Actions.FirstOrDefault(x => x.Name == actionName);
        }
    }
}

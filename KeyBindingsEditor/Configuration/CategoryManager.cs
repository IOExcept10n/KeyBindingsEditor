using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyBindingsEditor.Configuration
{
    class CategoryManager
    {
        public IList<GameplayCategory> Categories { get; set; }

        public GameplayCategory? this[string? name]
        {
            get => Categories.FirstOrDefault(c => c.Name == name);
        }

        public CategoryManager(IList<GameplayCategory> categories) 
        {
            Categories = categories;
        }

        public ActionInfo? GetAction(string categoryName, string actionName)
        {
            var category = Categories.FirstOrDefault(x => x.Name == categoryName);
            if (category == null) return null;
            return category.Actions.FirstOrDefault(x => x.Name == actionName);
        }

        public ActionInfo? GetAction(string combinedName)
        {
            string[] parts = combinedName.Split('.');
            if (parts.Length == 2)
            {
                return GetAction(parts[0], parts[1]);
            }
            throw new ArgumentException("The category name should not contain points in the name, they're used only for the 'Category.Action' combination.");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WSIP.RMS
{
    public class RecipeEntity
    {
        /// <summary>
        /// Which machine create or use this recipe.
        /// </summary>
        public string MachineID = "";

        /// <summary>
        /// Unique tag name for this recipe.
        /// </summary>
        public string TAG = "";

        /// <summary>
        /// Recipe source from
        /// "Unknown", "Server", "Machine", "UserDefine"
        /// </summary>
        public RecipeSourceEnum RecipeFrom { get; set; } = RecipeSourceEnum.Unknown;

        /// <summary>
        /// Version from source if it has.
        /// </summary>
        public string RecipeVersion { get; set; } = "";

        /// <summary>
        /// Recipe content. The content format depends on cusotmer requirement.
        /// </summary>
        public string RecipeContent { get; set; } = "";

        /// <summary>
        /// Recipe description.
        /// </summary>
        public string RecipeDescription { get; set; } = "";
    }
}

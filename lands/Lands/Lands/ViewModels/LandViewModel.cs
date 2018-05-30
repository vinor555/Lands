using Lands.Models;

namespace Lands.ViewModels
{
    public class LandViewModel
    {
        #region Properties
        public Land Land
        {
            get;
            set;
        }
        #endregion

        #region Constructors
        public LandViewModel(Land land)
        {
            this.Land = land;
        } 
        #endregion
    }
}

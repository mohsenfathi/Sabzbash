
namespace Shahrdari.Enums
{
    public class E_FTPRoutes
    {
        public E_FTPRoutes(string RootPatch)
        {
            ROOT = RootPatch;
        }
        public string ROOT { get; set; }
        public string USERS { get { return ROOT + "/Areas/Admin/Images/Profile/"; } }
        public string SERVICE_REQUEST_ITEMS { get { return ROOT + "/Areas/Admin/Images/Categories/xxxhdpi/"; } }
        public string CATEGORIES { get { return ROOT + "/Areas/Admin/Images/Categories/xxxhdpi/"; } }
        public string PERSONELS { get { return ROOT + "/Areas/Admin/Images/Personels/"; } }
        public string LEARNS { get { return ROOT + "/Areas/Admin/Images/Learns/"; } }
        public string CAR_INFO { get { return ROOT + "/Areas/Admin/Images/CarInfo/"; } }
        public string BOOTH_INFO { get { return ROOT + "/Areas/Admin/Images/BoothInfo/"; } }
        public string NEWS { get { return ROOT + "/Areas/Admin/Images/News/"; } }
    }
}

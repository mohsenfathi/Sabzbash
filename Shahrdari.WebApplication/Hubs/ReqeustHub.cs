using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Shahrdari.ViewModels;
using Shahrdari.BussinessLayer;
using Shahrdari.Models;
using Shahrdari.Models.Loging;
using Shahrdari.Enums.Loging;
using Shahrdari.Enums;
using Shahrdari.Logging;
using Shahrdari.WebApplication.WebApis;

namespace Shahrdari.WebApplication
{
    [HubName("Reqeust")]
    public class ReqeustHub : Hub
    {
        public static volatile List<V_Personels> Personels = new List<V_Personels>();
        public static volatile List<V_Users> Users = new List<V_Users>();

        public override Task OnConnected()
        {
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            var personel = Personels.Where(c => c.ConnectionId == Context.ConnectionId).FirstOrDefault();
            if (personel != null)
                Personels.Remove(personel);
            var user = Users.Where(c => c.ConnectionId == Context.ConnectionId).FirstOrDefault();
            if (user != null)
                Users.Remove(user);
            return base.OnDisconnected(stopCalled);
        }
        
        public M_Personels LoginPersonel(string Username, string Password)
        {
            var personel = new B_Personels().GetPersonelByUserName(Username, Password);
            if (personel == null)
                return null;
            var finalPersonel = B_PublicFunctions.GenericMaper<M_Personels, V_Personels>(new List<M_Personels> { personel }).FirstOrDefault();
            finalPersonel.ConnectionId = Context.ConnectionId;
            Personels.Add(finalPersonel);

            return personel;
        }
        
        public bool LoginUser(int Id, string UnicKey)
        {
            var users = new B_Users().GetUsers(Id, UnicKey);
            if (users == null)
                return false;
            var finalUser = B_PublicFunctions.GenericMaper<M_Users, V_Users>(new List<M_Users> { users }).FirstOrDefault();
            finalUser.ConnectionId = Context.ConnectionId;
            Users.Add(finalUser);
            return true;
        }
        
        public bool SendRequestForBooth(object Request, int PersonelId)
        {
            try
            {
                var personel = Personels.Where(c => c.Id == PersonelId).ToList();
                if (personel.Count == 0)
                {
                    // Send SMS
                }
                else
                {
                    Clients.Clients(personel.Select(c=>c.ConnectionId).ToList()).AsignedRequest(Request);
                }
                return true;
            }
            catch (Exception ex)
            {
                M_SystemLog exx = new M_SystemLog(E_SystemType.SIGNALR_HUB, E_LogType.ERROR, ex);
                if (ex.Source == E_LogType.SYSTEM_ERROR.ToString())
                    exx.LogType = E_LogType.SYSTEM_ERROR;
                L_Log.SubmitLog(exx);
                return false;
            }
        }

        public bool RejectRequest(int RequestId, int UserId)
        {
            try
            {
                var user = Users.Where(c => c.Id == UserId).ToList();
                if (user.Count == 0)
                    return false;
                foreach (var li in user)
                    Clients.Client(li.ConnectionId).RejectRequestForUser(RequestId);
                return true;
            }
            catch (Exception ex)
            {
                M_SystemLog exx = new M_SystemLog(E_SystemType.SIGNALR_HUB, E_LogType.ERROR, ex);
                if (ex.Source == E_LogType.SYSTEM_ERROR.ToString())
                    exx.LogType = E_LogType.SYSTEM_ERROR;
                L_Log.SubmitLog(exx);
                return false;
            }
        }

        public bool NewRequest(object Request, double Lat, double lng)
        {
            try
            {
                var nearDrivers = new B_Location().GetNearDrivers(Lat, lng);
                var driverIds = nearDrivers.Select(x => x.PersonelId).ToList();
                var nearPersonels = Personels.Where(c => driverIds.Contains(c.Id)).ToList();
                Clients.Clients(nearPersonels.Select(c => c.ConnectionId).ToList()).NewRequestCar(Request);
                return true;
            }
            catch (Exception ex)
            {
                M_SystemLog exx = new M_SystemLog(E_SystemType.SIGNALR_HUB, E_LogType.ERROR, ex);
                if (ex.Source == E_LogType.SYSTEM_ERROR.ToString())
                    exx.LogType = E_LogType.SYSTEM_ERROR;
                L_Log.SubmitLog(exx);
                return false;
            }
        }

        public bool UpdateLocation(double Lat, double Lng, double Bearing)
        {
            try
            {
                var personel = Personels.Where(c => c.ConnectionId == Context.ConnectionId).FirstOrDefault();
                if (personel == null)
                    return false;
                new B_Location().Add(new M_Location
                {
                    Bearing = Bearing,
                    Date = DateTime.Now,
                    IsOld = false,
                    Lat = Lat,
                    Lng = Lng,
                    PersonelId = personel.Id
                });
                return true;
            }
            catch (Exception ex)
            {
                M_SystemLog exx = new M_SystemLog(E_SystemType.SIGNALR_HUB, E_LogType.ERROR, ex);
                if (ex.Source == E_LogType.SYSTEM_ERROR.ToString())
                    exx.LogType = E_LogType.SYSTEM_ERROR;
                L_Log.SubmitLog(exx);
                return false;
            }
        }

        public bool RequestChangedStatus(object Request, int UserId)
        {
            try
            {
                var clientRecipment = Users.Where(c => c.Id == UserId).ToList();
                Clients.Clients(clientRecipment.Select(c => c.ConnectionId).ToList()).RequestStatusChanged(Request);
                return true;
            }
            catch (Exception ex)
            {
                M_SystemLog exx = new M_SystemLog(E_SystemType.SIGNALR_HUB, E_LogType.ERROR, ex);
                if (ex.Source == E_LogType.SYSTEM_ERROR.ToString())
                    exx.LogType = E_LogType.SYSTEM_ERROR;
                L_Log.SubmitLog(exx);
                return false;
            }
        }


        public bool AcceptRequestByCar(object Request,int UserId)
        {
            try
            {
                var clientRecipment = Users.Where(c => c.Id == UserId).ToList();
                Clients.Clients(clientRecipment.Select(c => c.ConnectionId).ToList()).AcceptRequestByCar(Request);
                return true;
            }
            catch (Exception ex)
            {
                M_SystemLog exx = new M_SystemLog(E_SystemType.SIGNALR_HUB, E_LogType.ERROR, ex);
                if (ex.Source == E_LogType.SYSTEM_ERROR.ToString())
                    exx.LogType = E_LogType.SYSTEM_ERROR;
                L_Log.SubmitLog(exx);
                return false;
            }
        }

        public object LocationDriver(int RequestId)
        {
            try
            {
                var request = new B_ServicesRequests().GetServicesRequests(RequestId);
                if (request == null)
                    return false;
                var clientRecipment = Users.Where(c => c.Id == request.UserId).ToList();
                if (clientRecipment == null)
                    return false;
                var cController = new ServicesRequestsController();
                return cController.getRequestDetail(request.Id, request.UserId);
            }
            catch (Exception ex)
            {
                M_SystemLog exx = new M_SystemLog(E_SystemType.SIGNALR_HUB, E_LogType.ERROR, ex);
                if (ex.Source == E_LogType.SYSTEM_ERROR.ToString())
                    exx.LogType = E_LogType.SYSTEM_ERROR;
                L_Log.SubmitLog(exx);
                return false;
            }
        }

        public object FinishRequestPersonel(object Request, int UserId)
        {
            try
            {
                var clientRecipment = Users.Where(c => c.Id == UserId).ToList();
                Clients.Clients(clientRecipment.Select(c => c.ConnectionId).ToList()).FinishRequestPersonel(Request);
                return true;
            }
            catch (Exception ex)
            {
                M_SystemLog exx = new M_SystemLog(E_SystemType.SIGNALR_HUB, E_LogType.ERROR, ex);
                if (ex.Source == E_LogType.SYSTEM_ERROR.ToString())
                    exx.LogType = E_LogType.SYSTEM_ERROR;
                L_Log.SubmitLog(exx);
                return false;
            }
        }

        #region New Methods

        public object NewRequestDriver(object Request)
        {
            try
            {
                Clients.Clients(Personels.Where(c=>c.PersonelType == E_PublicCategory.PERSONEL_TYPE.DRIVER).Select(c => c.ConnectionId).ToList()).NewRequestCar(Request);
                return Request;
            }
            catch (Exception ex)
            {
                M_SystemLog exx = new M_SystemLog(E_SystemType.SIGNALR_HUB, E_LogType.ERROR, ex);
                if (ex.Source == E_LogType.SYSTEM_ERROR.ToString())
                    exx.LogType = E_LogType.SYSTEM_ERROR;
                L_Log.SubmitLog(exx);
                return false;
            }
        }

        public object NewRequestStation(object Request,int PersonelId)
        {
            try
            {
                var cids = Personels.Where(c => c.Id == PersonelId).Select(c => c.ConnectionId).ToList();
                Clients.Clients(cids).NewRequestCar(Request);
                return Request;
            }
            catch (Exception ex)
            {
                M_SystemLog exx = new M_SystemLog(E_SystemType.SIGNALR_HUB, E_LogType.ERROR, ex);
                if (ex.Source == E_LogType.SYSTEM_ERROR.ToString())
                    exx.LogType = E_LogType.SYSTEM_ERROR;
                L_Log.SubmitLog(exx);
                return false;
            }
        }

        public bool RequestChangedStatusPersonel(object Request, int PersonelId)
        {
            try
            {
                var clientRecipment = Personels.Where(c => c.Id == PersonelId).ToList();
                Clients.Clients(clientRecipment.Select(c => c.ConnectionId).ToList()).RequestChangedStatusPersonel(Request);
                return true;
            }
            catch (Exception ex)
            {
                M_SystemLog exx = new M_SystemLog(E_SystemType.SIGNALR_HUB, E_LogType.ERROR, ex);
                if (ex.Source == E_LogType.SYSTEM_ERROR.ToString())
                    exx.LogType = E_LogType.SYSTEM_ERROR;
                L_Log.SubmitLog(exx);
                return false;
            }
        }

        #endregion New Methods
    }
}
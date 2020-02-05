using Microsoft.AspNet.SignalR.Client;
using Shahrdari.BussinessLayer;
using Shahrdari.Enums;
using Shahrdari.Enums.Loging;
using Shahrdari.Factory.Log;
//using Shahrdari.Enums.SignalR;
using Shahrdari.Logging;
using Shahrdari.Models;
using Shahrdari.Models.Loging;
using Shahrdari.Settings;
using Shahrdari.ViewModels;
using Shahrdari.WebApplication.Classes.Enums;
//using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Shahrdari.WebApplication.WebApis
{
    public class ServicesRequestsController : BaseController
    {
        [HttpPost]
        public IHttpActionResult AddServicesRequests([FromBody] AddModel Datas)
        {
            try
            {
                int userId = validateUser();
                var bServicesRequests = new B_ServicesRequests();
                Datas.ServicesRequests = bServicesRequests.Add(Datas.ServicesRequests);
                return Json(PrepareSuccessResult(Datas.ServicesRequests));
            }
            catch (Exception ex)
            {
                M_SystemLog exx = new M_SystemLog(E_SystemType.SHAHRDARI_WEB_APPLICATION, E_LogType.ERROR, ex);
                if (ex.Source == E_LogType.SYSTEM_ERROR.ToString())
                    exx.LogType = E_LogType.SYSTEM_ERROR;
                L_Log.SubmitLog(exx);
                if (ex.Source != E_LogType.SYSTEM_ERROR.ToString())
                    return Json(PrepareFaildResult("خطایی در سرور وجود دارد", (int)E_ErrorCodes.SERVER_ERROR + S_Seprators.ErrorFieldNameSeprator.ToString() + ""));
                return Json(PrepareFaildResult(exx.LogMessage, ex.HelpLink));
            }
        }

        [HttpPost]
        public IHttpActionResult GetServicesRequestsByUserId([FromBody] GetServicesRequestsByUserIdModel Datas)
        {
            try
            {
                int userId = validateUser();
                var resultRequests = B_PublicFunctions.GenericMaper<M_ServicesRequests, V_ServicesRequests>(new B_ServicesRequests().GetServicesRequests(userId, Datas.IgnoreIds));
                return Json(PrepareSuccessResult(resultRequests));
            }
            catch (Exception ex)
            {
                M_SystemLog exx = new M_SystemLog(E_SystemType.SHAHRDARI_WEB_APPLICATION, E_LogType.ERROR, ex);
                if (ex.Source == E_LogType.SYSTEM_ERROR.ToString())
                    exx.LogType = E_LogType.SYSTEM_ERROR;
                L_Log.SubmitLog(exx);
                if (ex.Source != E_LogType.SYSTEM_ERROR.ToString())
                    return Json(PrepareFaildResult("خطایی در سرور وجود دارد", (int)E_ErrorCodes.SERVER_ERROR + S_Seprators.ErrorFieldNameSeprator.ToString() + ""));
                return Json(PrepareFaildResult(exx.LogMessage, ex.HelpLink));
            }
        }

        [HttpPost]
        public IHttpActionResult GetServicesRequests([FromBody] GetServicesRequestsModel Datas)
        {
            try
            {
                var validateResult = validateUserOrPersonel();
                int userId = validateResult.Item1;
                Tuple<int, List<M_ServicesRequests>> resultRequests;
                if (validateResult.Item2 == MFValidationUserRole.USER)
                    resultRequests = new B_ServicesRequests().GetServicesRequests(userId, Datas.Status, Datas.PageNumber, Datas.PageCapacity);
                else
                    resultRequests = new B_ServicesRequests().GetServicesRequestsForPersonel(userId, Datas.Status, Datas.PageNumber, Datas.PageCapacity);
                var tmpResult = new List<object>();
                foreach (var li in resultRequests.Item2)
                {
                    tmpResult.Add(new
                    {
                        Id = li.Id,
                        Description = li.Description,
                        Address = li.Address,
                        Plaque = li.PlaqueNumber,
                        Unit = li.UnitNumber,
                        CreationTime = TimeSpan.FromTicks(li.CreateDate.Ticks).TotalMilliseconds,
                        CreationTimeStr = li.CreateDate.ConvertMiladiToShamsi(),
                        ReferenceTime = TimeSpan.FromTicks(li.ReferralDate.Ticks).TotalMilliseconds,
                        ReferenceTimeStr = li.ReferralDate.ConvertMiladiToShamsi(),
                        State = (int)li.Status
                    });
                }
                var result = new
                {
                    Count = resultRequests.Item1,
                    Items = tmpResult
                };
                return Json(PrepareSuccessResult(result));
            }
            catch (Exception ex)
            {
                M_SystemLog exx = new M_SystemLog(E_SystemType.SHAHRDARI_WEB_APPLICATION, E_LogType.ERROR, ex);
                if (ex.Source == E_LogType.SYSTEM_ERROR.ToString())
                    exx.LogType = E_LogType.SYSTEM_ERROR;
                L_Log.SubmitLog(exx);
                if (ex.Source != E_LogType.SYSTEM_ERROR.ToString())
                    return Json(PrepareFaildResult("خطایی در سرور وجود دارد", (int)E_ErrorCodes.SERVER_ERROR + S_Seprators.ErrorFieldNameSeprator.ToString() + ""));
                return Json(PrepareFaildResult(exx.LogMessage, ex.HelpLink));
            }
        }

        [HttpPost]
        public IHttpActionResult GetServicesRequestsDetails([FromBody] GetServicesRequestsDetailsModel Datas)
        {
            try
            {
                var validateResult = validateUserOrPersonel();
                int userId = validateResult.Item1;
                var user = new B_Users().GetUsers(userId);
                var resultRequests = new B_ServicesRequests().GetServicesRequests(Datas.Id, userId);
                if (resultRequests == null)
                    resultRequests = new B_ServicesRequests().GetServicesRequestsForPersonel(Datas.Id, userId);
                //var factor = new B_Factors().GetRequestFactors(resultRequests.Id, resultRequests.UserId);
                M_Personels personel = null;
                if (resultRequests.PersonelId.HasValue)
                    personel = new B_Personels().GetPersonelById(resultRequests.PersonelId.Value);
                var factorITems = new List<object>();
                /*if (factor != null)
                {
                    var tariffs = new B_Tariffs().GetTariffs(factor.FactorItems.Where(c => c.ItemType == E_PublicCategory.FACTOR_ITEM_TYPE.SERVICE_TARIFF).Select(c => c.ItemId).ToList(), true);
                    var shops = new B_Products().GetProducts(factor.FactorItems.Where(c => c.ItemType == E_PublicCategory.FACTOR_ITEM_TYPE.SHOP_ITEM).Select(c => c.ItemId).ToList());
                    foreach (var li in factor.FactorItems)
                    {
                        if(li.ItemType == E_PublicCategory.FACTOR_ITEM_TYPE.SHOP_ITEM)
                        {
                            var item = shops.Where(c => c.Id == li.ItemId).FirstOrDefault();
                            factorITems.Add(new {
                                Title = item.Title,
                                Price = item.Price,
                                Id = item.Id,
                                Type = E_PublicCategory.FACTOR_ITEM_TYPE.SHOP_ITEM,
                                Count = li.Count
                            });
                        }
                        else
                        {
                            var item = tariffs.Where(c => c.Id == li.ItemId).FirstOrDefault();
                            factorITems.Add(new
                            {
                                Title = item.Title,
                                Price = item.Price,
                                Id = item.Id,
                                Type = E_PublicCategory.FACTOR_ITEM_TYPE.SERVICE_TARIFF,
                                Count = li.Count
                            });
                        }
                    }
                }*/

                //var category = new B_ServicesCategories().GetServicesCategories(resultRequests.ServicesCategoryId);

                var result = new
                {
                    Address = resultRequests.Address,
                    CreateDate = TimeSpan.FromTicks(resultRequests.CreateDate.Ticks).TotalMilliseconds,
                    CreateDateStr = resultRequests.CreateDate.ConvertMiladiToShamsi(),
                    Description = resultRequests.Description,
                    //Title = category.Title,
                    //Icom = category.ImageName,
                    FinishComment = resultRequests.FinishComment,
                    FinishDate = resultRequests.FinishDate.HasValue ? TimeSpan.FromTicks(resultRequests.FinishDate.Value.Ticks).TotalMilliseconds : 0,
                    FinishDateStr = resultRequests.FinishDate.HasValue ? resultRequests.FinishDate.Value.ConvertMiladiToShamsi() : null,
                    GeographicalCoordinates = resultRequests.GeographicalCoordinates,
                    Id = resultRequests.Id,
                    ImmediatelyRequest = resultRequests.ImmediatelyRequest,
                    PersonelId = resultRequests.PersonelId,
                    PlaqueNumber = resultRequests.PlaqueNumber,
                    Rate = resultRequests.Rate,
                    ReferralDate = TimeSpan.FromTicks(resultRequests.ReferralDate.Ticks).TotalMilliseconds,
                    ReferralDateStr = resultRequests.ReferralDate.ConvertMiladiToShamsi(),
                    //ServicesCategoryId = resultRequests.ServicesCategoryId,
                    Status = resultRequests.Status,
                    ToDoDate = resultRequests.ToDoDate.HasValue ? TimeSpan.FromTicks(resultRequests.ToDoDate.Value.Ticks).TotalMilliseconds : 0,
                    ToDoDateStr = resultRequests.ToDoDate.HasValue ? resultRequests.ToDoDate.Value.ConvertMiladiToShamsi() : null,
                    UnitNumber = resultRequests.UnitNumber,
                    UpdateDate = resultRequests.UpdateDate.HasValue ? TimeSpan.FromTicks(resultRequests.UpdateDate.Value.Ticks).TotalMilliseconds : 0,
                    UpdateDateStr = resultRequests.UpdateDate.HasValue ? resultRequests.UpdateDate.Value.ConvertMiladiToShamsi() : null,
                    UserId = resultRequests.UserId,
                    User = new
                    {
                        BirthDate = user.BirthDate.HasValue ? TimeSpan.FromTicks(user.BirthDate.Value.Ticks).TotalMilliseconds : 0,
                        BirthDateStr = user.BirthDate.HasValue ? user.BirthDate.Value.ConvertMiladiToShamsi() : null,
                        Email = user.Email,
                        FirstName = user.FirstName,
                        Gender = user.Gender,
                        Id = user.Id,
                        ImageName = user.ImageName,
                        LastName = user.LastName,
                        MobileNumber = user.MobileNumber,
                    },
                    Personel = personel == null ? null : new
                    {
                        BirthDate = TimeSpan.FromTicks(personel.BirthDate.Ticks).TotalMilliseconds,
                        BirthDateStr = personel.BirthDate.ConvertMiladiToShamsi(),
                        FirstName = personel.FirstName,
                        LastName = personel.LastName,
                        Gender = personel.Gender,
                        Id = personel.Id,
                        ImageName = personel.ImageName,
                        LastOnline = TimeSpan.FromTicks(personel.LastOnline.Ticks).TotalMilliseconds,
                        LastOnlineStr = personel.LastOnline.ConvertMiladiToShamsi(),
                        MobileNumber = personel.MobileNumber
                    },
                    /*Factor = factor == null ? null : new {
                        FactorITems = factorITems,
                        PaidDate = factor.PaiedDate.HasValue ? TimeSpan.FromTicks(factor.PaiedDate.Value.Ticks).TotalMilliseconds : 0,
                        PaidDateStr = factor.PaiedDate.HasValue ? factor.PaiedDate.Value.ConvertMiladiToShamsi() : null,
                        IsPaid = factor.Status == E_PublicCategory.FACTOR_STATUS.PAIED ? true : false,
                        TotalPrice = factor.TotalPrice,
                        FinalPrice = factor.FinalPrice,
                        Discount = factor.Discount == null ? null : new {
                            Code = factor.Discount.Code,
                            Maximum = factor.Discount.Maximum,
                            Percent = factor.Discount.Percent,
                            Title = factor.Discount.Title
                        }
                    }*/
                };
                return Json(PrepareSuccessResult(result));
            }
            catch (Exception ex)
            {
                M_SystemLog exx = new M_SystemLog(E_SystemType.SHAHRDARI_WEB_APPLICATION, E_LogType.ERROR, ex);
                if (ex.Source == E_LogType.SYSTEM_ERROR.ToString())
                    exx.LogType = E_LogType.SYSTEM_ERROR;
                L_Log.SubmitLog(exx);
                if (ex.Source != E_LogType.SYSTEM_ERROR.ToString())
                    return Json(PrepareFaildResult("خطایی در سرور وجود دارد", (int)E_ErrorCodes.SERVER_ERROR + S_Seprators.ErrorFieldNameSeprator.ToString() + ""));
                return Json(PrepareFaildResult(exx.LogMessage, ex.HelpLink));
            }
        }

        [HttpPost]
        public IHttpActionResult EditServicesRequests([FromBody] AddModel Datas)
        {
            try
            {
                int userId = validateUser();
                var bServicesRequests = new B_ServicesRequests();
                bServicesRequests.Edit(Datas.ServicesRequests, true);
                return Json(PrepareSuccessResult(true));
            }
            catch (Exception ex)
            {
                M_SystemLog exx = new M_SystemLog(E_SystemType.SHAHRDARI_WEB_APPLICATION, E_LogType.ERROR, ex);
                if (ex.Source == E_LogType.SYSTEM_ERROR.ToString())
                    exx.LogType = E_LogType.SYSTEM_ERROR;
                L_Log.SubmitLog(exx);
                if (ex.Source != E_LogType.SYSTEM_ERROR.ToString())
                    return Json(PrepareFaildResult("خطایی در سرور وجود دارد", (int)E_ErrorCodes.SERVER_ERROR + S_Seprators.ErrorFieldNameSeprator.ToString() + ""));
                return Json(PrepareFaildResult(exx.LogMessage, ex.HelpLink));
            }
        }

        [HttpPost]
        public IHttpActionResult DeleteRequests([FromBody] DeleteModel Datas)
        {
            try
            {
                int userId = validateUser();
                new B_ServicesRequests().Delete(Datas.ServiceRequestId, userId, true);
                return Json(PrepareSuccessResult(true));
            }
            catch (Exception ex)
            {
                M_SystemLog exx = new M_SystemLog(E_SystemType.SHAHRDARI_WEB_APPLICATION, E_LogType.ERROR, ex);
                if (ex.Source == E_LogType.SYSTEM_ERROR.ToString())
                    exx.LogType = E_LogType.SYSTEM_ERROR;
                L_Log.SubmitLog(exx);
                if (ex.Source != E_LogType.SYSTEM_ERROR.ToString())
                    return Json(PrepareFaildResult("خطایی در سرور وجود دارد", (int)E_ErrorCodes.SERVER_ERROR + S_Seprators.ErrorFieldNameSeprator.ToString() + ""));
                return Json(PrepareFaildResult(exx.LogMessage, ex.HelpLink));
            }
        }

        [HttpPost]
        public IHttpActionResult CloseRequest([FromBody] CloseRequetsModel Datas)
        {
            try
            {
                int userId = validateUser();
                new B_ServicesRequests().CloseRequest(Datas.Comment, Datas.Rate, Datas.ServiceRequestId, userId);
                return Json(PrepareSuccessResult(true));
            }
            catch (Exception ex)
            {
                M_SystemLog exx = new M_SystemLog(E_SystemType.SHAHRDARI_WEB_APPLICATION, E_LogType.ERROR, ex);
                if (ex.Source == E_LogType.SYSTEM_ERROR.ToString())
                    exx.LogType = E_LogType.SYSTEM_ERROR;
                L_Log.SubmitLog(exx);
                if (ex.Source != E_LogType.SYSTEM_ERROR.ToString())
                    return Json(PrepareFaildResult("خطایی در سرور وجود دارد", (int)E_ErrorCodes.SERVER_ERROR + S_Seprators.ErrorFieldNameSeprator.ToString() + ""));
                return Json(PrepareFaildResult(exx.LogMessage, ex.HelpLink));
            }
        }

        [HttpPost]
        public IHttpActionResult ChangeStatus([FromBody] ChangeStatusModel Datas)
        {
            try
            {
                int userId = validateUser();
                new B_ServicesRequests().ChangeStatus(Datas.RequestId, userId, Datas.Status);
                return Json(PrepareSuccessResult(true));
            }
            catch (Exception ex)
            {
                M_SystemLog exx = new M_SystemLog(E_SystemType.SHAHRDARI_WEB_APPLICATION, E_LogType.ERROR, ex);
                if (ex.Source == E_LogType.SYSTEM_ERROR.ToString())
                    exx.LogType = E_LogType.SYSTEM_ERROR;
                L_Log.SubmitLog(exx);
                if (ex.Source != E_LogType.SYSTEM_ERROR.ToString())
                    return Json(PrepareFaildResult("خطایی در سرور وجود دارد", (int)E_ErrorCodes.SERVER_ERROR + S_Seprators.ErrorFieldNameSeprator.ToString() + ""));
                return Json(PrepareFaildResult(exx.LogMessage, ex.HelpLink));
            }
        }

        public IHttpActionResult Test()
        {
            string input = Request.Headers.GetValues("Cookies").FirstOrDefault();
            var exx = new M_SystemLog(E_SystemType.SHAHRDARI_WEB_APPLICATION, E_LogType.CUSTOM_LOG);
            exx.LogMessage = input;
            L_Log.SubmitLog(exx);
            return Json(input);
        }

        #region Khalaj Changes

        [HttpGet]
        public IHttpActionResult time(string FromDate, int Day)
        {
            try
            {
                validateUser();
                var date1 = DateTime.Now;
                var bDate = new B_Pouriya_Date();
                List<object> obj = new List<object>();
                for (int i = 0; i < Day; i++)
                {
                    if (i != 0)
                        date1 = date1.AddDays(1);
                    var date = bDate.GetDate(date1);
                    if (date == null)
                        date = bDate.Add(date1);
                    obj.Add(new
                    {
                        Title = date1.ConvertToPesianDateName(),
                        Id = date.Id,
                        Hours = new List<object> {
                        new { Hour = "8 تا 11",  Id = (int)E_PublicCategory.SERVICE_TIME.Time_8_11, IsActive = isPast(date1.AddDays(i),E_PublicCategory.SERVICE_TIME.Time_8_11) },
                        new { Hour = "11 تا 14", Id = (int)E_PublicCategory.SERVICE_TIME.Time_11_14, IsActive = isPast(date1.AddDays(i),E_PublicCategory.SERVICE_TIME.Time_11_14) },
                        new { Hour = "14 تا 17", Id = (int)E_PublicCategory.SERVICE_TIME.Time_14_17, IsActive = isPast(date1.AddDays(i),E_PublicCategory.SERVICE_TIME.Time_14_17) },
                        new { Hour = "17 تا 20", Id = (int)E_PublicCategory.SERVICE_TIME.Time_17_20, IsActive = isPast(date1.AddDays(i),E_PublicCategory.SERVICE_TIME.Time_17_20) },
                        new { Hour = "20 تا 22", Id = (int)E_PublicCategory.SERVICE_TIME.Time_20_22, IsActive =  isPast(date1.AddDays(i),E_PublicCategory.SERVICE_TIME.Time_20_22) }
                    }
                    });
                }
                return Json(PrepareSuccessResult(obj));
            }
            catch (Exception ex)
            {
                M_SystemLog exx = new M_SystemLog(E_SystemType.SHAHRDARI_WEB_APPLICATION, E_LogType.ERROR, ex);
                if (ex.Source == E_LogType.SYSTEM_ERROR.ToString())
                    exx.LogType = E_LogType.SYSTEM_ERROR;
                L_Log.SubmitLog(exx);
                if (ex.Source != E_LogType.SYSTEM_ERROR.ToString())
                    return Json(PrepareFaildResult("خطایی در سرور وجود دارد", (int)E_ErrorCodes.SERVER_ERROR + S_Seprators.ErrorFieldNameSeprator.ToString() + ""));
                return Json(PrepareFaildResult(exx.LogMessage, ex.HelpLink));
            }
        }

        [HttpPost]
        public IHttpActionResult request([FromBody] RequestAdd model)
        {
            try
            {
                int userId = validateUser();
                var bServicesRequests = new B_ServicesRequests();
                var request = new M_ServicesRequests
                {
                    Address = model.Address.Address,
                    Description = model.Description,
                    GeographicalCoordinates = model.Address.Lat + "," + model.Address.Lng,
                    PlaqueNumber = model.Address.Plaque,
                    Pouriya_DateId = model.DayId,
                    Pouriya_TimeId = model.TimeId,
                    Pouriya_Type = model.Type,
                    Status = model.Type == "BOOTH" ? E_PublicCategory.REQUEST_STATUS.WAIT_FOR_GET : E_PublicCategory.REQUEST_STATUS.NEW_REQUEST,
                    UnitNumber = model.Address.Unit,
                    UserId = userId,
                    PersonelId = model.Type == "BOOTH" ? new B_Booth().GetBoothes(model.BoothId).PersonelId : (int?)null
                };
                request = bServicesRequests.Add(request);
                var bItem = new B_ServicesRequestItems();
                if (model.Items != null && model.Items.Count > 0)
                    foreach (var li in model.Items)
                    {
                        var category = new B_ServicesCategories().GetServicesCategories(li.Id);
                        if (category != null)
                            bItem.Add(new M_ServicesRequestItems
                            {
                                CreateDate = DateTime.Now,
                                RequestId = request.Id,
                                UserType = E_PublicCategory.SYSTEM_USER_TYPE.NORMAL_USER,
                                Value = li.Value,
                                ImageName = category.ImageName,
                                ScorePerUnit = category.ScorePerUnit,
                                Title = category.Title,
                                Unit = category.Unit,
                                IsFailed = true,
                                UserId = userId,
                                CategoryId = category.Id
                            });
                    }
                if (model.Type == "BOOTH")
                {
                    try
                    {
                        var hubConnection = new HubConnection(url: SignalRUrl);
                        var chat = hubConnection.CreateHubProxy(hubName: "Reqeust");

                        hubConnection.Start().Wait();

                        chat.Invoke<bool>("SendRequestForBooth", getRequestDetail(request.Id, userId), request.PersonelId);
                    }
                    catch { }
                }
                if (model.Type == "CAR")
                {
                    try
                    {
                        var hubConnection = new HubConnection(url: SignalRUrl);
                        var chat = hubConnection.CreateHubProxy(hubName: "Reqeust");

                        hubConnection.Start().Wait();

                        chat.Invoke<bool>("NewRequest", getRequestDetail(request.Id, userId), model.Address.Lat, model.Address.Lng);
                    }
                    catch { }
                }
                return Json(PrepareSuccessResult(getRequestDetail(request.Id, userId)));
            }
            catch (Exception ex)
            {
                M_SystemLog exx = new M_SystemLog(E_SystemType.SHAHRDARI_WEB_APPLICATION, E_LogType.ERROR, ex);
                if (ex.Source == E_LogType.SYSTEM_ERROR.ToString())
                    exx.LogType = E_LogType.SYSTEM_ERROR;
                L_Log.SubmitLog(exx);
                if (ex.Source != E_LogType.SYSTEM_ERROR.ToString())
                    return Json(PrepareFaildResult("خطایی در سرور وجود دارد", (int)E_ErrorCodes.SERVER_ERROR + S_Seprators.ErrorFieldNameSeprator.ToString() + ""));
                return Json(PrepareFaildResult(exx.LogMessage, ex.HelpLink));
            }
        }

        [HttpGet]
        public IHttpActionResult GetRequest()
        {
            try
            {
                int userId = validateUser();
                var bServicesRequests = new B_ServicesRequests();
                var bItem = new B_ServicesRequestItems();
                var requests = bServicesRequests.GetServicesRequests(userId, null);
                var res = new List<object>();
                foreach (var li in requests)
                {
                    var itemsScore = bItem.GetItemsPoint(li.Id);
                    res.Add(new
                    {
                        Id = li.Id,
                        CreateTime = li.CreateDate,
                        TotalPoint = itemsScore,
                        Type = li.Pouriya_Type
                    });
                }
                return Json(PrepareSuccessResult(res));
            }
            catch (Exception ex)
            {
                M_SystemLog exx = new M_SystemLog(E_SystemType.SHAHRDARI_WEB_APPLICATION, E_LogType.ERROR, ex);
                if (ex.Source == E_LogType.SYSTEM_ERROR.ToString())
                    exx.LogType = E_LogType.SYSTEM_ERROR;
                L_Log.SubmitLog(exx);
                if (ex.Source != E_LogType.SYSTEM_ERROR.ToString())
                    return Json(PrepareFaildResult("خطایی در سرور وجود دارد", (int)E_ErrorCodes.SERVER_ERROR + S_Seprators.ErrorFieldNameSeprator.ToString() + ""));
                return Json(PrepareFaildResult(exx.LogMessage, ex.HelpLink));
            }
        }

        [HttpGet]
        public IHttpActionResult GetRequestDetails(int ParentId)
        {
            try
            {
                int userId = validateUser();
                return Json(PrepareSuccessResult(getRequestDetail(ParentId, userId)));
            }
            catch (Exception ex)
            {
                M_SystemLog exx = new M_SystemLog(E_SystemType.SHAHRDARI_WEB_APPLICATION, E_LogType.ERROR, ex);
                if (ex.Source == E_LogType.SYSTEM_ERROR.ToString())
                    exx.LogType = E_LogType.SYSTEM_ERROR;
                L_Log.SubmitLog(exx);
                if (ex.Source != E_LogType.SYSTEM_ERROR.ToString())
                    return Json(PrepareFaildResult("خطایی در سرور وجود دارد", (int)E_ErrorCodes.SERVER_ERROR + S_Seprators.ErrorFieldNameSeprator.ToString() + ""));
                return Json(PrepareFaildResult(exx.LogMessage, ex.HelpLink));
            }
        }

        [HttpPost]
        public IHttpActionResult Delete([FromBody] KhalkajDeleteModel Datas)
        {
            try
            {
                int userId = validateUser();
                new B_ServicesRequests().Delete(Datas.message, userId, true, Datas.description);
                return Json(PrepareSuccessResult(true));
            }
            catch (Exception ex)
            {
                M_SystemLog exx = new M_SystemLog(E_SystemType.SHAHRDARI_WEB_APPLICATION, E_LogType.ERROR, ex);
                if (ex.Source == E_LogType.SYSTEM_ERROR.ToString())
                    exx.LogType = E_LogType.SYSTEM_ERROR;
                L_Log.SubmitLog(exx);
                if (ex.Source != E_LogType.SYSTEM_ERROR.ToString())
                    return Json(PrepareFaildResult("خطایی در سرور وجود دارد", (int)E_ErrorCodes.SERVER_ERROR + S_Seprators.ErrorFieldNameSeprator.ToString() + ""));
                return Json(PrepareFaildResult(exx.LogMessage, ex.HelpLink));
            }
        }

        [HttpPost]
        public IHttpActionResult Rate([FromBody]RateModel Data)
        {
            try
            {
                int userId = validateUser();
                new B_ServicesRequests().ChangeStatus(Data.requestId, userId, E_PublicCategory.REQUEST_STATUS.CLOSED, Data.description, Data.rate);
                foreach (var li in Data.items)
                {
                    new B_CommentItems().Add(new M_CommentItems
                    {
                        CommentItemId = li,
                        RequestId = Data.requestId
                    });
                }
                return Json(PrepareSuccessResult(true));
            }
            catch (Exception ex)
            {
                M_SystemLog exx = new M_SystemLog(E_SystemType.SHAHRDARI_WEB_APPLICATION, E_LogType.ERROR, ex);
                if (ex.Source == E_LogType.SYSTEM_ERROR.ToString())
                    exx.LogType = E_LogType.SYSTEM_ERROR;
                L_Log.SubmitLog(exx);
                if (ex.Source != E_LogType.SYSTEM_ERROR.ToString())
                    return Json(PrepareFaildResult("خطایی در سرور وجود دارد", (int)E_ErrorCodes.SERVER_ERROR + S_Seprators.ErrorFieldNameSeprator.ToString() + ""));
                return Json(PrepareFaildResult(exx.LogMessage, ex.HelpLink));
            }
        }

        [HttpGet]
        public IHttpActionResult Educate()
        {
            try
            {
                validateUser();
                var res = new List<object>();
                var learns = new B_Learns().GetLearns();
                foreach (var li in learns)
                {
                    res.Add(new
                    {
                        Id = li.Id,
                        Title = li.Title,
                        ShortText = li.ShortDesc,
                        FullText = li.LongDesc,
                        VideoUrl = string.IsNullOrEmpty(li.Video) ? string.Empty : HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + "/Admin/AttachedFiles/" + li.Video.Split('/')[li.Video.Split('/').Length - 1],
                        Type = "HYPER_TEXT",
                        Imageurl = string.IsNullOrEmpty(li.CoverImage) ? string.Empty : GetImageAddress(li.CoverImage, new E_FTPRoutes(BaseUrl).LEARNS),
                        RateSum = li.RateSum,
                        RateCount = li.RateCount
                    });
                }
                return Json(PrepareSuccessResult(res));
            }
            catch (Exception ex)
            {
                M_SystemLog exx = new M_SystemLog(E_SystemType.SHAHRDARI_WEB_APPLICATION, E_LogType.ERROR, ex);
                if (ex.Source == E_LogType.SYSTEM_ERROR.ToString())
                    exx.LogType = E_LogType.SYSTEM_ERROR;
                L_Log.SubmitLog(exx);
                if (ex.Source != E_LogType.SYSTEM_ERROR.ToString())
                    return Json(PrepareFaildResult("خطایی در سرور وجود دارد", (int)E_ErrorCodes.SERVER_ERROR + S_Seprators.ErrorFieldNameSeprator.ToString() + ""));
                return Json(PrepareFaildResult(exx.LogMessage, ex.HelpLink));
            }
        }

        [HttpGet]
        public IHttpActionResult EducateDetails(int Id)
        {
            try
            {
                validateUser();
                var learns = new B_Learns().GetLearns(Id);
                return Json(PrepareSuccessResult(new
                {
                    Id = learns.Id,
                    Title = learns.Title,
                    ShortText = learns.ShortDesc,
                    FullText = learns.LongDesc,
                    VideoUrl = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + "/Admin/AttachedFiles/" + learns.Video.Split('/')[learns.Video.Split('/').Length - 1],
                    Type = "HYPER_TEXT",
                    Imageurl = "http://sabzbash.ir/news.png",
                    RateSum = learns.RateSum,
                    RateCount = learns.RateCount
                }));
            }
            catch (Exception ex)
            {
                M_SystemLog exx = new M_SystemLog(E_SystemType.SHAHRDARI_WEB_APPLICATION, E_LogType.ERROR, ex);
                if (ex.Source == E_LogType.SYSTEM_ERROR.ToString())
                    exx.LogType = E_LogType.SYSTEM_ERROR;
                L_Log.SubmitLog(exx);
                if (ex.Source != E_LogType.SYSTEM_ERROR.ToString())
                    return Json(PrepareFaildResult("خطایی در سرور وجود دارد", (int)E_ErrorCodes.SERVER_ERROR + S_Seprators.ErrorFieldNameSeprator.ToString() + ""));
                return Json(PrepareFaildResult(exx.LogMessage, ex.HelpLink));
            }
        }

        [HttpGet]
        public IHttpActionResult GetNews()
        {
            {
                try
                {
                    validateUser();
                    return null;
                }
                catch (Exception ex)
                {
                    M_SystemLog exx = new M_SystemLog(E_SystemType.SHAHRDARI_WEB_APPLICATION, E_LogType.ERROR, ex);
                    if (ex.Source == E_LogType.SYSTEM_ERROR.ToString())
                        exx.LogType = E_LogType.SYSTEM_ERROR;
                    L_Log.SubmitLog(exx);
                    if (ex.Source != E_LogType.SYSTEM_ERROR.ToString())
                        return Json(PrepareFaildResult("خطایی در سرور وجود دارد", (int)E_ErrorCodes.SERVER_ERROR + S_Seprators.ErrorFieldNameSeprator.ToString() + ""));
                    return Json(PrepareFaildResult(exx.LogMessage, ex.HelpLink));
                }
            }
        }

        [HttpGet]
        public IHttpActionResult GetMessages()
        {
            try
            {
                int userId = validateUser();
                var tempRes = new B_Messages().GetMessgas();
                var res = new List<object>();
                foreach (var li in tempRes)
                {
                    res.Add(new
                    {
                        Id = li.Id,
                        Title = li.title,
                        Messages = li.messages,
                    });
                }
                return Json(PrepareSuccessResult(res));
            }
            catch (Exception ex)
            {
                M_SystemLog exx = new M_SystemLog(E_SystemType.SHAHRDARI_WEB_APPLICATION, E_LogType.ERROR, ex);
                if (ex.Source == E_LogType.SYSTEM_ERROR.ToString())
                    exx.LogType = E_LogType.SYSTEM_ERROR;
                L_Log.SubmitLog(exx);
                if (ex.Source != E_LogType.SYSTEM_ERROR.ToString())
                    return Json(PrepareFaildResult("خطایی در سرور وجود دارد", (int)E_ErrorCodes.SERVER_ERROR + S_Seprators.ErrorFieldNameSeprator.ToString() + ""));
                return Json(PrepareFaildResult(exx.LogMessage, ex.HelpLink));
            }
        }

        [HttpGet]
        public IHttpActionResult GetComments()
        {
            try
            {
                validateUser();
                return Json(PrepareSuccessResult(new List<object>()));
            }
            catch (Exception ex)
            {
                M_SystemLog exx = new M_SystemLog(E_SystemType.SHAHRDARI_WEB_APPLICATION, E_LogType.ERROR, ex);
                if (ex.Source == E_LogType.SYSTEM_ERROR.ToString())
                    exx.LogType = E_LogType.SYSTEM_ERROR;
                L_Log.SubmitLog(exx);
                if (ex.Source != E_LogType.SYSTEM_ERROR.ToString())
                    return Json(PrepareFaildResult("خطایی در سرور وجود دارد", (int)E_ErrorCodes.SERVER_ERROR + S_Seprators.ErrorFieldNameSeprator.ToString() + ""));
                return Json(PrepareFaildResult(exx.LogMessage, ex.HelpLink));
            }
        }

        [HttpGet]
        public IHttpActionResult GetSetting()
        {
            try
            {
                validateUser();
                return Json(PrepareSuccessResult(new
                {
                    RateItems = new List<object> {
                        new { Rate = 1, Items = new List<object> {
                            new {Id = (int)E_PublicCategory.FEEDBACK.DISRESPECTFUL_BEHAVIOR, Title = "رفتار نا محترمانه" },
                            new {Id = (int)E_PublicCategory.FEEDBACK.GET_TO_YOUR_PLACE_LATE, Title = "دیر رسیدن به محل شما" }
                        } },
                        new { Rate = 2, Items = new List<object> {
                            new {Id = (int)E_PublicCategory.FEEDBACK.DISRESPECTFUL_BEHAVIOR, Title = "رفتار نا محترمانه" },
                            new {Id = (int)E_PublicCategory.FEEDBACK.GET_TO_YOUR_PLACE_LATE, Title = "دیر رسیدن به محل شما" }
                        } },
                        new { Rate = 3, Items = new List<object> {
                            new {Id = (int)E_PublicCategory.FEEDBACK.DISRESPECTFUL_BEHAVIOR, Title = "رفتار نا محترمانه" },
                            new {Id = (int)E_PublicCategory.FEEDBACK.GET_TO_YOUR_PLACE_LATE, Title = "دیر رسیدن به محل شما" },
                            new {Id = (int)E_PublicCategory.FEEDBACK.ARRIVE_ON_TIME, Title = "به موقع رسیدن" },
                            new {Id = (int)E_PublicCategory.FEEDBACK.RESPECTFUL_BEHAVIOR, Title = "رفتار محترمانه" },
                            new {Id = (int)E_PublicCategory.FEEDBACK.SPEED_OF_SERVICE, Title = "سرعت در سرویس دهی" }
                        } },
                        new { Rate = 4, Items = new List<object> {
                            new {Id = (int)E_PublicCategory.FEEDBACK.ARRIVE_ON_TIME, Title = "به موقع رسیدن" },
                            new {Id = (int)E_PublicCategory.FEEDBACK.RESPECTFUL_BEHAVIOR, Title = "رفتار محترمانه" },
                            new {Id = (int)E_PublicCategory.FEEDBACK.SPEED_OF_SERVICE, Title = "سرعت در سرویس دهی" }
                        } },
                        new { Rate = 5, Items = new List<object> {
                            new {Id = (int)E_PublicCategory.FEEDBACK.ARRIVE_ON_TIME, Title = "به موقع رسیدن" },
                            new {Id = (int)E_PublicCategory.FEEDBACK.RESPECTFUL_BEHAVIOR, Title = "رفتار محترمانه" },
                            new {Id = (int)E_PublicCategory.FEEDBACK.SPEED_OF_SERVICE, Title = "سرعت در سرویس دهی" }
                        } },
                    },
                    AboutUs = "سبز باش سامانه هوشمند آموزش وفرهنگ سازی تفکیک زباله از مبدااست که با هدف حمایت از محیط ‌زیست و بهبود چرخه مدیریت پسماند، به عنوان پلي بین علاقمندان محیط زیست، دوست داران طبیعت و زیست بانان در مسير بهبود مدیریت شهری است. سبز باش هزینه های تفکیک زباله را به طرز چشم‌گیری کاهش می دهد و تاثیر به سزايي در حفظ و بقای محیط زیست و پيش بيني اینده ای سبز برای ما و فرزندانمان دارد.",
                    CancelRequestItem = new List<object> {
                            new {Id = (int)E_PublicCategory.FEEDBACK.DISRESPECTFUL_BEHAVIOR, Title = "رفتار نا محترمانه" },
                            new {Id = (int)E_PublicCategory.FEEDBACK.GET_TO_YOUR_PLACE_LATE, Title = "دیر رسیدن به محل شما" },
                            new {Id = (int)E_PublicCategory.FEEDBACK.PERSONAL_REASONS, Title = "دلایل شخصی" }
                        },
                    IsForceUpdate = bool.Parse(ConfigurationManager.AppSettings["IsForceUpdate"]),
                    AndroidVersion = int.Parse(ConfigurationManager.AppSettings["AndroidVersion"]),
                    ApkLink = ConfigurationManager.AppSettings["ApkLink"],
                    InviteFriend = ConfigurationManager.AppSettings["ShareMessage"]
                }));
            }
            catch (Exception ex)
            {
                M_SystemLog exx = new M_SystemLog(E_SystemType.SHAHRDARI_WEB_APPLICATION, E_LogType.ERROR, ex);
                if (ex.Source == E_LogType.SYSTEM_ERROR.ToString())
                    exx.LogType = E_LogType.SYSTEM_ERROR;
                L_Log.SubmitLog(exx);
                if (ex.Source != E_LogType.SYSTEM_ERROR.ToString())
                    return Json(PrepareFaildResult("خطایی در سرور وجود دارد", (int)E_ErrorCodes.SERVER_ERROR + S_Seprators.ErrorFieldNameSeprator.ToString() + ""));
                return Json(PrepareFaildResult(exx.LogMessage, ex.HelpLink));
            }
        }

        [HttpPost]
        public IHttpActionResult ListenEar([FromBody]M_ListenEar Data)
        {
            try
            {
                int userId = validateUser();
                Data.CreationDate = DateTime.Now;
                Data.IsRead = false;
                Data.UserId = userId;
                return Json(PrepareSuccessResult(new B_ListenEar().AddListenEar(Data)));
            }
            catch (Exception ex)
            {
                M_SystemLog exx = new M_SystemLog(E_SystemType.SHAHRDARI_WEB_APPLICATION, E_LogType.ERROR, ex);
                if (ex.Source == E_LogType.SYSTEM_ERROR.ToString())
                    exx.LogType = E_LogType.SYSTEM_ERROR;
                L_Log.SubmitLog(exx);
                if (ex.Source != E_LogType.SYSTEM_ERROR.ToString())
                    return Json(PrepareFaildResult("خطایی در سرور وجود دارد", (int)E_ErrorCodes.SERVER_ERROR + S_Seprators.ErrorFieldNameSeprator.ToString() + ""));
                return Json(PrepareFaildResult(exx.LogMessage, ex.HelpLink));
            }
        }

        [HttpPost]
        public IHttpActionResult AsignUserToRequest([FromBody]AsignUserToRequestModel Data)
        {
            try
            {
                validateUser();
                if (Data.PersonalId == 0)
                    throw F_ExeptionFactory.MakeExeption("خطای ارسال اطلاعات",
                        ((int)E_ErrorCodes.NOT_FOUND) + S_Seprators.ErrorFieldNameSeprator.ToString() + "PersonalId", Enums.Loging.E_LogType.SYSTEM_ERROR);
                if (Data.RequestId == 0)
                    throw F_ExeptionFactory.MakeExeption("خطای ارسال اطلاعات",
                        ((int)E_ErrorCodes.NOT_FOUND) + S_Seprators.ErrorFieldNameSeprator.ToString() + "RequestId", Enums.Loging.E_LogType.SYSTEM_ERROR);
                new B_ServicesRequests().AsignUserToRequest(Data.PersonalId, Data.RequestId);
                return Json(PrepareSuccessResult(true));
            }
            catch (Exception ex)
            {
                M_SystemLog exx = new M_SystemLog(E_SystemType.SHAHRDARI_WEB_APPLICATION, E_LogType.ERROR, ex);
                if (ex.Source == E_LogType.SYSTEM_ERROR.ToString())
                    exx.LogType = E_LogType.SYSTEM_ERROR;
                L_Log.SubmitLog(exx);
                if (ex.Source != E_LogType.SYSTEM_ERROR.ToString())
                    return Json(PrepareFaildResult("خطایی در سرور وجود دارد", (int)E_ErrorCodes.SERVER_ERROR + S_Seprators.ErrorFieldNameSeprator.ToString() + ""));
                return Json(PrepareFaildResult(exx.LogMessage, ex.HelpLink));
            }
        }

        public class RequestAdd
        {
            public int BoothId { get; set; }
            public string Description { get; set; }
            public List<ItemsModel> Items { get; set; }
            public int TimeId { get; set; }
            public int DayId { get; set; }
            public AddressModel Address { get; set; }
            public string Type { get; set; }
            public class ItemsModel
            {
                public int Id { get; set; }
                public int Value { get; set; }
            }
            public class AddressModel
            {
                public string Lat { get; set; }
                public string Lng { get; set; }
                public string Address { get; set; }
                public string Unit { get; set; }
                public string Plaque { get; set; }
            }
        }
        public class KhalkajDeleteModel
        {
            public int message { get; set; }
            public string description { get; set; }
        }
        public class RateModel
        {
            public int rate { get; set; }
            public List<E_PublicCategory.FEEDBACK> items { get; set; }
            public string description { get; set; }
            public int requestId { get; set; }
            public int foruser { get; set; }
        }
        public class AsignUserToRequestModel
        {
            public int PersonalId { get; set; }
            public int RequestId { get; set; }
        }

        #endregion Khalaj Changes

        #region Private Functions
        private object getBooth(int id)
        {
            if (id == 0)
                return null;
            var personel = getPersonel(id);
            if (personel != null)
            {
                var boothInfo = new B_Booth().GetBoothByPersonelId(id);
                if (boothInfo != null)
                    return new
                    {
                        Personel = personel,
                        BoothInfo = new
                        {
                            Address = boothInfo.Address,
                            Description = boothInfo.Description,
                            Id = boothInfo.Id,
                            ImageUrl = GetImageAddress(boothInfo.ImageName, new E_FTPRoutes(BaseUrl).BOOTH_INFO),
                            Lat = boothInfo.Lat,
                            Lng = boothInfo.Lng,
                            Name = boothInfo.Name
                        }
                    };
                else
                    return new { Personel = personel };
            }
            else
                return null;
        }

        private object getCar(int id)
        {
            if (id == 0)
                return null;
            var personel = getPersonel(id);
            if (personel != null)
            {
                var carInfo = new B_CarInfo().GetCarInfo(id);
                var loc = new B_Location().GetLocationByPersonelId(id);
                if (carInfo != null)
                    return (new
                    {
                        Personel = personel,
                        CarInfo = new
                        {
                            Name = carInfo.Name,
                            Color = carInfo.Color,
                            Type = ((E_PublicCategory.CAR_TYPE)carInfo.Type).ToString(),
                            TagNational = carInfo.TagNational,
                            TagMiddle = carInfo.TagMiddle,
                            TagLast = carInfo.TagLast,
                            TagFirst = carInfo.TagFirst,
                            TagColor = ((E_PublicCategory.TAG_COLOR)carInfo.TagColor).ToString(),
                            ImageUrl = GetImageAddress(carInfo.Image, new E_FTPRoutes(BaseUrl).CAR_INFO),
                            Id = carInfo.Id,
                            Capacity = carInfo.Capacity
                        },
                        Location = loc != null ? new
                        {
                            Bearing = loc.Bearing,
                            Date = loc.Date,
                            Lat = loc.Lat,
                            Lng = loc.Lng
                        } : null
                    });
                else
                    return new { Personel = personel };
            }
            else
                return null;
        }

        private object getPersonel(int id)
        {
            var personel = new B_Personels().GetPersonelById(id);
            return new
            {
                FirstName = personel.FirstName,
                LastName = personel.LastName,
                MobileNumber = personel.MobileNumber,
                ImageUrl = GetImageAddress(personel.ImageName, new E_FTPRoutes(BaseUrl).PERSONELS),
                BirthDate = personel.BirthDate,
                DeactiveDate = personel.DeactiveDate,
                DeletedDate = personel.DeletedDate,
                Gender = personel.Gender,
                Id = personel.Id,
                IsActive = personel.IsActive,
                IsDeleted = personel.IsDeleted,
                LastOnline = personel.LastOnline,
                PersonelType = personel.PersonelType,
                RegisterDate = personel.RegisterDate,
                SumCenterAddress = personel.SumCenterAddress,
                SumCenterTell = personel.SumCenterTell
            };
        }

        public object getRequestDetail(int requestId, int userId,E_PublicCategory.SYSTEM_USER_TYPE Type = E_PublicCategory.SYSTEM_USER_TYPE.NORMAL_USER)
        {
            var requests = new B_ServicesRequests().GetServicesRequests(requestId);
            if (requests.UserId != userId)
                throw F_ExeptionFactory.MakeExeption("درخواست مورد نظر برای کاربر جاری نمیباشد",
                    ((int)E_ErrorCodes.NOT_FOUND) + S_Seprators.ErrorFieldNameSeprator.ToString() + "UserId", Enums.Loging.E_LogType.SYSTEM_ERROR);

            var item = new B_ServicesRequestItems().GetItems(requestId).Where(x=>x.UserType == Type).ToList();
            var personel = requests.PersonelId.HasValue ? new B_Personels().GetPersonelById(requests.PersonelId.Value) : null;
            double? totalPoint = new B_ServicesRequestItems().GetItemsPoint(requestId);
            var items = new List<object>();
            foreach (var li in item)
            {
                items.Add(new
                {
                    Id = li.Id,
                    Value = li.Value,
                    Title = li.Title,
                    Unit = li.Unit,
                    PointPerUnit = li.ScorePerUnit,
                    ParentId = new B_ServicesCategories().GetParentId(li.Title),
                    CategoryId = li.CategoryId
                });
            }
            var date = new B_Pouriya_Date().GetDate(requests.Pouriya_DateId.HasValue ? (int)requests.Pouriya_DateId : 0);
            var DriverModel = new object();
            var BoothModel = new object();
            if (requests.Pouriya_Type == "CAR")
                DriverModel = getCar(requests.PersonelId.HasValue ? (int)requests.PersonelId : 0);
            else if (requests.Pouriya_Type == "BOOTH")
                BoothModel = getBooth(requests.PersonelId.HasValue ? (int)requests.PersonelId : 0);
            return new
            {
                Address = new RequestAdd.AddressModel { Address = requests.Address, Lng = requests.GeographicalCoordinates.Split(',')[1], Lat = requests.GeographicalCoordinates.Split(',')[0], Plaque = requests.PlaqueNumber, Unit = requests.UnitNumber },
                DayId = requests.Pouriya_DateId,
                Description = requests.Description,
                TimeId = requests.Pouriya_TimeId,
                Type = requests.Pouriya_Type,
                Items = items,
                Status = ((E_PublicCategory.REQUEST_STATUS)requests.Status).ToString(),
                FinishDate = requests.FinishDate,
                Id = requests.Id,
                CreationTime = requests.CreateDate,
                TotalPoint = totalPoint.HasValue ? int.Parse(totalPoint.ToString()) : 0,
                TimeStr = new B_PublicCategory().GetCategoryTitle(requests.Pouriya_TimeId.HasValue ? (int)requests.Pouriya_TimeId : 0),
                DayStr = date != null ? date.Date.ConvertToPesianDateName() : string.Empty,
                DriverModel = DriverModel,
                BoothModel = BoothModel
            };
        }

        private bool isPast(DateTime date, E_PublicCategory.SERVICE_TIME time)
        {
            DateTime dt = new DateTime();
            if (time == E_PublicCategory.SERVICE_TIME.Time_8_11)
                dt = date.AddHours(11);
            if (time == E_PublicCategory.SERVICE_TIME.Time_11_14)
                dt = date.AddHours(14);
            if (time == E_PublicCategory.SERVICE_TIME.Time_14_17)
                dt = date.AddHours(17);
            if (time == E_PublicCategory.SERVICE_TIME.Time_17_20)
                dt = date.AddHours(20);
            if (time == E_PublicCategory.SERVICE_TIME.Time_20_22)
                dt = date.AddHours(22);

            int result = DateTime.Compare(dt, DateTime.Now);

            if (result < 0)
                return false;
            else if (result == 0)
                return false;
            else
                return true;
        }
        #endregion

        #region RequestModel
        public class AddModel
        {
            public M_ServicesRequests ServicesRequests { get; set; }
        }

        public class GetServicesRequestsByUserIdModel
        {
            public List<int> IgnoreIds { get; set; }
        }

        public class GetServicesRequestsModel
        {
            public E_PublicCategory.REQUEST_STATUS? Status { get; set; }
            public int PageNumber { get; set; }
            public int PageCapacity { get; set; }
        }

        public class GetServicesRequestsDetailsModel
        {
            public int Id { get; set; }
        }

        public class DeleteModel
        {
            public int ServiceRequestId { get; set; }
        }

        public class CloseRequetsModel
        {
            public int ServiceRequestId { get; set; }
            public string Comment { get; set; }
            public int Rate { get; set; }
        }

        public class DiscountModel
        {
            public int FactorId { get; set; }
            public string DiscountCode { get; set; }
        }

        public class ChangeStatusModel
        {
            public int RequestId { get; set; }
            public E_PublicCategory.REQUEST_STATUS Status { get; set; }
        }

        #endregion RequestModel
    }
}
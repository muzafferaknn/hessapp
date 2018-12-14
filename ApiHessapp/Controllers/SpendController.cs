using ApiHessapp.DTO.Activity;
using ApiHessapp.DTO.Activitys;
using ApiHessapp.DTO.Spend;
using ApiHessapp.Models.EntitiyFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ApiHessapp.Controllers
{
    public class SpendController : BaseController
    {
        [HttpPost]
        public IHttpActionResult Create(CreateSpendRequestDTO model)
        {
            if (ModelState.IsValid)
            {
                using (HessappEntities ent = new HessappEntities())
                {
                    // spend ekleme
                    Spends spend = new Spends();
                    spend.From = model.from;
                    spend.Description = model.description;
                    spend.GroupID = model.groupId;
                    spend.TotalAmount = model.totalAmount;
                    spend.Date = DateTime.Now;
                    ent.Spends.Add(spend);
                    ent.SaveChanges();

                    // grup kişi sayısı bulma
                    List<String> parts = SplitParticipants(model.groupId);

                    // activity ekleme
                    foreach (var item in parts)
                    {
                        if (!(item == model.from))
                        {
                            Activity act = new Activity();
                            act.GroupID = model.groupId;
                            act.From = model.from;
                            act.Destination = item;
                            act.Description = model.description;
                            act.Date = spend.Date;
                            act.Amount = model.totalAmount / parts.Count();
                            ent.Activity.Add(act);
                            ent.SaveChanges();
                        }
                    }

                    CreateSpendResponseDTO rsp = new CreateSpendResponseDTO();
                    rsp.groupId = model.groupId;
                    rsp.from = model.from;
                    rsp.description = model.description;
                    rsp.totalAmount = model.totalAmount;
                    rsp.date = spend.Date.Value.ToString("dd.MM.yyyy HH:mm");
                    return Content(HttpStatusCode.OK, rsp);
                }

            }
            else
                return BadRequest();
        }
        [HttpGet]
        public IHttpActionResult GetSpends(int id)
        {
            if (ModelState.IsValid)
            {
                using (HessappEntities ent = new HessappEntities())
                {
                    List<Spends> list = ent.Spends.Where(q => q.GroupID == id).Select(q => q).ToList();
                    ListSpendsResponseDTO model = new ListSpendsResponseDTO();
                    List<SpendListDTO> lst = new List<SpendListDTO>();
                    foreach (var item in list)
                    {

                        SpendListDTO dto = new SpendListDTO();
                        dto.GroupID = item.GroupID;
                        dto.From = item.From;
                        dto.TotalAmount = item.TotalAmount;
                        dto.Description = item.Description;
                        dto.Date = item.Date.Value.ToString("dd.MM.yyyy HH:mm");
                        lst.Add(dto);
                    }
                    model.spends = lst;
                    if (model.spends != null)
                        return Content(HttpStatusCode.OK, model);
                    else
                        return BadRequest();
                }
            }
            else
                return BadRequest();
        }

        [HttpGet]
        public IHttpActionResult GetGeneralStatus(string id)
        {
            if (ModelState.IsValid)
            {
                using (HessappEntities ent=new HessappEntities())
                {
                    // grup bazında alacak listesi
                    var credits = (from q in ent.Activity
                                     where q.From == id
                                     group q by new
                                     {
                                         q.GroupID,
                                         q.Date
                                     }
                                   into grp
                                     select new
                                     {
                                         grp.Key.GroupID,
                                         amount =grp.Select(q=>q.Amount).FirstOrDefault()
                                     }).ToList();

                    List<GeneralStatus> listCredits = (from q in credits
                                      group q by new
                                      {
                                          q.GroupID
                                          
                                      }
                                    into grp
                                       select new GeneralStatus
                                       {
                                           groupId = grp.Key.GroupID,
                                           groupName = ent.Group.Where(q => q.GroupID == grp.Key.GroupID).Select(q => q.Name).SingleOrDefault(),
                                           totalCredit = grp.Sum(q=>q.amount),
                                           totalDebt=0
                                       }).AsEnumerable().ToList();

                    // grup bazında borç listesi
                    var dept = (from q in ent.Activity
                                   where q.Destination == id
                                   group q by new
                                   {
                                       q.GroupID,
                                       q.Date
                                   }
                                   into grp
                                   select new
                                   {
                                       grp.Key.GroupID,
                                       amount = grp.Select(q => q.Amount).Sum()
                                   }).ToList();

                    List<GeneralStatus> listDept = (from q in dept
                                       group q by new
                                       {
                                           q.GroupID
                                           
                                       }
                                    into grp
                                       select new GeneralStatus
                                       {
                                           groupId=grp.Key.GroupID,
                                           groupName=ent.Group.Where(q=>q.GroupID==grp.Key.GroupID).Select(q=>q.Name).SingleOrDefault(),
                                           totalDebt=grp.Sum(q=>q.amount),
                                           totalCredit=0
                                       }).AsEnumerable().ToList();

                    // listelerin birleştirilmesi
                    List<GeneralStatus> fullList=new List<GeneralStatus>();
                    foreach (var item in listCredits)
                    {
                        fullList.Add(item);
                    }
                    foreach (var item in listDept)
                    {
                        var find = fullList.Where(q => q.groupId == item.groupId).Select(q => q).ToList();
                        if (find.Count!=0)
                        {
                            foreach (var item1 in find)
                            {
                                item1.totalDebt = item.totalDebt;
                            }
                        }
                        else
                        {
                            fullList.Add(item);
                        }
                    }
                    GeneralStatusListDTO rsp = new GeneralStatusListDTO();
                    rsp.list = fullList;
                    return Content(HttpStatusCode.OK, rsp);
                }
            }
            else
                return BadRequest();
        }

        [HttpPost]
        public IHttpActionResult GetGroupStatus(GetGroupStatusRequestDTO model)
        {
            if (ModelState.IsValid)
            {
                using (HessappEntities ent=new HessappEntities())
                {
                    // toplam borç
                    var totalDebt = ent.Activity.Where(q => q.GroupID == model.groupId && q.Destination == model.nickname).Select(q => q.Amount).Sum();
                    // toplam alacak
                    var totalCredit = ent.Activity.Where(q => q.GroupID == model.groupId && q.From == model.nickname).Select(q => q.Amount).Sum();
                    List<Debts> listDebts = (from q in ent.Activity
                                    where q.GroupID == model.groupId && q.Destination == model.nickname
                                    select new Debts
                                    {
                                        ActivityId=q.ActivityID,
                                        Amount=q.Amount,
                                        Description=q.Description,
                                        From=q.From
                                    }).ToList();
                    GetGroupStatusResponseDTO rsp = new GetGroupStatusResponseDTO();
                    rsp.totalCredit = totalCredit;
                    rsp.totalDebt = totalDebt;
                    rsp.listDebt = listDebts;
                    return Content(HttpStatusCode.OK,rsp);
                }
            }
            else
                return BadRequest();
        }

        [HttpGet]
        public IHttpActionResult PayDebt(int id)
        {
            if (ModelState.IsValid)
            {
                using (HessappEntities ent=new HessappEntities())
                {
                    var del = ent.Activity.Find(id);
                    PayDebtResponseDTO rsp = new PayDebtResponseDTO();
                    rsp.activityId = id;
                    ent.Activity.Remove(del);
                    ent.SaveChanges();
                    return Content(HttpStatusCode.OK, rsp);
                }
            }
            else
                return BadRequest();
        }

    }
}

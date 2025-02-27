﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AMS.Models;

namespace AMS.Controllers
{
    public class LeaveRequestsController : Controller
    {
        private Entities db = new Entities();

        // GET: LeaveRequests
        public ActionResult Index()
        {
            return View(db.LeaveRequests);
        }

        // GET: LeaveRequests/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LeaveRequests leaveRequests = db.LeaveRequests.Find(id);
            if (leaveRequests == null)
            {
                return HttpNotFound();
            }
            return View(leaveRequests);
        }

        // GET: LeaveRequests/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: LeaveRequests/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "LeaveRequestID,EmployeeID,RequestTime,StartTime,EndTime,LeaveType,LeaveReason,ReviewStatusID,ReviewTime,Attachment")] LeaveRequests leaveRequests)
        {
            if (ModelState.IsValid)
            {
                if (Request.Files["LeaveFile"].ContentLength != 0)
                {
                    byte[] data = null;
                    using (BinaryReader br=new BinaryReader(
                        Request.Files["LeaveFile"].InputStream))
                    {
                        data = br.ReadBytes(Request.Files["LeaveFile"].ContentLength);
                    }
                    leaveRequests.Attachment = data;
                }

                db.LeaveRequests.Add(leaveRequests);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(leaveRequests);
        }

        // GET: LeaveRequests/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LeaveRequests leaveRequests = db.LeaveRequests.Find(id);
            if (leaveRequests == null)
            {
                return HttpNotFound();
            }
            return View(leaveRequests);
        }

        // POST: LeaveRequests/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "LeaveRequestID,EmployeeID,RequestTime,StartTime,EndTime,LeaveType,LeaveReason,ReviewStatusID,ReviewTime,Attachment")] LeaveRequests leaveRequests)
        {
            if (ModelState.IsValid)
            {
                if (Request.Files["LeaveFile"].ContentLength != 0)
                {
                    byte[] data = null;
                    using (BinaryReader br = new BinaryReader(
                        Request.Files["LeaveFile"].InputStream))
                    {
                        data = br.ReadBytes(Request.Files["LeaveFile"].ContentLength);
                    }
                    leaveRequests.Attachment = data;
                }
                else
                {
                    LeaveRequests L = db.LeaveRequests.Find(leaveRequests.LeaveRequestID);
                    L.EmployeeID = leaveRequests.EmployeeID;
                    L.RequestTime = leaveRequests.RequestTime;
                    L.StartTime = leaveRequests.StartTime;
                    L.EndTime = leaveRequests.StartTime;
                    L.LeaveType = leaveRequests.LeaveType;
                    L.LeaveReason = leaveRequests.LeaveReason;
                    L.LeaveRequestID = leaveRequests.LeaveRequestID;
                    L.RequestTime = leaveRequests.RequestTime;
                    leaveRequests = L;
                }


                db.Entry(leaveRequests).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(leaveRequests);
        }

        // GET: LeaveRequests/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LeaveRequests leaveRequests = db.LeaveRequests.Find(id);
            if (leaveRequests == null)
            {
                return HttpNotFound();
            }
            return View(leaveRequests);
        }

        // POST: LeaveRequests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            LeaveRequests leaveRequests = db.LeaveRequests.Find(id);
            db.LeaveRequests.Remove(leaveRequests);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        public FileResult ShowPhoto(string id)
        {
            byte[] content = db.LeaveRequests.Find(id).Attachment;
            return File(content, "image/jpeg");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using DataAccess;
using CommonFunctions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BasicWebApp.Models;

namespace BasicWebApp.Controllers
{
    public class CRUDController : BaseController
    {
        // GET: CRUD
        public ActionResult Index()
        {
            CRUDListViewModel crud = new CRUDListViewModel();
            DataSet ds = SQLServerDBConn.RunSelectQuery("select * from TABLENAME ");
            if (DataSetUtil.DataSetNullOrEmpty(ds))
            {
                crud.crud_lst = GenericFunctions.ConvertDataTable<CRUDViewModel>(ds.Tables[0]);
            }
            return View(crud);
        }

        // GET: CRUD/Details/5
        public ActionResult Details(int id)
        {
            CRUDViewModel crud = new CRUDViewModel();
            DataSet ds = SQLServerDBConn.RunSelectQuery("select * from TABLENAME where id={0}", new List<string>() { "@id" }, new List<object>() { id });
            if (DataSetUtil.DataSetNullOrEmpty(ds))
            {
                crud = GenericFunctions.ConvertDataRow<CRUDViewModel>(ds.Tables[0].Rows[0]);
            }

            return View(crud);
        }

        // GET: CRUD/Create
        public ActionResult Create()
        {

            return View();
        }

        // POST: CRUD/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CRUDListViewModel Model)
        {
            try
            {
                // TODO: Add insert logic here
                CRUDListViewModel crud = new CRUDListViewModel();
                if (ModelState.IsValid)
                {
                    SQLServerDBConn.RunInsertQuery("insert into TABLENAME(Name,Address) values({0}, {1})", new List<string>() { "@Name", "@Address" }, new List<object>() { Model.Name, Model.Address });
                }

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CRUD/Edit/5
        public ActionResult Edit(int id)
        {
            CRUDViewModel crud = new CRUDViewModel();
            DataSet ds = SQLServerDBConn.RunSelectQuery("select * from TABLENAME where id={0}", new List<string>() { "@id" }, new List<object>() { id });
            if (DataSetUtil.DataSetNullOrEmpty(ds))
            {
                crud = GenericFunctions.ConvertDataRow<CRUDViewModel>(ds.Tables[0].Rows[0]);
            }
            return View(crud);
        }

        // POST: CRUD/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, CRUDViewModel Model)
        {
            try
            {
                // TODO: Add update logic here
                if (ModelState.IsValid)
                {
                    SQLServerDBConn.RunUpdateQuery("Update TABLENAME set Name={0}, Address={1} where id={2} ", new List<string>() { "@Name", "@Address", "@id" }, new List<object>() { Model.Name, Model.Address, id });
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CRUD/Delete/5
        public ActionResult Delete(int id)
        {
            SQLServerDBConn.RunDeleteQuery("Detete From TABLENAME where id={0}", new List<string>() { "@id" }, new List<object>() { id });
            return View();
        }

        // POST: CRUD/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
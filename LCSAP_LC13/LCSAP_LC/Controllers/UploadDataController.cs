using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LCSAPTest.Controllers
{
    public class UploadDataController : Controller
    {
        // GET: UploadData
        public RedirectResult UploadDataResult()
        {
            return Redirect("~/WebForms/UploadData.aspx" );
        }
    }
}
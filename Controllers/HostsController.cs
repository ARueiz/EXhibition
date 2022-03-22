using EXhibition.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EXhibition.Controllers
{
    public class HostsController : Controller
    {
        private ExhibitionEntities db = new ExhibitionEntities();
        // GET: Hosts

        //黃亭愷
        public ActionResult Index(int id)
        {
            /*
             
             
              塞萬提斯說過，自由是上帝賜給人類的最大的
            幸福之一。
            這讓我的思緒清晰了。盧梭曾說過一句意義深遠的
            話，自
            然與美德，受到社會、財產的產物學問和藝術的侵害。
            
            這似乎解答了我的疑惑。高爾基講過，世界上
            最快而又最慢，最長而又最短，最平凡而又最珍貴，最易被忽視而又最令人後悔的就是時間。這段話的餘韻不斷在我腦海中迴盪著。
            
            唬爛對我來說，已經成為了我生活的一部分。把唬爛輕鬆帶過，顯然並不
            
            適合。需要考慮周詳唬爛的影響
            及因應對策。若到今天結
            束時我們都還無法釐清唬
            爛的意義，那想必
            我們昨天也
            無法釐清。我們都有個共識，若問題很
            困難，那就勢必不好解決。富蘭克林講過一段深奧的話，在這世界上，除了死亡和
            稅收以
            外，沒有可以肯定的事。請諸位將這段
            話在心中
            默念三遍。
             
             
             
             
             
             */
            return View();
        }

        //林昶廷
        public ActionResult EditHost()
        {

            return View();
        }

        //馬誠遠
        public ActionResult EditEvent()
        {
            return View();
        }

        //邱品叡
        public ActionResult CreateEvent()
        {
            return View();
        }
    }
}
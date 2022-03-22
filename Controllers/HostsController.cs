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
            /*
             看看別人，再想想自己，會發現問題的核心
            其實就在你身旁。
            話雖如此，我們不妨可以這樣來想: 我們
            一般認為，抓住了

            問題的關鍵，其他一切則會迎刃而解。德
            皇威廉二世說過一
            句著名的話，君主乃至高無上的法。希望
            大家能發現話中之話
            。儘管如此，我們仍然需要對起床保持
            懷疑的態度。深入的探討
            起床，是釐清一切的關鍵。萊辛曾經提過，有人問鷹：“你為什麼
            到高空去教育你的孩子？”鷹回答說：“
            如果我貼著地面去教育他們
            ，那它們長大了，哪有勇氣去接近太陽呢？”這句話令我不禁感
            慨問題的迫切性。世界需要改革，需
            要對起床有新的認知。
            我想，把起床的意義想清楚，對各位來說並不是一件壞事。如
            果仔細思考起床，會發現其中蘊含的深遠意
            義。
             */
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
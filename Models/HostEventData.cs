namespace EXhibition.Controllers
{
    internal class HostEventData
    {
        public string name { get; set; }
        public string phone { get; set; }
        public string startdate { get; set; }
        public string enddate { get; set; }
        public string exhibitionname { get; set; }
        public int evid { get; set; }
        public decimal ticketPrice { get; set; }
        public bool isOver { get; internal set; }
        public int hid { get; internal set; }
    }
}
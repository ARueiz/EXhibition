namespace EXhibition.Models
{
    public class CartItem
    {
        public int cartId { get; set; }
        public int EVID { get; set; }
        public int HID { get; set; }
        public string name { get; set; }
        public System.DateTime startdate { get; set; }
        public System.DateTime enddate { get; set; }
        public string venue { get; set; }
        public string image { get; set; }
        public string floorplanimg { get; set; }
        public string category { get; set; }
        public string eventinfo { get; set; }
        public string note { get; set; }
        public decimal ticketprice { get; set; }

        public CartItem()
        {
        }

        public CartItem(Models.events events)
        {
            this.EVID = events.EVID;
            this.HID = events.HID;
            this.name = events.name;
            this.startdate = events.startdate;
            this.enddate = events.enddate;
            this.venue = events.venue;
            this.image = GlobalVariables.HostImageUrl + events.image;
            this.floorplanimg = events.floorplanimg;
            this.category = events.category;
            this.eventinfo = events.eventinfo;
            this.note = events.note;
            this.ticketprice = events.ticketprice;
        }
    }
}
//------------------------------------------------------------------------------
// <auto-generated>
//     這個程式碼是由範本產生。
//
//     對這個檔案進行手動變更可能導致您的應用程式產生未預期的行為。
//     如果重新產生程式碼，將會覆寫對這個檔案的手動變更。
// </auto-generated>
//------------------------------------------------------------------------------

namespace EXhibition.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class orders
    {
        public int id { get; set; }
        public string paypalId { get; set; }
        public Nullable<System.DateTime> createDateTime { get; set; }
        public Nullable<int> userId { get; set; }
        public Nullable<int> totalPrice { get; set; }
        public Nullable<int> discount { get; set; }
        public Nullable<int> finalPrice { get; set; }
        public Nullable<bool> isPay { get; set; }
        public string paypal_Id { get; set; }
    }
}

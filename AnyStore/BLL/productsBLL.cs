using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnyStore.BLL
{
    class productsBLL
    {
        //Getters and Setters for Product Module
        public int id { get; set; }
        public string name { get; set; }
        public string Categoria { get; set; }
        public string description { get; set; }
        public decimal rate { get; set; }
        public decimal qty { get; set; }
        public decimal StockMinimum { get; set; }
        public decimal PriceMinimum { get; set; }
        public decimal Gain { get; set; }
        public DateTime added_date { get; set; }
        public int added_by { get; set; }
        public string warehouse { get; set; }

    }
}

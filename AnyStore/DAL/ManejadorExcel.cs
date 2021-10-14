using SpreadsheetLight;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AnyStore.DAL
{
    public class ManejadorExcel
    {
        public bool ExportarExcel()
        {
            //string rutaExe = Assembly.GetEntryAssembly().Location;
            //SLDocument document = new SLDocument(Directory.GetParent(rutaExe).FullName + "/Recursos/Modelo Factura.xlsx");  
            return true;
        }
    }
}

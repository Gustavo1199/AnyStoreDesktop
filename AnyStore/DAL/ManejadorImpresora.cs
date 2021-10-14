using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Configuration;

namespace AnyStore.DAL
{
    public class ManejadorImpresora
    {
        private static int Width = 380;
        private int Height = 10;
        private int LongitudSaltoLinea = 170;

        public void Imprimir(PrintPageEventArgs Event, DataGridView DGV, string Total, string Descuento, string Itbis, string SubTotal, PrintDocument Document, DataTable TransSelec)
        {
            List<ProductoDGV> productos = null;
            if (DGV != null)
                productos = CrearListaProductosDGV(DGV);
            if (TransSelec != null)
                productos = CrearListaProductosDGV(TransSelec);
            CrearHeader(Event, "RM MOTOREPUESTO");                        
            if (!string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["Telefono"].ToString()))
            {
                CrearTexto(Event, "Telefono: " + ConfigurationManager.AppSettings["Telefono"].ToString() , 16, "L");
                Height += 30;
            }
           
            CrearTexto(Event, "Fecha:" + DateTime.Now, 16, "L");
            Height += 30;
            CrearSeparacion(Event);
            CrearListaProductos(Event, productos);
            decimal SubTtl = decimal.Parse(SubTotal);
            if (Itbis != null && Itbis != "" && Itbis != "0")
            {
                decimal ITB = SubTtl * (decimal.Parse(Itbis) / 100);
                SubTtl += ITB;
                CrearTexto(Event, "ITBIS:" + ITB, 16, "R");
                Height += 25;
            }
            if (Descuento != null && Descuento != "")
            {
                decimal DES = SubTtl * (decimal.Parse(Descuento) / 100);
                CrearTexto(Event, "Descuento:" + DES, 16, "R");
                Height += 25;
            }
            if (SubTotal != null && SubTotal != Total)
            {
                CrearTexto(Event, "Sub-Total:" + SubTotal, 16, "R");
                Height += 25;
            }
            if (Total != null)
            {
                CrearTexto(Event, "Total:" + Total, 16, "R");
                Height += 25;
            }
            CrearTexto(Event, "Gracias por su compra!", 16, "R");
            Height += 30;
            CrearTexto(Event, "Santo Domingo, Republica Dominicana", 16, "L");
            Height += 25;
            CrearTexto(Event, "Calle Dr. Defilló #221, Los Praditos", 16, "L");
            Height += 20;
            Document.DefaultPageSettings.PaperSize = new PaperSize("custom", Width + 60, Height + 100);
        }

        public void CrearHeader(PrintPageEventArgs Event, string titulo)
        {
            Event.Graphics.DrawString(titulo, ObtenerFuente(18), Brushes.Black, ObtenerPuntoMedio(titulo));
            Height += 50;
        }
        public void CrearTexto(PrintPageEventArgs Event, string Texto, int Size, string Alineacion)
        {
            if (Alineacion == "R")
            {
                Event.Graphics.DrawString(Texto, ObtenerFuente(Size), Brushes.Black, ObtenerPuntoMargenRight(Texto, Size));
            }
            if (Alineacion == "L")
            {
                Event.Graphics.DrawString(Texto, ObtenerFuente(Size), Brushes.Black, ObtenerPuntoMargenLeft());
            }
        }
        public Font ObtenerFuente(int size)
        {
            return new Font("Arial", size, FontStyle.Regular);
        }
        public Point ObtenerPuntoMedio(string texto)
        {
            int X = ((Width / 2) - (texto.Length / 2)) - 20;
            return new Point(X, Height);
        }
        public Point ObtenerPuntoMargenLeft()
        {
            return new Point(0, Height);
        }
        public Point ObtenerPuntoMargenRight(string texto, int Size)
        {
            int X = Width - (texto.Length * (Size / 2));
            return new Point(X, Height);
        }
        public void CrearFila(Fila Fila, PrintPageEventArgs Event)
        {
            foreach(Fila FilaElement in SepararFilas(new List<Fila> { Fila }))
            {
                if(FilaElement.Left != null)
                {
                    CrearTexto(Event, FilaElement.Left.Texto, FilaElement.Left.Size, FilaElement.Left.Alineacion);

                }
                if (FilaElement.Right != null)
                {
                    CrearTexto(Event, FilaElement.Right.Texto, FilaElement.Right.Size, FilaElement.Right.Alineacion);
                }

                Height += 20;


            }
        }
        public List<Fila> SepararFilas(List<Fila> Filas)
        {
            Fila Creada = null;
            foreach (Fila Fila in Filas)
            {
                if (SaltoLinea(Fila.Right.Texto.Length, Fila.Right.Size))
                {
                    Creada = (Fila)Fila.Clone();
                    int LengthTaken = GetLengthSaltoFila(Fila.Right.Texto.Length, Fila.Right.Size);
                    Creada.Left = null;
                    Creada.Right.Texto = Creada.Right.Texto.Substring(LengthTaken, Creada.Right.Texto.Length - LengthTaken);
                    Fila.Right.Texto = Fila.Right.Texto.Substring(0, LengthTaken);
                    break;
                }
            }
            if(Creada != null)
            {
                Filas.Add(Creada);
                Filas = SepararFilas(Filas);
            }
            return Filas;
        }

        public int GetLengthSaltoFila(int lenght, int size)
        {
            return (LongitudSaltoLinea / (size / 2));
        }
        public void CrearSeparacion(PrintPageEventArgs Event)
        {
            for (int i = 0; i <= Width; i += 10)
            {
                Event.Graphics.DrawString("-", ObtenerFuente(16), Brushes.Black, new Point(i, Height));
            }
            Height += 20;
        }
        public void CrearListaProductos(PrintPageEventArgs Event, List<ProductoDGV> productos)
        {
            ElementoFila NombreProductoCampo = new ElementoFila
            {
                Texto = "Nombre Producto:",
                Alineacion = "L",
                Size = 16
            };
            ElementoFila PrecioCampo = new ElementoFila
            {
                Texto = "Precio:",
                Alineacion = "L",
                Size = 16
            };
            ElementoFila CantidadCampo = new ElementoFila
            {
                Texto = "Cantidad:",
                Alineacion = "L",
                Size = 16
            };
            foreach (ProductoDGV producto in productos)
            {
                ElementoFila NombreProducto = new ElementoFila
                {
                    Texto = producto.NombreProducto.ToLower(),
                    Alineacion = "R",
                    Size = 16
                };
                ElementoFila Precio = new ElementoFila
                {
                    Texto = producto.Precio,
                    Alineacion = "R",
                    Size = 16
                };
                ElementoFila Cantidad = new ElementoFila
                {
                    Texto = producto.Cantidad,
                    Alineacion = "R",
                    Size = 16
                };
                Fila FilaNombre = new Fila { Left = NombreProductoCampo, Right = NombreProducto };
                Fila FilaPrecio = new Fila { Left = PrecioCampo, Right = Precio };
                Fila FilaCantidad = new Fila { Left = CantidadCampo, Right = Cantidad };
                CrearFila(FilaNombre, Event);
                CrearFila(FilaPrecio, Event);
                CrearFila(FilaCantidad, Event);
                CrearSeparacion(Event);
            }
        }
        public List<ProductoDGV> CrearListaProductosDGV(DataGridView DGV)
        {
            List<ProductoDGV> productos = new List<ProductoDGV> { };
            foreach (DataGridViewRow row in DGV.Rows)
            {
                if (row.Index == DGV.Rows.Count - 1)
                    break;
                productos.Add(new ProductoDGV
                {
                    NombreProducto = row.Cells[0].Value.ToString(),
                    Precio = row.Cells[1].Value.ToString(),
                    Cantidad = row.Cells[2].Value.ToString()
                });
            }
            return productos;
        }
        public List<ProductoDGV> CrearListaProductosDGV(DataTable DGV)
        {
            List<ProductoDGV> productos = new List<ProductoDGV> { };
            for(int i = 0; i < DGV.Rows.Count; i++)
            {
                productos.Add(new ProductoDGV
                {
                    NombreProducto = DGV.Rows[i].ItemArray[0].ToString(),
                    Precio = DGV.Rows[i].ItemArray[1].ToString(),
                    Cantidad = DGV.Rows[i].ItemArray[2].ToString()
                });
            }
               
            
            return productos;
        }
        public bool SaltoLinea(int Length, int Size)
        {
            if (Length * (Size / 2) > LongitudSaltoLinea)
                return true;
            else
                return false;
        }
    }
    public class ElementoFila
    {
        public string Texto { get; set; }
        public string Alineacion { get; set; }
        public int Size { get; set; }
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
    public class ProductoDGV
    {
        public string NombreProducto { get; set; }
        public string Precio { get; set; }
        public string Cantidad { get; set; }
    }
    public class Fila
    {
        public ElementoFila Left { get; set; }
        public ElementoFila Right { get; set; }
        public object Clone()
        {
            Fila other = (Fila)this.MemberwiseClone();
            other.Left = (ElementoFila)Left.Clone();
            other.Right = (ElementoFila)Right.Clone();
            return other;
        }
    }
    internal static class RawPrinterHelper
    {
        [StructLayout(LayoutKind.Sequential)]
        private struct DOCINFO
        {
            [MarshalAs(UnmanagedType.LPWStr)]
            public string pDocName;

            [MarshalAs(UnmanagedType.LPWStr)]
            public string pOutputFile;

            [MarshalAs(UnmanagedType.LPWStr)]
            public string pDataType;

            public DOCINFO(string documentName) : this()
            {
                if (string.IsNullOrEmpty(documentName))
                {
                    documentName = "RAW Document";
                }

                pDocName = documentName;
                pDataType = "RAW";
            }
        }

        [DllImport("winspool.drv", SetLastError = true, CharSet = CharSet.Unicode, ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        private static extern bool OpenPrinter([MarshalAs(UnmanagedType.LPWStr)] string szPrinter, out SafePrinterHandle hPrinter, IntPtr pd);

        [DllImport("winspool.drv", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern bool ClosePrinter(IntPtr hPrinter);

        [DllImport("winspool.drv", SetLastError = true, CharSet = CharSet.Unicode, ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        private static extern bool StartDocPrinter(SafePrinterHandle hPrinter, int level, ref DOCINFO di);

        [DllImport("winspool.drv", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern bool EndDocPrinter(SafePrinterHandle hPrinter);

        [DllImport("winspool.drv", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern bool StartPagePrinter(SafePrinterHandle hPrinter);

        [DllImport("winspool.drv", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern bool EndPagePrinter(SafePrinterHandle hPrinter);

        [DllImport("winspool.drv", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern bool WritePrinter(SafePrinterHandle hPrinter, [MarshalAs(UnmanagedType.LPStr)] string data, int dwCount, out int dwWritten);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        private static extern int FormatMessage(int dwFlags, IntPtr lpSource, int dwMessageId, int dwLanguageId, StringBuilder lpBuffer, int nSize, IntPtr vaListArguments);

        private static string GetErrorMessage(int errorCode)
        {
            var lpBuffer = new StringBuilder(0x200);
            if (FormatMessage(0x3200, IntPtr.Zero, errorCode, 0, lpBuffer, lpBuffer.Capacity, IntPtr.Zero) != 0)
            {
                return lpBuffer.ToString();
            }

            return $"Unknown error: {errorCode}";
        }

        private static void ThrowLastWin32Error(string path = null)
        {
            int errorCode = Marshal.GetLastWin32Error();
            if (errorCode != 0)
            {
                int hr = Marshal.GetHRForLastWin32Error();
                if (0 <= hr) throw new Win32Exception(errorCode);

                switch (errorCode)
                {
                    case 2: // File not found
                        {
                            if (string.IsNullOrEmpty(path)) throw new FileNotFoundException();
                            throw new FileNotFoundException(null, path);
                        }
                    case 3: // Directory not found
                        {
                            if (string.IsNullOrEmpty(path)) throw new DirectoryNotFoundException();
                            throw new DirectoryNotFoundException($"The directory '{path}' was not found.");
                        }
                    case 5: // Access denied
                        {
                            if (string.IsNullOrEmpty(path)) throw new UnauthorizedAccessException();
                            throw new UnauthorizedAccessException($"Access to the path '{path}' was denied.");
                        }
                    case 15: // Drive not found
                        {
                            if (string.IsNullOrEmpty(path)) throw new DriveNotFoundException();
                            throw new DriveNotFoundException($"Could not find the drive '{path}'. The drive might not be ready or might not be mapped.");
                        }
                    case 32: // Sharing violation
                        {
                            if (string.IsNullOrEmpty(path)) throw new IOException(GetErrorMessage(errorCode), hr);
                            throw new IOException($"The process cannot access the file '{path}' because it is being used by another process.", hr);
                        }
                    case 80: // File already exists
                        {
                            if (!string.IsNullOrEmpty(path))
                            {
                                throw new IOException($"The file '{path}' already exists.", hr);
                            }
                            break;
                        }
                    case 87: // Invalid parameter
                        {
                            throw new IOException(GetErrorMessage(errorCode), hr);
                        }
                    case 183: // File or directory already exists
                        {
                            if (!string.IsNullOrEmpty(path))
                            {
                                throw new IOException($"The file or directory '{path}' already exists.", hr);
                            }
                            break;
                        }
                    case 206: // Path too long
                        {
                            throw new PathTooLongException();
                        }
                    case 995: // Operation cancelled
                        {
                            throw new OperationCanceledException();
                        }
                    case 1801: // Invalid printer name
                        {
                            throw new ArgumentException($"The printer '{path}' was not found.", "printerName");
                        }
                    default:
                        {
                            Marshal.ThrowExceptionForHR(hr);
                            break;
                        }
                }
            }
        }

        private sealed class SafePrinterHandle : SafeHandle
        {
            private SafePrinterHandle() : base(IntPtr.Zero, true)
            {
            }

            public override bool IsInvalid
            {
                get { return handle == IntPtr.Zero; }
            }

            protected override bool ReleaseHandle()
            {
                return ClosePrinter(handle);
            }

            public static SafePrinterHandle Open(string printerName)
            {
                SafePrinterHandle result;
                if (!OpenPrinter(printerName, out result, IntPtr.Zero))
                {
                    ThrowLastWin32Error(printerName);
                }

                return result;
            }

            public IDisposable StartDocument(string name)
            {
                if (IsInvalid || IsClosed) return null;

                var di = new DOCINFO(name);
                if (!StartDocPrinter(this, 1, ref di))
                {
                    ThrowLastWin32Error();
                }

                return new PrinterDocument(this);
            }

            public IDisposable StartPage()
            {
                if (IsInvalid || IsClosed) return null;

                if (!StartPagePrinter(this))
                {
                    ThrowLastWin32Error();
                }

                return new PrinterPage(this);
            }

            public void Write(string data)
            {
                if (IsInvalid) throw new InvalidOperationException();
                if (IsClosed) throw new ObjectDisposedException(nameof(SafePrinterHandle));

                int bytesWritten;
                if (!WritePrinter(this, data, data.Length, out bytesWritten))
                {
                    ThrowLastWin32Error();
                }
            }
        }

        private sealed class PrinterDocument : IDisposable
        {
            private SafePrinterHandle _printer;

            public PrinterDocument(SafePrinterHandle printer)
            {
                _printer = printer;
            }

            public void Dispose()
            {
                var printer = Interlocked.Exchange(ref _printer, null);
                if (printer != null) EndDocPrinter(printer);
            }
        }

        private sealed class PrinterPage : IDisposable
        {
            private SafePrinterHandle _printer;

            public PrinterPage(SafePrinterHandle printer)
            {
                _printer = printer;
            }

            public void Dispose()
            {
                var printer = Interlocked.Exchange(ref _printer, null);
                if (printer != null) EndPagePrinter(printer);
            }
        }

        /// <summary>
        /// Sends the specified raw string to the printer.
        /// </summary>
        /// <param name="printerName">
        /// The name of the printer which receives the string.
        /// </param>
        /// <param name="valueToSend">
        /// The string to send.
        /// </param>
        /// <param name="documentName">
        /// The document name to use, if any.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="printerName"/> is <see langword="null"/> or <see cref="string.Empty"/>.</para>
        /// <para>-or-</para>
        /// <para><paramref name="valueToSend"/> is <see langword="null"/> or <see cref="string.Empty"/>.</para>
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <para>The specified printer was not found.</para>
        /// </exception>
        /// <exception cref="Win32Exception">
        /// There was an error sending the data to the printer.
        /// </exception>
        public static void SendStringToPrinter(string printerName, string valueToSend, string documentName)
        {
            if (string.IsNullOrWhiteSpace(printerName)) throw new ArgumentNullException(nameof(printerName));
            if (string.IsNullOrEmpty(valueToSend)) throw new ArgumentNullException(nameof(valueToSend));

            using (var printer = SafePrinterHandle.Open(printerName))
            using (printer.StartDocument(documentName))
            using (printer.StartPage())
            {
                printer.Write(valueToSend);
            }
        }
    }
}

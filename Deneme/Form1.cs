using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Printing;
using MessagingToolkit.QRCode.Codec;
using Newtonsoft.Json.Linq;
using MessagingToolkit.Barcode;
using System.Drawing.Text;
using ZXing;

namespace Deneme
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

        }
        //BarcodeEncoder Generator;
        private void Form1_Load(object sender, EventArgs e)
        {
            if (PrinterSettings.InstalledPrinters.Count <= 0)
            {
                MessageBox.Show("Printer not found!");
                return;
            }

            //Get all available printers and add them to the combo box  
            foreach (String printer in PrinterSettings.InstalledPrinters)
            {
                printersList.Items.Add(printer.ToString());
            }

            printersList.SelectedIndex = 0;
        }

        const int StartY = 10;
        const int StartX = 0;
        const int QRSIZE = 80;
        const int BARCODE1D = 40;
        const int PADDING = 20;
        const int PAGEWIDTH = 400;

        public void pd_PrintPage(object sender, PrintPageEventArgs ev)
        {
            Graphics g = ev.Graphics;
            Graphics gr1 = ev.Graphics;
            Font bigFont = new Font("Arial Black", 14, FontStyle.Bold);
            Font smallFont = new Font("Arial", 12);
            SolidBrush brush = new SolidBrush(Color.Black);

            int currentY = StartY;
            int currentX = StartX;


            ////1d Barkod Code128BarcodeDraw Code128WithChecksum
            //Zen.Barcode.Code128BarcodeDraw brCode = Zen.Barcode.BarcodeDrawFactory.Code128WithChecksum;
            string brData = "8698777951112";
            //Image brImage = brCode.Draw(brData.Trim(), 100);
            //brImage.RotateFlip(RotateFlipType.Rotate90FlipXY);
            //g.DrawImage(brImage, new Rectangle(10, 30, 40, 120));

            BarcodeWriter writer = new BarcodeWriter()
            {
                Format = ZXing.BarcodeFormat.CODE_128
            };
            Image brImage = writer.Write(brData.Trim());
            brImage.RotateFlip(RotateFlipType.Rotate90FlipXY);
            g.DrawImage(brImage,new Rectangle(10, 30, 50, 120));

            /*MEssaingQRToolkit ile yapıldı.*/
            //Generator = new BarcodeEncoder();
            //Generator.IncludeLabel = true;
            //string brData = "b8698777951112b";
            //Generator.CustomLabel = brData;
            //Image brCode = new Bitmap(Generator.Encode(BarcodeFormat.Codabar, brData));
            //brCode.RotateFlip(RotateFlipType.Rotate90FlipXY);
            //g.DrawImage(brCode, new Rectangle(0, 40, 40, 120));

            //Stok Kodu
            Size textSize = TextRenderer.MeasureText(brData.Trim(), smallFont);
            //g.DrawString( "Stok Kodu: ", bigFont, brush, new Rectangle( currentX, currentY, textSize.Width, textSize.Height ) );
            //currentX += textSize.Width;

            textSize = TextRenderer.MeasureText(code.Text.Trim(), bigFont);
            g.DrawString(code.Text.Trim(), bigFont, brush, new Rectangle(currentX+60, currentY, PAGEWIDTH - QRSIZE - currentX, textSize.Height));


            //Stok İsmi
            currentX = StartX;
            currentY += textSize.Height + 10;

            StringFormat format = new StringFormat();
            format.Trimming = StringTrimming.Character;

            textSize = TextRenderer.MeasureText(name.Text.Trim(), smallFont);
            g.DrawString(name.Text.Trim(), smallFont, brush, new Rectangle(currentX+60, currentY, PAGEWIDTH - QRSIZE - currentX, textSize.Height * 4), format);


            //Amb.Tarihi
            currentX = StartX;
            currentY += (textSize.Height * 2) + 10;
            g.DrawString(date.Value.ToString("dd.MM.yyyy"), smallFont, brush, new Rectangle(PAGEWIDTH - QRSIZE, 20, PAGEWIDTH - QRSIZE - currentX, textSize.Height));

            //Etiket MADE IN TURKEY
            currentX = StartX;
            currentY += textSize.Height + 10;
            //g.DrawString( "Amb.Tarihi:", bigFont, brush, new Rectangle( currentX, currentY, textSize.Width, textSize.Height ) );
            //currentX += textSize.Width;

            textSize = TextRenderer.MeasureText(date.Value.ToString(), bigFont);
            g.DrawString("MADE IN TURKEY", bigFont, brush, new Rectangle(currentX+60, currentY, PAGEWIDTH - QRSIZE - currentX, textSize.Height * 4),format);

            //Numara String Value

            g.DrawString(brData, smallFont, brush, new Rectangle(currentX + 60, currentY+100, PAGEWIDTH - QRSIZE - currentX, textSize.Height));

            //QR
            Image qrImage = generateQrCode(qrData.Text.Trim(), 3);
            g.DrawImage(qrImage, new Rectangle(PAGEWIDTH - QRSIZE, 40, QRSIZE, QRSIZE));

            //EAC
            g.DrawString("EAC", bigFont, brush, new Rectangle(PAGEWIDTH - QRSIZE, currentY, QRSIZE, QRSIZE));
            //Etiket boyutu
            //g.DrawRectangle( pen, new Rectangle( 0, 60, 408, 170 ) );
        }

        private Image generateCodeBar(string data, int ddlvl)
        {
            QRCodeEncoder codebar = new QRCodeEncoder();
            codebar.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
            codebar.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.L;
            codebar.QRCodeVersion = ddlvl;

            Bitmap bm = codebar.Encode(data);
            return bm;

        }

        private Image generateQrCode(string data, int kkLevel)
        {
            QRCodeEncoder qe = new QRCodeEncoder();
            qe.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
            qe.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.L;
            qe.QRCodeVersion = kkLevel;
            Bitmap bm = qe.Encode(data);
            return bm;
        }

        private async void iTalk_Button_21_Click(object sender, EventArgs e)
        {
            if (code.Text != "" && name.Text != "" && qrData.Text != "")
            {
                PrintDocument pd = new PrintDocument();
                pd.PrinterSettings.PrinterName = printersList.SelectedItem.ToString();

                pd.PrintPage += new PrintPageEventHandler(pd_PrintPage);
                pd.Print();
                pd.Print();
                Helper h = new Helper();
                String resultString = await h.saveProduct(code.Text.Trim(), qrData.Text.Trim());
                var result = JObject.Parse(resultString);
                if ((String)result["status"] == "success")
                {
                    MessageBox.Show((String)result["message"], "Durum");
                }
            }
            else
            {
                MessageBox.Show("Lütfen alanların hepsini doldurunuz.");
            }

        }

        private async void iTalk_Button_11_Click(object sender, EventArgs e)
        {
            date.Value = DateTime.Now;
            Helper h = new Helper();
            String resultString = await h.getProduct(code.Text.Trim());
            var result = JObject.Parse(resultString);
            if ((String)result["status"] == "success")
            {
                name.Text = (String)result["data"]["product_title"];
            }
        }

        private void iTalk_Button_22_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void iTalk_Button_12_Click(object sender, EventArgs e)
        {
            PrintDocument pd = new PrintDocument();
            pd.PrinterSettings.PrinterName = printersList.SelectedItem.ToString();
            pd.PrintPage += new PrintPageEventHandler(pd_PrintPage);
            //pd.DefaultPageSettings.PaperSize = new PaperSize("First custom size",800,800);
            printPreviewDialog1.Document = pd;
            printPreviewDialog1.ShowDialog();
            //pd.Print();

        }
    }
}

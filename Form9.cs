using ABC_Car_Traders.ABC_Car_TradersDataSetTableAdapters;
using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ABC_Car_Traders
{
    public partial class GenerateReports : Form
    {
        public GenerateReports()
        {
            InitializeComponent();
        }

        private void GenerateReports_Load(object sender, EventArgs e, ABC_Car_TradersDataSet ABC_Car_TradersDataSet, CarTableAdapter carTableAdapter)
        {
            carTableAdapter.Fill(ABC_Car_TradersDataSet.Car);

            reportViewer1.LocalReport.ReportEmbeddedResource = "ReportViewerDemo.Report1.rdlc";
            reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", ABC_Car_TradersDataSet.Tables[0]));

            this.reportViewer1.RefreshReport();
            this.reportViewer1.RefreshReport();
        }

        private void GenerateReports_Load(object sender, EventArgs e)
        {

        }
    }
}

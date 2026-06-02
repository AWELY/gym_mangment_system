using System;
using System.Data.SqlClient;
using System.Windows.Forms;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.Windows.Forms;

namespace gym_mangment_system
{
    /// <summary>
    /// Preview/print window for the GymMembers Crystal Report (all members).
    /// The viewer's toolbar provides the print button, so the user can preview
    /// the full members list and send it to any printer.
    /// </summary>
    public sealed class MembersReportForm : Form
    {
        private readonly GymMembers _report = new GymMembers();

        public MembersReportForm()
        {
            Text = "تقرير الأعضاء";
            StartPosition = FormStartPosition.CenterParent;
            ClientSize = new System.Drawing.Size(1000, 700);
            RightToLeft = RightToLeft.Yes;
            RightToLeftLayout = true;

            var viewer = new CrystalReportViewer
            {
                Dock = DockStyle.Fill,
                ShowGroupTreeButton = false,
                ToolPanelView = ToolPanelViewType.None
            };

            ApplyDatabaseLogon(_report);

            viewer.ReportSource = _report;
            Controls.Add(viewer);
        }

        /// <summary>
        /// Points the report at the application's current SQL Server connection so it
        /// reads every member straight from the database, regardless of where the
        /// report was originally designed.
        /// </summary>
        private static void ApplyDatabaseLogon(ReportDocument report)
        {
            string connectionString = Db.ConnectionString;
            if (string.IsNullOrWhiteSpace(connectionString))
                return;

            var builder = new SqlConnectionStringBuilder(connectionString);

            var connectionInfo = new ConnectionInfo
            {
                ServerName = builder.DataSource,
                DatabaseName = builder.InitialCatalog
            };

            if (builder.IntegratedSecurity)
            {
                connectionInfo.IntegratedSecurity = true;
            }
            else
            {
                connectionInfo.UserID = builder.UserID;
                connectionInfo.Password = builder.Password;
            }

            foreach (Table table in report.Database.Tables)
            {
                TableLogOnInfo logOnInfo = table.LogOnInfo;
                logOnInfo.ConnectionInfo = connectionInfo;
                table.ApplyLogOnInfo(logOnInfo);
            }
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
            _report.Close();
            _report.Dispose();
        }
    }
}

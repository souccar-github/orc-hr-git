using NHibernateDBGenerator.NHibernate.Enumerations;
using NHibernateDBGenerator.NHibernate.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Souccar.NHibernateManager
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void butDrop_Click(object sender, EventArgs e)
        {
            foreach (var item in ConfigurationSettings.AppSettings)
            {
                var ss=item.ToString();
            }
            //FluentSessionProvider.EnableAllDatabaseConstraints();
            var pass = FluentSessionProvider.GenerateSchema(GenerateSchemaMode.Drop);
        }

        private void butCreate_Click(object sender, EventArgs e)
        {
             FluentSessionProvider.GenerateSchema(GenerateSchemaMode.Create);
        }

        private void butUpdate_Click(object sender, EventArgs e)
        {
             FluentSessionProvider.GenerateSchema(GenerateSchemaMode.Update);
        }
    }
}

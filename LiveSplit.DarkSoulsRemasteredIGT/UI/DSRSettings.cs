using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

/**
 * Pretty much a copy of the SourceSplit settings :
 * https://github.com/fatalis/SourceSplit/blob/master/SourceSplitSettings.cs
 */ 
namespace LiveSplit.DarkSoulsRemasteredIGT
{
    internal partial class DSRSettings : UserControl
    {
        private const bool DEFAULT_INVENTORY_RESET_ENABLED = false;

        public bool InventoryResetEnabled { get; set; }

        public DSRSettings()
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;

            this.cbxInventoryReset.DataBindings.Add("Checked", this, nameof(InventoryResetEnabled), false, DataSourceUpdateMode.OnPropertyChanged);
            this.InventoryResetEnabled = DEFAULT_INVENTORY_RESET_ENABLED;
        }

        public XmlNode GetSettings(XmlDocument document)
        {
            XmlElement settingsNode = document.CreateElement("Settings");
            settingsNode.AppendChild(ToElement(document, nameof(this.InventoryResetEnabled), this.InventoryResetEnabled));
            return settingsNode;
        }

        public void SetSettings(XmlNode settings)
        {
            this.InventoryResetEnabled = settings[nameof(this.InventoryResetEnabled)] != null ?
               (Boolean.TryParse(settings[nameof(this.InventoryResetEnabled)].InnerText, out bool bval) ? bval : DEFAULT_INVENTORY_RESET_ENABLED)
               : DEFAULT_INVENTORY_RESET_ENABLED;
        }

        static XmlElement ToElement<T>(XmlDocument document, string name, T value)
        {
            XmlElement str = document.CreateElement(name);
            str.InnerText = value.ToString();
            return str;
        }
    }
}

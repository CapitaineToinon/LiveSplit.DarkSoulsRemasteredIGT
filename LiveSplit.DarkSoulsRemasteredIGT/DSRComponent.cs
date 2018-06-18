using LiveSplit.Model;
using LiveSplit.UI;
using LiveSplit.UI.Components;
using System;
using System.Xml;

namespace LiveSplit.DarkSoulsRemasteredIGT
{
    public class DSRComponent : LogicComponent
    {
        private LiveSplitState state;

        private DSRProcess gameProcess;
        private DSRIGT dsigt;
        private DSRInventoryReset inventoryReset;
        private DSRSettings settings;

        public override string ComponentName => "Dark Souls Remastered In-Game Timer";

        public DSRComponent(LiveSplitState state)
        {
            gameProcess = new DSRProcess();
            dsigt = new DSRIGT(gameProcess);
            inventoryReset = new DSRInventoryReset(gameProcess);
            settings = new DSRSettings();

            this.state = state;
            this.state.IsGameTimePaused = true;
            this.state.OnReset += (sender, value) => { Reset(); } ;
            this.state.OnStart += (sender, e) => { Reset(); };
        }

        private void Reset()
        {
            dsigt.Reset();

            if (settings.InventoryResetEnabled)
            {
                inventoryReset.ResetInventory();
            }
        }

        public override void Dispose()
        {
            gameProcess.Unhook();
        }

        public override XmlNode GetSettings(XmlDocument document)
        {
            return this.settings.GetSettings(document);
        }

        public override System.Windows.Forms.Control GetSettingsControl(LayoutMode mode)
        {
            return this.settings;
        }

        public override void SetSettings(XmlNode settings)
        {
            this.settings.SetSettings(settings);
        }

        public override void Update(IInvalidator invalidator, LiveSplitState state, float width, float height, LayoutMode mode)
        {
            state.IsGameTimePaused = true; // Avoids flickering 
            state.SetGameTime(new TimeSpan(0, 0, 0, 0, dsigt.IGT));
        }
    }
}

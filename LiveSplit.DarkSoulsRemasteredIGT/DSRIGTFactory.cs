using LiveSplit.DarkSoulsRemasteredIGT;
using LiveSplit.Model;
using LiveSplit.UI.Components;
using System;
using System.Reflection;

[assembly: ComponentFactory(typeof(DSRIGTFactory))]
namespace LiveSplit.DarkSoulsRemasteredIGT
{
    internal class DSRIGTFactory : IComponentFactory
    {
        public string ComponentName => "Dark Souls Remastered In-Game Timer";
        public string Description => "Dark Souls Remastered In-Game Timer by CapitaineToinon";
        public string UpdateName => ComponentName;

        public ComponentCategory Category => ComponentCategory.Timer;
        public string UpdateURL => "https://raw.githubusercontent.com/CapitaineToinon/LiveSplit.DarkSoulsRemasteredIGT/livesplit-update";
        public string XMLURL => UpdateURL + "LiveSplit.DarkSoulsRemasteredIGT/Components/update.LiveSplit.DarkSoulsRemastered.xml";

        public Version Version => Assembly.GetExecutingAssembly().GetName().Version;

        public IComponent Create(LiveSplitState state)
        {
            return new DSRIGTComponent(state);
        }
    }
}

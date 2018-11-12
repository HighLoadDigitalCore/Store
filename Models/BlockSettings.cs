using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Smoking.Models
{
    public class BlockSettings
    {

        private static List<BlockSettings> _settings = new List<BlockSettings>()
        {
            new BlockSettings()
            {
                BlockName = "TextEditor",
                Settings = new List<BlockSettingsData>()
                {
                    new BlockSettingsData()
                    {
                        Description = "Кнопки свернуть/развернуть",
                        Field = "ShowExpandCollapse",
                        OrderNum = 1,
                        Editor = "CheckBox"
                    }
                }
            }
        };

        public static BlockSettings GetSettings(string blockName, int viewID)
        {
            var block = _settings.FirstOrDefault(x => x.BlockName == blockName);
            if (block == null)
                return null;

            var db = new DB();
            var settings = db.CMSPageCellViewSettings.Where(x => x.ViewID == viewID).ToList();
            foreach (var data in block.Settings)
            {
                var item = settings.FirstOrDefault(x => x.Name == data.Field);
                data.StoredValue = item == null ? "" : item.Value;
            }
            return block;
        }

        public string BlockName { get; set; }
        public List<BlockSettingsData> Settings { get; set; }

        public string GetValue(string fieldName)
        {
            var model = Settings.FirstOrDefault(x => x.Field == fieldName);
            if (model == null)
                return "";
            return model.StoredValue;
        }

        
    }

    public class BlockSettingsData  
    {
        public string Field { get; set; }
        public string Description { get; set; }
        public string StoredValue { get; set; }
        public int OrderNum { get; set; }
        public string Editor { get; set; }
    }
}
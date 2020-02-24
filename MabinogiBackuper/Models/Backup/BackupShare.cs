using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MabinogiBackuperLib.Boolean;

namespace MabinogiBackuper.Models.Backup
{
    public class BackupShare
    {
        public string SavedPath { get; set; }

        public bool ContainsDrawChat { get; set; }
        public bool ContainsScreenshot { get; set; }
        public bool ContainsPetAi { get; set; }
        public bool ContainsKeyAlerm { get; set; }
        public bool ContainsInteraction { get; set; }
        public bool ContainsMovie { get; set; }


        public int CheckedCount()
        {
            var counter = new BoolCounter();
            counter.Change(nameof(ContainsDrawChat), ContainsDrawChat);
            counter.Change(nameof(ContainsScreenshot), ContainsScreenshot);
            counter.Change(nameof(ContainsPetAi), ContainsPetAi);
            counter.Change(nameof(ContainsKeyAlerm), ContainsKeyAlerm);
            counter.Change(nameof(ContainsInteraction), ContainsInteraction);
            counter.Change(nameof(ContainsMovie), ContainsMovie);
            return counter.Count();
        }

        public List<string> CheckedPathList()
        {
            var list = new List<string>();

            if (ContainsDrawChat)
                list.Add("\\マビノギ\\お絵描きチャット");
            if (ContainsScreenshot)
                list.Add("\\マビノギ\\スクリーンショット");
            if (ContainsPetAi)
                list.Add("\\マビノギ\\ペットキャラクター AI");
            if (ContainsKeyAlerm)
                list.Add("\\マビノギ\\設定");
            if (ContainsInteraction)
                list.Add("\\マビノギ\\インタラクション");
            if (ContainsMovie)
                list.Add("\\マビノギ\\動画");

            return list;
        }
    }
}

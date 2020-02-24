using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MabinogiBackuperLib.Backup;
using MabinogiBackuperLib.Boolean;

namespace MabinogiBackuper.Models.Backup
{
    public class BackupShare
    {
        private bool _containsDrawChat;
        private bool _containsScreenshot;
        private bool _containsPetAi;
        private bool _containsKeyAlerm;
        private bool _containsInteraction;
        private bool _containsMovie;

        public string SavedPath { get; set; }

        public bool IsChanged { get; set; }

        public bool ContainsDrawChat
        {
            get => _containsDrawChat;
            set
            {
                _containsDrawChat = value;
                IsChanged = true;
            }
        }

        public bool ContainsScreenshot
        {
            get => _containsScreenshot;
            set
            {
                _containsScreenshot = value;
                IsChanged = true;
            }
        }

        public bool ContainsPetAi
        {
            get => _containsPetAi;
            set
            {
                _containsPetAi = value;
                IsChanged = true;
            }
        }

        public bool ContainsKeyAlerm
        {
            get => _containsKeyAlerm;
            set
            {
                _containsKeyAlerm = value;
                IsChanged = true;
            }
        }

        public bool ContainsInteraction
        {
            get => _containsInteraction;
            set
            {
                _containsInteraction = value;
                IsChanged = true;
            }
        }

        public bool ContainsMovie
        {
            get => _containsMovie;
            set
            {
                _containsMovie = value;
                IsChanged = true;
            }
        }

        public bool IsAnalyzed { get; set; }
        public ulong AnalyzedSize { get; set; }
        public Backupper Backupper { get; } = new Backupper();


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

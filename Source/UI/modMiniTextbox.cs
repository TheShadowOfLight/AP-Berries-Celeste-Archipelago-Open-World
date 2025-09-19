﻿using Monocle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Celeste.Mod.Celeste_Multiworld.UI
{
    [Tracked]
    public class modMiniTextbox : MiniTextbox
    {
        public static void Load()
        {

        }

        public static void Unload()
        {

        }

        public modMiniTextbox(string input, bool bLiterature = false) : base("!AP")
        {
            base.Tag = Tags.HUD | Tags.TransitionUpdate | Tags.Persistent;
            this.portraitSize = 112f;
            this.box = GFX.Portraits["textbox/ap_mini"];
            this.text = FancyText.Parse(input, (int)(1688f - this.portraitSize - 32f), 2, 1f, null, null);

            FancyText.Portrait portrait = new FancyText.Portrait();
            portrait.Sprite = "ap_icon";
            portrait.Animation = "normal";
            this.portraitData = portrait;
            this.portrait = GFX.PortraitsSpriteBank.Create("portrait_ap_icon");
            XmlElement xml = GFX.PortraitsSpriteBank.SpriteData["portrait_ap_icon"].Sources[0].XML;
            this.portraitScale = this.portraitSize / Monocle.Calc.AttrFloat(xml, "size", 160f);
            base.Add(this.portrait);

            base.Add(this.routine = new Monocle.Coroutine(this.Routine(), true));
            this.routine.UseRawDeltaTime = true;

            if (!bLiterature)
            {
                base.Add(new TransitionListener
                {
                    OnOutBegin = delegate
                    {
                        if (!this.closing)
                        {
                            this.routine.Replace(this.Close());
                        }
                    }
                });
            }
            else
            {
                TransitionListener listener = base.Get<TransitionListener>();
                if (listener != null)
                {
                    base.Remove(listener);
                }
            }

            if (Level.DialogSnapshot == null)
            {
                Level.DialogSnapshot = Audio.CreateSnapshot("snapshot:/dialogue_in_progress", false);
            }
            Audio.ResumeSnapshot(Level.DialogSnapshot);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.IO;
using Inv.Support;

namespace Pathos
{
  public sealed class CodexRumours : CodexPage<ManifestRumours, RumourEditor, Rumour>
  {
    private CodexRumours() { }
#if MASTER_CODEX
    internal CodexRumours(Codex Codex)
      : base(Codex.Manifest.Rumours)
    {
      void AddRumours(bool Truth, string MessageText)
      {
        foreach (var Message in MessageText.SplitLines())
        {
          if (!string.IsNullOrWhiteSpace(Message))
          {
            Register.Add(R =>
            {
              R.Truth = Truth;
              R.Message = Message;
            });
          }
        }
      }

      // LIES.
      AddRumours(false, @"
This is a fortune cookie.
Great tragedy falls upon those who eat the yellow snow.
Two wrongs do not make a right, but three rights do make a left.
There can be only ONE!
When reading a scroll of amnesia, remember the golden rule.
This is an automatic response from the College of Wizardry. We have received your message and an advisor will reply shortly.
The swallow may fly south with the sun or the house martin or the plover may seek warmer climes in winter, yet these are not strangers to our land?
To view inventory, press 'backpack'.
When you crave ice cream, your body is telling you that it needs water.
Post withdrawn by author and will be automatically deleted in 24 hours unless flagged.
When in doubt, refrain from reply all.
Learn to say slime mould in Draconic: mould di slime (Mole-duh dee slie-mah).
Learn to say peach tree in Draconic: skjall di peaches (Skah-jawl dee peech-is).
Learn to say cherry blossom in Draconic: selavra di wer cherry (Cell-ave-rah dee wur chah-ray).
Learn to say mortal fools in Draconic: mablik hofibai (Mah-blick hauph-ee-bay).
Eat a giant ant and you may be surprised.
Dip a scroll in a fountain for blessing.
Eat a flea and save a dog.
Acid blobs make you hallucinate.
Yellow mould brings you gold.
You can be charmed to death.
Throw an acid blob at a rust monster.
A cornuthaum is a dunce cap.
Gain the powers of Indiana Jones by wearing a fedora.
A magic whistle tames dragons.
Give your lamp a rub.
Give the lamp a caress and the djinni will undress.
Eat a leprechaun and gain teleport control.
Zap your enemy with a unicorn horn.
Gnomish weapons are always cursed.
Go towards the light.
A wand of nothing is something.
Occasionally you should give your Gremlin a bath.
Gnomes weapons are often enchanted.
Donations are 100% tax deductible.
Wearing solid metal gear and eating rations will recover health.
When in trouble, when in doubt; run in circles, scream and shout.
");

      // TRUTHS.
      AddRumours(true, @"
Good karma brings you great favours.
Divine before you wear or you may end up a bear.
Never turn a blind eye to carrots.
Grave yards are not just for the dead.
Do you really want to eat that mushroom?
Charmed enemies make great allies.
Dipping a scroll in a fountain gives you a blank slate.
Eating bugs can gain you talents.
See the priest to cure what ails you.
Pyrolisk are fire eaters.
Not all moulds are created equal.
Giants are not smart but they can make you strong.
Is eating Death worth the chance of teleport control?
Keep enchanted items away from disenchanter.
Hitch a ride with a charmed centaur.
Lost your pet? Try whistling for it.
For relief eat a eucalyptus leaf.
The utterly confused should chew on a lizard.
A pick-axe has many uses - be creative.
Kobolds have nice darts.
Jelly can give you heartburn.
Don't play with the green slime.
Adopt an older pet.
There is a scroll that can defeat the merchant.
Never trust a Nymph.
Phase spiders can be trippy.
Grave robbers are not good people.
Sacrificing a priest on his own altar is a very bad idea.
Is that a scroll in your pocket or are you just happy to tame me?
It wasn't luck; it is only karma.
");
    }
#endif
  }
}

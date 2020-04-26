using System.ComponentModel.DataAnnotations;

namespace Data
{
    public enum Emulator
    {
        [Display(Name = "Nox")]
        Nox,

        [Display(Name = "MEmu")]
        Memu,

        [Display(Name = "BlueStacks")]
        BlueStack,

        [Display(Name = "LDPlayer")]
        LDPlayer,

        [Display(Name = "Robotmon")]
        Robotmon,

        [Display(Name = "HiroMacro")]
        HiroMacro,
      
        [Display(Name = "AutoTouch")]
        AutoTouch,

        [Display(Name = "AnkuLua")]
        AnkuLua
    }
}

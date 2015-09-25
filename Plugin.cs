using System.ComponentModel.Composition;
using XrmToolBox.Extensibility;
using XrmToolBox.Extensibility.Interfaces;

namespace Ryr.XrmToolBox.EntityImageUpdater
{
    [Export(typeof(IXrmToolBoxPlugin)),
    ExportMetadata("Name", "Entity Image Updater"),
    ExportMetadata("Description", "Updates entity image using the url attribute"),
    ExportMetadata("SmallImageBase64", "iVBORw0KGgoAAAANSUhEUgAAABgAAAAYCAYAAADgdz34AAABDElEQVRIie2V0W3EIAyGPUJGcJggI2SEjtARMkI24CHCPN4IN0JHyAgdweLwe/oQyHEqNJeSPlQ6S/8DEviz8S8AeDJQc6PIX1vyH6i5efbc0eRLEKOR7rTkLfk5Tb5BJt/XJbeMLclnS35JOpjRuDECkW5vJyQXva5XAAAAmtv7HeKGw4BYsSLRqLkJV7UBAABw8v0GsYzHOph8n1aWA6ydSIfkhmpnlQCnRTUANTdo3KisXFJFK2ZnkDsTZvYdkLgilSLRRcDdWQ/KuioCYtWbQjXFIVvGuFdZuewC0LixdIV7M0BywwuwKPLXPwGse6T76UmoBuzFC/BPAOHPnStUAkiXe7h+IU7/6S9wpw1twkckSAAAAABJRU5ErkJggg=="),
    ExportMetadata("BigImageBase64", "iVBORw0KGgoAAAANSUhEUgAAAFAAAABQCAYAAACOEfKtAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAAYdEVYdFNvZnR3YXJlAHBhaW50Lm5ldCA0LjAuNWWFMmUAAAZTSURBVHhe7ZxLiBxFHMY3Mb4fgQhqcN1 + 7IoxggaNL2JYHxcPgpEEQ0QvgogGD4IHEw + DRA8xRFm3qycbA0HEgyL4QD2oENRDDjERxVcWIRg0QUyyM9Uzuxtjpv3 + tbXLbk1tT83UTJiZrQ8 + JqSr / tXfb6q7q3tmp6f9lC4aGCr0B3Gy3Y + T0YDxY35UZF5UukY2cMpSODLeB3jfAVwFr6kw / ZvxQw5iDXn5CR + g9s + AU82SX724fJds7jSjNF3k5cf8IJ8c0oKbZRzWPIhLD / Xk0sWyt5PH + I1 + XPpBB0xnnyUFLypskt0XsjDzorFVgPKLCqmmGS8HUfL8Ap6JBK + 0Cue137SAjMzPAOQbA0OVC2XRhSMsTe7EoTiqB2Nun / F / AfK93rcKy2TpLhcOOYQeRPgTKgwbo + YXfbtKy + Uo3Ssctg8i7N86CFnGbD2AmXZWt23auEIfuA6LcDlUl4lmXsQfxjnvpC78lPnZqUNSs43xLdi + Hv3HtNtnzL8Pd5VXy1G7RO + n53nD / BHMkKI + NMz4f37M38Hrvnm2b6E3oW + 4dCsg / wlQFW07MkuOYftaWl / KPehs4fDbmAUP22nWbV8ZpZch + KfqdmECiCs31euPTt5U6 + pNa0UsuJ + kN0 / sRCeKlhfBcGEzwpd1IcmYdZOA + 8r0UsQEICmMk6sA6ZusmYi6kzhtPO3tTS + S3TpLPiu + gBC49ZonIM08xl8aGBqdWceZAiStGC5eiTfgE217aXFOZfy1jlpw3zaSnk / wACPjqsnH8fryYC5dIrsJ1QOQFI6cWoo15R5tn1mmR2I37KlcLru1sQAE8F7FzDutCyLMkokg4lsH982FR6oXIIneMC / iL6IdvSnVfYXpCl / 8dvnIX5fIbu2pMM / XAFDWOa8IGM + qM29ajQAkiVkf8afQNqnqO9ss2dbWV2fMrGe0O05mySn4cZqlsnmVGgUoBDBhlGzCufUfbQ0yS77u3Vm5WPZoP / lx6QmE1V0ZSwi2sdaywgqglMeS + 3DxmO9u50PdqaNt1B8VBrDzf8zZ6am7j8dkk0w1AyDJf / PULThd / Di7Bl31PcbXySbtqnRR3 + 6JAIvYr7DTRwDv8zA / vsb0vNMsgCR6QgOIw / BR7Md + L5 / c21EL66t3HL + 03hN2MwFOy9t7pDMX0o2oFQAXlBxASzmAlnIALeUAWsoBtFRXA6Qb9pW59IJ6nXXvq6qZAHX7Usuya3MVjqRLg4jvwC3aKAIeb8To + yU8WAsC2toBzO1bQvfC6POZug8m9uPkMD0hp + eMsqKdBoYqVyA4dkYTqn4n9IG6LK0VQjQOEHc9eKM34NYt + 5GWgZH53ZW5n + xnZBjxe1Bs / oej9Rr3xlmP120A0mcfqP + xtn + dxptwujdfvlaWblxBnm / QDdCwcVOf9V0WG4B0743g9BCjun8D9obH75alGxc9w9MVb9x8qyytlfUhHPMdcEVbo05PnbMtlQHwLE1zMyeTAEDfKNjdu / No5tNgK4BQ787CMrT / COELGHuyel + qXTWWdKsB / o6w603cz8rrgnz5Di9X + 7ES2lsBJNEHRwEr3y6 + SqLZH9XIOPcBsHSrAR6UTZoqBLIGWK9Q / 2DVeLADaCjUdwBthPoOoI1Q3wG0Eeo7gDZCfQfQRqjvANoI9R1AG6G + A2gj1HcAbYT6DqCNUN8BtBHqdw / AkJVerx6LV7Afz8kmTRdlqR6zQwH2RWMPBPQd6rljjbXyr48oizKecEcCpD95AMBtfsyPYmxOrxhrc6sOXxJlmZVrxh0JUCiXLhY / AcCSm / 1o3Mv6EKoZoixKNuHOBXiORVmUbMIOoKEoi5JN2AE0FGVRsgk7gIaiLEo2YQfQUJRFySbsABqKsijZhB1AQ1EWJZuwA2goyqJkE3YADUVZlGzCDqChKIuSTdgBNBRlUbIJO4CGoixKNmEH0FCURckm7AAairIo2YQdQENRFiWbsANoKMqiZBN2AA1FWZRswg6goSiLkk24pQDx / 6PhrsLqbnDAkp / 1GVs7A8mlbjBAncFrVb5zAbCr3RyAUeFRXfGFYb5WYmhcYZzcj0KZv13apT7TF0 + EEkPjos9lMZUPawbobjN + aGDoxBUSg4XoB73yxet9VvwAM1F7su0yn0TOt70RvkISyFBPz//8LHEZ1yV1UgAAAABJRU5ErkJggg=="),
    ExportMetadata("BackgroundColor", "Lavender"),
    ExportMetadata("PrimaryFontColor", "Black"),
    ExportMetadata("SecondaryFontColor", "Gray")]

    public class Plugin : PluginBase
    {
        public override IXrmToolBoxPluginControl GetControl()
        {
            return new EntityImageUpdater();
        }
    }
}

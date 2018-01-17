using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;

namespace Purdue.Agrawal.Distillation
{
    /// <summary>
    /// Economic, design, and physical parameters.
    /// Maybe should split into separate classes, but it affects Matlab code.
    /// </summary>
    public class ParamCost
    {
        public ParamCost()
        {
            ParameterSets.SetDefaults(this);
        }

        // Note double? is syntax for Nullable<double>

        [Description("Fixed height (m) added to column independent of number of trays")]
        [DefaultValue(4)]
        public double? BaseHeight;

        [Description("Installation cost of a condenser (USD)")]
        [DefaultValue(15000)]
        public double? ConCost;

        [Description("Maximum number of trays in a column")]
        [DefaultValue(200)]
        public int? MaxTrays;

        [Description("Minimum number of trays in a column")]
        [DefaultValue(2)]
        public int? MinTrays;

        [Description("Hours of operation in a year")]
        [DefaultValue(8000)]
        public double? OpHrs;

        [Description("Column pressure (atm)")]
        [DefaultValue(1)]
        public double? OpPres;

        [Description("Years of operation")]
        [DefaultValue(2)]
        public double? OpYrs;

        [Description("Recovery of assumed heavy key in bottoms")]
        [DefaultValue(0.98)]
        public double? RecovHK;

        [Description("Recovery of assumed light key in distillate.")]
        [DefaultValue(0.98)]
        public double? RecovLK;

        [Description("Liquid density (kg/m3)")]
        [DefaultValue(735)]
        public double? RoL;

        [Description("Steam cost (USD/MJ)")]
        [DefaultValue(5.09*1E-3)]
        public double? Scost;

        [Description("Feed temperature (K)")]
        [DefaultValue(362.7286)]
        public double? TF;

        [Description("Height of 1 tray (m)")]
        [DefaultValue(0.6)]
        public double? TrayHeight;

        [Description("Heat transfer coefficient (W/m2/K)")]
        [DefaultValue(800)]
        public double? Uexch;

        [Description("Cooling water cost (USD/MJ)")]
        [DefaultValue(0.19*1E-3)]
        public double? Wcost;

        #region Not scalars, but expected by Matlab within this class

        /// <summary> Vector of latent heat of vaporization (MJ/kmol).  Empty implies defaults. </summary>
        public double[] Lambda { get; set; }

        /// <summary> Vector of molecular weights.  Empty implies defaults.</summary>
        public double[] PM { get; set; }

        #endregion

    }
}

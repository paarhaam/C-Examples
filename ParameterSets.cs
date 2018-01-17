using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ComponentModel;

namespace Purdue.Agrawal.Distillation
{
    /// <summary>
    /// Names of possible user-specified scalar parameters
    /// (not vector data designed to be in InstanceDataset)
    /// </summary>
    public class ParameterSets
    {
        public const string KeyFeedQual = "FeedQuality";
        public const string KeyProbType = "ProblemType";

        public static IEnumerable<ParamKey> ParametersFromFields<T>()
        {
            return ParametersFromFields(typeof(T));
        }

        /// <summary>
        /// Uses all public fields (not properties) of type having
        /// System.ComponentModel.Description attribute and optionally DefaultValue
        /// </summary>
        protected static IEnumerable<ParamKey> ParametersFromFields(Type type)
        {
            FieldInfo[] fields = type.GetFields();

            return from f in fields
                   let texta = f.GetCustomAttribute<DescriptionAttribute>() // required
                   where texta != null
                   let dfa = f.GetCustomAttribute<DefaultValueAttribute>() // optional
                   let df = (dfa == null ) ? null : dfa.Value
                   let label = (texta == null) ? "" : texta.Description
                   let suffix = (df == null) ? null : " (default " + df + ")"
                   let nullable = f.FieldType.IsGenericType
                   select new ParamKey(f.Name, label + suffix, nullable) { Default = df };
        }


        public static void SetValue(object target, ParamKey p, object v, bool strict)
        {
            Type type = target.GetType();
            FieldInfo f = type.GetField(p.Key);
            if (f == null)
                throw new ApplicationException(type.Name + " does not contain " + p);

            try
            {
                object converted = null;

                if (v != null)
                {
                    Type targetType;
                    if (f.FieldType.IsGenericType) // Nullable
                        targetType = f.FieldType.GetGenericArguments().First();
                    else
                        targetType = f.FieldType;

                    converted = Convert.ChangeType(v, targetType, System.Globalization.CultureInfo.InvariantCulture);
                }

                if (f.FieldType.IsGenericType || converted != null)
                {
                    f.SetValue(target, converted);
                }
            }
            catch ( Exception ex )
            {
                string msg = string.Format("Cannot determine {0} value from {1} input, {2}", f.FieldType.Name.ToLower(), p.Key, v);
                if (strict)
                    throw new ApplicationException(msg, ex);
                else
                    Debug.WriteLine(msg);
            }
        }

        /// <summary>
        /// For numeric types.
        /// </summary>
        /// <param name="target"> of the Param* classes with properties </param>
        /// <remarks> 
        /// Note that for Nullable types, normally the null default is appropriate, 
        /// e.g. for parameters where no numerical default is sensible,
        ///  like a constraint that is absent.
        /// </remarks>
        public static void SetDefaults(object target) 
        {
            foreach (ParamKey param in ParametersFromFields(target.GetType()))
            {
                if ( false == param.Nullable )
                    SetValue(target, param, param.Default, strict: true);
            }
        }

        public static IEnumerable<ParamKey> UserInterfaceParamKeys
        {
            get
            {
                return SolverParameters().OrderBy(p => p.Key)
                    .Concat(CostParameters().OrderBy(p => p.Key));
            }
        }

        public static IEnumerable<string> UserInterfaceKeys
        {
            get
            {
                return UserInterfaceParamKeys.Select(p => p.Key).ToList();
            }
        }

        /// <summary> They do not belong to structs input to Matlab </summary>
        public static IEnumerable<ParamKey> RequiredParameters()
        {
            yield return new ParamKey(KeyFeedQual);
            yield return new ParamKey(KeyProbType);
        }

        public static IEnumerable<ParamKey> SolverParameters()
        {
            return ParametersFromFields<ParamSolving>();
        }

        public static IEnumerable<ParamKey> CostParameters()
        {
            return ParametersFromFields<ParamCost>();
        }
    }
}

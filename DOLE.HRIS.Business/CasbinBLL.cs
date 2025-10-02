using DOLE.HRIS.Application.DataAccess;
using Casbin;
using Casbin.Model;
using Casbin.Persist.Adapter.File;
using Casbin.Persist.Adapter.Stream;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOLE.HRIS.Application.Business
{
    public static class CasbinBll
    {
        // Enforcer singleton cargado desde modelo embebido y archivo policy.csv
        private static readonly Lazy<Enforcer> _enforcer = new Lazy<Enforcer>(() =>
        {
            var modelText = @"
            [request_definition]
            r = globalOrder, sub, obj, act

            [policy_definition]
            p = GlobalOrder, sub, obj, act, desc_es, desc_en

            [role_definition]
            g = _, _

            [policy_effect]
            e = some(where (p.eft == allow))

            [matchers]
            m = g(r.sub, p.sub) && r.obj == p.obj && r.act == p.act && r.globalOrder == p.GlobalOrder
            ";
            var model = DefaultModel.CreateFromText(modelText);

            var client = new KeyVault();
            string secret = client.GetSecret("HRISPoliticasCasbin");
            byte[] buffer = Encoding.UTF8.GetBytes(secret);
            Stream stream = new MemoryStream(buffer);
            StreamAdapter adapter = new StreamAdapter(stream);

            var enforcer = new Enforcer(model, adapter);

            return enforcer;
        });

        /// <summary>
        /// Verifica si al menos uno de los roles tiene permiso para realizar la acción sobre el recurso.
        /// </summary>
        public static bool CheckAccess(string userName, string action, string resource, List<string> roles)
        {
            bool chkAccess = false;
            var normalizedResource = NormalizeUrl(resource.Contains("~") ? resource.Substring(1) : resource);

            foreach (var role in roles)
            {
                foreach (var policy in _enforcer.Value.GetPolicy())
                {
                    var values = new List<string>();
                    foreach (var val in policy)
                    {
                        values.Add(val);
                    }

                    var policyRole = values[1];
                    var policyResource = NormalizeUrl(values[2]);
                    var policyAction = values[3];

                    var linkedRoles = _enforcer.Value.GetRolesForUser(role);
                    bool hasRole = role == policyRole || linkedRoles.Contains(policyRole);

                    if (hasRole && normalizedResource == policyResource && action == policyAction)
                    {
                        return true;
                    }
                }

                if (chkAccess) break;
            }

            return chkAccess;
        }

        /// <summary>
        /// Quita los parametros de una url
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string NormalizeUrl(string url)
        {
            if (string.IsNullOrEmpty(url))
                return url;

            // quita fragmento (#...)
            int hashIndex = url.IndexOf('#');
            if (hashIndex >= 0)
                url = url.Substring(0, hashIndex);

            // quita query parameters (?...)
            int queryIndex = url.IndexOf('?');
            if (queryIndex >= 0)
                url = url.Substring(0, queryIndex);

            return url.ToLower();
        }

        public static DataTable ObtenerMenuPorRoles(List<string> roles, string cultureName)
        {
            var dt = new DataTable();
            dt.Columns.Add("GlobalOrder", typeof(string));
            dt.Columns.Add("url", typeof(string));
            dt.Columns.Add("act", typeof(string));
            dt.Columns.Add("desc", typeof(string));

            var todasLasPoliticas = _enforcer.Value.GetPolicy();

            foreach (var policy in todasLasPoliticas)
            {
                var value = new List<string>();
                foreach (var val in policy)
                {
                    value.Add(val);
                }

                string GlobalOrder = value[0];
                string policyRole = value[1];
                string policyObj = value[2];
                string policyAct = value[3];
                string desc = "";
                if (!cultureName.Contains("US"))
                    desc = value[4];
                else
                    desc = value[5];

                foreach (var rol in roles)
                {
                    if (_enforcer.Value.Enforce(GlobalOrder, rol, policyObj, policyAct))
                    {
                        var row = dt.NewRow();
                        row["GlobalOrder"] = GlobalOrder;
                        row["url"] = policyObj == " " ? null : policyObj;
                        row["act"] = policyAct;
                        row["desc"] = desc;
                        dt.Rows.Add(row);
                    }
                }
            }

            return dt;
        }

    }
}

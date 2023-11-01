using System.Collections.Generic;
using System.Windows.Forms;
using Artech.Architecture.Common.Objects;
using Artech.Architecture.Common.Services;
using Artech.Genexus.Common;
using Artech.Genexus.Common.Objects;
using Artech.Genexus.Common.Parts;
using gx = Artech.Genexus.Common.Objects;

namespace GeneXus.Packages.SupportTools.ShortNames
{
    public class ShortenNames
    {
        private const string ShorteningSection = "ShortenNames";

        public static bool Execute(KBModel model)
        {
            if (model == null)
                return false;

            int maxAttNameLength = model.GetPropertyValue<int>(Properties.MODEL.SignificantAttributeNameLength);
            int maxTblNameLength = model.GetPropertyValue<int>(Properties.MODEL.SignificantTableNameLength);
            int maxObjNameLength = model.GetPropertyValue<int>(Properties.MODEL.SignificantObjectNameLength);

            ShortenNamesDlg dlg = new ShortenNamesDlg(maxAttNameLength, maxTblNameLength, maxObjNameLength);
            if (dlg.ShowDialog() != DialogResult.OK)
                return false;

            IOutputService output = CommonServices.Output;
            output.Show(output.GetDefaultOutputId());
            output.StartSection(ShorteningSection, Resources.ShortenNamesSection);
            bool success = false;

            try
            {
                using (KnowledgeBase.Transaction transaction = model.KB.BeginTransaction())
                {
                    if (dlg.ShortenAttributeNames)
                        ShortenAttributeNames(model, maxAttNameLength, output);

                    if (dlg.ShortenTableNames)
                        ShortenTableNames(model, maxTblNameLength, output);

                    if (dlg.ShortenObjectNames)
                        ShortenObjectNames(model, maxObjNameLength, output);

                    transaction.Commit();
                    success = true;
                }
            }
            catch (System.Exception exception)
            {
                output.AddErrorLine(exception);
            }
            finally
            {
                output.EndSection(ShorteningSection, Resources.ShortenNamesSection, success);
            }

            return success;
        }

        private static void ShortenAttributeNames(KBModel model, int maxAttNameLength, IOutputTarget output)
        {
            ShortenKBObjectNames("Attributes", Attribute.GetAll(model), maxAttNameLength, output);
        }

        private static void ShortenKBObjectNames(string categoryName, IEnumerable<KBObject> objects, int maxNameLength, IOutputTarget output)
        {
            output.AddLine(string.Format(Resources.RenamingObjects, categoryName, maxNameLength));

            int objsTotal = 0;
            int objsRenamed = 0;

            foreach (KBObject obj in objects)
            {
                objsTotal++;
                output.AddText(string.Format(Resources.ProcessingObject, obj.TypeDescriptor.Description, obj.Name));

                if (ShortenObjectNameIfNeeded(obj, maxNameLength))
                {
                    obj.Save();
                    output.AddLine(string.Format(Resources.Renamed, obj.Name));
                    objsRenamed++;
                }
                else
                    output.AddLine(Resources.NotRenamed);
            }

            output.AddLine(string.Format(Resources.RenamedObjects, objsRenamed, objsTotal, categoryName));
        }

        private static void ShortenTableNames(KBModel model, int maxTblNameLength, IOutputTarget output)
        {
            output.AddLine(string.Format(Resources.RenamingObjects, "Tables and Indexes", maxTblNameLength));

            int tblsTotal = 0;
            int tblsRenamed = 0;
            int idxsTotal = 0;
            int idxsRenamed = 0;
            foreach (Table tbl in Table.GetAll(model))
            {
                tblsTotal++;
                output.AddText(string.Format(Resources.ProcessingObject, tbl.TypeDescriptor.Description, tbl.Name));

                bool changed = false;
                if (ShortenObjectNameIfNeeded(tbl, maxTblNameLength))
                {
                    changed = true;
                    output.AddLine(string.Format(Resources.Renamed, tbl.Name));
                    tblsRenamed++;
                }
                else
                    output.AddLine(Resources.NotRenamed);

                foreach (TableIndex index in tbl.TableIndexes.Indexes)
                {
                    idxsTotal++;
                    output.AddText(string.Format(Resources.ProcessingObject, index.Index.TypeDescriptor.Description, index.Index.Name));

                    if (ShortenObjectNameIfNeeded(index.Index, maxTblNameLength))
                    {
                        changed = true;
                        output.AddLine(string.Format(Resources.Renamed, index.Index.Name));
                        idxsRenamed++;
                    }
                    else
                        output.AddLine(Resources.NotRenamed);
                }

                if (changed)
                    tbl.Save();
            }

            output.AddLine(string.Format(Resources.RenamedObjects, tblsRenamed, tblsTotal, "Tables"));
            output.AddLine(string.Format(Resources.RenamedObjects, idxsRenamed, idxsTotal, "Indexes"));
        }

        private static void ShortenObjectNames(KBModel model, int maxObjNameLength, IOutputTarget output)
        {
            ShortenKBObjectNames("Menus", gx.Menu.GetAll(model), maxObjNameLength, output);
            ShortenKBObjectNames("Menu Bars", Menubar.GetAll(model), maxObjNameLength, output);
            ShortenKBObjectNames("Procedures", Procedure.GetAll(model), maxObjNameLength, output);
            ShortenKBObjectNames("SDTs", SDT.GetAll(model), maxObjNameLength, output);
            ShortenKBObjectNames("Transactions", Transaction.GetAll(model), maxObjNameLength, output);
            ShortenKBObjectNames("Work Panels", WorkPanel.GetAll(model), maxObjNameLength, output);
            ShortenKBObjectNames("Web Panels", WebPanel.GetAll(model), maxObjNameLength, output);
        }

        private static bool ShortenObjectNameIfNeeded(KBObject obj, int maxLength)
        {
            if (obj.Name.Length > maxLength)
            {
                obj.Name = obj.Name.Remove(maxLength);
                return true;
            }

            return false;
        }
    }
}

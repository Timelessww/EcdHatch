using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Colors;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;
using System.Collections.Generic;

namespace EcdHatch
{
    public class ZCHatch
    {

   
     
        /// <summary>
        /// 卫生间填充
        /// 标板高为Hj-0.350
        /// 板厚100
        /// </summary>
        [CommandMethod("EcdLooHatch")]
        public void LooHatch()
        {
            Database db = HostApplicationServices.WorkingDatabase;
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;

            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                BlockTable blockTbl = tr.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord modelSpace = tr.GetObject(blockTbl[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;

                PromptSelectionOptions looh = new PromptSelectionOptions
                {
                    MessageForAdding = "请选择卫生间进行填充"
                };
                
                PromptSelectionResult selectionResult = ed.GetSelection(looh);
                if (selectionResult.Status != PromptStatus.OK) return;
                List<ObjectId> objectIds = new List<ObjectId>(selectionResult.Value.GetObjectIds());
      
                CreateHatch("SACNCR", modelSpace,tr, objectIds);
            
            }

        }
        /// <summary>
        /// 厨房填充
        /// 标板高为Hj-0.400
        /// 板厚120
        /// </summary>
        [CommandMethod("EcdCFHatch")]
        public void KHatch()
        {
            Database db = HostApplicationServices.WorkingDatabase;
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;

            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                BlockTable blockTbl = tr.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord modelSpace = tr.GetObject(blockTbl[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;

                PromptSelectionOptions kh = new PromptSelectionOptions
                {
                    MessageForAdding = "请选择厨房进行填充"
                };

                PromptSelectionResult selectionResult = ed.GetSelection(kh);
                if (selectionResult.Status != PromptStatus.OK) return;
                List<ObjectId> objectIds = new List<ObjectId>(selectionResult.Value.GetObjectIds());

                CreateHatch("JIS_LC_20", modelSpace, tr, objectIds);
            }
        }
       
        /// <summary>
        /// 卫生间填充
        /// 标板高为Hj-0.100
        /// 板厚120
        /// </summary>
        [CommandMethod("EcdBathHatch")]
        public void BathHatch()
        {
            Database db = HostApplicationServices.WorkingDatabase;
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                BlockTable blockTbl = tr.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord modelSpace = tr.GetObject(blockTbl[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;

                PromptSelectionOptions bh = new PromptSelectionOptions
                {
                    MessageForAdding = "请选择卫生间进行填充"
                };

                PromptSelectionResult selectionResult = ed.GetSelection(bh);
                if (selectionResult.Status != PromptStatus.OK) return;
                List<ObjectId> objectIds = new List<ObjectId>(selectionResult.Value.GetObjectIds());
                CreateHatch("ACAD_ISO07W100", modelSpace, tr, objectIds);
            }
        }
        /// <summary>
        /// 厨房出阳台填充
        /// 标板高为Hj-0.100
        /// 板厚100
        /// </summary>
        [CommandMethod("EcdYTHatch")]
        public void Hatch4()
        {
            Database db = HostApplicationServices.WorkingDatabase;
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                BlockTable blockTbl = tr.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord modelSpace = tr.GetObject(blockTbl[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;

                PromptSelectionOptions bh = new PromptSelectionOptions
                {
                    MessageForAdding = "请选择厨房阳台进行填充"
                };

                PromptSelectionResult selectionResult = ed.GetSelection(bh);
                if (selectionResult.Status != PromptStatus.OK) return;
                List<ObjectId> objectIds = new List<ObjectId>(selectionResult.Value.GetObjectIds());
                CreateHatch("HEX", modelSpace, tr, objectIds);
            }
        }
        /// <summary>
        /// 车库顶板填充
        /// 标板高为Hj-0.100
        /// 板厚100
        /// </summary>
        [CommandMethod("EcdCKHatch")]
        public void Hatch5()
        {
            Database db = HostApplicationServices.WorkingDatabase;
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                BlockTable blockTbl = tr.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord modelSpace = tr.GetObject(blockTbl[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;

                PromptSelectionOptions bh = new PromptSelectionOptions
                {
                    MessageForAdding = "请选择车库顶板进行填充"
                };
                PromptSelectionResult selectionResult = ed.GetSelection(bh);
                if (selectionResult.Status != PromptStatus.OK) return;
                List<ObjectId> objectIds = new List<ObjectId>(selectionResult.Value.GetObjectIds());
                CreateHatch("ANSI37", modelSpace, tr, objectIds);
            }
        }


        /// <summary>
        /// 填充
        /// </summary>
        /// <param name="hatch"></param>
        /// <param name="填充图案名称"></param>
        /// <param name="块表记录"></param>
        /// <param name="tr"></param>
        /// <param name="objectIds"></param>
        public void CreateHatch(string hatchname, BlockTableRecord record, Transaction tr, List<ObjectId> objectIds)
        {


            Hatch hatch = new Hatch();
            record.AppendEntity(hatch);
            tr.AddNewlyCreatedDBObject(hatch, true);
            hatch.SetHatchPattern(HatchPatternType.PreDefined, hatchname);
            hatch.Associative = true;
            ObjectIdCollection ids = new ObjectIdCollection();
            foreach (var id in objectIds)
            {
                ids.Add(id);
                hatch.AppendLoop(HatchLoopTypes.Outermost, ids);
                hatch.EvaluateHatch(true);
                hatch.PatternScale = 1;
                ids.Clear();
            }

            tr.Commit();

        }
    }
}
